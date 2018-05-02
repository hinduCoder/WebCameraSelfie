using WebCameraSelfie.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPFMediaKit.DirectShow.Controls;

namespace WebCameraSelfie
{ //AQAAAAACatVlAAT4JTBxFSdd_0YTikZkbJ9EYEQ
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string videoFileName;
        public MainWindow()
        {
            InitializeComponent();
            videoCapture.VideoCaptureDevice = MultimediaUtil.VideoInputDevices.First();
            videoFileName = Resources["videoFileName"] as String;
        }
        private async Task Test()
        {
            var storate = new YandexDiskStorage();

            var url = await storate.UploadAndGetLink(File.ReadAllBytes(videoFileName)); 
            
            Process.Start(url);
        }

        private void StartRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            //var watcher = new FileSystemWatcher(Directory.GetCurrentDirectory());
            videoCapture.Play();
            //watcher.Changed += Watcher_Changed;
            //watcher.EnableRaisingEvents = true;

        }

        //private async void Watcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    if (e.Name == videoFileName)
        //        await Test();
        //    ((FileSystemWatcher)sender).Dispose();
        //}

        private void TimerControl_Stopped(object sender, EventArgs e)
        {
            videoCapture.Stop();
        }
    }
}
