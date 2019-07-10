using Microsoft.EntityFrameworkCore;
using NewsDump.Lib.Data;
using NewsDump.Lib.Operations;
using NewsDump.Lib.Util;
using NewsDump.UI.Utils;
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

        private void AcrylicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var a = Conf.GetAll();
            check.IsChecked = Conf.Get<bool>("Hazf");
        }

        private void EventBus_OnMessageFired(MessageArgs message)
        {
            Dispatcher.Invoke(() =>
            {
                if (message.Type == "DoneOperation")
                {
                    System.Windows.MessageBox.Show(message.Message, "اطلاعات");
                }
                else
                {
                    console.Text += Environment.NewLine;
                    console.Text += message;
                    console.Text += Environment.NewLine;
                    console.ScrollToEnd();
                }

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
            try
            {
                await NewsHandler.RunAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Unhandled exception occurred: \n" + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                EventBus.Log(ex.ToString(), "Error");
            }

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
            if (mindate.SelectedDate != null && maxdate.SelectedDate != null)
            {
                var firstdate = mindate.SelectedDate.ToDateTime();
                var seconddate = maxdate.SelectedDate.ToDateTime();

                var min = new DateTime(firstdate.Year, firstdate.Month, firstdate.Day, minhr.Text.To<int>(), minmin.Text.To<int>(), 0);
                var max = new DateTime(seconddate.Year, seconddate.Month, seconddate.Day, maxhr.Text.To<int>(), maxmin.Text.To<int>(), 0);

                using (var fbd = new FolderBrowserDialog())
                {
                    var result = fbd.ShowDialog();

                    if (fbd.SelectedPath.HasValue())
                    {
                        ExportHandler.Export(fbd.SelectedPath, min, max);
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

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            Conf.Set("Hazf", check.IsChecked.ToString().ToLower());
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            Conf.Set("Hazf", check.IsChecked.ToString().ToLower());
        }
    }
}
