using AutoMapper;
using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs;
using KoiDeli.Domain.DTOs.KoiFishDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class PackingService : IPackingService
    {
        private readonly IBoxRepository _boxRepository;
        private readonly IKoiFishRepository _koiFishRepository;
        private readonly IBoxOptionRepository _boxOptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public PackingService(IBoxRepository boxRepository,
            IKoiFishRepository koiFishRepository,
            IBoxOptionRepository boxOptionRepository,

            IUnitOfWork unitOfWork,
            ICurrentTime currentTime,
            AppConfiguration configuration,
            IMapper mapper)
        {
            _boxRepository = boxRepository;
            _koiFishRepository = koiFishRepository;
            _boxOptionRepository = boxOptionRepository;
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;

        }
        public async Task<ApiResult<List<BoxWithFishDetailDTO>>> OptimizePackingAsync(List<KoiFishModelOptimize> fishList, List<BoxModelOptimize> boxList)
        {
            var response = new ApiResult<List<BoxWithFishDetailDTO>>();
            try
            {
                // Sắp xếp cá theo thể tích giảm dần
                var sortedFishList = fishList.OrderByDescending(f => f.Volume).ToList();
                // Sắp xếp hộp theo dung tích giảm dần, sau đó theo giá tăng dần
                var sortedBoxList = boxList.OrderByDescending(b => b.MaxVolume).ThenBy(b => b.Price).ToList();

                // Dùng để lưu các hộp đã sử dụng
                List<BoxWithFishDetailDTO> usedBoxes = new List<BoxWithFishDetailDTO>();

                foreach (var fish in sortedFishList)
                {
                    Int64 remainingQuantity = fish.Quantity;

                    while (remainingQuantity > 0)
                    {
                        bool placed = false;

                        // Kiểm tra các hộp đã sử dụng
                        foreach (var usedBox in usedBoxes)
                        {
                            if (usedBox.Box.RemainingVolume >= fish.Volume)
                            {
                                // Tính số lượng cá có thể thêm vào hộp hiện tại
                                Int64 quantityThatCanFit = usedBox.Box.RemainingVolume / fish.Volume;
                                Int64 fishToPlace = Math.Min(quantityThatCanFit, remainingQuantity);

                                usedBox.Box.RemainingVolume -= fish.Volume * fishToPlace;

                                var fishInBox = usedBox.Fishes.FirstOrDefault(f => f.Id == fish.Id);
                                if (fishInBox != null)
                                {
                                    fishInBox.Quantity += fishToPlace;
                                }
                                else
                                {
                                    usedBox.Fishes.Add(new KoiFishModelOptimize
                                    {
                                        Id = fish.Id,
                                        Volume = fish.Volume,
                                        Description = fish.Description,
                                        Size = fish.Size,
                                        Quantity = fishToPlace
                                    });
                                }

                                remainingQuantity -= fishToPlace;
                                placed = true;

                                if (remainingQuantity <= 0)
                                {
                                    break;
                                }
                            }
                        }

                        // Nếu chưa đặt hết cá và không có hộp hiện tại nào đủ chứa, tìm hộp mới
                        if (!placed)
                        {
                            BoxModelOptimize newBox;

                            // Nếu cá có thể tích nhỏ hơn 225, chỉ lấy hộp medium
                            if (fish.Volume < 223)
                            {
                                newBox = sortedBoxList.FirstOrDefault(b => b.MaxVolume >= fish.Volume && b.Name.Contains("Medium"));
                            }
                            else
                            {
                                newBox = sortedBoxList.FirstOrDefault(b => b.MaxVolume >= fish.Volume);
                            }

                            if (newBox == null)
                            {
                                throw new InvalidOperationException("Không có hộp nào có thể chứa được cá.");
                            }

                            var newBoxDetails = new BoxModelOptimize
                            {
                                Id = usedBoxes.Count + 1,
                                MaxVolume = newBox.MaxVolume,
                                RemainingVolume = newBox.MaxVolume,
                                Price = newBox.Price,
                                Name = newBox.Name
                            };

                            Int64 fishToPlaceInNewBox = Math.Min(remainingQuantity, newBoxDetails.RemainingVolume / fish.Volume);
                            newBoxDetails.RemainingVolume -= fish.Volume * fishToPlaceInNewBox;

                            var boxWithDetails = new BoxWithFishDetailDTO
                            {
                                Box = newBoxDetails,
                                BoxPrice = newBox.Price,
                                Fishes = new List<KoiFishModelOptimize>()
                            };

                            boxWithDetails.Fishes.Add(new KoiFishModelOptimize
                            {
                                Id = fish.Id,
                                Volume = fish.Volume,
                                Description = fish.Description,
                                Size = fish.Size,
                                Quantity = fishToPlaceInNewBox
                            });

                            usedBoxes.Add(boxWithDetails);
                            remainingQuantity -= fishToPlaceInNewBox;
                        }
                    }
                }


                // Trả về kết quả với thông tin đầy đủ về cá, kích thước và số lượng
                response.Data = usedBoxes;
                response.Success = true;
                response.Message = "Packing optimization completed successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred during packing.";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }



    }
}
