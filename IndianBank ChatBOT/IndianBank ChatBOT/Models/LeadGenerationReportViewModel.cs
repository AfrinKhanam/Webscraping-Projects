﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class LeadGenerationReportViewModel : ReportParams
    {
        public List<LeadGenerationAction> LeadGenerationActions { get; set; }
        public List<ConversationByIntent> ConversationsByIntent { get; set; }
    }
}
