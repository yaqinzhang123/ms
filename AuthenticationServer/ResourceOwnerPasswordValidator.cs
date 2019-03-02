using AuthenticationServer.DataObjects;
using AuthenticationServer.ServiceContracts;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserInfoService userInfoService;

        public ResourceOwnerPasswordValidator(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = this.userInfoService.Get(context.UserName, context.Password);

            if (user!=null)
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: GetUserClaims(user)
                    );
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

        private IEnumerable<Claim> GetUserClaims(UserInfoDataObject userInfo)
        {
            JsonObject user = new JsonObject(JsonConvert.SerializeObject(userInfo));
            PropertyInfo[] properties = typeof(UserInfoDataObject).GetProperties();
            List<Claim> claims = new List<Claim>();
            foreach(PropertyInfo property in properties)
            {                
                object value = property.GetValue(userInfo);
                if (value is ICollection<int>)
                {
                    string valueStr = string.Join(",", (value as ICollection<int>).Select(p => p));
                    claims.Add(new Claim(property.Name, value != null ? valueStr : ""));
                }
                else
                    claims.Add(new Claim(property.Name, value != null ? value.ToString() : ""));
            }
            return claims;
        }

        private IEnumerable<Claim> GetUserClaims()
        {
            return new List<Claim>();
        }
    }
}
