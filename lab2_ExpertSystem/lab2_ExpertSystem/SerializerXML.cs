using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace lab2_ExpertSystem
{
    static class SerializerXML
    {
        public static Rules[] rules;
        public static void SerializeXML(string path)
        {
            XmlSerializer formatter1 = new XmlSerializer(typeof(Rules[]));

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                rules = (Rules[])formatter1.Deserialize(fs);
                for (int i = 0; i < rules.Length; i++)
                    rules[i].isExist = false;
            }
        }
    }
}
