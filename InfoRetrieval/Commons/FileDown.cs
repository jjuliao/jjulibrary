using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;

namespace Commons
{
    /// <summary>
    /// 文件下载类
    /// </summary>
    public class FileDown
    {
        public FileDown()
        { }

        /// <summary>
        /// 参数为虚拟路径
        /// </summary>
        public  string FileNameExtension(string FileName)
        {
            return Path.GetExtension(MapPathFile(FileName));
        }

        /// <summary>
        /// 获取物理地址
        /// </summary>
        public  string MapPathFile(string FileName)
        {
            return System.Web.HttpContext.Current.Server.MapPath(FileName);
        }

        /// <summary>
        /// WriteFile普通下载
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="FilePath">文件虚拟路径</param>
        public  void NormalDownLoad(string FileName, string FilePath)
        {
            ////String destFileName = System.Web.HttpContext.Current.Server.MapPath("");
            ////destFileName = destFileName + "\\" + "upfiles\\" + FileName;
            string destFileName = MapPathFile(FilePath) + FileName;
            if (File.Exists(destFileName))
            {
                FileInfo fi = new FileInfo(destFileName);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(destFileName), System.Text.Encoding.UTF8));
                HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(destFileName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// WriteFile分块下载
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="FilePath">文件虚拟路径</param>
        public  void DevideDownLoad(string FileName, string FilePath)
        {
            string filePath = MapPathFile(FilePath) + FileName;
            long chunkSize = 204800;             //指定块大小 
            byte[] buffer = new byte[chunkSize]; //建立一个200K的缓冲区 
            long dataToRead = 0;                 //已读的字节数   
            FileStream stream = null;
            try
            {
                // 打开文件   
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = stream.Length; // 获取下载的文件总大小

                //添加Http头   
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        dataToRead = -1; //防止client失去连接 
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
                HttpContext.Current.Response.Close();
            }
        }

        /// <summary>
        /// 流方式下载
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="FilePath">文件虚拟路径</param>
        public  void StreamDownLoad(string FileName, string FilePath)
        {

            string filePath = MapPathFile(FilePath) + FileName; // 客户端保存的文件路径
           
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

        }

        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="_Request">Page.Request对象</param>
        /// <param name="_Response">Page.Response对象</param>
        /// <param name="_fileName">下载文件名</param>
        /// <param name="_fullPath">带文件名下载路径</param>
        /// <param name="_speed">每秒允许下载的字节数</param>
        /// <returns>返回是否成功</returns>
        //---------------------------------------------------------------------
        //调用：
        //string FullPath=Server.MapPath("upfiles/"+ FileName);
        //ResponseFile(this.Request,this.Response,FileName,FullPath,500000);
        //---------------------------------------------------------------------
        public  bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    //
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;

                    long fileLength = myFile.Length;
                    long startBytes = 0;
                    int pack = 10240;  //10K bytes
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;

                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }

                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
