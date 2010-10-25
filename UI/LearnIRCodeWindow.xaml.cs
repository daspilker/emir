/*
   Copyright 2010 Daniel A. Spilker

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using UsbUirt;

namespace EmIR.UI
{
    public partial class LearnIRCodeWindow : Window
    {
        public static DependencyProperty SignalQuality = DependencyProperty.Register("SignalQuality", typeof(int), typeof(LearnIRCodeWindow));
        public static DependencyProperty CarrierFrequency = DependencyProperty.Register("CarrierFrequency", typeof(int), typeof(LearnIRCodeWindow));
        public static DependencyProperty Progress = DependencyProperty.Register("Progress", typeof(int), typeof(LearnIRCodeWindow));

        private bool learningCompleted = false;

        public LearnIRCodeWindow()
        {
            DataContext = this;
            InitializeComponent();

            Controller.Learning += LearningProgress;
            Controller.LearnCompleted += LearningCompleted;
            Controller.LearnAsync();
        }

        public string IRCode { get; private set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            Controller.LearnCompleted -= LearningCompleted;
            Controller.Learning -= LearningProgress;
            if (!learningCompleted)
            {
                Controller.LearnAsyncCancel();
            }
            base.OnClosing(e);
        }

        void LearningCompleted(object sender, LearnCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                learningCompleted = true;
                IRCode = e.Code;
                CommandManager.InvalidateRequerySuggested();
            });
        }

        void LearningProgress(object sender, LearningEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                SetValue(SignalQuality, e.SignalQuality);
                SetValue(CarrierFrequency, e.CarrierFrequency);
                SetValue(Progress, e.Progress);
            });
        }

        void CancelLearningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
        }

        void AcceptLearningCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = learningCompleted;
        }

        void AcceptLearningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }

        static Controller Controller
        {
            get { return ((App)Application.Current).USBUIRTInterface.Controller; }
        }
    }
}
