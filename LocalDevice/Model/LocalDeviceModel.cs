using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevice.Model
{
    public class LocalDeviceModel
    {
        //    enum Tip { ANALOG, DIGITAL };

        public int Id { get; set; }

        public string Type { get; set; }
        public int LocalDeviceCode { get; set; }
        public int Timestamp { get; set; }
        public string Value { get; set; }
        public double WorkTime { get; set; }
        public string Configuration { get; set; }

        public LocalDeviceModel(int Id, string Type, int LocalDeviceCode, int Timestamp, string Value, double WorkTime, string Configuration)
        {

            this.Id = Id;
            this.Type = Type;
            this.LocalDeviceCode = LocalDeviceCode;
            this.Timestamp = Timestamp;
            this.Value = Value;
            this.WorkTime = WorkTime;
            this.Configuration = Configuration;

        }

        public LocalDeviceModel()
        {
            Console.WriteLine("Dodavanje novog lokalnog uredjaja:");

            Console.WriteLine("Id:");
            Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Tip uredjaja (ANALOG/DIGITAL):");
            Type = Console.ReadLine();

            Console.WriteLine("LocalDeviceCode:");
            LocalDeviceCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Timestamp:");
            Timestamp = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Value:");
            Value = Console.ReadLine();

            Console.WriteLine("WorkTime:");
            WorkTime = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Configuration:");
            Configuration = Console.ReadLine();
        }


    }
}
