using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/InvoiceTruck/[action]")]
    public class InvoiceShipmentController : Controller
    {
        private IInvoiceShipmentService invoiceShipmentService;

        public InvoiceShipmentController(IInvoiceShipmentService invoiceShipmentService)
        {
            this.invoiceShipmentService = invoiceShipmentService;
        }
        [HttpGet]
        public IList<InvoiceShipmentDataObject> GetInvoiceShipmentList()
        {

            return this.invoiceShipmentService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.invoiceShipmentService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public InvoiceShipmentDataObject Update([FromBody]InvoiceShipmentDataObject invoiceShipment)
        {
            return this.invoiceShipmentService.Update(invoiceShipment);
        }
        [HttpPost]
        public void Add()
        {
            Stream stream = HttpContext.Request.Body;
            byte[] buf = new byte[HttpContext.Request.ContentLength.Value];
            stream.Read(buf, 0, buf.Length);
            string str = Encoding.UTF8.GetString(buf);
            Console.WriteLine(str);
            //return this.invoiceTruckService.Add(invoiceTruck);
        }
    }
}