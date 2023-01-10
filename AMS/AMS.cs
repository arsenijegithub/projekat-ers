﻿// AMS

using System;
using System.Xml;
using System.Net.Sockets;
using System.Text;

namespace AMS
{
    public class AMS
    {
        static void Main(string[] args)
        {
            // primanje podataka od lokalnog uredjaja
            // server ceka nove konekcije... slusa
            TcpListener server = new TcpListener(8086);
            server.Start();

            Console.WriteLine("AMS ceka novu konekciju...");

            try
            {
                // prihvatanje konekcije od klijenta
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                // citanje upita od klijenta
                byte[] data = new byte[4096];
                int bytes = stream.Read(data, 0, data.Length);

                string request = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                // sve sto sam dobio od klijenta
                Console.WriteLine("[AMS] Primljeno: {0}", request);

                // slanje odgovora nazad klijentu
                string response = "AMS je uspesno primio podatke o lokalnom uredjaju...";
                data = System.Text.Encoding.ASCII.GetBytes(response);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("[AMS] Poslato: {0}", response);

                stream.Close();
                client.Close();

                server.Stop();

            }
            catch
            {
                Console.WriteLine("Greska kod AMS-a.");
            }
        }

    }
}