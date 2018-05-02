using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WebCameraSelfie.Controls
{
    public class TimerControl : Control
    {
        public bool IsStarted
        {
            get { return (bool)GetValue(IsStartedProperty); }
            set { SetValue(IsStartedProperty, value); }
        }
        
        public static readonly DependencyProperty IsStartedProperty =
            DependencyProperty.Register("IsStarted", typeof(bool), typeof(TimerControl), new PropertyMetadata(false, IsStartedChangedCallback));

        public int TotalSeconds
        {
            get { return (int)GetValue(TotalSecondsProperty); }
            set { SetValue(TotalSecondsProperty, value); }
        }

        public static readonly DependencyProperty TotalSecondsProperty =
            DependencyProperty.Register("TotalSeconds", typeof(int), typeof(TimerControl), new PropertyMetadata(0));

        public int CurrentSeconds
        {
            get { return (int)GetValue(CurrentSecondsProperty); }
            private set { SetValue(CurrentSecondsProperty, value); }
        }

        public static readonly DependencyProperty CurrentSecondsProperty =
            DependencyProperty.Register("CurrentSeconds", typeof(int), typeof(TimerControl), new PropertyMetadata(0));
       
        public ICommand StopCommand
        {
            get { return (ICommand)GetValue(StopCommandProperty); }
            set { SetValue(StopCommandProperty, value); }
        }

        public static readonly DependencyProperty StopCommandProperty =
            DependencyProperty.Register("StopCommand", typeof(ICommand), typeof(TimerControl), new PropertyMetadata(null));

        public object StopCommandParameter
        {
            get { return (object)GetValue(StopCommandParameterProperty); }
            set { SetValue(StopCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty StopCommandParameterProperty =
            DependencyProperty.Register("StopCommandParameter", typeof(object), typeof(TimerControl), new PropertyMetadata(null));

        public event EventHandler Stopped;
        
        private static void IsStartedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timerControl = d as TimerControl;
            if ((bool)e.NewValue)
                timerControl.StartTimer();
            else
                timerControl.StopTimer();
        }

        private DispatcherTimer Timer;

        private void StartTimer()
        {
            CurrentSeconds = TotalSeconds;
            Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            Timer.Tick += (sender, args) => { 
                CurrentSeconds--;
                if (CurrentSeconds == 0)
                    IsStarted = false;
                };
            Timer.Start();
        }

        private void StopTimer()
        {
            Timer.Stop();
            Stopped?.Invoke(this, EventArgs.Empty);
            if (StopCommand?.CanExecute(StopCommandParameter) == true)
                StopCommand?.Execute(StopCommandParameter);
        }
    }
}
