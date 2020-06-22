using Cgame.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public class ConsoleListener
    {
        private static Action<string> action;

        public ConsoleListener(Action<string> act)
        {
            action = act;
        }

        public void Update()
        {
            if (ConsoleControl.isShown)
            while (Console.KeyAvailable)
            {
                var command = Console.ReadLine();
                    action(command);
            }
        }
    }
}
