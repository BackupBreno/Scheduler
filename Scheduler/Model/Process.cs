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
        public Boolean isReady { get; set; }
        public System.Threading.Tasks.Task program { get; set; }
        public String toReturn { get; set; }

        public Process()
        {
            name = "";
            id = 0;
            duration = 0;
            begin = 0;
            priority = 0;
            duration_simu = duration;
            isReady = true;
        }

        public Process(String _name_, UInt16 _id_, UInt32 _duration_, UInt32 _begin_, UInt16 _priority_)
        {
            name = _name_;
            id = _id_;
            duration = _duration_;
            begin = _begin_;
            priority = _priority_;
            duration_simu = duration;
            isReady = true;

            program = new System.Threading.Tasks.Task(() => runAsync());
        }

        public void start()
        {
            program.Start();
        }
        public void finish()
        {
            
        }
        public void stop()
        {

        }
        public void resume()
        {
            program.ConfigureAwait(false);
        }

        public async System.Threading.Tasks.Task runAsync()
        {
            for (int i = 1; i <= 10; i++)
            {
                await System.Threading.Tasks.Task.Delay(1000);
                Debug.WriteLine(id + " - " + i);
            }
        }
    }
}
