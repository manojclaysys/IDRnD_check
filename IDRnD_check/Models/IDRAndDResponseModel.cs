using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDRnD_check.Models
{
    public class IDRAndDResponseModel
    {
        public float score { get; set; }
        public float quality { get; set; }
        public float probability { get; set; }
        public string error { get; set; }
        public string error_code { get; set; }

        public string SNR { get; set; }
    }


    public class MatchTemplate
    {
        public string template1 { get; set; }
        public string template2 { get; set; }
    }

}