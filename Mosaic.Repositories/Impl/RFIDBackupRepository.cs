using DYFramework.Domain;
using DYFramework.Repository;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.Impl
{
    public class RFIDBackupRepository : Repository<RFIDBackup>, IRFIDBackupRepository
    {
        public RFIDBackupRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}
