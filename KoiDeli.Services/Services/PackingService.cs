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

                // Tính tổng thể tích của tất cả cá cần đóng gói
                Int64 totalRemainingVolume = sortedFishList.Sum(f => f.Volume * f.Quantity);

                // Dùng để lưu các hộp đã sử dụng
                List<BoxWithFishDetailDTO> usedBoxes = new List<BoxWithFishDetailDTO>();

                foreach (var fish in sortedFishList)
                {
                    Int64 remainingQuantity = fish.Quantity;

                    if (remainingQuantity <= 0)
                    {
                        throw new InvalidOperationException($"Fish quantity is invalid: {remainingQuantity} for Fish ID: {fish.Id}");
                    }

                    while (remainingQuantity > 0)
                    {
                        bool placed = false;

                        // Kiểm tra các hộp đã sử dụng
                        foreach (var usedBox in usedBoxes)
                        {
                            if (usedBox.Box.RemainingVolume >= fish.Volume)
                            {
                                Int64 quantityThatCanFit = usedBox.Box.RemainingVolume / fish.Volume;
                                Int64 fishToPlace = Math.Min(quantityThatCanFit, remainingQuantity);

                                if (fishToPlace <= 0)
                                {
                                    throw new InvalidOperationException("Calculated fishToPlace is invalid (<= 0). Possible loop error.");
                                }

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

                                // Cập nhật tổng thể tích còn lại của cá sau khi một lượng cá đã được xếp
                                totalRemainingVolume -= fish.Volume * fishToPlace;

                                if (remainingQuantity <= 0)
                                {
                                    break;
                                }
                            }
                        }

                        // Cập nhật số lượng cá còn lại trong danh sách fishList
                        fish.Quantity = remainingQuantity;

                        // Nếu chưa đặt hết cá và không có hộp hiện tại nào đủ chứa, tìm hộp mới
                        if (!placed)
                        {
                            // Tìm hộp mới từ danh sách hộp dựa trên tổng thể tích còn lại của cá và giá
                            var newBox = sortedBoxList
                                .Where(b => b.MaxVolume >= totalRemainingVolume) // Chọn hộp có thể tích lớn hơn hoặc bằng tổng thể tích còn lại
                                .OrderBy(b => b.Price) // Ưu tiên hộp có giá thấp hơn
                                .FirstOrDefault();

                            // Nếu không có hộp nào đủ lớn, chọn hộp lớn nhất
                            if (newBox == null)
                            {
                                newBox = sortedBoxList.FirstOrDefault(); // Chọn hộp có thể tích lớn nhất
                            }

                            if (newBox == null)
                            {
                                throw new InvalidOperationException("Không có hộp nào có thể chứa được cá.");
                            }

                            // Thêm kiểm tra để đảm bảo tránh loop quá nhiều lần
                            if (usedBoxes.Count > 10)
                            {
                                throw new InvalidOperationException("Loop exceeded maximum allowable box usage, potential overflow issue.");
                            }

                            // Tạo một hộp mới từ hộp được chọn (newBox)
                            var newBoxDetails = new BoxModelOptimize
                            {
                                Id = newBox.Id, // Tái sử dụng ID của hộp
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
                                BoxPrice = newBoxDetails.Price,
                                Fishes = new List<KoiFishModelOptimize>(),
                                UsageCount = usedBoxes.Count(b => b.Box.Id == newBoxDetails.Id) + 1 // Đếm số lần sử dụng hộp theo ID
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

                            // Cập nhật tổng thể tích còn lại sau khi thêm cá vào hộp mới
                            totalRemainingVolume -= fish.Volume * fishToPlaceInNewBox;
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
