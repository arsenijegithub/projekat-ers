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
                        Console.WriteLine("\nLokalni uredjaj upaljen u: " + DateTime.Now);
                        LocalDeviceClass localDevice = new LocalDeviceClass();

                        localDevice.Start();
                        localDevice.PosaljiPodatke();
                        Console.WriteLine("Lokalni uredjaj je uspesno poslao podatke!");
                        break;

                    case "2":
                        LocalDeviceClass.listaSvihUredjaja();

                        string idMod, noviId;
                        Console.WriteLine("* UNESITE ID UREDJAJA KOJI MENJATE *");
                        idMod = Console.ReadLine();

                        Console.WriteLine("* UNESITE NOVI ID *");
                        noviId = Console.ReadLine();

                        LocalDeviceClass.modifikacijaUredjaj(idMod, noviId);

                        break;

                    case "3":
                        LocalDeviceClass.listaSvihUredjaja();

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
