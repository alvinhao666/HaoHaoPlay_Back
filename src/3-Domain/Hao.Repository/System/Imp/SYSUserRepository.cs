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
