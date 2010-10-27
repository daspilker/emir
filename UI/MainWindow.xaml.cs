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
using System.IO;
using System.Windows;
using System.Windows.Input;
using EmIR.Models;
using EmIR.Properties;
using Microsoft.Win32;

namespace EmIR.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void InitializeFileDialog(FileDialog fileDialog)
        {
            if (string.IsNullOrEmpty(Settings.Default.FileName))
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                fileDialog.InitialDirectory = new FileInfo(Settings.Default.FileName).DirectoryName;
            }
            fileDialog.DefaultExt = ".emir";
            fileDialog.Filter = "EmIR Files|*.emir|All Files|*.*";
        }

        void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Settings.Default.FileName = null;
            Settings.Default.Save();

            App.CurrentApp.ClearData();
        }

        void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            InitializeFileDialog(fileDialog);
            bool? result = fileDialog.ShowDialog(this);
            if (result.Value)
            {
                App.CurrentApp.ClearData();

                new DataFile(fileDialog.FileName).Load(App.CurrentApp.IRCodes, App.CurrentApp.Rules);

                Settings.Default.FileName = fileDialog.FileName;
                Settings.Default.Save();
            }
        }

        void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.FileName))
            {
                SaveAsExecuted(sender, e);
            }
            else
            {
                new DataFile(Settings.Default.FileName).Save(App.CurrentApp.IRCodes, App.CurrentApp.Rules);
            }
        }

        void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            InitializeFileDialog(fileDialog);
            bool? result = fileDialog.ShowDialog(this);
            if (result.Value)
            {
                new DataFile(fileDialog.FileName).Save(App.CurrentApp.IRCodes, App.CurrentApp.Rules);

                Settings.Default.FileName = fileDialog.FileName;
                Settings.Default.Save();
            }
        }

        void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        void ConnectToControlPanelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.CurrentApp.EmotivInterface.Port = 3008;
                App.CurrentApp.EmotivInterface.Reconnect();
            }
            catch (Emotiv.EmoEngineException ex)
            {
                MessageBox.Show(ex.Message, "EmoEngine Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void ConnectToEmoComposerExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.CurrentApp.EmotivInterface.Port = 1726;
                App.CurrentApp.EmotivInterface.Reconnect();
            }
            catch (Emotiv.EmoEngineException ex)
            {
                MessageBox.Show(ex.Message, "EmoEngine Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void ReconnectEmoEngineExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.CurrentApp.EmotivInterface.Reconnect();
            }
            catch (Emotiv.EmoEngineException ex)
            {
                MessageBox.Show(ex.Message, "EmoEngine Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        void ReconnectIRInterfaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.CurrentApp.USBUIRTInterface.Reconnect();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message + ".", "USB-UIRT Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void ShowAboutDialogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Window window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        void AddRuleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Rule newRule = new Rule { Enabled = true };
            App.CurrentApp.Rules.Add(newRule);
            RulesListView.SelectedItem = newRule;
        }

        void DeleteRuleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = RulesListView != null && RulesListView.SelectedItem != null;
        }

        void DeleteRuleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            App.CurrentApp.Rules.Remove((Rule)RulesListView.SelectedItem);
        }

        void LearnIRCodeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.CurrentApp.USBUIRTInterface.Connected == false)
            {
                MessageBox.Show("USB-UIRT not connected", "EmIR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LearnIRCodeWindow learnIRCodeWindow = new LearnIRCodeWindow();
            learnIRCodeWindow.Owner = this;
            bool? dialogResult = learnIRCodeWindow.ShowDialog();
            if (dialogResult.GetValueOrDefault(false))
            {
                IRCode newCode = new IRCode { Code = learnIRCodeWindow.IRCode };
                App.CurrentApp.IRCodes.Add(newCode);
                IRCodesListView.SelectedItem = newCode;
            }
        }

        void DeleteIRCodeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IRCodesListView != null && IRCodesListView.SelectedItem != null;
        }

        void DeleteIRCodeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            App.CurrentApp.IRCodes.Remove((IRCode)IRCodesListView.SelectedItem);
        }
    }
}
