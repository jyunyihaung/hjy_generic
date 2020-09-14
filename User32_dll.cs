using System;
using System.Runtime.InteropServices;

namespace winapi
{
    public class User32_dll
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        /// <summary>
        /// 自定義的結構
        /// </summary>

        // 注意：必須是結構體不能是類即必須為struct關鍵字不能是class,否則在接收訊息時會產生異常 
        public struct My_lParam
        {
            public int i;
            public string s;
        }
        /// <summary>
        /// 使用COPYDATASTRUCT來傳遞字串
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        //訊息傳送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            int lParam          //引數2
        );


        //訊息傳送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            ref My_lParam lParam //引數2
        );

        //訊息傳送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            ref COPYDATASTRUCT lParam  //引數2
        );

        //訊息傳送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            int lParam            // 引數2
        );



        //訊息傳送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            ref My_lParam lParam //引數2
        );

        //非同步訊息傳送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 資訊發往的視窗的控制代碼
            int Msg,            // 訊息ID
            int wParam,         // 引數1
            ref COPYDATASTRUCT lParam  // 引數2
        );

    }
}