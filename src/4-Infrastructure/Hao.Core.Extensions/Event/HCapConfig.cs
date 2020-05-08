using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Extensions
{
    public class HCapConfig
    {
        public string PostgreSqlConnection { get; set; }

        public string HostName { get; set; }

        public string VirtualHost { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
