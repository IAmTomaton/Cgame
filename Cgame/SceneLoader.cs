using Cgame.Core;
using Cgame.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cgame
{
    static class SceneLoader
    {
        private static List<string> scenes = new List<string>
        {
            "Resources/Scenes/scene1.txt",
            "Resources/Scenes/scene2.txt"
        };
        private static int currentSceneNumber = -1;
        private static Scene currentScene = null;

        public static void Update()
        {
            if (!(currentScene is null) && currentScene.IsEnded ||
                GameContext.Space.FindLocalObject<GameObject>().Count()==0)
            {
                GameContext.Space.ClearLocals();
                LoadNextScene();
            }
        }

        public static void LoadNextScene()
        {
            currentSceneNumber += 1;
            var path = scenes[currentSceneNumber % scenes.Count()];
            bool playerAdded = false;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var pair = new SceneProcesser().Process(line);
                    if (pair.ifAdded && pair.newObject is Player player)
                    {
                        currentScene = new Scene(player, new OpenTK.Vector3(-900, 0, 0),
                            new OpenTK.Vector3(1100, 0, 0));
                        playerAdded = true;
                    }
                }
            }
            if (!playerAdded)
                Console.WriteLine("No player in the scene");
        }
    }
}
