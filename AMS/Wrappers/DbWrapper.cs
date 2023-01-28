﻿using LocalDevice;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Wrappers
{
    public class DbWrapper
    {
        public SqlConnection Connection { get; set; }
        private readonly string relativePath = @"..\..\db.mdf";


        public DbWrapper()
        {
        }

        public virtual bool PosaljiKomandu(string command)
        {
            try
            {
                Connection.Open();
                SqlCommand tmp = new SqlCommand(command, Connection);
                tmp.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Komanda neuspesno poslata u bazu podataka! " + e);
                Connection.Close();
                return false;
            }
        }






    }
}
