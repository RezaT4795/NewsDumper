using Microsoft.EntityFrameworkCore;
using NewsDump.Lib.Data;
using NewsDump.Lib.Operations;
using NewsDump.Lib.Util;
using Olive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace NewsDump.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Repository.PerformMigration();

            EventBus.OnMessageFired += EventBus_OnMessageFired;
        }

        private void EventBus_OnMessageFired(MessageArgs message)
        {
            Dispatcher.Invoke(() =>
            {
                console.Text += Environment.NewLine;
                console.Text += message;
                console.ScrollToEnd();
            });

        }

        private void Clrconsole_Click(object sender, RoutedEventArgs e)
        {
            console.Text = string.Empty;
        }

        private async void Rundumper_Click(object sender, RoutedEventArgs e)
        {
            commandbar.IsEnabled = false;
            prog.IsIndeterminate = true;
            await NewsHandler.RunAsync();
            commandbar.IsEnabled = true;
            prog.IsIndeterminate = false;


        }



        private async void Exportall_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (fbd.SelectedPath.HasValue())
                {
                    ExportHandler.Export(fbd.SelectedPath);
                }
            }
        }


        private void Exportthis_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (fbd.SelectedPath.HasValue())
                {
                    ExportHandler.Export(fbd.SelectedPath, skip.Text.To<int>(), take.Text.To<int>());
                }
            }
        }

        private void Exportdue_Click(object sender, RoutedEventArgs e)
        {
            if (mindate.SelectedDate.HasValue && maxdate.SelectedDate.HasValue)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (fbd.SelectedPath.HasValue())
                    {
                        ExportHandler.Export(fbd.SelectedPath, mindate.SelectedDate, maxdate.SelectedDate);
                    }
                }
            }
            else
            {
                EventBus.Notify("Dates are empty", "Alert");
            }
        }

        private void Resetdb_Click(object sender, RoutedEventArgs e)
        {
            Repository.ResetDb();
        }
    }
}
