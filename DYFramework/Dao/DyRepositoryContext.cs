using DYFramework.Domain;
using DYFramework.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYFramework.Dao
{
    public class DyRepositoryContext : RepositoryContext, IRepositoryContext
    {
        public DyRepositoryContext(DyContext context) : base(context)
        {
        }
    }
}
