﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalController
{
    public interface ILocalControllerClass
    {
        bool PrimiPodatke();

        void Start();

        void PokreniServer();

        bool PosaljiPodatke();

    }
}
