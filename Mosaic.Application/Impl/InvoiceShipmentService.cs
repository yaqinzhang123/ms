using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using Mosaic.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class InvoiceShipmentService : Service<InvoiceShipmentDataObject, InvoiceShipment>, IInvoiceShipmentService
    {
        public InvoiceShipmentService(IInvoiceShipmentRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}