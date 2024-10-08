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
                var sortedFishList = fishList.OrderByDescending(f => f.Volume).ToList();
                var sortedBoxList = boxList.OrderBy(b => b.Price).ToList();

                List<BoxWithFishDetailDTO> usedBoxes = new List<BoxWithFishDetailDTO>();

                foreach (var fish in sortedFishList)
                {
                    int remainingQuantity = (int)fish.Quantity;

                    while (remainingQuantity > 0)
                    {
                        bool placed = false;

                        foreach (var box in sortedBoxList)
                        {
                            if (box.CanFitFish(fish))
                            {
                                remainingQuantity = box.AddFish(fish);

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

                                placed = true;
                                break;
                            }
                        }

                        if (!placed)
                        {
                            // Tạo hộp mới để chứa cá còn lại
                            var newBox = new BoxModelOptimize
                            {
                                MaxVolume = sortedBoxList.First().MaxVolume,
                                Price = sortedBoxList.First().Price
                            };
                            remainingQuantity = newBox.AddFish(fish);

                            var boxWithDetails = new BoxWithFishDetailDTO
                            {
                                Box = newBox
                            };
                            boxWithDetails.Fishes.Add(fish);
                            usedBoxes.Add(boxWithDetails);
                        }
                    }
                }

                response.Data = usedBoxes;
                response.Success = true;
                response.Message = "Packing optimization completed successfully (results printed to console).";
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
