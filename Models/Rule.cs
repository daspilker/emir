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

using Emotiv;

namespace EmIR.Models
{
    public class Rule : ModelBase
    {
        private string name;
        private bool enabled;
        private int headset;
        private EdkDll.EE_CognitivAction_t action;
        private float threshold;
        private IRCode irCode;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    NotifyPropertyChanged("Enabled");
                }
            }
        }

        public int Headset
        {
            get { return headset; }
            set
            {
                if (headset != value)
                {
                    headset = value;
                    NotifyPropertyChanged("Headset");
                }
            }
        }

        public EdkDll.EE_CognitivAction_t Action
        {
            get { return action; }
            set
            {
                if (action != value)
                {
                    action = value;
                    NotifyPropertyChanged("Action");
                }
            }
        }

        public float Threshold
        {
            get { return threshold; }
            set
            {
                if (threshold != value)
                {
                    threshold = value;
                    NotifyPropertyChanged("Threshold");
                }
            }
        }

        public IRCode IRCode
        {
            get { return irCode; }
            set
            {
                if (irCode != value)
                {
                    irCode = value;
                    NotifyPropertyChanged("IRCode");
                }
            }
        }

        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        return string.IsNullOrWhiteSpace(Name) ? "Required field" : null;
                    case "IRCode":
                        return IRCode == null ? "Required field" : null;
                    default:
                        return null;
                }
            }
        }
    }
}
