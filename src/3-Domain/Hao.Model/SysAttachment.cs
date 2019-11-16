using Hao.Core;
using SqlSugar;

namespace Hao.Model
{
    [SugarTable("SysAttachment")]
    public class SysAttachment : FullAuditedEntity<long>
    {
        public string BindTableId { get; set; }

        public string BindTableName { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
