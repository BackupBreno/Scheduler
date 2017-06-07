using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace Scheduler
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Model.Scheduler scheduler;

        private List<Windows.UI.Xaml.Shapes.Rectangle> simulation_graph;
        private Windows.UI.Xaml.Shapes.Rectangle rectangle;
        private UInt16 counter;

        public MainPage()
        {
            this.InitializeComponent();

            scheduler = new Model.Scheduler();
            simulation_graph = new List<Windows.UI.Xaml.Shapes.Rectangle>();
            rectangle = new Windows.UI.Xaml.Shapes.Rectangle();
            counter = 1;
        }

        private void Add_Process_Click(object sender, RoutedEventArgs e)
        {
            if (String.Compare(input_process_name.Text, "") != 0 && String.Compare(input_process_duration.Text, "") != 0 && String.Compare(input_process_time_begin.Text, "") != 0 && String.Compare(input_priority.Text, "") != 0)
            {
                /* Adedando Processo na Lista de Processos */
                scheduler.add_process(input_process_name.Text, counter++, UInt32.Parse(input_process_duration.Text), UInt32.Parse(input_process_time_begin.Text), UInt16.Parse(input_priority.Text));

                /* Feedback */
                String to_add = "Nome: " + input_process_name.Text + "\t | Duracao: " + input_process_duration.Text + "\t | Inicio: " + input_process_time_begin.Text;
                output_list_process.Items.Add(to_add);

                /* Clear Display */
                input_process_name.Text = "";
                input_process_duration.Text = "";
                input_process_time_begin.Text = "";
            }
            else
            {
                scheduler.add_process("Word", 1, 123, 0, 0);
                scheduler.add_process("Excel", 2, 23, 3, 0);
                scheduler.add_process("Power", 3, 12, 15, 0);
                scheduler.add_process("Note", 4, 32, 300, 0);
            }
        }

        private void Simulation_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<UInt32, UInt32, UInt16, String>> simulation_details;

            simulation_details = scheduler.scheduler_by_priority_preemptive(10);

            float scale;

            if (scheduler.time >= (1080 - 30))
                scale = scheduler.time / (1080 - 30);
            else
                scale = 1;

            int j = 0;
            for (int i = 0; i < simulation_details.Count; i++)
            {
                String details = "";

                details = "Inicio: "
                    + ((simulation_details.ElementAt(i).Item1 == 0) ? "0" : "")
                    + ((simulation_details.ElementAt(i).Item1 < 100) ? "0" : "")
                    + simulation_details.ElementAt(i).Item1 + "\t->\t";

                details += "Fim: "
                    + ((simulation_details.ElementAt(i).Item2 == 0) ? "0" : "")
                    + ((simulation_details.ElementAt(i).Item2 < 100) ? "0" : "")
                    + simulation_details.ElementAt(i).Item2 + "\t|\t";

                details += simulation_details.ElementAt(i).Item4;

                list_escalonador.Items.Add(details);

                Windows.UI.Xaml.Shapes.Rectangle aux_rectangle = new Windows.UI.Xaml.Shapes.Rectangle();
                aux_rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                aux_rectangle.VerticalAlignment = VerticalAlignment.Top;
                aux_rectangle.Margin = new Thickness(simulation_details.ElementAt(i).Item1 * scale + 30, 500 + simulation_details.ElementAt(i).Item3 * 20, 0, 0);
                aux_rectangle.Fill = new SolidColorBrush(Windows.UI.Colors.SteelBlue);

                aux_rectangle.Height = 19;
                aux_rectangle.Width = (simulation_details.ElementAt(i).Item2 - simulation_details.ElementAt(i).Item1) * scale;

                layout_root.Children.Add(aux_rectangle);
            }
        }
    }
}
