using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Commons
{
    public class Common
    {
        private MySqlConnection Conn;
        private MySqlDataReader Dr;
        private MySqlCommand Com;
        private MySqlDataAdapter Adp;
        private DataSet Ds;
        private String strConnection;

        /// <summary>
        /// 初始化连接字符串
        /// </summary>
        public Common()
        {
            InitComm();
        }

        /// <summary>
        /// 初始化数据访问对象，用于多次使用
        /// </summary>
        public void InitComm()
        {
            strConnection = WebConfigurationManager.OpenWebConfiguration("~/").ConnectionStrings.
   ConnectionStrings["ConnectionString"].ConnectionString;
            Conn = new MySqlConnection(strConnection);           
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
            }
            catch
            {
                DbTools.MsgBox("数据库打开失败！");
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();               
                Conn.Dispose();
            }
        }

       /// <summary>
       /// 得到数据连接对象
       /// </summary>
       /// <returns></returns>
        public MySqlConnection GetConn() {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
            }
            catch
            {
                DbTools.MsgBox("数据库打开失败！");
            }
            return Conn;
        }

        /// <summary>
        /// 执行查询操作得到DataReader对象
        /// </summary>
        /// <param name="sql">sql命令</param>
        /// <returns>DataReader对象</returns>
        public MySqlDataReader GetReader(string sql)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Open();
            }
            Com = new MySqlCommand(sql, Conn);
            Dr = Com.ExecuteReader();
            Com.Dispose(); //释放Com占的资源
            return Dr;
        }

        /// <summary>
        /// 通过存储过程得到DataReader对象
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parms">存储过程参数数组</param>
        /// <returns>SqlDataReader对象，失败为null</returns>
        public MySqlDataReader GetReader(string spName, MySqlParameter[] parms)
        {
            try
            {
                Open();
                Com = new MySqlCommand();
                Com.Connection = Conn;
                Com.CommandType = CommandType.StoredProcedure;
                Com.CommandText = spName;
                for (int intCounter = 0; intCounter < parms.GetLength(0); intCounter++)
                {
                    Com.Parameters.Add(parms[intCounter]);
                }
                Dr = Com.ExecuteReader(CommandBehavior.CloseConnection);

                return Dr;
            }
            catch (MySqlException)
            {
                return null;
            }
            finally
            {
                Com.Parameters.Clear();
            }
        }

        /// <summary>
        /// 执行没有返回值的SQL命令
        /// </summary>
        /// <param name="sql">sql命令</param>
        /// <returns>执行结果：-1失败；其他：影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            int Result;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Com = new MySqlCommand(sql, Conn);
                Result = Com.ExecuteNonQuery();
            }
            catch
            {
                Result = -1;
            }
            finally
            {
                Com.Dispose();
                Close();
            }
            return Result;
        }

        /// <summary>
        /// 用于执行没有返回值的存储过程，包括Update,Insert,Delete
        /// </summary>
        /// <param name="pstrStoreProcedureName">存储过程名</param>
        /// <param name="pParams">存储过程的参数数组</param>
        /// <returns>执行结果：-1失败；其他：影响的行数</returns>
        public int ExecuteNonQuery(string pstrStoreProcedureName, MySqlParameter[] pParams)
        {
            int Result;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Com = new MySqlCommand();
                Com.Connection = Conn;
                Com.CommandType = CommandType.StoredProcedure;
                Com.CommandText = pstrStoreProcedureName;
                for (int intCounter = 0; intCounter < pParams.GetLength(0); intCounter++)
                {
                    Com.Parameters.Add(pParams[intCounter]);
                }
                Result = Com.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                return -1;
            }
            finally
            {
                Com.Parameters.Clear();
                Com.Dispose();
                Close();
                Conn.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 用于执行没有返回值的sql语句，包括Update,Insert,Delete
        /// </summary>
        /// <param name="cmdTxt">sql语句</param>
        /// <param name="i">与上一个函数区别的固化参数，永远设置为 1</param>
        /// <param name="pars">要传入的参数列表</param>
        /// <returns>执行结果：-1失败；其他：影响的行数</returns>
        public int ExecuteNonQuery(string cmdTxt, int i, params MySqlParameter[] pars)
        {
            int Result;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Com = new MySqlCommand(cmdTxt, Conn);
                if (pars != null)
                    Com.Parameters.AddRange(pars);
                Result = Com.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                return -1;
            }
            finally
            {
                Com.Parameters.Clear();
                Com.Dispose();
                Close();
                Conn.Dispose();
            }
            return Result;
        }

        /// <summary>
        /// 执行一个sql命令返回整形值。
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <param name="o">一般值为0，告知函数返回值为整形</param>
        /// <returns>sql命令执行后的单个整形值,执行失败返回-999</returns>
        public int ExecuteScalar(string sql, int o)
        {
            int line = 0;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Com = new MySqlCommand(sql, Conn);
                line = Convert.ToInt32(Com.ExecuteScalar());
            }
            catch
            {
                line = -999;
            }
            finally
            {
                Com.Dispose();
                Close();
            }
            return line;
        }

        /// <summary>
        /// 函数的重载
        /// 执行一个sql命令返回一个字符串的值。
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <param name="str">空串，标志返回值为字符型</param>
        /// <returns>SQL命令返回的单个字符串</returns>
        public String ExecuteScalar(string sql, string str)
        {
            string line = "";
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Com = new MySqlCommand(sql, Conn);
                line = Com.ExecuteScalar().ToString();
            }
            catch
            {
                line = "";
            }
            finally
            {
                Com.Dispose();
                Close();
            }
            return line;
        }

        /// <summary>
        /// 执行SQL命令得到DataSet对象
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <returns>DataSet对象</returns>
        public DataSet GetDataSet(string sql)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Open();
            }
            Adp = new MySqlDataAdapter(sql, Conn);
            Ds = new DataSet();
            Adp.Fill(Ds, "ds");
            Close();
            return Ds;
        }

        /// <summary>
        /// 执行SQL命令得到DataTable对象
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <returns>DataTable对象</returns>
        public DataTable GetDataTable(string sql)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                Ds = new DataSet();
                Com = new MySqlCommand(sql, Conn);
                Adp = new MySqlDataAdapter(Com);
                Adp.Fill(Ds);
                return Ds.Tables[0];

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region 执行查询语句，返回DataSet
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public  DataSet Query(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(strConnection))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        /// <summary>
        /// 返回从StartIndex开始的Count条记录，stcTable为表名填充到中DataSet中
        /// </summary>
        /// <param name="SQLText">SQL命令</param>
        /// <param name="srcTable">填充的表名</param>
        /// <param name="StartIndex">开始的记录号</param>
        /// <param name="Count">记录个数</param>
        /// <returns>DataSet对象</returns>
        public DataSet GetDataSet(string SQLText, string srcTable,
        int StartIndex, int Count)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Open();
            }
            Adp = new MySqlDataAdapter(SQLText, Conn);
            Ds = new DataSet();
            Adp.Fill(Ds, StartIndex, Count, srcTable);
            Close();
            return Ds;
        }

        /// <summary>
        /// 返回从StartIndex开始的Count条记录，“ds”为表名填充到中DataSet中
        /// </summary>
        /// <param name="SQLText">SQL命令</param>
        /// <param name="StartIndex">开始的记录号</param>
        /// <param name="Count">记录个数</param>
        /// <returns>DataSet对象</returns>
        public DataSet GetDataSet(string SQLText, int StartIndex, int Count)
        {
            return GetDataSet(SQLText, "ds", StartIndex, Count);
        }

        /// <summary>
        /// 调研存储过程得到DataSet
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parms">存储过程参数数组</param>
        /// <returns>Dataset对象，失败为null</returns>
        public DataSet GetDataSet(string spName, MySqlParameter[] parms)
        {
            try
            {
                Open();
                Adp = new MySqlDataAdapter(spName, Conn);
                Adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                for (int intCounter = 0; intCounter < parms.GetLength(0); intCounter++)
                {
                    Adp.SelectCommand.Parameters.Add(parms[intCounter]);
                }
                Ds = new DataSet();
                Adp.Fill(Ds, "ds");
                return Ds;
            }
            catch (MySqlException)
            {
                return null;
            }
            finally
            {
                Adp.SelectCommand.Parameters.Clear();
                Adp.Dispose();
                Close();
            }
        }

        /// <summary>
        /// 得到DataAdapter对象
        /// </summary>
        /// <param name="sql">Sql命令</param>
        /// <returns>DataAdapter对象</returns>
        public MySqlDataAdapter GetAdapter(string sql)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Open();
            }
            Adp = new MySqlDataAdapter(sql, Conn);
            Close();
            return Adp;
        }


        public DataSet Query(string sql, int Times)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Open();
                }
                MySqlDataAdapter command = new MySqlDataAdapter(sql, Conn);
                command.SelectCommand.CommandTimeout = Times;
                command.Fill(ds, "ds");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }


    }
}
