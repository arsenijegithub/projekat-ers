using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Wrappers
{
    public class MyTcpListener
    {
        public TcpListener Listener { get; set; }


        public MyTcpListener()
        {

        }
        public MyTcpListener(IPAddress address, int port)
        {
            Listener = new TcpListener(address, port);
        }

        public virtual void Start()
        {
            Listener.Start();
        }

        public virtual TcpClient AcceptTcpClient()
        {
            return Listener.AcceptTcpClient();
        }

    }

      
}
