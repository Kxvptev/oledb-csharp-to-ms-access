using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace OLEDBToMSAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=online_shop.mdb"))
            {
                string dateString;

                Console.WriteLine("Введите дату, начиная с которой будет выполнен наш запрос (через /):");
                dateString = Console.ReadLine();

                OleDbCommand command = new OleDbCommand();
                command.CommandText = "SELECT КодКурьера, КодКлиента FROM Заказ WHERE ДатаЗаказа >= #" + dateString + "# ORDER BY КодКурьера";
                command.Connection = connection;

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();

                int code = -1;
                while (reader.Read())
                {
                    OleDbCommand findNameClient = new OleDbCommand();
                    findNameClient.Connection = connection;

                    if (code != Convert.ToInt32(reader[0].ToString()))
                    {
                        OleDbCommand findNameCourier = new OleDbCommand();
                        findNameCourier.Connection = connection;

                        findNameCourier.CommandText = "SELECT ФИО FROM Курьер WHERE КОДКУРЬЕРА =" + reader[0].ToString();
                        OleDbDataReader readCourierName = findNameCourier.ExecuteReader();

                        readCourierName.Read();
                        
                        Console.WriteLine(readCourierName[0].ToString() + ":");
                    }

                    findNameClient.CommandText = "SELECT НАИМЕНОВАНИЕ FROM Клиент WHERE КОДКЛИЕНТА =" + reader[1].ToString();
                    OleDbDataReader readClientName = findNameClient.ExecuteReader();

                    readClientName.Read();
                    Console.WriteLine("  " + readClientName[0].ToString());

                    code = Convert.ToInt32(reader[0].ToString());
                }

                reader.Close();

                Console.ReadKey();

            }
        }
    }
}
