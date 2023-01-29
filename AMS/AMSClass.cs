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


        private DateTime UnixToDateTime(long timeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();
            return dateTime;
        }
        public double BrojRadnihSati(string id, DateTime startDate, DateTime endDate)
        {
            double suma = 0;
            foreach (LocalDeviceClass device in localDevices)
            {
                if (device.Id == id)
                {
                    DateTime dateTime = UnixToDateTime(device.Timestamp);
                    if (dateTime > startDate && dateTime < endDate)
                    {
                        suma += device.WorkTime;
                    }

                }
            }
            return suma;
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

        public void UredjajiPrekoracili()
        {
            List<LocalDeviceClass> lista = new List<LocalDeviceClass>();
            foreach (LocalDeviceClass device in localDevices)
            {
                if (!Provera(lista, device))
                {
                    lista.Add(device);
                }

            }

            List<LocalDeviceClass> lista2 = new List<LocalDeviceClass>();
            foreach (LocalDeviceClass d in lista)
            {
                if (d.WorkAmmount > d.WorkTime)
                {
                    lista2.Add(d);
                }
            }

            IspisiUredjaje(lista2);
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
                MyStream.Stream = client.GetStream();

                byte[] data = new byte[8192];
                int bytes = MyStream.Read(data, 0, data.Length);

                List<LocalDeviceClass> ld = new List<LocalDeviceClass>();

                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, data.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    ld = (List<LocalDeviceClass>)bf.Deserialize(ms);
                    localDevices.AddRange(ld);
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

	public void Main()
        {
            PokreniServer();
            while (true)
            {
                PrimiPodatke();
            }
        }

        public void PokreniServer()
        {
            MyStream = new MyNetworkStream();
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            MyServer = new MyTcpListener(localAddr, 4160);
            MyServer.Start();
        }
       

    }
}
