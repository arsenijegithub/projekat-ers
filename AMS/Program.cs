using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMS
{
    public class Program
    {
        static int Meni() 
        {
            Console.WriteLine("1. Ukupan broj radnih sati za uredjaj ");
            Console.WriteLine("2. Ispis uredjaja koji su prekoracili dozvoljen broj radnih sati");
            Console.WriteLine("3. Ispis svih uredjaja ");
            Console.WriteLine("0. KRAJ");
            int option;
            Console.WriteLine("Unesite opciju: ");
            option = Convert.ToInt32(Console.ReadLine());
            return option;
        }
        static void Main(string[] args)
        {
            AMSClass ams = new AMSClass();

            Thread thread = new Thread(new ThreadStart(ams.Main));
            thread.Start();


            while (true) 
            {
                int option = Meni();
                if (option == 1)
                {
                    Console.WriteLine("Unesite id: ");
                    string id = Console.ReadLine();
                    Console.WriteLine("Unesite datum od");
                    string strDatumStart = Console.ReadLine();

                    Console.WriteLine("Unesite datum do");
                    string strDatumEnd = Console.ReadLine();

                    DateTime datumStart = DateTime.ParseExact(strDatumStart, "dd.MM.yyyy.", null);
                    DateTime datumEnd = DateTime.ParseExact(strDatumEnd, "dd.MM.yyyy.", null);

                    double result = ams.BrojRadnihSati(id, datumStart, datumEnd);
                    Console.WriteLine("Broj radnih sati je " + result);
                }
                else if(option == 2) 
                {
                    ams.UredjajiPrekoracili();
                }
                else if(option == 3) 
                {
                    ams.Ispisi();
                }
                else if(option == 0) 
                {
                    break;
                }

            }
        }
    }
}
