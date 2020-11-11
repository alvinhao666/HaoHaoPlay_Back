using Hao.Core;
using Hao.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysUserRepository : Repository<SysUser, long>, ISysUserRepository
    {

        /// <summary>
        /// 根据登录名密码查询用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<List<SysUser>> GetUserByLoginName(string loginName, string password)
        {
            //string sql = "select * from  sysuser where loginname=@loginname and password=@password and isdeleted=false";
            //var param = new List<SugarParameter>();
            //param.Add(new SugarParameter("loginname", loginName));
            //param.Add(new SugarParameter("password", password));
            //var result = await Db.Ado.SqlQueryAsync<SysUser>(sql, param);

            var result = await DbContext.Queryable<SysUser>()
                                 .Where(a => a.LoginName == loginName && a.Password == password && a.IsDeleted == false)
                                 .ToListAsync();

            return result;
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        public async Task UpdateAuth(long roleId, string authNumbers)
        {
            //string sql = "update  sysuser set authnumbers=@authnumbers where roleid=@roleid";
            //var param = new List<SugarParameter>();
            //param.Add(new SugarParameter("authnumbers", authNumbers));
            //param.Add(new SugarParameter("roleid", roleId));
            //await Db.Ado.ExecuteCommandAsync(sql, param);

            await DbContext.Updateable<SysUser>()
                    .SetColumns(a => new SysUser { AuthNumbers = authNumbers })
                    .Where(a => a.RoleId == roleId && a.IsDeleted == false)
                    .ExecuteCommandAsync();
        }



        //public override async Task<PagedList<Student>> GetPagedListAysnc(Query<Student> query)
        //{
        //    int totalNumber = 0;
        //    bool flag = string.IsNullOrWhiteSpace(query.OrderFileds);

        //    List<Student> items = await Task.Factory.StartNew(() => _db.Queryable<Student, Class>((st, sc) => new object[] {
        //        JoinType.Left,st.ClassCode==sc.ClassCode})
        //        .Select((st, sc) => new Student()
        //        {
        //            Id = st.Id,
        //            Code = st.Code,
        //            Name = st.Name,
        //            Gender = st.Gender,
        //            Mobile = st.Mobile,
        //            Status = st.Status,
        //            ClassCode = st.ClassCode,
        //            CreateTime = st.CreateTime,
        //            CreateUserId = st.CreateUserId,
        //            CreateUserName = st.CreateUserName,
        //            ModifyTime = st.ModifyTime,
        //            ModifyUserId = st.ModifyUserId,
        //            ModifyUserName = st.ModifyUserName,
        //            ClassInfo = sc ?? null
        //        })
        //        .Where(query.QuerySql)
        //        .OrderByIF(flag, st => st.Id, OrderByType.Desc)
        //        .OrderByIF(!flag, query.OrderFileds)
        //        .ToPageList(query.PageIndex.Value, query.PageSize.Value, ref totalNumber));



        //    PagedList<Student> pageList = new PagedList<Student>()
        //    {
        //        Items = items,
        //        TotalCount = totalNumber,
        //        PageIndex = query.PageIndex.Value,
        //        PageSize = query.PageSize.Value,
        //        TotalPagesCount = (totalNumber + query.PageSize.Value - 1) / query.PageSize.Value
        //    };
        //    return pageList;
        //}

    }
}
