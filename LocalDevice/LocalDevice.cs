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

        public static void konekcija(LocalDevice device)
        {
            // neka po default-u bude podeseno na slanje kontroleru
            TcpClient client = new TcpClient("127.0.0.1", 8085);

            if (device.Configuration.ToUpper() == "AMS")
            {
                client = new TcpClient("127.0.0.1", 8086);
            }
            else if (device.Configuration.ToUpper() != "LK")
            {
                Console.WriteLine("Greska prilikom dodavanja konfiguracije (AMS/LK).");
                Environment.Exit(0);
            }

            // client stream za citanje i pisanje
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Uspostavljanje konekcije sa serverom...");

            // poruka za server
            string message = "Podaci o novom lokalnom uredjaju su poslati.";

            string mess = String.Format("USPESNO JE DODAT NOVI LOKALNI UREDJAJ\n ID: {0}\n Type: {1}\n LocalDeviceCode: {2}\n Timestamp: {3}\n Value: {4}\n WorkTime: {5}\n Configuration: {6}", device.Id, device.Type, device.LocalDeviceCode, device.Timestamp, device.Value, device.WorkTime, device.Configuration);

            string xmlString = System.IO.File.ReadAllText("D:\\fakultet\\5 - semestar\\Elementi razvoja softvera\\projekat-step-by-step\\projekat\\LocalDevice\\data.xml");
            //            string xmlString = System.IO.File.ReadAllText(@"..\..\..\..\" + "data.xml");
            byte[] data = System.Text.Encoding.ASCII.GetBytes(mess); // ovde prosledjujem ono sta ce da posalje serveru

            //while (true)
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            data = new byte[4096];
            int bytes = stream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", response);

            stream.Close();
            client.Close();
        }

        static void Main()
        {
            string answer;
            do
            {
                Console.WriteLine("1 - Dodavanje novog Lokalnog uredjaja");
                Console.WriteLine("2 - Izmena postojeceg lokalnog uredjaja");
                Console.WriteLine("3 - Izlazak iz programa");

                Console.WriteLine("Izaberite opciju:");
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        LocalDevice device = new LocalDevice();     // korisnik unosi novi lokalni uredjaj
                        konekcija(device);                          // uspostavlja se konekcija sa konrolerom/ams-om - podaci se upisuju u xml

                        break;

                    case "2":
                        int id_temp;
                        Console.WriteLine("* LISTA SVIH LOKALNIH UREDJAJA***");

                        listaSvihUredjaja();
                        
                        Console.WriteLine("Izaberite ID uredjaja koji zelite da modifikujete:");
                        id_temp = Convert.ToInt32(Console.ReadLine());
                        break;

                    case "3":
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