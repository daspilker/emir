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
using System.Linq;
using System.Threading;
using EmIR.Models;
using Emotiv;
using UsbUirt;

namespace EmIR
{
    public class Engine : INotifyPropertyChanged, IDisposable
    {
        private Thread thread;
        private volatile bool running;
        private IRCode lastIRCode;
        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        public void Start()
        {
            App.CurrentApp.EmotivInterface.PropertyChanged += InterfacePropertyChanged;
            App.CurrentApp.USBUIRTInterface.PropertyChanged += InterfacePropertyChanged;

            UpdateStatus();
        }

        public void Dispose()
        {
            App.CurrentApp.EmotivInterface.PropertyChanged -= InterfacePropertyChanged;
            App.CurrentApp.USBUIRTInterface.PropertyChanged -= InterfacePropertyChanged;

            if (running)
            {
                Stop();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        void Run()
        {
            if (!running)
            {
                running = true;
                thread = new Thread((ThreadStart)delegate
                {
                    while (running)
                    {
                        App.CurrentApp.EmotivInterface.EmoEngine.ProcessEvents(100);
                    }
                });
                thread.Name = "EmIR Engine Thread";
                thread.Start();

                App.CurrentApp.EmotivInterface.EmoEngine.CognitivEmoStateUpdated += CognitivEmoStateUpdated;
                App.CurrentApp.USBUIRTInterface.Controller.TransmitCompleted += TransmitCompleted;
            }
        }

        void Stop()
        {
            if (running)
            {
                if (App.CurrentApp.USBUIRTInterface.Controller != null)
                {
                    App.CurrentApp.USBUIRTInterface.Controller.TransmitCompleted -= TransmitCompleted;
                }
                App.CurrentApp.EmotivInterface.EmoEngine.CognitivEmoStateUpdated -= CognitivEmoStateUpdated;

                running = false;
                thread.Join();
            }
        }

        void UpdateStatus()
        {
            if (App.CurrentApp.EmotivInterface.Connected && App.CurrentApp.USBUIRTInterface.Connected)
            {
                Run();

                Status = "Ready";
            }
            else
            {
                Stop();

                string message = "";
                if (!App.CurrentApp.EmotivInterface.Connected)
                {
                    message = "EmoEngine not connected";
                }
                if (!App.CurrentApp.USBUIRTInterface.Connected)
                {
                    if (message.Length > 0)
                    {
                        message += ", ";
                    }
                    message += "USB-UIRT not connected";
                }
                Status = message;
            }
        }

        void InterfacePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateStatus();
        }

        void CognitivEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            Rule rule = App.CurrentApp.Rules.FirstOrDefault(r => r.Headset == e.userId && r.Action == e.emoState.CognitivGetCurrentAction());
            if (rule != null && e.emoState.CognitivGetCurrentActionPower() >= rule.Threshold)
            {
                if (rule.IRCode != lastIRCode)
                {
                    Status = "Sending IR Code: " + rule.IRCode.Name;
                    App.CurrentApp.USBUIRTInterface.Controller.TransmitAsync(rule.IRCode.Code, CodeFormat.Pronto, 2, TimeSpan.Zero);
                    lastIRCode = rule.IRCode;
                }
            }
            else
            {
                lastIRCode = null;
            }
        }

        void TransmitCompleted(object sender, TransmitCompletedEventArgs e)
        {
            Status = e.Error == null ? "Ready" : "IR Transmission Error: " + e.Error.Message;
        }
    }
}
