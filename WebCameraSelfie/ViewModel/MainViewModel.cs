using WebCameraSelfie.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WebCameraSelfie.ViewModel
{
    public class MainViewModel
    {
        public ICommand UploadCommand => new UploadCommand();
        public int Time => 3;
    }

    public class UploadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            await Task.Delay(500); //временное решение, чтобы дождаться, пока файл освободится. В будущем можно воспользоваться вотчером
            var fileName = parameter as string;
            var storate = new YandexDiskStorage();

            var url = await storate.UploadAndGetLink(File.ReadAllBytes(fileName));

            Process.Start(url);
        }
    }
}
