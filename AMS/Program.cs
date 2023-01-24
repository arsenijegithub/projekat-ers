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
        static void Main(string[] args)
        {
            AMSClass ams = new AMSClass();

            Thread thread = new Thread(new ThreadStart(ams.Main));
            thread.Start();
        }
    }
}
