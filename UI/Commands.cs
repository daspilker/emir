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

using System.Windows.Input;

namespace EmIR.UI
{
    class Commands
    {
        public static RoutedUICommand AddRule;
        public static RoutedUICommand DeleteRule;
        public static RoutedUICommand LearnIRCode;
        public static RoutedUICommand DeleteIRCode;
        public static RoutedUICommand AcceptIRCode;
        public static RoutedUICommand CancelLearning;
        public static RoutedUICommand ConnectToControlPanel;
        public static RoutedUICommand ConnectToEmoComposer;
        public static RoutedUICommand ReconnectEmoEngine;
        public static RoutedUICommand ReconnectIRInterface;
        public static RoutedUICommand ShowAboutDialog;
        public static RoutedUICommand Exit;

        static Commands()
        {
            AddRule = new RoutedUICommand("Add Rule", "AddRule", typeof(Commands));
            DeleteRule = new RoutedUICommand("Delete Rule", "DeleteRule", typeof(Commands));
            LearnIRCode = new RoutedUICommand("Learn IR Code", "LearnIRCode", typeof(Commands));
            DeleteIRCode= new RoutedUICommand("Delete IR Code", "DeleteIRCode", typeof(Commands));
            AcceptIRCode = new RoutedUICommand("Accept", "AcceptIRCode", typeof(Commands));
            CancelLearning = new RoutedUICommand("Cancel", "CancelLearning", typeof(Commands));
            ConnectToControlPanel = new RoutedUICommand("Connect To Control Panel", "ConnectToControlPanel", typeof(Commands));
            ConnectToEmoComposer = new RoutedUICommand("Connect To EmoComposer", "ConnectToEmoComposer", typeof(Commands));
            ReconnectEmoEngine = new RoutedUICommand("Reconnect", "ReconnectEmoEngine", typeof(Commands));
            ReconnectIRInterface = new RoutedUICommand("Reconnect", "ReconnectIRInterface", typeof(Commands));
            ShowAboutDialog = new RoutedUICommand("About EmIR", "ShowAboutDialog", typeof(Commands));
            Exit = new RoutedUICommand("Exit", "Exit", typeof(Commands));

            ConnectToControlPanel.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));
            ConnectToEmoComposer.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));
            ReconnectEmoEngine.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            ReconnectIRInterface.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));
            Exit.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));
        }
    }
}
