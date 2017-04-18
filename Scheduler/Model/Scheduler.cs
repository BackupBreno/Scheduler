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

            /* Testes Rapidos
            let Pages = Process(name: "Pages", id: 0, duration: 33, begin: 0, priority: 0)
            let Keynote = Process(name: "Keynote", id: 1, duration: 20, begin: 20, priority: 0)
            let Numbers = Process(name: "Numbers", id: 2, duration: 50, begin: 70, priority: 1)
            let Word = Process(name: "Word", id: 3, duration: 35, begin: 22, priority: 2)
            let Excel = Process(name: "Excel", id: 4, duration: 31, begin: 210, priority: 2)
            array_process.append(Pages)
            array_process.append(Keynote)
            array_process.append(Numbers)
            array_process.append(Word)
            array_process.append(Excel)
             -------------- */
        }

        public void add_process(String _name_, UInt32 _duration_, UInt32 _begin_, UInt16 _priority_)
        {
            Process toAdd = new Process(_name_, _duration_, _begin_, _priority_);
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
            return details;
        }
        /* --- */

        /* Schedulers */

        /* --- */
    }
}
