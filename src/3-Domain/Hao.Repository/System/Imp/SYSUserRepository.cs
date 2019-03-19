using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hao.Core.Repository;
using Hao.Core;
using Hao.Model;
using SqlSugar;
using Microsoft.Extensions.Configuration;

namespace Hao.Repository
{
    public class SYSUserRepository : Repository<SYSUser,long>, ISYSUserRepository
    {
        public SYSUserRepository(SqlSugarClient dbContext, ICurrentUser currentUser,IConfigurationRoot config) : base(dbContext, currentUser,config)
        {
        }
    }
}
