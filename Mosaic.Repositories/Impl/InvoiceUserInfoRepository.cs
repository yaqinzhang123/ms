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
    public class InvoiceUserInfoRepository:Repository<InvoiceUserInfo>, IInvoiceUserInfoRepository
    {
        public InvoiceUserInfoRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<InvoiceUserInfo> Get(Expression<Func<InvoiceUserInfo, bool>> expression)
        {
            return base.Get(expression);
        }
    }
}
