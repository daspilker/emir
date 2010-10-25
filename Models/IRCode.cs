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

namespace EmIR.Models
{
    public class IRCode : ModelBase
    {
        private string name;
        private string code;

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

        public string Code
        {
            get { return code; }
            set
            {
                if (code != value)
                {
                    code = value;
                    NotifyPropertyChanged("Code");
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
                    case "Code":
                        return string.IsNullOrWhiteSpace(Code) ? "Required field" : null;
                    default:
                        return null;
                }
            }
        }
    }
}