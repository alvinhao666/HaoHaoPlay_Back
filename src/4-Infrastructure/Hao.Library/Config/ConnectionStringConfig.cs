using System.Collections.Generic;

namespace Hao.Library
{
    public class ConnectionStringConfig
    {
        public string MySql_Master { get; set; }

        public List<Sql_SlaveConfig> MySql_Slave { get; set; }

        public string PostgreSql_Master { get; set; }

        public List<Sql_SlaveConfig> PostgreSql_Slave { get; set; }

        public string Redis { get; set; }

        public string SqlServer { get; set; }
    }

    public class Sql_SlaveConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }
    }
}
