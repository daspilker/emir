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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EmIR.Models;
using Emotiv;

namespace EmIR
{
    public class DataFile
    {
        private string fileName;

        public DataFile(string fileName)
        {
            this.fileName = fileName;
        }

        public void Load(ICollection<IRCode> irCodes, ICollection<Rule> rules)
        {
            XElement element = XElement.Load(fileName);

            IDictionary<string, IRCode> loadedIRCodes = (from e in element.Element("ir-codes").Elements("ir-code")
                                                         select new
                                                         {
                                                             Id = (string)e.Element("id"),
                                                             IRCode = new IRCode
                                                             {
                                                                 Name = (string)e.Element("name"),
                                                                 Code = (string)e.Element("code")
                                                             }
                                                         }).ToDictionary(i => i.Id, i => i.IRCode);
            IEnumerable<Rule> loadedRules = from e in element.Element("rules").Elements("rule")
                                            select new Rule
                                            {
                                                Name = (string)e.Element("name"),
                                                Enabled = (bool)e.Element("enabled"),
                                                Headset = (int)e.Element("headset"),
                                                Action = (EdkDll.EE_CognitivAction_t)Enum.Parse(typeof(EdkDll.EE_CognitivAction_t), (string)e.Element("action")),
                                                Threshold = (float)e.Element("threshold"),
                                                IRCode = (string)e.Element("ir-code") == "" ? null : loadedIRCodes[(string)e.Element("ir-code")]
                                            };

            foreach (IRCode irCode in loadedIRCodes.Values)
            {
                irCodes.Add(irCode);
            }
            foreach (Rule rule in loadedRules)
            {
                rules.Add(rule);
            }
        }

        public void Save(IEnumerable<IRCode> irCodes, IEnumerable<Rule> rules)
        {
            IDictionary<IRCode, Guid> irCodeGuids = irCodes.ToDictionary(code => code, id => Guid.NewGuid());

            new XElement("emir",
                new XElement("ir-codes",
                    from code in irCodes
                    select new XElement("ir-code",
                        new XElement("id", irCodeGuids[code]),
                        new XElement("name", code.Name),
                        new XElement("code", code.Code))),
                new XElement("rules",
                    from rule in rules
                    select new XElement("rule",
                        new XElement("name", rule.Name),
                        new XElement("enabled", rule.Enabled),
                        new XElement("headset", rule.Headset),
                        new XElement("action", rule.Action),
                        new XElement("threshold", rule.Threshold),
                        new XElement("ir-code", rule.IRCode == null ? null : irCodeGuids[rule.IRCode].ToString())))).Save(fileName);
        }
    }
}
