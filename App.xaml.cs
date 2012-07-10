/*
   Copyright 2012 Daniel A. Spilker

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
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using EmIR.Interfaces;
using EmIR.Models;
using EmIR.Properties;

namespace EmIR
{
    public partial class App : Application
    {
        public ObservableCollection<IRCode> IRCodes { get; private set; }

        public ObservableCollection<Rule> Rules { get; private set; }

        public USBUIRTInterface USBUIRTInterface { get; private set; }

        public EmotivInterface EmotivInterface { get; private set; }

        public Engine Engine { get; private set; }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void ClearData()
        {
            Rules.Clear();
            IRCodes.Clear();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            IRCodes = new ObservableCollection<IRCode>();
            Rules = new ObservableCollection<Rule>();

            if (!string.IsNullOrEmpty(Settings.Default.FileName))
            {
                try
                {
                    new DataFile(Settings.Default.FileName).Load(IRCodes, Rules);
                }
                catch (FileNotFoundException)
                {
                    Settings.Default.FileName = null;
                }
            }

            try
            {
                USBUIRTInterface = new USBUIRTInterface();
                USBUIRTInterface.Connect();
            }
            catch (Exception) { }

            try
            {
                EmotivInterface = new EmotivInterface();
                EmotivInterface.Connect();
            }
            catch (Exception) { }

            Engine = new Engine();
            Engine.Start();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (Engine != null)
            {
                Engine.Dispose();
            }

            if (EmotivInterface != null)
            {
                EmotivInterface.Dispose();
            }

            if (USBUIRTInterface != null)
            {
                USBUIRTInterface.Dispose();
            }

            if (!string.IsNullOrEmpty(Settings.Default.FileName))
            {
                new DataFile(Settings.Default.FileName).Save(IRCodes, Rules);
            }

            base.OnExit(e);
        }

        public static App CurrentApp
        {
            get { return (App)Application.Current; }
        }
    }
}
