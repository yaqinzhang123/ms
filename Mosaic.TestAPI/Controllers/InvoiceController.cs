using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mosaic.DTO;
using Mosaic.Resolve;

namespace Mosaic.TestAPI.Controllers
{
    [Produces("Application/Json")]
    [Route("api/[controller]/[action]")]
    public class InvoiceController : Controller
    {

        public async Task<InvoiceDataObject> Add([FromBody]PrinterData printerData)
        {
            var invoice = InvoiceResolve.Resolve(printerData);
            return invoice;
        }
    }
}