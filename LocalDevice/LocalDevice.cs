// LocalDevice

// pokretanje:
// 1. LocalController -> Debug -> Start withoout debugging (sacekati dok se ne pojavi ispis...)
// 2. LocalDevice -> Debug -> Start without debugging

using LocalDevice.Model;
using System;
using System.Xml;
using System.Net.Sockets;
using static LocalDevice.Model.ILocalDevice;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace LocalDevice
{
    public class LocalDevice : ILocalDevice
    {

        public int Id { get; set; }

        public string Type { get; set; }
        public int LocalDeviceCode { get; set; }
        public int Timestamp { get; set; }
        public string Value { get; set; }
        public double WorkTime { get; set; }
        public string Configuration { get; set; }

        public LocalDevice(int Id, string Type, int LocalDeviceCode, int Timestamp, string Value, double WorkTime, string Configuration)
        {

            this.Id = Id;
            this.Type = Type;
            this.LocalDeviceCode = LocalDeviceCode;
            this.Timestamp = Timestamp;
            this.Value = Value;
            this.WorkTime = WorkTime;
            this.Configuration = Configuration;

        }

        public LocalDevice()
        {
            Console.WriteLine("Dodavanje novog lokalnog uredjaja:");

            Console.WriteLine("ID:");
            Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Da li je uredjaj ANALOGNI ili DIGITALNI? Unesite A/D:");
            Type = Console.ReadLine();

            if (!(Type.ToUpper().Equals("A") || Type.ToUpper().Equals("D")))
            {
                do
                {
                    Console.WriteLine("Pogresan unos!");
                    Console.WriteLine("Unesite A/D:");
                    Type = Console.ReadLine();
                } while (Type.ToUpper().Equals("A") && Type.ToUpper().Equals("D"));
            }


            Console.WriteLine("LocalDeviceCode:");
            LocalDeviceCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Timestamp:");
            Timestamp = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Value:");
            Value = Console.ReadLine();

            Console.WriteLine("WorkTime:");
            WorkTime = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Uneti AMS/LK.Configuration:");
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
        }

        public static void listaSvihUredjaja()
        {
            string putanja = @"..\..\..\..\data.xml";
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
                Console.WriteLine("*** LISTA LOKALNIH UREDJAJA ***");
                Console.WriteLine(File.ReadAllText(absolutePutanja));
            }


        }

        public static void modifikacijaUredjaj(string idMod, string noviId)
        {
            string putanja = @"..\..\..\..\data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePutanja = Path.Combine(dir, putanja);

            XmlDocument doc = new XmlDocument();
            doc.Load(absolutePutanja);

            XmlNode root = doc.DocumentElement;
            XmlNode deviceID;

            deviceID = doc.SelectSingleNode("data/item[deviceID='" + idMod + "']/deviceID");

            // pronasli smo deviceID za trazeni id -> Console.WriteLine("**data/item[deviceID='1']/deviceID** => " + deviceID.InnerText);

            deviceID.InnerText = noviId;

            doc.Save(absolutePutanja);
        }


        static void konekcijaUredjaja(LocalDevice device)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint1 = new IPEndPoint(localAddr, 8001);
            IPEndPoint serverEndPoint2 = new IPEndPoint(localAddr, 8002);

            TcpClient server = new TcpClient();

            int counter = 0;

            string mess = String.Format("USPESNO JE DODAT NOVI LOKALNI UREDJAJ\n ID: {0}\n Type: {1}\n LocalDeviceCode: {2}\n Timestamp: {3}\n Value: {4}\n WorkTime: {5}\n Configuration: {6}", device.Id, device.Type, device.LocalDeviceCode, device.Timestamp, device.Value, device.WorkTime, device.Configuration);

            while (!server.Connected || counter == 10)
            {
                try
                {
                    if (device.Configuration.ToUpper() == "LK")
                    {
                        server.Connect(serverEndPoint1);
                    }
                    else if (device.Configuration.ToUpper() == "AMS")
                    {
                        server.Connect(serverEndPoint2);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("LU: LK/AMS nije aktivan na traženoj adresi i portu, pokušavam ponovo...");
                }
                finally
                {
                    counter++;
                    Thread.Sleep(100);
                }

            }

            if (server.Connected)
            {
                Console.WriteLine("U " + DateTime.Now + " povezani smo sa serverom " + server.Client.RemoteEndPoint);

                NetworkStream stream = server.GetStream();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(mess);
                try
                {
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greska kod slanja " + ex);
                }

                stream.Close();
                server.Close();

                Console.WriteLine("U " + DateTime.Now + " zatvorili smo vezu sa serverom\n");
            }
            else
            {
                Console.WriteLine("Nisam uspeo da se povežem na server/ams, probudite me ponovo kasnije...");
            }
        }

        static void Main()
        {
            string answer;
            do
            {
                Console.WriteLine("1 - Dodavanje novog Lokalnog uredjaja");
                Console.WriteLine("2 - Izmena postojeceg lokalnog uredjaja");
                Console.WriteLine("3 - Lista svih lokalnih uredjaja");
                Console.WriteLine("4 - Izlazak iz programa");

                Console.WriteLine("Izaberite opciju:");
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        LocalDevice device = new LocalDevice();     // korisnik unosi novi lokalni uredjaj

                        Console.WriteLine("\nKlijent upaljen u: " + DateTime.Now);
                        Thread clientThread = new Thread(() => konekcijaUredjaja(device));
                        clientThread.Start();

                        clientThread.Join();

                        break;

                    case "2":
                        listaSvihUredjaja();

                        string idMod, noviId, noviType, noviCode, noviTime, noviValue, noviWork, noviConfig;
                        Console.WriteLine("*** UNESITE ID UREDJAJA KOJI MENJATE ***");
                        idMod = Console.ReadLine();

                        Console.WriteLine("*** UNESITE NOVI ID ***");
                        noviId = Console.ReadLine();

                        modifikacijaUredjaj(idMod, noviId);

                        break;

                    case "3":
                        listaSvihUredjaja();

                        break;

                    case "4":
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