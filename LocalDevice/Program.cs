using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalDevice
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("local device");
            LocalDeviceClass localDevice = new LocalDeviceClass("11", "11", 1, "11", 1, "11");
            while (true)
            {
                Thread.Sleep(2000);
                localDevice.Start();
                localDevice.PosaljiPodatke();
                Console.WriteLine("Podaci poslati.\n");
            }
    }
}
