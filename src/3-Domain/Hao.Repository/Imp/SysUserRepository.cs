using Hao.Core;
using Hao.Model;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysUserRepository : Repository<SysUser,long>, ISysUserRepository
    {
        private readonly ISysLoginRecordRepository _loginRecordRep;

        public SysUserRepository(ISysLoginRecordRepository loginRecordRep)
        {
            _loginRecordRep = loginRecordRep;
        }

        /// <summary>
        /// 根据登录名密码查询用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<List<SysUser>> GetUserByLoginName(string loginName, string password)
        {
            string sql = "select * from  sysuser where loginname=@loginName and password=@password and isdeleted=false";

            List<SugarParameter> param = new List<SugarParameter>();
            param.Add(new SugarParameter("@loginName", loginName));
            param.Add(new SugarParameter("@password", password));

            return await Db.Ado.SqlQueryAsync<SysUser>(sql, param);
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        public async Task UpdateAuth(long roleId, string authNumbers)
        {

            string sql = "update  sysuser set authNumbers=@authNumbers where roleid=@roleid";

            List<SugarParameter> param = new List<SugarParameter>();
            param.Add(new SugarParameter("@authNumbers", authNumbers));
            param.Add(new SugarParameter("@roleid", roleId));

            await Db.Ado.ExecuteCommandAsync(sql, param);
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
