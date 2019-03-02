using DYFramework.Domain;
using DYFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mosaic.Repositories.Impl
{
    public class RelationRFIDQRCodeRepository : Repository<RelationRFIDQRCode>, IRelationRFIDQRCodeRepository
    {
        public RelationRFIDQRCodeRepository(IRepositoryContext context) : base(context)
        {
        }
       
    }
}
