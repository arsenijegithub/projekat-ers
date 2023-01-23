// AMS

using System;
using System.Xml;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace AMS
{
    public static class Globals
    {
        public const string IP = "127.0.0.1";
        public const int PORT = 8002;
    }

    public class AMS
    {
        public static List<string> IsolateValues(string fullMessage)
        {
            List<string> vrednosti = new List<string>();

            string all, type, val, config;
            int id, code, tempTime, worktime;

            all = Convert.ToString(fullMessage);

            string[] lines = all.Split('\n');

            id = Convert.ToInt32(lines[1].Trim().Remove(0, 4));
            vrednosti.Add(Convert.ToString(id));

            type = lines[2].Trim().Remove(0, 6);
            vrednosti.Add(type);

            code = Convert.ToInt32(lines[3].Trim().Remove(0, 17));
            vrednosti.Add(Convert.ToString(code));

            tempTime = Convert.ToInt32(lines[4].Trim().Remove(0, 11));
            vrednosti.Add(Convert.ToString(tempTime));

            val = lines[5].Trim().Remove(0, 7);
            vrednosti.Add(val);

            worktime = Convert.ToInt32(lines[6].Trim().Remove(0, 10));
            vrednosti.Add(Convert.ToString(worktime));

            config = lines[7].Trim().Remove(0, 15);
            vrednosti.Add(config);

            return vrednosti;
        }

        public static void WriteToXML(string fullMessage)
        {
            List<string> vr = new List<string>();

            string path = @"..\..\..\..\data.xml";
            //string path = "data.xml";
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string absolutePath = Path.Combine(dir, path);
            //Console.WriteLine("APSOLUTNA PUTANJA TEST 1: " + absolutePath);

            vr = IsolateValues(fullMessage);

            List<string> lista = new List<string>();

            lista = IsolateValues(fullMessage);

            XmlDocument doc = new XmlDocument();

            doc.Load(absolutePath);

            XmlNode item = doc.CreateElement("item");

            XmlNode deviceID = doc.CreateElement("deviceID");
            deviceID.InnerText = vr[0];
            item.AppendChild(deviceID);

            XmlNode deviceType = doc.CreateElement("deviceType");
            deviceType.InnerText = vr[1];
            item.AppendChild(deviceType);

            XmlNode deviceCode = doc.CreateElement("deviceCode");
            deviceCode.InnerText = vr[2];
            item.AppendChild(deviceCode);

            XmlNode time = doc.CreateElement("time");
            time.InnerText = vr[3];
            item.AppendChild(time);

            XmlNode value = doc.CreateElement("value");
            value.InnerText = vr[4];
            item.AppendChild(value);

            XmlNode workTime = doc.CreateElement("workTime");
            workTime.InnerText = vr[5];
            item.AppendChild(workTime);

            XmlNode configuration = doc.CreateElement("configuration");
            configuration.InnerText = vr[6];
            item.AppendChild(configuration);

            doc.DocumentElement.AppendChild(item);

            doc.Save(absolutePath);

            Console.WriteLine("Uspesno dodavanje podataka u XML datoteku.");
        }

        public static void ReciveFromDevice(int id, string type, int code, int tempTime, string val, double worktime, string config, string all)
        {
            try
            {
                while (true)
                {
                    IPAddress localAddr = IPAddress.Parse(Globals.IP);
                    TcpListener server = new TcpListener(localAddr, Globals.PORT);
                    server.Start();

                    Console.WriteLine("AMS ceka novu konekciju...");

                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    // citanje upita od klijenta
                    byte[] data = new byte[4096];
                    int bytes = stream.Read(data, 0, data.Length);

                    string request = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    // sve sto sam dobio od klijenta
                    Console.WriteLine("[LU->LK] Primljeno: {0}", request);


                    // upis u sql



                    // slanje odgovora nazad klijentu
                    string response = "AMS je uspesno primio podatke o lokalnom uredjaju od LK...";
                    data = System.Text.Encoding.ASCII.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("[AMS->LK] Poslato: {0}", response);

                    stream.Close();
                    client.Close();

                    server.Stop();
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Greska kod kontrolera - servera");
            }
        }

        static void Main(string[] args)
        {
            int tempId = 0;
            string tempType = "";
            int tempCode = 0;
            int tempTime = 0;
            string tempValue = "";
            double tempWorkTime = 0;
            string tempConfig = "";

            string tempRequest = "";

            ReciveFromDevice(tempId, tempType, tempCode, tempTime, tempValue, tempWorkTime, tempConfig, tempRequest);

            IPAddress localAddr = IPAddress.Parse(Globals.IP);
            TcpListener server = new TcpListener(localAddr, Globals.PORT);
            server.Start();

            Console.WriteLine("AMS je pokrenut u " + DateTime.Now + " " + Globals.IP + ":" + Globals.PORT);

        }
    }
}
