﻿using FluentValidation;
using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    public class UserAddRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int?  Age { get; set; }
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


    }

    public class UserAddValidator : AbstractValidator<UserAddRequest>
    {
        public UserAddValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空").Length(6, 16).WithMessage("密码长度应在6~16个字符");

            RuleFor(x => x.Gender).NotEmpty().WithMessage("性别不能为空").IsInEnum().WithMessage("性别数据有误");

        }
    }
}