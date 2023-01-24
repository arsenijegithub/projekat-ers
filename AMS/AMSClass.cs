// AMS

using System;
using System.Xml;
using System.Net.Sockets;
using System.Text;
using System.Net;
using LocalDevice.Wrappers;
using AMS.Wrappers;
using LocalController.Wrappers;
using LocalDevice;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AMS
{
    public class AMSClass
    {
        public MyNetworkStream MyStream { get; set; }
        public MyTcpListener MyServer { get; set; }


        public DbWrapper Db{ get; set; }

        public AMSClass()
        {
            Db = new DbWrapper();
        }


        public bool PrimiPodatke()
        {
            try
            {
                TcpClient client = MyServer.AcceptTcpClient();
                MyStream.Stream = client.GetStream();

                byte[] data = new byte[8192];
                int bytes = MyStream.Read(data, 0, data.Length);

                List<LocalDeviceClass> localDevices = new List<LocalDeviceClass>();

                BinaryFormatter bf = new BinaryFormatter();
            }
        }

       

    }
}
