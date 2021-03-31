using Hao.Redis;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.Core
{
    /// <summary>
    /// 防止重复提交过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AntiDuplicateRequestAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 锁类型
        /// </summary>
        public LockType Type { get; set; } = LockType.User;

        /// <summary>
        /// 全局锁业务标识
        /// </summary>
        public string GlobalLockKey { get; set; }

        /// <summary>
        /// 再次提交时间间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否自动解锁，默认false (true:时间间隔内请求处理完成，可以继续提交)
        /// </summary>
        public bool AutoUnLock { get; set; } = false;

        /// <summary>
        /// 执行
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isSuccess = false;

            var cacheKey = GetKey(context);

            try
            {
                isSuccess = Lock(cacheKey);

                if (!isSuccess)
                {
                    if (!string.IsNullOrWhiteSpace(Message)) throw new H_Exception(Message);
                    throw new H_Exception(GetFailMessage());
                }

                OnActionExecuting(context);

                var executedContext = await next();

                OnActionExecuted(executedContext);
            }
            finally
            {
                if (isSuccess && AutoUnLock) UnLock(cacheKey);
            }
        }


        /// <summary>
        /// 获取锁定标识
        /// </summary>
        protected virtual string GetKey(ActionExecutingContext context)
        {
            if (Type == LockType.User)
            {
                var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value;

                return $"{userId}{context.HttpContext.Request.Path}";
            }

            return GlobalLockKey;
        }

        /// <summary>
        /// 获取失败消息
        /// </summary>
        protected virtual string GetFailMessage()
        {
            if (Type == LockType.User) return "请不要重复提交";

            return "其他用户正在执行该操作,请稍后再试";
        }


        /// <summary>
        /// 锁定，成功锁定返回true，false代表之前已被锁定
        /// </summary>
        /// <param name="key">锁定标识</param>
        private bool Lock(string key)
        {
            if (RedisHelper.Exists(key)) return false;

            RedisHelper.Set(key, 1, Interval);

            return true;
        }

        /// <summary>
        /// 解除锁定
        /// </summary>
        private void UnLock(string key)
        {
            if (!RedisHelper.Exists(key)) return;

            RedisHelper.Del(key);
        }
    }

    /// <summary>
    /// 锁类型
    /// </summary>
    public enum LockType
    {
        /// <summary>
        /// 用户锁，当用户发出多个执行该操作的请求，只有第一个请求被执行，其它请求被抛弃，其它用户不受影响
        /// </summary>
        User = 0,

        /// <summary>
        /// 全局锁，该操作同时只有一个用户的请求被执行，结合全局锁使用
        /// </summary>
        Global = 1
    }
}
