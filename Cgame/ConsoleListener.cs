using Cgame.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public static class ConsoleListener
    {
        public static void Update()
        {
            if (ConsoleControl.isShown)
            while (Console.KeyAvailable)
            {
                var command = Console.ReadLine();
                    if (command == "end")
                        ConsoleControl.HideWindow();
                    else
                        SceneProcesser.Process(command);
            }
        }
    }
}
