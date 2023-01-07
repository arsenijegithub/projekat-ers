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

        static void Main()
        { 
        
        }
             
    }
}