using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace Wcf_lib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {

        public Dictionary<string, string> Book = new Dictionary<string, string>();
        

        public Service1()
        {
            //DataInit();
        }

        void DataInit()
        {
            
        }

        public void AddPhone(string name, string number)
        {
            string query = "Insert into book (name, phone) values ('" + name + "','" + number + "')";
            SqlMaster master = new SqlMaster();
            master.OpenConnection();
            master.ExecuteNonQuery(query);
            master.CloseConnection();
        }
        
        public string GetData(string value)
        {
            try
            {
                Book = GetAllData();
                return Book[value].ToString();
            }
            catch (Exception e)
            {
                return "Такой контакт отсутствует";
            }
            
            
            return Book[value].ToString();
        }

        public Dictionary<string, string> GetAllData()
        {
            SqlMaster master = new SqlMaster();
            string query = "SELECT name, phone FROM book";
            return master.GetDictionary(query);
        }

        public DataTable GetDataTable()
        {
            SqlMaster master = new SqlMaster();
            string query = "SELECT name, phone FROM book";
            return master.Getdata(query);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
         
    }
}
