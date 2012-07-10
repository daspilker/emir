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
using Emotiv;
using EmIR.Properties;

namespace EmIR.Interfaces
{
    public class EmotivInterface : InterfaceBase, IDisposable
    {
        private ushort port;

        public EmotivInterface()
        {
            EmoEngine = EmoEngine.Instance;
            Port = Settings.Default.EmoEnginePort;
        }

        public EmoEngine EmoEngine { get; private set; }

        public ushort Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    Settings.Default.EmoEnginePort = value;
                    Settings.Default.Save();
                    NotifyPropertyChanged("Port");
                }
            }
        }

        public override void Connect()
        {
            EmoEngine.RemoteConnect("127.0.0.1", Port);
            Connected = true;
        }

        public override void Disconnect()
        {
            if (Connected)
            {
                EmoEngine.Disconnect();
                Connected = false;
            }
        }
    }
}
