using Aspose.Words;
using System;
using System.Data;

namespace Hao.Word
{
    /// <summary>
    /// word文档操作辅助类
    /// </summary>
    public class AsposeWordHelper
    {
        /// <summary>
        /// Word
        /// </summary>
        private Document wordDoc;

        /// <summary>
        /// 基于模版新建Word文件
        /// </summary>
        /// <param name="path">模板路径</param>
        public void OpenTempelte(string path)
        {

            wordDoc = new Document(path);
        }

        /// <summary>
        /// 书签赋值用法
        /// </summary>
        /// <param name="LabelId">书签名</param>
        /// <param name="Content">内容</param>
        public void WriteBookMark(string LabelId, string Content)
        {
            if (wordDoc.Range.Bookmarks[LabelId] != null)
            {
                wordDoc.Range.Bookmarks[LabelId].Text = Content;
            }
        }

        /// <summary>
        /// 列表赋值用法
        /// </summary>
        /// <param name="dt"></param>
        public void WriteTable(DataTable dt)
        {
            wordDoc.MailMerge.ExecuteWithRegions(dt);
        }

        /// <summary>
        /// 文本域赋值用法
        /// </summary>
        /// <param name="fieldNames">key</param>
        /// <param name="fieldValues">value</param>
        public void Executefield(string[] fieldNames, object[] fieldValues)
        {
            wordDoc.MailMerge.Execute(fieldNames, fieldValues);
        }

        /// <summary>
        /// Pdf文件保存
        /// </summary>
        /// <param name="filename">文件路径+文件名</param>
        public void SavePdf(string filename)
        {
            wordDoc.Save(filename, SaveFormat.Pdf);
        }

        /// <summary>
        /// Doc文件保存
        /// </summary>
        /// <param name="filename">文件路径+文件名</param>
        public void SaveDoc(string filename)
        {
            wordDoc.Save(filename, SaveFormat.Doc);
        }

        /// <summary>
        /// 不可编辑受保护，需输入密码
        /// </summary>
        /// <param name="pwd">密码</param>
        public void NoEdit(string pwd)
        {
            wordDoc.Protect(ProtectionType.ReadOnly, pwd);
        }

        /// <summary>
        /// 只读
        /// </summary>
        public void ReadOnly()
        {
            wordDoc.Protect(ProtectionType.ReadOnly);
        }
    }
}
