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
using LocalDevice;

namespace LocalDevice
{

    [Serializable()]

    public class LocalDeviceClass : ILocalDevice
    {
        [NonSerialized()] public MyNetworkStream MyStream;


        public double WorkAmmount { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public long Timestamp { get; set; }
        public string Value { get; set; }
        public double WorkTime { get; set; }
        public string Configuration { get; set; }
        public string LocalDeviceCode { get; set; }


        public LocalDeviceClass(string id, string type, long timeStamp, string value, double workingTime, string configuration, double workAmmount)
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
            WorkAmmount = workAmmount;

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


            Console.WriteLine("Unesite broj radnih sati uredjaja");
            WorkAmmount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Uneti AMS/LK Configuration:");
            Configuration = Console.ReadLine();
            if (!(Configuration.ToUpper().Equals("AMS") || Configuration.ToUpper().Equals("LK")))
            {
                do
                {
                    Console.WriteLine("Pogresan unos!");
                    Console.WriteLine("Unesite AMS/LK:");
                    Configuration = Console.ReadLine();
                } while (Configuration.ToUpper().Equals("AMS") && Configuration.ToUpper().Equals("LK"));
            }

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


        public override string ToString()
        {
            return Id + ";" + Type + ";" + Timestamp + ";" + Value + ";" + WorkTime + ";" + Configuration;
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

        public static void listaSvihUredjaja()
        {
            string putanja = @"..\..\..\data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePutanja = Path.Combine(dir, putanja);

            XmlDocument doc = new XmlDocument();
            doc.Load(absolutePutanja);
            if (File.ReadAllText(absolutePutanja).Length == 0)
            {
                Console.WriteLine("XML datoteka je prazna");
            }
            else
            {
                Console.WriteLine("*** LISTA LOKALNIH UREDJAJA - XML DATOTEKA ***");
                Console.WriteLine(File.ReadAllText(absolutePutanja));
            }
        }

        public static void modifikacijaUredjaj(string idMod, string noviId)
        {
            string putanja = @"..\..\..\data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePutanja = Path.Combine(dir, putanja);

            XmlDocument doc = new XmlDocument();
            doc.Load(absolutePutanja);

            XmlNode root = doc.DocumentElement;
            XmlNode deviceID;

           // deviceID = doc.SelectSingleNode("LocalDevice/[deviceID='" + idMod + "']/deviceID");
           
            deviceID = doc.SelectSingleNode("data/LocalDevice[deviceID='" + idMod + "']/deviceID");

            Console.WriteLine("**data/LocalDevice[deviceID='IDMOD']/deviceID** => " + deviceID.InnerText);

            // pronasli smo deviceID za trazeni id -> Console.WriteLine("**data/item[deviceID='1']/deviceID** => " + deviceID.InnerText);

            deviceID.InnerText = noviId;

            doc.Save(absolutePutanja);
        }

    }
}
