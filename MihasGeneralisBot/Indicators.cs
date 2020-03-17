using System;
using System.Collections.Generic;
using System.Text;

namespace MihasGeneralisBot
{
    class Indicators
    {
        public string counterNumber { get; set; }
        public string counterIndicators { get; set; }
        public string dateOfIndicate { get; set; }

        public void sendIndicators()
        {
            System.IO.File.WriteAllText($"D://indicators/{dateOfIndicate} {counterNumber}.txt", counterIndicators);
        }
    }
}
