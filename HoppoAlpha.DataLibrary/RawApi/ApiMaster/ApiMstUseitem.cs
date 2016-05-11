﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstUseitem
    {
        public int api_id { get; set; }
        public int api_usetype { get; set; }
        public int api_category { get; set; }
        public string api_name { get; set; }
        public List<string> api_description { get; set; }
        public int api_price { get; set; }
    }
}
