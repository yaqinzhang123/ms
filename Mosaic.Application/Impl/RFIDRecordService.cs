using AutoMapper;
using DYFramework.Application;
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
    public class RFIDRecordService : Service<RFIDRecordDataObject, RFIDRecord>, IRFIDRecordService
    {
        public RFIDRecordService(IRFIDRecordRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public int AddList(IList<RFIDRecordDataObject> rfidList)
        {
            var list = this.mapper.Map<IList<RFIDRecordDataObject>, IList<RFIDRecord>>(rfidList);
            this.repository.AddList(list);
            return this.repository.Commit();
        }

        public List<int> GetProductionLineIDList()
        {
            var ids = this.repository.Context.GetReadEntity<RFIDRecord>()
                    .GroupBy(p => p.LineID).Select(p => p.Key).ToList();
            return ids;
        }
        public IList<RFIDRecordDataObject> GetUnhandleListByProduction(int lineId)
        {
            var list = this.repository.Context.GetReadEntity<RFIDRecord>()
                .Where(p => p.LineID == lineId && !p.Flag).ToList();
            return this.mapper.Map<IList<RFIDRecord>, IList<RFIDRecordDataObject>>(list);
        }
    }
}
