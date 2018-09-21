﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api
{
    public static class CORSConfig
    {
        public const string allowedOrigins = "http://localhost:4200";
        public const string allowedHeaders = "*";
        public const string allowedMethods = "*";
    }
}