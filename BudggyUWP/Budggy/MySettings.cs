using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Budggy
{
    public class MySettings
    {

        public Budget budggyLoad { get; set; }

        public void Save(string filename, Budget budggy)
        {
            FileStream file = File.Create(filename);
            using (StreamWriter sw = new StreamWriter(file))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(Budget));
                xmls.Serialize(sw, budggy);

            }
        }

        public Budget Read(string filename)
        {
            FileStream file = File.Create(filename);
            using (StreamReader sr = new StreamReader(file))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(Budget));
                return xmls.Deserialize(sr) as Budget;
            }
        }
    }
}
