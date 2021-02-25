using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Hao.Snowflake
{
    public class SnowflakeBackgroundServices : BackgroundService
    {
        private readonly IDistributedSupport _distributed;

        private readonly SnowflakeOptions option;

        public SnowflakeBackgroundServices(IDistributedSupport distributed, IOptions<SnowflakeOptions> options)
        {
            option = options.Value;
            _distributed = distributed;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                while (true)
                {
                    //定时刷新机器id的存活状态
                    await _distributed.RefreshAlive();
                    await Task.Delay(option.RefreshAliveInterval.Add(TimeSpan.FromMinutes(1)), stoppingToken);
                }
            }
        }
    }
}