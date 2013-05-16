using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI;
using System.Net.Mail;
using System.Net;
using System.Web;


namespace Commons
{
    public class DbTools
    {
        public DbTools()
        {

        }

        /// <summary>
        /// JS消息对话框
        /// </summary>
        /// <param name="strMess">消息内容</param>
        public static void MsgBox(string strMess)
        {
            string str = String.Format("<script language='javascript'>alert('{0}');</script>", strMess);
            HttpContext.Current.Response.Write(str);
        }

        /// <summary>
        /// JS消息对话框
        /// </summary>
        /// <param name="strMess">消息内容</param>
        public static void MsgBox2(string strMess, Page pPage)
        {
            string str = String.Format("alert('{0}');", strMess);
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }

        /// <summary>
        /// 关闭浏览器窗口
        /// </summary>
        public static void CloseWeb()
        {
            string str = "<script language='javascript'>opener=null; window.close();</script>";
            HttpContext.Current.Response.Write(str);
        }

        /// <summary>
        /// 关闭浏览器窗口
        /// </summary>
        public static void CloseWeb2(Page pPage)
        {
            string str = "opener=null; window.close();";
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }

        /// <summary>
        /// 显示JS消息并后退。
        /// </summary>
        /// <param name="strMess">消息内容</param>
        public static void MsgAndBack(string strMess)
        {
            string str = String.Format("<script language='javascript'>alert('{0}');history.go(-1);</script>", strMess);
            HttpContext.Current.Response.Write(str);
        }

        // <summary>
        /// 显示JS消息并后退。
        /// </summary>
        /// <param name="strMess">消息内容</param>
        public static void MsgAndBack2(string strMess, Page pPage)
        {
            string str = String.Format("alert('{0}');history.go(-1);", strMess);
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }

        /// <summary>
        /// 显示消息并跳转
        /// </summary>
        /// <param name="strMess">消息内容</param>
        /// <param name="strUrl">跳转地址</param>
        public static void MsgAndRedirect(string strMess, string strUrl)
        {
            string str = String.Format("<script language='javascript'>alert('{0}');location.href='{1}';</script>", strMess, strUrl);
            HttpContext.Current.Response.Write(str);
        }

        /// <summary>
        /// 显示消息并跳转
        /// </summary>
        /// <param name="strMess">消息内容</param>
        /// <param name="strUrl">跳转地址</param>
        public static void MsgAndRedirect2(string strMess, string strUrl, Page pPage)
        {
            string str = String.Format("alert('{0}');location.href='{1}';", strMess, strUrl);
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }

        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="strUrl">跳转地址</param>
        public static void Redirect(string strUrl, Page pPage)
        {
            string str = String.Format("location.href='{0}';",strUrl);
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }

        /// <summary>
        /// 显示消息并跳出框架跳转
        /// </summary>
        /// <param name="strMess">消息内容</param>
        /// <param name="strUrl">跳转地址</param>
        public static void MsgAndParentRedirect(string strMess, string strUrl)
        {
            string str = String.Format("<script language='javascript'>alert('{0}');parent.location.href='{1}';</script>", strMess, strUrl);
            HttpContext.Current.Response.Write(str);
        }

        /// <summary>
        /// 显示消息并跳出框架跳转
        /// </summary>
        /// <param name="strMess">消息内容</param>
        /// <param name="strUrl">跳转地址</param>
        public static void MsgAndParentRedirect2(string strMess, string strUrl, Page pPage)
        {
            string str = String.Format("alert('{0}');parent.location.href='{1}';", strMess, strUrl);
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + GenerateFileName(), str, true);
        }


        /// <summary>
        /// 用MD5对密码加密
        /// </summary>
        /// <param name="pSeed">原字符串</param>
        /// <returns></returns>
        public static string Md5(string pSeed)
        {
            return (FormsAuthentication.HashPasswordForStoringInConfigFile(pSeed, "MD5"));
        }
        /// <summary>
        /// 删除指定路径的文件
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>删除是否成功</returns>
        public  bool DeleteFile(string Path)
        {
            if (System.IO.File.Exists(Path))
                try
                {
                    System.IO.File.Delete(Path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            else
                return true;
        }
        /// <summary>
        /// 在指定路径创建目录
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>创建是否成功</returns>
        public static bool CreateDirectory(string Path)
        {
            if (System.IO.Directory.Exists(Path))
                return true;
            else
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 以时间为种子生成文件名称
        /// </summary>
        /// <returns></returns>
        public static string GenerateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHmmssffff");
        }
        /// <summary>
        /// 注册提示客户端脚本
        /// </summary>
        /// <param name="pScript">脚本</param>
        /// <param name="pKey">键值</param>
        public static void RegisterAlertScript(string pMessage, string pNavigateTo, string pKey, Page pPage)
        {
            string Script = @"alert('" + pMessage + @"');window.navigate('" + pNavigateTo + @"');";
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + pKey,
            Script, true);
        }
        /// <summary>
        /// 注册提示客户端脚本
        /// </summary>
        /// <param name="pScript">脚本</param>
        /// <param name="pKey">键值</param>
        public static void RegisterConfirmScript(string pMessage, string pYesNavigateTo, string pNoNavigateTo, string pKey, Page pPage)
        {
            string Script = @"if(confirm('" + pMessage + @"'))
                      window.navigate('" + pYesNavigateTo + @"');
                    else
                      window.navigate('" + pNoNavigateTo + @"')";
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + pKey,
            Script, true);
        }
        /// <summary>
        /// 注册提示客户端脚本，并返回上一步
        /// </summary>
        /// <param name="pScript">脚本</param>
        /// <param name="pKey">键值</param>
        public static void RegisterAlertAndBackScript(string pMessage, string pKey, Page pPage)
        {
            string Script = @"alert('" + pMessage + @"');history.back();";
            pPage.ClientScript.RegisterClientScriptBlock(pPage.GetType(), pPage.UniqueID + pKey,
            Script, true);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="pTitle">邮件标题</param>
        /// <param name="pBody">邮件内容</param>
        /// <param name="pSendTo">邮件接收者</param>
        /// <param name="pSendFrom">邮件发送者</param>
        /// <param name="pSMTP">邮件SMTP地址</param>
        /// <param name="pSMTPUser">发送邮件用户名</param>
        /// <param name="pSMTPPassword">发送邮件密码</param>
        public static string SendEmail(string pTitle, string pBody, string pSendTo, string pSendFrom,
         string pSMTP, string pSMTPUser, string pSMTPPassword)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = pSMTP;
            smtp.Credentials = new NetworkCredential(pSMTPUser, pSMTPPassword);

            MailMessage msg = new MailMessage();
            msg.Body = pBody;
            msg.From = new MailAddress(pSendFrom);
            msg.To.Add(pSendTo);
            msg.IsBodyHtml = true;
            msg.Subject = pTitle;

            try
            {
                smtp.Send(msg);
            }
            catch (SmtpException e)
            {
                string str = e.StatusCode.ToString();
                return e.StatusCode.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "success";
        }
        /// <summary>
        /// 将文件上传到指定路径
        /// </summary>
        /// <param name="fld">文件上传控件</param>
        /// <param name="Path">文件保存的物理路径</param>
        /// <returns>保存的文件名</returns>
        public static string UploadFileToServer(FileUpload fld, string Path)
        {
            string returnName = GenerateFileName() + System.IO.Path.GetExtension(fld.FileName);
            if (CreateDirectory(Path))
            {
                string fileName = Path + @"/" + returnName;
                fld.SaveAs(fileName);
            }
            else
            {
                returnName = string.Empty;
            }
            return returnName;
        }

        #region 绑定到下拉列表函数原型
        /// <summary>
        /// 用来将数据绑定到GridView的FooterRow中或DataRow中的下拉列表，绑定到FooterRow时，该方法一般放在使FooterRow显示的事件中，绑定到DataRow时，该方法一般放在GridView的RowDataBound事件中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ddl">下拉列表名</param>
        /// <param name="ddlSelectedItemText">DropDownList选定项的文本</param>
        /// <param name="ddlSelectedItemValue">DropDownList选定项的值</param>
        /// <param name="condition">条件字符串</param>
        /// <param name="IsInsert0">是否插入值为0的行,真插入</param>
        public static void BindtoDropDownList(string tableName, DropDownList ddl, string ddlSelectedItemText, string ddlSelectedItemValue, string condition, string orderByCondition, bool IsInsert0)
        {
            Common common = new Common();
            string sql = "select * from {0} where {1} order by {2} ";
            sql = string.Format(sql, tableName, condition, orderByCondition);
            DataSet ds = common.GetDataSet(sql);
            ddl.DataSource = ds.Tables[0].DefaultView;
            ddl.DataTextField = ddlSelectedItemText;
            ddl.DataValueField = ddlSelectedItemValue;
            ddl.DataBind();
            if (IsInsert0)
                ddl.Items.Insert(0, new ListItem("--请选择--", "0"));
        }
        #endregion

        #region 绑定从表中查询到的数据到Repeater
        /// <summary>
        /// 绑定从表中查询到的数据到Repeater
        /// </summary>
        /// <param name="tableName">被绑定数据的来源表的表名</param>
        /// <param name="whereCondition">执行查询的where条件</param>
        /// <param name="orderByCondition">查询结果的排序条件</param>
        /// <param name="repeater">要绑定数据的Repeater</param>
        public static void RepeaterBind(string tableName, string whereCondition, string orderByCondition, Repeater repeater)
        {
            string sql = "select * from {0} where {1} order by {2}";
            sql = string.Format(sql, tableName, whereCondition, orderByCondition);
            Common comm = new Common();
            DataSet ds = comm.GetDataSet(sql);
            repeater.DataSource = ds.Tables[0].DefaultView;
            repeater.DataBind();
        }

        /// <summary>
        /// 绑定数据表中的前几条记录到Repeater
        /// </summary>
        /// <param name="tableName">被绑定数据的来源表的表名</param>
        /// <param name="whereCondition">执行查询的where条件</param>
        /// <param name="orderByCondition">查询结果的排序条件</param>
        /// <param name="repeater">要绑定数据的Repeater</param>
        /// <param name="TopCount">绑定的记录个数</param>
        public static void RepeaterBind(string tableName, string whereCondition, string orderByCondition, Repeater repeater, int TopCount)
        {
            string sql = "select * from {0} where {1} order by {2} Limit {3}";
            sql = string.Format(sql, tableName, whereCondition, orderByCondition, TopCount);
            Common comm = new Common();
            DataSet ds = comm.GetDataSet(sql);
            repeater.DataSource = ds.Tables[0].DefaultView;
            repeater.DataBind();
        }

        #endregion

        #region 根据id获取对应的中文名，此id为指定的表中主键字段对应的一个具体值
        /// <summary>
        /// 根据id获取对应的中文名，此id为指定的表中主键字段对应的一个具体值
        /// </summary>
        /// <param name="id">中文名对应的id的某个具体值</param>
        /// <param name="tableName">具有id与中文名对应关系的表名</param>
        /// <param name="tableRegion">id在表中对应的字段</param>
        /// <param name="chineseName">中文名在表中对应的字段</param>
        /// <returns></returns>
        public static string GetIdChineseName(int id, string tableName, string tableRegion, string chineseName)
        {
            string sql = "select * from " + tableName + " where " + tableRegion + " = " + id;
            Common common = new Common();
            DataSet ds = common.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                return dr[chineseName].ToString();
            }
            else
                return "";
        }
        #endregion

        #region 根据汉字获得对应的拼音
        /// <summary>
        /// 通过汉字得到拼音首字母
        /// </summary>
        /// <param name="text">汉字字符串</param>
        /// <returns>拼音首字母字符串</returns>
        public static string GetPinYin(string text)
        {
            char pinyin;
            byte[] array;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                pinyin = c;
                array = System.Text.Encoding.Default.GetBytes(new char[] { c });

                if (array.Length == 2)
                {
                    int i = array[0] * 0x100 + array[1];

                    if (i < 0xB0A1) pinyin = c;
                    else
                        if (i < 0xB0C5) pinyin = 'a';
                        else
                            if (i < 0xB2C1) pinyin = 'b';
                            else
                                if (i < 0xB4EE) pinyin = 'c';
                                else
                                    if (i < 0xB6EA) pinyin = 'd';
                                    else
                                        if (i < 0xB7A2) pinyin = 'e';
                                        else
                                            if (i < 0xB8C1) pinyin = 'f';
                                            else
                                                if (i < 0xB9FE) pinyin = 'g';
                                                else
                                                    if (i < 0xBBF7) pinyin = 'h';
                                                    else
                                                        if (i < 0xBFA6) pinyin = 'j';
                                                        else
                                                            if (i < 0xC0AC) pinyin = 'k';
                                                            else
                                                                if (i < 0xC2E8) pinyin = 'l';
                                                                else
                                                                    if (i < 0xC4C3) pinyin = 'm';
                                                                    else
                                                                        if (i < 0xC5B6) pinyin = 'n';
                                                                        else
                                                                            if (i < 0xC5BE) pinyin = 'o';
                                                                            else
                                                                                if (i < 0xC6DA) pinyin = 'p';
                                                                                else
                                                                                    if (i < 0xC8BB) pinyin = 'q';
                                                                                    else
                                                                                        if (i < 0xC8F6) pinyin = 'r';
                                                                                        else
                                                                                            if (i < 0xCBFA) pinyin = 's';
                                                                                            else
                                                                                                if (i < 0xCDDA) pinyin = 't';
                                                                                                else
                                                                                                    if (i < 0xCEF4) pinyin = 'w';
                                                                                                    else
                                                                                                        if (i < 0xD1B9) pinyin = 'x';
                                                                                                        else
                                                                                                            if (i < 0xD4D1) pinyin = 'y';
                                                                                                            else
                                                                                                                if (i < 0xD7FA) pinyin = 'z';
                }

                sb.Append(pinyin);
            }

            return sb.ToString();
        }
        #endregion

        #region 把表中ID转化为名称
        /// <summary>
        /// 把表中ID转化为名称
        /// </summary>
        /// <param name="Id">ID值</param>
        /// <param name="selfield">名称字段</param>
        /// <param name="table">表名</param>
        /// <param name="consel">ID字段名称</param>
        /// <returns></returns>
        public static string TransFieldFromAnotherTable(Int32 Id, string selfield, string table, string consel)
        {
            Common cmn = new Common();
            string sql = "select {0} from {1} where {2}={3}";
            sql = string.Format(sql, selfield, table, consel, Id);
            return cmn.ExecuteScalar(sql, "");
        }
        /// <summary>
        /// 把ID转化为名称
        /// </summary>
        /// <param name="str">名称值</param>
        /// <param name="selfield">显示的字段</param>
        /// <param name="table">表名</param>
        /// <param name="consel">条件字段</param>
        /// <returns></returns>
        public static Int32 TransFieldFromAnotherTable(string str, string selfield, string table, string consel)
        {
            Common cmn = new Common();
            string sql = "select {0} from {1} where {2}='{3}'";
            sql = string.Format(sql, selfield, table, consel, str);
            return cmn.ExecuteScalar(sql, 0);
        } 
        #endregion


        #region 通过Sql得到DataTable
        public static DataTable GetDataTable(string Sql)
        {
            Common comm = new Common();
            DataTable dt = comm.GetDataSet(Sql).Tables[0];
            return dt;
        }
        #endregion

        #region 得到截取的新闻标题
        /// <summary>
        /// 得到截取的新闻标题
        /// </summary>
        /// <param name="newsTitle"></param>
        /// <param name="titleLeng">标题长度</param>
        /// <returns></returns>
        public static string GetShortTitle(string newsTitle, int titleLeng)
        {
            if (newsTitle.Length > titleLeng)
            {
                newsTitle = newsTitle.Substring(0, titleLeng) + "...";
            }
            return newsTitle;
        }
        #endregion

        #region 存文本转化为HTML
        public static string TxtToHtml(string strTM)
        {
            if (strTM.IndexOf("&nbsp;") == -1 && strTM.IndexOf("<BR>") == -1 && strTM.IndexOf("<P>") == -1)
            {
                strTM = strTM.Replace("<", " &lt; ");
                strTM = strTM.Replace(">", " &gt; ");
                strTM = strTM.Replace(" ", "&nbsp;");
                strTM = strTM.Replace("\n", " <BR> ");
            }
            return strTM;
        }
        #endregion



    }
}
