using BOTAIML.ChatBot.DirectLineServer.Core.Utils;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BOTAIML.ChatBot.DirectLineServer.Core.Models
{

    public class ActivitySet
    {
        [JsonProperty("activities")]
        [JsonConverter(typeof(ActivityNullPropsProcessingConverter))]
        public IList<Activity> Activities { get; set; }
        [JsonProperty("watermark")]
        public int Watermark { get; set; }
    }


}