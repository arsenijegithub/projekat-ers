// AMSMain

using System;
using System.Xml;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace AMSMain
{
    public class AMSMain
    {
        /*
        public static void konekcijaBP()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Database1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertString = "INSERT INTO Data (Id, Type, Timestamp, Code, Value, WorkTime, Configuration) VALUES ('12', 'tip-test', '125123', '44', 'value-test', '5', 'config-test')";
                using (SqlCommand command = new SqlCommand(insertString, connection))
                {
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
        */

        public static void izlistajSveUredjaje()
        {
            // 1
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Database1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DataTable schema = connection.GetSchema("Tables");
                if (true)   // napraviti da ako je nova tabela prazna - ne prolazi kroz nju
                {
                    foreach (DataRow row in schema.Rows)
                    {
                        Console.WriteLine("\nData from table: Data");
                        using (SqlCommand command = new SqlCommand("SELECT * FROM Data", connection))
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(reader[i] + " ");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }



        static void Main(string[] args)
        {
            string answer;
            do
            {
                Console.WriteLine("1 - Ispisati listu svih postojecih uredjaja u sistemu");
                Console.WriteLine("2 - Ispisati broj radnih sati za izabrani uredjaj");
                Console.WriteLine("3 - Ispisati detalje promena za izabrani period za izabrani lokalni uredjaj");
                Console.WriteLine("4 - Ispisati sve uredjaje ciji je broj radnih sati preko konfigurisane vrednosti");
                Console.WriteLine("5 - Izlazak iz programa");

                Console.WriteLine("Izaberite opciju:");
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        Console.WriteLine("--- LISTA SVIH UREDJAJA U SISTEMU ---");
                        izlistajSveUredjaje();

                        Console.WriteLine();

                        break;

                    case "2":

                        break;

                    case "3":

                        break;

                    case "4":

                        break;

                    case "5":
                        Console.WriteLine("Izlazak iz programa...");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Nije unet validan unos.");
                        break;
                }

            } while (!answer.ToUpper().Equals("X"));
        }

    }
}
