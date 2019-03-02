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
    public class RoleService: Service<RoleDataObject, Role>, IRoleService
    {
        public RoleService(IRoleRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public IList<RoleDataObject> GetListByCompanyID(int id)
        {
            IList<RoleDataObject> roleList = Mapper.Map<IList<Role>, IList<RoleDataObject>>(this.repository.Get(p => p.CompanyID == id&&!p.Deleted).ToList());
            if (roleList == null)
                return roleList;
            IList<RoleDataObject> roles = new List<RoleDataObject>();
            for(int i = 0; i < roleList.Count(); i++)
            {
                roles.Add(this.Get(roleList[i].ID));
            }
            return roles;
        }
        public IList<RoleDataObject> RoleQuery(string name, int id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return this.GetListByCompanyID(id);
            }
            var query = this.repository.Get(p => p.CompanyID == id).ToList();
            query = query.Where(p => p.Name.Contains(name.Trim())).ToList();
            return Mapper.Map<IList<Role>, IList<RoleDataObject>>(query);
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p => p.Name == name);
        }

        public RoleDataObject Get(int id)
        {
            RoleDataObject role = this.GetByID(id);
            IList<RightsDataObject> rightsList= Mapper.Map<IList<Rights>, IList<RightsDataObject>>(this.repository.Context.Get<Rights>(p => !p.Delete && p.RoleID == id).ToList());
            role.RightsList=rightsList;
            return role;
        }
    }
}
