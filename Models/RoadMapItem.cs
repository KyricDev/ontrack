using System;

namespace SDRM.Models{
    public class RoadMapItem{
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsComplete { get; set; }
    }
}