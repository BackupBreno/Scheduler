using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    class Scheduler
    {
        public List<Process> list_process { get; set; }
        public Process CPU { get; set; }
        public UInt32 time { get; set; }
        public UInt16 scheduler_type { get; set; }

        public Scheduler()
        {
            list_process = new List<Process>();
            scheduler_type = 0;
            time = 0;

            Process toTest;
            toTest = new Process("Pages", 1, 33, 0, 0);
            list_process.Add(toTest);
            toTest = new Process("Keynote", 2, 20, 20, 0);
            list_process.Add(toTest);

            /*
            Process toTest;
            toTest = new Process("Pages", 33, 0, 0);
            list_process.Add(toTest);
            toTest = new Process("Keynote", 20, 20, 0);
            list_process.Add(toTest);
            toTest = new Process("Numbers", 50, 70, 1);
            list_process.Add(toTest);
            toTest = new Process("Word", 35, 22, 2);
            list_process.Add(toTest);
            toTest = new Process("Excel", 31, 210, 2);
             Testes Rapidos
            let Pages = Process(name: "Pages", id: 0, duration: 33, begin: 0, priority: 0)
            */
        }

        public void add_process(String _name_, UInt16 _id_, UInt32 _duration_, UInt32 _begin_, UInt16 _priority_)
        {
            Process toAdd = new Process(_name_, _id_, _duration_, _begin_, _priority_);
            list_process.Add(toAdd);
        }

        public void clear()
        {
            list_process.Clear();
        }

        /* Run */
        public List<Tuple<UInt32, UInt32, UInt16, String>> run()
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();

            list_process[0].start();
            list_process[0].stop();
            System.Threading.Tasks.Task.Delay(1000);

            return details;
        }
        /* --- */

        /* Schedulers */

        /* --- */
    }
}
