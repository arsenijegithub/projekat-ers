using LocalDevice;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Wrappers
{
    public class DbWrapper
    {
        public SqlConnection Connection { get; set; }
        private readonly string relativePath = @"..\..\db.mdf";


        public DbWrapper()
        {
        }

        public virtual bool PosaljiKomandu(string command)
        {
            try
            {
                Connection.Open();
                SqlCommand tmp = new SqlCommand(command, Connection);
                tmp.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Komanda neuspesno poslata u bazu podataka! " + e);
                Connection.Close();
                return false;
            }
        }

        public virtual void UbaciUTabelu(LocalDeviceClass ld)
        {
            var command = "insert into Local_device (Id,Type,Value,Work_time,Timestamp,LocalDeviceCode) values ('" + ld.Id + "', '" + ld.Type + "', '" + ld.WorkTime + "', '" + ld.Timestamp + "', '" + ld.LocalDeviceCode + "')";
            PosaljiKomandu(command);
        }


        public virtual void KreirajTabelu()
        {
            string absolutePath = Path.GetFullPath(relativePath);
            string connectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename={0};Integrated Security=True", absolutePath);
            Console.WriteLine(connectionString);
            Connection = new SqlConnection(connectionString);
            string createTableSql = "CREATE TABLE LocalDevice ( Id varchar(255),Type varchar(255),Value varchar(255),Work_time decimal(13,2), Timestamp int, LocalDeviceCode varchar(255));";
            PosaljiKomandu(createTableSql);
        }



    }
}
