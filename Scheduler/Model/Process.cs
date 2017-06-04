using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scheduler.Model
{
    class Process
    {
        public String name { get; set; }
        public UInt16 id { get; set; }
        public UInt32 duration { get; set; }
        public UInt32 begin { get; set; }
        public UInt16 priority { get; set; }
        public UInt32 duration_simu { get; set; }

        public Process()
        {
            name = "";
            id = 0;
            duration = 0;
            begin = 0;
            priority = 0;
            duration_simu = duration;
        }

        public Process(String _name_, UInt16 _id_, UInt32 _duration_, UInt32 _begin_, UInt16 _priority_)
        {
            name = _name_;
            id = _id_;
            duration = _duration_;
            begin = _begin_;
            priority = _priority_;
            duration_simu = duration;
        }
    }
}
