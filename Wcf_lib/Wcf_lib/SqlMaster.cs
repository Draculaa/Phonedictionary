using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace Wcf_lib
{
    class SqlMaster
    {
        private SQLiteTransaction G_Transaction;
        private SQLiteConnection G_connection;
        private string G_path;

        public SqlMaster()
        {
            Init();
        }
       
//-------------------------------------------------------------------------------------->
        public void Init()
        {

            G_path = "..\\..\\database.db";
            
        }
//-------------------------------------------------------------------------------------->
        public void SetPath(string path)
        {
           G_path = path;
        }
//-------------------------------------------------------------------------------------->
        public string GetPath()
        {
            return G_path;
        }
//-------------------------------------------------------------------------------------->
        public void OpenConnection()
        {
            G_connection = new SQLiteConnection("Data Source=" + GetPath() + ";Version=3;");
            G_connection.Open();

        }
//-------------------------------------------------------------------------------------->
        public void CloseConnection()
        {
            G_connection.Close();
            G_connection.Dispose();
            
        }
//-------------------------------------------------------------------------------------->
        public void BeginTransaction()
        {
            G_connection = new SQLiteConnection("Data Source=" + GetPath() + ";Version=3;");
            G_connection.Open();
            G_Transaction = G_connection.BeginTransaction();
        }
//-------------------------------------------------------------------------------------->
        public void EndTransaction()
        {
            G_Transaction.Commit();
            G_Transaction.Dispose();
            G_connection.Close();
        }

//-------------------------------------------------------------------------------------->
        public void WriteToDb(string[] ary, string id_experiment)
        {
            string sql = "INSERT INTO data (id_experiment, time, P0, P1, P2, P3, P4, P5, P6, P7, P8) ";
            sql += string.Format("VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                id_experiment, ary[9], ary[0], ary[1], ary[2], ary[3], ary[4], ary[5], ary[6], ary[7], ary[8]);
            try
            {
                SQLiteCommand command = G_connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                
            }
        }
//-------------------------------------------------------------------------------------->
        public void WriteToDbFull(DataTable table, string id_experiment)
        {
            BeginTransaction();
            string[] ary = new string[10];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    ary[j] = table.Rows[i][j].ToString();

                }
                WriteToDb(ary, id_experiment);
            }
            EndTransaction();
        }
//-------------------------------------------------------------------------------------->
        public int GetLastExperiment()
        {
            OpenConnection();
            int count = 0;
            string sql = "SELECT max(ID) FROM EXPERIMENT";
            SQLiteCommand command = G_connection.CreateCommand();
            command.CommandText = sql;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader[0].ToString());
            }
            CloseConnection();
            //return count;
            return 0;
            
        }

//-------------------------------------------------------------------------------------->
             public void CreateTableStandSettings() {
                 string sql = "CREATE TABLE stand_settings (\"ID\" INTEGER PRIMARY KEY, \"name\" TEXT, \"point\" INTEGER, \"type\" TEXT);";
                 SQLiteCommand command = new SQLiteCommand(sql, G_connection);
                 command.ExecuteNonQuery();
             }

//-------------------------------------------------------------------------------------->
             public void ExecuteNonQuery(string query)
             {
                 string sql = query;
                 SQLiteCommand command = new SQLiteCommand(sql, G_connection);
                 command.ExecuteNonQuery();
             }
//-------------------------------------------------------------------------------------->
             public DataTable Getdata(string query)
             {
                 OpenConnection();
                 SQLiteCommand command = G_connection.CreateCommand();
                 SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, G_connection);
                 DataTable table = new DataTable();
                 try
                 {
                    adapter.Fill(table);
                 }catch (System.Exception e){
                     CloseConnection();
                     return table;
                 }
                 CloseConnection();
                 return table;

	        }
//-------------------------------------------------------------------------------------->
             public Dictionary<string, string> GetDictionary(string query)
             {
                 OpenConnection();
                 SQLiteCommand command = G_connection.CreateCommand();
                 SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, G_connection);
                 DataTable table = new DataTable();
                 try
                 {
                    adapter.Fill(table);
                 }catch (System.Exception e){
                     CloseConnection();
                     
                 }
                 CloseConnection();

                Dictionary<string, string> Book = new Dictionary<string, string>();
                foreach(DataRow row in table.Rows) {

                    Book.Add(row[0].ToString(), row[1].ToString());
                }
                return Book;
}

//-------------------------------------------------------------------------------------->
             public int  GetRowsCount(string query)
             {
                 OpenConnection();
                 SQLiteCommand command = new SQLiteCommand(query, G_connection);
                 SQLiteDataReader reader = command.ExecuteReader();

                 int rowcount = 0;
                 try
                 {
                     while (reader.Read())
                     {
                         rowcount = Convert.ToInt32(reader[0].ToString());
                     }
                     reader.Close();
                 }
                 catch (System.Exception e)
                 {
                     CloseConnection();
                     return rowcount;
                 }
                 CloseConnection();
                 return rowcount;
                 
             }
//-------------------------------------------------------------------------------------->
             public void MultiInsert(DataTable table, string[] columns, string tablename)
             {
                 BeginTransaction();
                 string[] ary = new string[table.Columns.Count];
                 for (int i = 0; i < table.Rows.Count; i++)
                 {
                     for (int j = 0; j < table.Columns.Count; j++)
                     {
                         ary[j] = table.Rows[i][j].ToString();

                     }
                     Insert(ary, columns, tablename);
                 }
                 EndTransaction();
             }
//-------------------------------------------------------------------------------------->
             public void Insert(string[] ary, string[] columns, string tablename)
             {
                 string sql = "INSERT INTO " + tablename + " (";
                 for (int i = 0; i < columns.Length; i++)
                 {
                     sql += columns[i];
                     if (i == columns.Length - 1) { sql += ")"; }
                     else { sql += ","; }
                 }
                 sql += " VALUES(";
                 for (int i = 0; i < ary.Length; i++)
                 {
                     sql += "'" + ary[i];
                     if (i == ary.Length - 1) { sql += "')"; }
                     else { sql += "',"; }
                 }
                     try
                     {
                         SQLiteCommand command = G_connection.CreateCommand();
                         command.CommandText = sql;
                         command.ExecuteNonQuery();
                     }
                     catch ( System.Data.SqlClient.SqlException ex)
                     {
                        
                     }
             }
//-------------------------------------------------------------------------------------->
             public void DeleteTable(string table)
             {
                string query = "DELETE FROM " +table+ ";";
                OpenConnection();
                ExecuteNonQuery(query);
                CloseConnection();
             }
//-------------------------------------------------------------------------------------->
             public DataTable GetTable(string tablename, string[] columns)
             {
                string query = "SELECT ";
                for (int i = 0; i < columns.Length; i++)
                 {
                     query += columns[i];
                     if (i == columns.Length - 1) { query += " "; }
                     else { query += ","; }
                 }
                    
                query += "FROM " +tablename+ ";";
                return Getdata(query);
             }
//-------------------------------------------------------------------------------------->
             
         }
}

