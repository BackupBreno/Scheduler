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
            input_latency.Text = "0";
            input_timeslice.Text = "0";

            input_scheduler.Items.Insert(0, "FIFO");
            input_scheduler.Items.Insert(1, "SJF");
            input_scheduler.Items.Insert(2, "PRIORIDADE S/ PREEMPÇÃO");
            input_scheduler.Items.Insert(3, "PRIORIDADE C/ PREEMPÇÃO");
            input_scheduler.Items.Insert(4, "SRTN");
            input_scheduler.Items.Insert(5, "ROUND ROBIN");
            input_scheduler.Items.Insert(6, "MULTILEVEL");
            input_scheduler.SelectedIndex = 0;
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
            List<Windows.UI.Color> colors = new List<Windows.UI.Color>();

            list_escalonador.Items.Clear();

            colors.Add(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            colors.Add(Windows.UI.Color.FromArgb(255, 0, 0, 255));
            colors.Add(Windows.UI.Color.FromArgb(255, 0, 255, 0));
            colors.Add(Windows.UI.Color.FromArgb(255, 255, 0, 0));

            colors.Add(Windows.UI.Color.FromArgb(255, 255, 0, 255));
            colors.Add(Windows.UI.Color.FromArgb(255, 0, 255, 255));
            colors.Add(Windows.UI.Color.FromArgb(255, 255, 255, 255));

            colors.Add(Windows.UI.Color.FromArgb(255, 100, 100, 100));
            colors.Add(Windows.UI.Color.FromArgb(255, 0, 100, 255));

            simulation_details = scheduler.run(input_scheduler.SelectedIndex, UInt32.Parse(input_timeslice.Text), UInt32.Parse(input_latency.Text));

            float scale;

            if (scheduler.time >= (1080 - 30))
                scale = scheduler.time / (1080 - 30);
            else
                scale = 1;

            for (int i = 0; i < simulation_graph.Count; i++)
            {
                layout_root.Children.Remove(simulation_graph[i]);
            }

            for (int i = 0; i < simulation_details.Count; i++)
            {
                /* ListView */
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
                /* ----- */

                /* Grafico */
                /* OBS: Item1 = Tempo de Entrada ; Item2 = Tempo de Saida ; Item3 = ID (unico para cada processo: 1..2..3..4..) ; Item4 = Nome do Processo*/
                Windows.UI.Xaml.Shapes.Rectangle aux_rectangle = new Windows.UI.Xaml.Shapes.Rectangle();
                aux_rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                aux_rectangle.VerticalAlignment = VerticalAlignment.Top;
                aux_rectangle.Margin = new Thickness(simulation_details.ElementAt(i).Item1 * scale + 30, 500 + simulation_details.ElementAt(i).Item3 * 20, 0, 0); /* Coordenadas */

                /* Seta a cor do processo, se passar da quantidade armazenada volta pra primeira (so o primeiro overflow) */
                if (simulation_details[i].Item3 >= colors.Count)
                {
                    aux_rectangle.Fill = new SolidColorBrush(colors[simulation_details[i].Item3 + 1 - colors.Count]);
                }
                else
                {
                    aux_rectangle.Fill = new SolidColorBrush(colors[simulation_details[i].Item3]);
                }

                /* Tamanho do retangulo baseado no tempo de entrada e saida */
                aux_rectangle.Height = 19;
                aux_rectangle.Width = (simulation_details.ElementAt(i).Item2 - simulation_details.ElementAt(i).Item1) * scale;

                /* Exibe o retangulo */
                simulation_graph.Add(aux_rectangle);
                layout_root.Children.Add(simulation_graph[simulation_graph.Count - 1]);
                /* ----- */
            }
        }
    }
}
