using Cgame.Core;
using Cgame.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    static class SceneLoader
    {
        private static List<string> scenes = new List<string>
        {
            "Resources/Scenes/scene1.txt"
        };
        private static int currentScene = -1;

        public static void LoadNextScene(ISpaceContext uc)
        {
            currentScene += 1;
            var path = scenes[currentScene % scenes.Count()];
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ConsoleListener.Process(line, uc);
                }
            }
        }
    }
}
