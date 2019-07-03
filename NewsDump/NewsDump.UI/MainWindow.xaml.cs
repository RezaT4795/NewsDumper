using NewsDump.Lib.Operations;
using NewsDump.Lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //EventBus.OnMessageFired += EventBus_OnMessageFired;
        }

        private void EventBus_OnMessageFired(MessageArgs message)
        {
            console.Text += Environment.NewLine;
            console.Text += message;
            scroll.ScrollToEnd();
        }

        private void Clrconsole_Click(object sender, RoutedEventArgs e)
        {
            console.Text = string.Empty;
        }

        private async void Rundumper_Click(object sender, RoutedEventArgs e)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                Thread.Sleep(3000);

            });
        }

        private void Exportall_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(3000);
        }

        private void Exportthis_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
