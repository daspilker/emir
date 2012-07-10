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
using UsbUirt;

namespace EmIR.Interfaces
{
    public class USBUIRTInterface : InterfaceBase, IDisposable
    {
        public Controller Controller { get; private set; }

        public override void Connect()
        {
            if (Controller.DriverVersion != 0x0100)
            {
                string message = string.Format("Unsupported USB-UIRT driver version found.\nFound: {0:X}, required {1:X}", Controller.DriverVersion, 0x0100);
                throw new ApplicationException(message);
            }
            Controller = new Controller();
            Connected = true;
        }

        public override void Disconnect()
        {
            if (Controller != null)
            {
                Controller.Dispose();
                Controller = null;
            }
            Connected = false;
        }
    }
}
