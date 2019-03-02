using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public class DYAuthorizeAttribute:AuthorizeAttribute
    { 
        public string Permission { get; }

        public DYAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }

    }
}
