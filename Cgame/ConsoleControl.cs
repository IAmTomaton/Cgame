using Cgame.Core;
using Cgame.Core.Interfaces;
using Cgame.objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using System.Windows.Input;
using Key = System.Windows.Input.Key;
using System.Windows.Forms.VisualStyles;

namespace Cgame
{
    public static class ConsoleControl
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private static System.IntPtr handle;
        public static bool isShown { get; private set; }

        static ConsoleControl()
        {
            isShown = true;
            handle = GetConsoleWindow();
        }

        public static void HideWindow()
        {
            ShowWindow(handle, SW_HIDE);
            isShown = false;
        }

        public static void ShowWindow()
        {
            ShowWindow(handle, SW_SHOW);
            isShown = true;
        }

        public static void Update()
        {
            var keyBoard = GameContext.KeyboardDevice;
            if (keyBoard.IsKeyDown(Key.K))
            {
                if (isShown)
                    HideWindow();
                else ShowWindow();
            }
        }
    }
}
