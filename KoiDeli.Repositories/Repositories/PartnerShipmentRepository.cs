﻿using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class PartnerShipmentRepository : GenericRepository<PartnerShipment>, IPartnerShipmentRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public PartnerShipmentRepository(
            KoiDeliDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
    }
}
