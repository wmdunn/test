using System;
using System.IO;
using System.Xml;

namespace ComparisonForm
{
    static class Config
    {
        public static XmlDocument LoadConfig(string path)
        {
            XmlDocument cfgFile = new XmlDocument();
            try
            {
                cfgFile.Load(path);
            }
            catch (Exception)
            {
                Console.Write(@"Could not load config file");
            }
            return cfgFile;
        }
    }
}
