using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using winapi;

namespace hjy_generic
{
    public class App
    {
        /// <summary>
        /// Create a mutex for make sure the program launch once
        /// </summary>
        /// <param name="strLockName">name for mutex</param>
        /// <returns>False is running</returns>
        public bool Lock(string strLockName)
        {
            bool blCreated = new bool();
            Mutex mutex_internal = new Mutex(true, strLockName, out blCreated);
            return blCreated;
        }

        public IntPtr FindWindow(string lpClassName, string lpWindowName)
        {
            return User32_dll.FindWindow(lpClassName, lpWindowName);
        }

        public IntPtr FindWindowEX(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName)
        {
            return User32_dll.FindWindowEx(hwndParent, hwndChildAfter, lpClassName, lpWindowName);
        }

        public int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            return User32_dll.SendMessage(hWnd, Msg, wParam, lParam);
        }
        public int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            return User32_dll.PostMessage(hWnd, Msg, wParam, lParam);
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);
        public static string GetActiveWindowTitle()
        {
            const int nChars = 512;
            System.Text.StringBuilder Buff = new System.Text.StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }

    public class Log
    {
        private string strLogPath;

        public Log(string input)
        {
            strLogPath = input;
        }

        /// <summary>
        /// Write log into log file
        /// </summary>
        /// <param name="strFilePath">Target path</param>
        /// <param name="input">Content</param>
        public void WriteLine(string input)
        {
            using (StreamWriter tw = new StreamWriter(strLogPath, true))  // 'true':新建或附加.
            {
                tw.WriteLine(DateTime.Now.ToString("<MM/dd/yyyy HH:mm:ss>") + input);
            }
        }
    }

    public class Dev
    {
        public enum dev_type
        {
            Description,
            HardwareID,
            PNPDeviceID,
            PNPClass,
        }

        /// <summary>
        /// Find Device by options
        /// </summary>
        /// <param name="_type">dev_type.Description/HardwareID/PNPDeviceID/PNPClass</param>
        /// <param name="_input">Target device</param>
        /// <returns>true/false</returns>
        public static bool FindDev(dev_type _type, string _input)
        {
            bool blRet = FindDev(_type, @"Win32_PnPEntity", _input);

            return blRet;
        }

        private static List<ManagementObject> objects_internal = new List<ManagementObject>();

        /// <summary>
        /// Find Device by options
        /// </summary>
        /// <param name="_type">dev_type.Description/HardwareID/PNPDeviceID/PNPClass</param>
        /// <param name="_class">Default is Win32_PnPEntity</param>
        /// <param name="_input">Target device</param>
        /// <returns>get or not</returns>
        public static bool FindDev(dev_type _type, string _class, string _input)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + _class);//Win32_PnPEntity
            if(searcher.Get() != null)
            {

                foreach (ManagementObject device in searcher.Get())
                {
                    bool blGetThisOne = false;

                    switch (_type)
                    {
                        case dev_type.Description:
                            {
                                if(device["Description"] != null && device["Description"].ToString().ToLower().Contains(_input.ToLower()))
                                {
                                    blGetThisOne = true;
                                }
                                break;
                            }
                        case dev_type.HardwareID:
                            {
                                if(device["HardwareID"] != null && device["HardwareID"].ToString().ToLower().Contains(_input.ToLower()))
                                {
                                    blGetThisOne = true;
                                }
                                break;
                            }
                        case dev_type.PNPClass:
                            {
                                if (device["PNPClass"] != null && device["PNPClass"].ToString().ToLower().Contains(_input.ToLower()))
                                {
                                    blGetThisOne = true;
                                }
                                break;
                            }
                        case dev_type.PNPDeviceID:
                            {
                                if (device["PNPDeviceID"] != null && device["PNPDeviceID"].ToString().ToLower().Contains(_input.ToLower()))
                                {
                                    blGetThisOne = true;
                                }
                                break;
                            }
                        default:
                            {
                                
                                break;
                            }
                    }

                    if(blGetThisOne)
                    {
                        objects_internal.Add(device);
                    }
                }
            }

            if(objects_internal.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read all devices fits FindDev
        /// </summary>
        /// <returns>ManagementObjects</returns>
        public static ManagementObject[] GetDevObjects()
        {
            return objects_internal.ToArray();
        }
    }

    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(16383);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            16383, this.path);
            return temp.ToString();

        }
    }


}
