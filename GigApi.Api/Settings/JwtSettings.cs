﻿using GigApi.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public string Secret { get; set; }
    }
}
