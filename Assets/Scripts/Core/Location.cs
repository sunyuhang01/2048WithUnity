using System;
using System.Collections.Generic;

namespace Console2048
{
    struct Location
    {
        public int RIndex { get; set; }
        public int CIndex { get; set; }

        public Location(int RIndex, int CIndex):this()
        {
            this.RIndex = RIndex;
            this.CIndex = CIndex;
        }
    }
}
