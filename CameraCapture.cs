using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Pointy
{
    class CameraCapture : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Create class-level accesible variables
        VideoCapture capture;
        Mat frame;
        private Thread camera;

        public bool isCameraRunning = false;

        int frames = 0;

        private BitmapSource currentCameraImage;
        public BitmapSource CurrentCameraImage
        {
            get { return currentCameraImage; }
            set
            {
                currentCameraImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentCameraImage"));
            }
        }
        
        // Declare required methods
        public void Start()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }

        private void CaptureCameraCallback()
        {
            frame = new Mat();

                   

            int idx = 2;
            capture = new VideoCapture(CaptureDevice.DShow, idx);
            capture.Open(CaptureDevice.DShow, idx);
            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    capture.Read(frame);

                    var bitmap = frame.ToWriteableBitmap();
                    bitmap.Freeze();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CurrentCameraImage = bitmap;
                    }, System.Windows.Threading.DispatcherPriority.Render);
                }
            }
        }


        public List<int> camIdList = new List<int>();
        private OpenCVDeviceEnumerator.OpenCVDeviceEnumerator enumerator = new OpenCVDeviceEnumerator.OpenCVDeviceEnumerator();


        public void EnumerateCameras()
        {
            enumerator.EnumerateCameras(camIdList);
        }
    }
}
