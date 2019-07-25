using Microsoft.EntityFrameworkCore;
using NewsDump.Lib.Data;
using NewsDump.Lib.Operations;
using NewsDump.Lib.Util;
using NewsDump.UI.Utils;
using Olive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private System.Windows.Forms.Timer timer1;
        private int totalSeconds = 10;
        private bool _isRuning = false;
        public MainWindow()
        {
            InitializeComponent();

            Repository.PerformMigration();
            EventBus.TouchLogFile();

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
            resetdb.IsEnabled = false;
            resetLog.IsEnabled = false;
            rundumper.IsEnabled = false;
            runTimer.IsEnabled = false;
            runparadumper.IsEnabled = false;
            prog.IsIndeterminate = true;
            _isRuning = true;
            try
            {
                await NewsHandler.RunAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Unhandled exception occurred: \n" + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                EventBus.Log(ex.ToString(), "Error");
            }
            _isRuning = false;
            rundumper.IsEnabled = true;
            runparadumper.IsEnabled = true;
            runTimer.IsEnabled = true;
            prog.IsIndeterminate = false;
            resetdb.IsEnabled = true;
            resetLog.IsEnabled = true;

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
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("آیا اطمینان دارید؟", "پاک کردن دیتابیس", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Repository.ResetDb();
            }

        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            Conf.Set("Hazf", check.IsChecked.ToString().ToLower());
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            Conf.Set("Hazf", check.IsChecked.ToString().ToLower());
        }

        private async void Runparadumper_Click(object sender, RoutedEventArgs e) => await RunDumper();

        async Task RunDumper(bool silent = false)
        {
            resetdb.IsEnabled = false;
            resetLog.IsEnabled = false;
            rundumper.IsEnabled = false;
            runTimer.IsEnabled = false;
            runparadumper.IsEnabled = false;
            prog.IsIndeterminate = true;
            _isRuning = true;
            try
            {
                await NewsHandler.RunParallelAsync(silent);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    System.Windows.MessageBox.Show("Unhandled exception occurred: \n" + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                EventBus.Log("Something went wrong, check out logs", "info");
                EventBus.Log(ex.ToString(), "Error");
            }
            _isRuning = false;
            rundumper.IsEnabled = true;
            runparadumper.IsEnabled = true;
            runTimer.IsEnabled = true;
            prog.IsIndeterminate = false;
            resetdb.IsEnabled = true;
            resetLog.IsEnabled = true;
        }

        private void RunTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer1 == null)
            {
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += async (s, e) =>
                {
                    if (!_isRuning)
                    {
                        totalSeconds -= 1;
                        SetTime();
                        if (totalSeconds == 0)
                        {

                            await RunDumper(true);
                            totalSeconds = 10800;
                        }
                    }


                };

                timer1.Interval = 1000; // in miliseconds
                timer1.Start();
            }
            else
            {
                timer1.Stop();
                timer1 = null;
                totalSeconds = 10;
                runTimer.Content = "اجرای زمانبندی";
            }

        }

        private void SetTime()
        {
            if (totalSeconds != 0)
            {
                var ts = TimeSpan.FromSeconds(totalSeconds);
                var txtDate = string.Format("{0}", new DateTime(ts.Ticks).ToString("HH:mm:ss"));
                runTimer.Content = txtDate;
            }
            else
            {
                runTimer.Content = "درحال اجرا";
            }


        }

        private void ResetLog_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("آیا اطمینان دارید؟", "پاک کردن لاگ", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                EventBus.ClearLog();
            }
        }

        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "/select, " + EventBus.GetLoggingPath());
        }
    }
}
