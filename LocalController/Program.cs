// LocalController - ponasa se i kao server i kao klijent

using System.Net.Sockets;
using System;
using System.Xml;
using System.Text;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace LocalController
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalControllerClass l = new LocalControllerClass();

            Thread thread = new Thread(new ThreadStart(l.DeviceListener));
            thread.Start();

            Thread thread2 = new Thread(new ThreadStart(l.AMSMain));
            thread2.Start();
        }
    }
}
