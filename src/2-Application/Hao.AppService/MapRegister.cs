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

            config.ForType<SysUser, UserVM>()
               .Map(x => x.GenderString, a => a.Gender.GetDescription())
               .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "启用" : "注销");

            config.ForType<SysUser, UserDetailVM>()
               .Map(x => x.GenderString, a => a.Gender.GetDescription())
               .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "启用" : "注销");

            config.ForType<SysUser, UserSecurityVM>()
               .Map(x => x.PasswordLevel, a => a.PasswordLevel.GetDescription())
               .Map(x => x.Phone, a => H_Util.HidePhoneNumber(a.Phone))
               .Map(x => x.Email, a => H_Util.HideEmailNumber(a.Email));

            config.ForType<UserQueryInput, UserQuery>()
               .Map(x => x.OrderByFileds, a => a.OrderByType.ToOrderByFields(a.SortField));
        }

        /// <summary>
        /// 模块
        /// </summary>
        private void MapModule(TypeAdapterConfig config)
        {
            config.ForType<SysModule, ModuleDetailVM>()
               .Map(x => x.Code, a => string.Format("{0}_{1}", a.Layer, a.Number));

            config.ForType<SysModule, ResourceItemVM>()
               .Map(x => x.ResourceCode, a => string.Format("{0}_{1}", a.Layer, a.Number));
        }
    }
}
