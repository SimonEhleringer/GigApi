﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application
{
    public interface IJwtSettings
    {
        public string Secret { get; set; }
    }
}
