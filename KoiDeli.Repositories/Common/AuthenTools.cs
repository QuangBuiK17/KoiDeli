﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Common
{
    public class AuthenTools
    {
        public static string GetCurrentUserId(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            }
            return null;
        }
    }
}
