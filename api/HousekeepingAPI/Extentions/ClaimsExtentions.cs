﻿using System.Security.Claims;

namespace HousekeepingAPI.Extentions
{
    public static class ClaimsExtentions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x =>
                x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
        }

    }
}
