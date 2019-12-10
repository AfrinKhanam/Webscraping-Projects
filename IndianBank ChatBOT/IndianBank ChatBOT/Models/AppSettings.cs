using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class AppSettings
     {
        public string TrainingEndPoint { get; set; }
        public string TestingEndPoint { get; set; }
        public string DeepPavlovPath { get; set; }
    }
}
