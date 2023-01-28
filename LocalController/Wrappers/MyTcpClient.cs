using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Wrappers
{
    public class MyTcpClient
    {
        public TcpClient TcpClient { get; set; }
        public MyTcpClient()
        {

        }
        public virtual NetworkStream GetStream()
        {
            return TcpClient.GetStream();
        }
    }
}
