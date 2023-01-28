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
using LocalDevice.Model;

namespace AMS
{
    public class AMSClass
    {
        public MyNetworkStream MyStream { get; set; }
        public MyTcpListener MyServer { get; set; }

        public List<LocalDeviceClass> localDevices { get; set; }

        public DbWrapper Db{ get; set; }

        public AMSClass()
        {
            Db = new DbWrapper();
            localDevices = new List<LocalDeviceClass>();
        }

        public void IspisiUredjaje(List<LocalDeviceClass> devices)
        {
            foreach (LocalDeviceClass d in devices)
            {
                Console.WriteLine(d);
            }
        }

        public void Ispisi()
        {
            List<LocalDeviceClass> lista = new List<LocalDeviceClass>();
            foreach (LocalDeviceClass device in localDevices)
            {
                if (!Provera(lista, device))
                {
                    lista.Add(device);
                }

            }
            IspisiUredjaje(lista);

        }
        public bool Provera(List<LocalDeviceClass> devices, LocalDeviceClass device)
        {
            foreach (LocalDeviceClass d in devices)
            {
                if (d.Id == device.Id)
                {
                    return true;
                }

            }
            return false;
        }


        public bool PrimiPodatke()
        {
            try
            {
                TcpClient client = MyServer.AcceptTcpClient();
                //MyStream.Stream = client.GetStream();

                byte[] data = new byte[8192];
               // int bytes = MyStream.Read(data, 0, data.Length);

                List<LocalDeviceClass> localDevices = new List<LocalDeviceClass>();

                BinaryFormatter bf = new BinaryFormatter();
            }
            catch { }

            return false;
        }

       

    }
}
