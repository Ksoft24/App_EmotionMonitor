using System;

namespace EmotionMonitor
{
    public class ApiResponse
    {
        public string status { get; set; }
        public List<EmotionData> data { get; set; }
    }

    public class EmotionData
    {
        public int happy { get; set; }
        public int sad { get; set; }
        public int angry { get; set; }
        public int surprised { get; set; }
        public int neutral { get; set; }
        public int disgust { get; set; }
        public int fear { get; set; }
    }
}
