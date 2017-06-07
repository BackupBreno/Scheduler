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
            CPU = new Process();
            scheduler_type = 0;
            time = 0;
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
        public List<Tuple<UInt32, UInt32, UInt16, String>> run(UInt32 _timeSlice_, UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();

            switch (scheduler_type)
            {
                case 0:
                    details = first_in_first_out_fifo(_latencia_);
                    break;
                case 1:
                    details = shortest_job_first_sjf(_latencia_);
                    break;
                case 2:
                    details = scheduler_by_priority_no_preemptive(_latencia_);
                    break;
                case 3:
                    details = shortest_remaining_time_next_srtn(_latencia_);
                    break;
                case 4:
                    details = scheduler_by_priority_preemptive(_latencia_);
                    break;
                case 5:
                    details = round_robin(_timeSlice_, _latencia_);
                    break;
                case 6:
                    details = multilevel();
                    break;
            }

            return details;
        }
        /* --- */

        /* Schedulers */
        public List<Tuple<UInt32, UInt32, UInt16, String>> first_in_first_out_fifo(UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process = new Process();
            time = 0;

            for (int i = 0; i < list_process.Count; i++)
            {
                list_process[i].duration_simu = list_process[i].duration;

                if (list_process[i].begin == 0)
                {
                    process_run.Add(list_process[i]);
                }
                else
                {
                    process_to_start.Add(list_process[i]);
                }
            }

            /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;// = new Tuple<UInt32, UInt32, UInt16, String>(0, 0, 0, "");
            UInt32 time_b, time_f;
            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    if (time >= process_run[0].begin)
                    {
                        CPU = process_run[0];

                        time_b = time;
                        time += CPU.duration_simu;
                        CPU.duration_simu = 0;
                        time_f = time;

                        process_run[0] = CPU;

                        toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, CPU.id, CPU.name);
                        details.Add(toAdd);

                        process_end.Add(process_run[0]);
                        process_run.RemoveAt(0);
                    }
                }
                else
                {
                    time++;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int i = 0; i < process_to_start.Count; i++)
                        {
                            if (time >= process_to_start[i].begin)
                            {
                                process_run.Add(process_to_start[i]);
                                process_to_start.RemoveAt(i);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
;                            }

                            if (i == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }

                /* Latencia */
                if (CPU.id != last_process.id)
                {
                    time += _latencia_;
                }

                last_process = CPU;

            }

            return details;
        }
        public List<Tuple<UInt32, UInt32, UInt16, String>> shortest_job_first_sjf(UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process = new Process();
            time = 0;

            for (int i = 0; i < list_process.Count; i++)
            {
                list_process[i].duration_simu = list_process[i].duration;

                if (list_process[i].begin == 0)
                {
                    process_run.Add(list_process[i]);
                }
                else
                {
                    process_to_start.Add(list_process[i]);
                }
            }

            /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;// = new Tuple<UInt32, UInt32, UInt16, String>(0, 0, 0, "");
            UInt32 time_b, time_f;

            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    int shorter_process = 0;
                    UInt32 shorter_duration = 9999;

                    for (int i = 0; i < process_run.Count; i++)
                    {
                        if (process_run[i].duration_simu < shorter_duration)
                        {
                            shorter_duration = process_run[i].duration_simu;
                            shorter_process = i;
                        }
                    }
                    // ---

                    CPU = process_run[shorter_process];

                    time_b = time;
                    time += CPU.duration_simu;
                    CPU.duration_simu = 0;
                    time_f = time;

                    process_run[shorter_process] = CPU;

                    toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, CPU.id, CPU.name);
                    details.Add(toAdd);

                    process_end.Add(process_run[shorter_process]);
                    process_run.RemoveAt(shorter_process);
                }
                else
                {
                    time++;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int i = 0; i < process_to_start.Count; i++)
                        {
                            if (time >= process_to_start[i].begin)
                            {
                                process_run.Add(process_to_start[i]);
                                process_to_start.RemoveAt(i);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
                                ;
                            }

                            if (i == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }

                /* Latencia */
                if (CPU.id != last_process.id)
                {
                    time += _latencia_;
                }

                last_process = CPU;
            }

            return details;
        }
        public List<Tuple<UInt32, UInt32, UInt16, String>> scheduler_by_priority_no_preemptive(UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process = new Process();
            time = 0;

            for (int i = 0; i < list_process.Count; i++)
            {
                list_process[i].duration_simu = list_process[i].duration;

                if (list_process[i].begin == 0)
                {
                    process_run.Add(list_process[i]);
                }
                else
                {
                    process_to_start.Add(list_process[i]);
                }
            }

            /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;// = new Tuple<UInt32, UInt32, UInt16, String>(0, 0, 0, "");
            UInt32 time_b, time_f;

            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    int higher_priority_process = 0;
                    UInt32 higher_priority = 9999;

                    for (int i = 0; i < process_run.Count; i++)
                    {
                        if (process_run[i].priority > higher_priority)
                        {
                            higher_priority = process_run[i].priority;
                            higher_priority_process = i;
                        }
                    }
                    // ---

                    CPU = process_run[higher_priority_process];

                    time_b = time;
                    time += CPU.duration_simu;
                    CPU.duration_simu = 0;
                    time_f = time;

                    process_run[higher_priority_process] = CPU;

                    toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, CPU.id, CPU.name);
                    details.Add(toAdd);

                    process_end.Add(process_run[higher_priority_process]);
                    process_run.RemoveAt(higher_priority_process);
                }
                else
                {
                    time++;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int i = 0; i < process_to_start.Count; i++)
                        {
                            if (time >= process_to_start[i].begin)
                            {
                                process_run.Add(process_to_start[i]);
                                process_to_start.RemoveAt(i);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
                                ;
                            }

                            if (i == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }

                /* Latencia */
                if (CPU.id != last_process.id)
                {
                    time += _latencia_;
                }

                last_process = CPU;
            }

            return details;
        }
        public List<Tuple<UInt32, UInt32, UInt16, String>> shortest_remaining_time_next_srtn(UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process = new Process();
            Boolean isFirst = true;
            time = 0;

            for (int i = 0; i < list_process.Count; i++)
            {
                list_process[i].duration_simu = list_process[i].duration;

                if (list_process[i].begin == 0)
                {
                    process_run.Add(list_process[i]);
                }
                else
                {
                    process_to_start.Add(list_process[i]);
                }
            }

            /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;// = new Tuple<UInt32, UInt32, UInt16, String>(0, 0, 0, "");
            UInt16 id_process = 0;
            UInt32 time_b = 0, time_f = 0;
            String name_process = "";
            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    int shorter_process = 0;
                    UInt32 shorter_duration = 9999;

                    for (int i = 0; i < process_run.Count; i++)
                    {
                        if (process_run[i].duration_simu < shorter_duration)
                        {
                            shorter_duration = process_run[i].duration_simu;
                            shorter_process = i;
                        }
                    }
                    // ---

                    CPU = process_run[shorter_process];

                    if (isFirst)
                    {
                        id_process = CPU.id;
                        name_process = CPU.name;
                        time_b = time;
                        isFirst = false;
                    }
                    else if (CPU.id != last_process.id)
                    {
                        time_f = time;

                        toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, id_process, name_process);
                        details.Add(toAdd);

                        /* Latencia */
                        time += _latencia_;

                        id_process = CPU.id;
                        name_process = CPU.name;
                        time_b = time;
                    }

                    last_process = CPU;

                    time++;
                    CPU.duration_simu--;

                    if (CPU.duration_simu == 0)
                    {
                        process_end.Add(process_run[shorter_process]);
                        process_run.RemoveAt(shorter_process);

                        if (process_run.Count == 0)
                        {
                            time_f = time;
                            toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, id_process, name_process);
                            details.Add(toAdd);
                        }
                    }

                }
                else
                {
                    time++;
                    isFirst = true;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int i = 0; i < process_to_start.Count; i++)
                        {
                            if (time >= process_to_start[i].begin)
                            {
                                process_run.Add(process_to_start[i]);
                                process_to_start.RemoveAt(i);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
                                ;
                            }

                            if (i == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }
            }

            return details;
        } /* Latencia - talvez nao faca a troca antes (postulado do Herleson) */
        public List<Tuple<UInt32, UInt32, UInt16, String>> scheduler_by_priority_preemptive(UInt32 _latencia_)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process_latencia = new Process();
            Process last_process = new Process();
            Boolean isFirst = true;
            time = 0;

            for (int i = 0; i < list_process.Count; i++)
            {
                list_process[i].duration_simu = list_process[i].duration;

                if (list_process[i].begin == 0)
                {
                    process_run.Add(list_process[i]);
                }
                else
                {
                    process_to_start.Add(list_process[i]);
                }
            }

            /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;// = new Tuple<UInt32, UInt32, UInt16, String>(0, 0, 0, "");
            UInt16 id_process = 0;
            UInt32 time_b = 0, time_f = 0;
            String name_process = "";
            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    int higher_priority_process = 0;
                    UInt32 higher_priority = 9999;

                    for (int i = 0; i < process_run.Count; i++)
                    {
                        if (process_run[i].priority > higher_priority)
                        {
                            higher_priority = process_run[i].priority;
                            higher_priority_process = i;
                        }
                    }
                    // ---

                    CPU = process_run[higher_priority_process];

                    if (isFirst)
                    {
                        id_process = CPU.id;
                        name_process = CPU.name;
                        time_b = time;
                        isFirst = false;
                    }
                    else if (CPU.id != last_process.id)
                    {
                        time_f = time;

                        toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, id_process, name_process);
                        details.Add(toAdd);

                        /* Latencia */
                        time += _latencia_;

                        id_process = CPU.id;
                        name_process = CPU.name;
                        time_b = time;
                    }

                    last_process = CPU;

                    time++;
                    CPU.duration_simu--;

                    if (CPU.duration_simu == 0)
                    {
                        process_end.Add(process_run[higher_priority_process]);
                        process_run.RemoveAt(higher_priority_process);

                        if (process_run.Count == 0)
                        {
                            time_f = time;
                            toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, id_process, name_process);
                            details.Add(toAdd);
                        }
                    }

                }
                else
                {
                    time++;
                    isFirst = true;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int i = 0; i < process_to_start.Count; i++)
                        {
                            if (time >= process_to_start[i].begin)
                            {
                                process_run.Add(process_to_start[i]);
                                process_to_start.RemoveAt(i);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
                                ;
                            }

                            if (i == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }

                /* Latencia */
                if (CPU.id != last_process_latencia.id)
                {
                    time += _latencia_;
                }

                last_process_latencia = CPU;
            }

            return details;
        } /* Latencia - talvez nao faca a troca antes (postulado do Herleson) */
        public List<Tuple<UInt32, UInt32, UInt16, String>> round_robin(UInt32 _timeSlice_, UInt32 _latencia_) /* Latencia - talvez nao faca a troca antes (postulado do Herleson) */
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            List<Process> process_run = new List<Process>();
            List<Process> process_to_start = new List<Process>();
            List<Process> process_end = new List<Process>();
            Process last_process = new Process();
            int i = 0;
            time = 0;

            for (int j = 0; j < list_process.Count; j++)
            {
                list_process[j].duration_simu = list_process[j].duration;

                if (list_process[j].begin == 0)
                {
                    process_run.Add(list_process[j]);
                }
                else
                {
                    process_to_start.Add(list_process[j]);
                }
            }

                /* begin    end     id      name */
            Tuple<UInt32, UInt32, UInt16, String> toAdd;
            UInt32 time_b = 0, time_f = 0;
            while (process_end.Count < list_process.Count)
            {
                if (process_run.Count > 0)
                {
                    CPU = process_run[i];

                    /* Latencia */
                    if (CPU.id != last_process.id)
                    {
                        time += _latencia_;
                    }

                    last_process = CPU;
                    /* -------- */

                    time_b = time;
                    if (CPU.duration_simu > _timeSlice_)
                    {
                        time += _timeSlice_;
                        CPU.duration_simu -= _timeSlice_;
                    }
                    else
                    {
                        time += CPU.duration_simu;
                        CPU.duration_simu = 0;

                        process_end.Add(process_run[i]);
                        process_run.RemoveAt(i);
                    }
                    time_f = time;

                    toAdd = new Tuple<UInt32, UInt32, UInt16, String>(time_b, time_f, CPU.id, CPU.name);
                    details.Add(toAdd);
                    
                    if (++i >= process_run.Count)
                    {
                        i = 0;
                    }
                }
                else
                {
                    time++;
                }

                if (process_to_start.Count > 0)
                {
                    bool ended = false;
                    while (!ended)
                    {
                        for (int j = 0; j < process_to_start.Count; j++)
                        {
                            if (time >= process_to_start[j].begin)
                            {
                                process_run.Add(process_to_start[j]);
                                process_to_start.RemoveAt(j);

                                if (process_to_start.Count == 0)
                                {
                                    ended = true;
                                }

                                break;
                                ;
                            }

                            if (j == (process_to_start.Count - 1))
                            {
                                ended = true;
                            }
                        }
                    }
                }
            }

            return details;
        } /* Falta Latencia */
        public List<Tuple<UInt32, UInt32, UInt16, String>> multilevel()
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> details = new List<Tuple<UInt32, UInt32, UInt16, String>>();
            return details;
        }
        /* --- */
    }
}
