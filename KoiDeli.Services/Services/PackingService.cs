using AutoMapper;
using KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs;
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

        public async Task<ApiResult<List<BoxWithFishDetailDTO>>> OptimizePackingAsync(List<KoiFish> fishList, List<Box> boxList)
        {
            var response = new ApiResult<List<BoxWithFishDetailDTO>>();

            try
            {
                // Sắp xếp danh sách cá theo volume giảm dần
                var sortedFishList = fishList.OrderByDescending(f => f.Volume).ToList();

                // Sắp xếp hộp theo chi phí thấp nhất trước
                var sortedBoxList = boxList.OrderBy(b => b.Price).ToList();

                List<BoxWithFishDetailDTO> usedBoxes = new List<BoxWithFishDetailDTO>();

                foreach (var fish in sortedFishList)
                {
                    bool placed = false;
                    foreach (var box in sortedBoxList)
                    {
                        if (box.CanFitFish(fish))
                        {
                            box.AddFish(fish);

                            var boxWithDetails = usedBoxes.FirstOrDefault(b => b.Box.Id == box.Id);
                            if (boxWithDetails == null)
                            {
                                boxWithDetails = new BoxWithFishDetailDTO
                                {
                                    Box = box
                                };
                                usedBoxes.Add(boxWithDetails);
                            }
                            boxWithDetails.Fishes.Add(fish);

                            // Thêm BoxOption
                            var boxOption = new BoxOption
                            {
                                FishId = fish.Id,
                                BoxId = box.Id,
                                IsChecked = true
                            };
                            await _unitOfWork.BoxOptionRepository.AddAsync(boxOption);

                            placed = true;
                            break;
                        }
                    }

                    if (!placed)
                    {
                        // Nếu không có hộp nào phù hợp, tạo hộp mới
                        var newBox = new Box
                        {
                            MaxVolume = sortedBoxList.First().MaxVolume,
                            Price = sortedBoxList.First().Price
                        };
                        newBox.AddFish(fish);

                        var boxWithDetails = new BoxWithFishDetailDTO
                        {
                            Box = newBox
                        };
                        boxWithDetails.Fishes.Add(fish);
                        usedBoxes.Add(boxWithDetails);

                        // Thêm BoxOption
                        var newBoxOption = new BoxOption
                        {
                            FishId = fish.Id,
                            BoxId = newBox.Id,
                            IsChecked = true
                        };
                        await _unitOfWork.BoxOptionRepository.AddAsync(newBoxOption);

                        // Thêm hộp mới vào CSDL
                        await _unitOfWork.BoxRepository.AddAsync(newBox);
                    }
                }

                // Lưu tất cả thay đổi vào cơ sở dữ liệu
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                if (isSuccess)
                {
                    response.Data = usedBoxes;
                    response.Success = true;
                    response.Message = "Packing optimization completed successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error saving box and fish details.";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
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
