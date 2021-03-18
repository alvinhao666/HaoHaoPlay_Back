using System;
using Hao.Core;
using Hao.Model;
using Hao.Utility;
using Mapster;

namespace Hao.AppService
{
    /// <summary>
    /// 注册模型映射
    /// </summary>
    public class MapRegister : IRegister
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="config"></param>
        public void Register(TypeAdapterConfig config)
        {
            MapUser(config);
            MapModule(config);
        }

        /// <summary>
        /// 用户
        /// </summary>
        private void MapUser(TypeAdapterConfig config)
        {

            config.ForType<SysUser, UserOutput>()
                .Map(x => x.GenderString, a => a.Gender.ToDescription())
                .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "有效" : "注销")
                .Map(x => x.Age, a => DateTime.Now.Year - a.Birthday.Value.Year);

            config.ForType<SysUser, UserDetailOutput>()
               .Map(x => x.GenderString, a => a.Gender.ToDescription())
               .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "有效" : "注销");

            config.ForType<SysUser, UserSecurityOutput>()
               .Map(x => x.PasswordLevel, a => a.PasswordLevel.ToDescription())
               .Map(x => x.Phone, a => H_Util.HidePhoneNumber(a.Phone))
               .Map(x => x.Email, a => H_Util.HideEmailNumber(a.Email));

            config.ForType<UserQueryInput, UserQuery>()
               .Map(x => x.OrderByConditions, a => a.SortFields.ToOrderByConditions(a.SortTypes));
        }

        /// <summary>
        /// 模块
        /// </summary>
        private void MapModule(TypeAdapterConfig config)
        {
            config.ForType<SysModule, ModuleDetailOutput>()
               .Map(x => x.Code, a => $"{a.Alias}_{a.Layer}_{a.Number}");

            config.ForType<SysModule, ResourceItemOutput>()
               .Map(x => x.ResourceCode, a => $"{a.ParentAlias}_{a.Alias}_{a.Layer}_{a.Number}");
        }
    }
}
