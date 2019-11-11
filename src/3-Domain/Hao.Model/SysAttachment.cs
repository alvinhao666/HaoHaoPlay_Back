using Hao.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    [SugarTable("SysAttachment")]
    public class SysAttachment : Entity<Guid>
    {
        public new Guid? Id { get; set; }

        public string BindTableId { get; set; }

        public string BindTableName { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
