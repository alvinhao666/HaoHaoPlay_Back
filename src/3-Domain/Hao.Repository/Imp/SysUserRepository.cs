using Hao.Core;
using Hao.Model;
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

        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        public async Task UpdateAuth(long roleId, string authNumbers)
        {
            await UnitOfWork.GetDbClient()
                            .Updateable<SysUser>()
                            .UpdateColumns(a => new { a.AuthNumbers })
                            .ReSetValue(a => a.AuthNumbers == authNumbers)
                            .WhereColumns(it => new { it.RoleId })
                            .ExecuteCommandAsync();
        }

    }
}
