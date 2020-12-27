using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    //数据传输对象（DTO)(Data Transfer Object)，是一种设计模式之间传输数据的软件应用系统。
    //数据传输目标往往是 数据访问对象从数据库中 检索数据。数据传输对象 与数据交互对象或数据访问对象之间的差异是一个以不具有任何行为   除了存储和检索的数据（访问和存取器）。
    public class ModuleLayerCountDTO
    {
        /// <summary>
        /// 层
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Count { get; set; }
    }
}
