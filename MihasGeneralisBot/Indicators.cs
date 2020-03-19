using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MihasGeneralisBot
{
    class Indicators
    {
        public string counterNumber { get; set; }
        public string counterIndicators { get; set; }
        public string dateOfIndicate { get; set; }

        public void sendIndicators(string path)
        {
            Directory.CreateDirectory(path);
            
            File.WriteAllText(Path.Combine(path, $"{dateOfIndicate} {counterNumber}.txt"), counterIndicators);
        }
    }
}
