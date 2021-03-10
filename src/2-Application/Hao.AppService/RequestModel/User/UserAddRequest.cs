using FluentValidation;
using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 添加用户请求
    /// </summary>
    public class UserAddRequest
    {
        
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
             
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public long? RoleId { get; set; }
    }


    /// <summary>
    /// 验证
    /// </summary>
    public class UserAddValidator : AbstractValidator<UserAddRequest>
    {
        public UserAddValidator()
        {
            RuleFor(x => x.Account).MustHasValue("账号").Must(a => H_Validator.IsLetterOrDigit(a)).WithMessage("只能输入英文或者数字");
            
            RuleFor(x => x.Password).MustFixedLength("密码", 6, 16);

            RuleFor(x => x.Name).MustHasValue("姓名");
            
            RuleFor(x => x.Gender).EnumMustHasValue("性别");

            RuleFor(x => x.Birthday).MustHasValue("出生日期");

            RuleFor(x => x.Phone).MustHasValue("手机");

            RuleFor(x => x.RoleId).MustHasValue("角色Id");
            
            //RuleFor(x => x.Data).SetCollectionValidator(new BoxOrderItemVMValidator()); // 集合子项数据验证
        }
    }
}
