using Hao.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    [SugarTable("SYSAttachment")]
    public class SYSAttachment:Entity<Guid>
    {
        public new Guid? ID { get; set; }

        public string BindTableID { get; set; }

        public string BindTableName { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
