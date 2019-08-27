using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace Pointy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
        }

        CameraCapture cc;
        
        public MainWindow()
        {
            InitializeComponent();
            cc = new CameraCapture();

            cc.EnumerateCameras();

            cc.isCameraRunning = true;
            cc.Start();

            outputframe.DataContext = cc;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cc.isCameraRunning = false;
        }
    }
}
