using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

using hjy_generic;

namespace hjy_generic_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log(@".\testlog.log");
            log.WriteLine(@"hjy_generic_example start");

            App app = new App();
            bool blLocked = app.Lock("hello");
            log.WriteLine(@"blLocked = " + blLocked.ToString());

            if(!blLocked)
            {
                log.WriteLine("other same app running");
                return;
            }

            bool blResult = Dev.FindDev(Dev.dev_type.Description, @"Microsoft Usbccid Smartcard Reader (WUDF)");

            ManagementObject[] devices = Dev.GetDevObjects();

            foreach(ManagementObject device in devices)
            {
                log.WriteLine(device.ToString());
                log.WriteLine(@"Description: " +    ((device["Description"].ToString().Length > 0) ? (device["Description"].ToString()) : (@"Fail")));
                log.WriteLine(@"HardwareID: " +     ((device["HardwareID" ].ToString().Length > 0) ? (device["HardwareID" ].ToString()) : (@"Fail")));
                log.WriteLine(@"PNPDeviceID: " +    ((device["PNPDeviceID"].ToString().Length > 0) ? (device["PNPDeviceID"].ToString()) : (@"Fail")));
                log.WriteLine(@"PNPClass: " +       ((device["PNPClass"   ].ToString().Length > 0) ? (device["PNPClass"   ].ToString()) : (@"Fail")));
            }
        }
    }
}
