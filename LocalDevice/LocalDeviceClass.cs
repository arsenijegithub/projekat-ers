// LocalDevice

// pokretanje:
// 1. LocalController -> Debug -> Start withoout debugging (sacekati dok se ne pojavi ispis...)
// 2. LocalDevice -> Debug -> Start without debugging

using LocalDevice.Model;
using System;
using System.Xml;
using System.Net.Sockets;
using System.Text;
using System.IO;
using LocalDevice.Wrappers;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;

namespace LocalDevice
{

    [Serializable()]

    public class LocalDeviceClass : ILocalDevice
    {
        [NonSerialized()] public MyNetworkStream MyStream;

        public string Id { get; set; }
        public string Type { get; set; }
        public long Timestamp { get; set; }
        public string Value { get; set; }
        public double WorkTime { get; set; }
        public string Configuration { get; set; }
        public string LocalDeviceCode { get; set; }


        public LocalDeviceClass(string id, string type, long timeStamp, string value, double workingTime, string configuration)
        {
            MyStream = new MyNetworkStream();
            Id = id;
            Type = type;
            Value = value;
            WorkTime = workingTime;
            Configuration = configuration;
            Timestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            LocalDeviceCode = HashGenerator();
            Configuration = "LK";

        }


        public LocalDeviceClass()
        {
            MyStream = new MyNetworkStream();
            Console.WriteLine("Dodavanje novog uredjaja: \n");

            Console.Write("Unesite ID uredjaja: ");
            Id = Console.ReadLine();
            do
            {
                Console.Write("Unesite tip uredjaja <A ili D>: ");
                Type = Console.ReadLine();
                if (Type.Equals("A") || Type.Equals("a"))
                {
                    Console.Write("Unesite inicijalnu vrednost za analogni uredjaj (broj): ");
                    Value = Console.ReadLine();

                    Console.Write("Unesite broj nominalnih radnih sati predvidjenih za uredjaj (broj) :");
                    WorkTime = Double.Parse(Console.ReadLine());
                    break;
                }
                else if (Type.Equals("D") || Type.Equals("d"))
                {
                    Console.Write("Unesite vrednost za digitalni uredjaj (1/0): ");
                    Value = Console.ReadLine();

                    Console.Write("Unesite broj nominalnih promena predvidjenih za uredjaj (broj) :");
                    WorkTime = Double.Parse(Console.ReadLine());
                    break;
                }
                else
                {
                    Console.WriteLine("Pogresno unet tip uredjaja... Unesite \'A\' ili \'D\' za tip uredjaja.");
                    throw new Exception("Pogresno unet tip uredjaja (unesite A ili D)");
                }
            } while (!Type.Equals("A") || !Type.Equals("a") || !Type.Equals("D") || !Type.Equals("d"));


            Console.WriteLine("Uneti AMS/LK.Configuration:");
            Configuration = ConfigurationManager.AppSettings["Configuration"];



            LocalDeviceCode = HashGenerator();
            Timestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }
        public string HashGenerator()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string Data = this.Id + this.Type + this.Value + this.WorkTime.ToString() + this.Timestamp.ToString();

                byte[] value = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));
                return Encoding.UTF8.GetString(value);
            }
        }

        public void Start()
        {
            TcpClient client;
            if (Configuration.ToUpper() == "LK")
            {
                client = new TcpClient("127.0.0.1", 3560);
            }
            else
            {
                client = new TcpClient("127.0.0.1", 4160);
            }

            MyStream.Stream = client.GetStream();
        }

        public bool PosaljiPodatke()
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] bytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, this);
                bytes = memoryStream.ToArray();
            }
            try
            {
                MyStream.Write(bytes, 0, bytes.Length);
                MyStream.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}