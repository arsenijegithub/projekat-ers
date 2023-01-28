using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevice.Wrappers
{
    public class MyNetworkStream
    {
	public NetworkStream Stream { get; set; }


      
        public virtual void Write(byte[] buffer, int offset, int size)
        {
            Stream.Write(buffer, offset, size);
        }

        public virtual void Close()
        {
            Stream.Close();
        }

        public virtual int Read(byte[] buffer, int offset, int size)
        {
            return Stream.Read(buffer, offset, size);
        }
    }
}
