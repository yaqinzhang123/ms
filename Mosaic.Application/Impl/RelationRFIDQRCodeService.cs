using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using Microsoft.EntityFrameworkCore;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class RelationRFIDQRCodeService : Service<RelationRFIDQRCodeDataObject, RelationRFIDQRCode>, IRelationRFIDQRCodeService
    {

        public RelationRFIDQRCodeService(IRelationRFIDQRCodeRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public RelationRFIDQRCodeDataObject AddQRCode(RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {
            if (relationRFIDQRCode.Content == null)
                return new RelationRFIDQRCodeDataObject();
            relationRFIDQRCode.TimeQRCode = DateTime.Now;
            return this.Add(relationRFIDQRCode);
        }

        public RelationRFIDQRCodeDataObject AddRFID(RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {
            relationRFIDQRCode.TimeRFID = DateTime.Now;
            if (relationRFIDQRCode.RFID == null)
                return new RelationRFIDQRCodeDataObject();
            if (relationRFIDQRCode.Content == null && relationRFIDQRCode.RFID != null)
                return this.Add(relationRFIDQRCode);
            RelationRFIDQRCodeDataObject relation = Mapper.Map<RelationRFIDQRCode, RelationRFIDQRCodeDataObject>(this.repository.Get(p => p.Content.Contains(relationRFIDQRCode.Content)).FirstOrDefault());
            if (relation == null)
                return this.Add(relationRFIDQRCode);
            relation.TimeRFID = DateTime.Now;
            return this.Update(relation);
        }

        public IList<RelationRFIDQRCodeDataObject> GetQRCodeList()
        {
            return Mapper.Map<IList<RelationRFIDQRCode>, IList<RelationRFIDQRCodeDataObject>>(this.repository.Get(p => p.RFID == null && p.Content != null).ToList());
        }
    }
}

