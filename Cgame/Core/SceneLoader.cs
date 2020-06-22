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
    public class SceneLoader:ISceneLoader
    {
        private static List<string> scenes = new List<string>
        {
            "Resources/Scenes/scene1.txt",
            "Resources/Scenes/scene2.txt",
            "Resources/Scenes/scene3.txt"
        };
        private static int currentSceneNumber = -1;
        private Scene currentScene = null;
        private ISceneProcesser sceneProcesser;

        public SceneLoader(ISceneProcesser sceneProcesser)
        {
            this.sceneProcesser = sceneProcesser;
        }

        public void Update()
        {
            if (!(currentScene is null) && (currentScene.IsLast||
                currentScene.IsEnded) ||
                GameContext.Space.FindLocalObject<GameObject>().Count()==0)
            {
                if (!(currentScene is null) && currentScene.IsLast)
                    GameContext.Space.AddLocalObject(new Menu(false, true, null));
                GameContext.Space.ClearLocals();
                LoadNextScene();
            }
        }

        public void LoadNextScene()
        {
            currentSceneNumber += 1;
            var path = scenes[currentSceneNumber % scenes.Count()];
            bool playerAdded = false;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var pair = sceneProcesser.Process(line);
                    if (pair.ifAdded && pair.newObject is Player player)
                    {
                        currentScene = new Scene(player, new OpenTK.Vector3(-900, 0, 0),
                            new OpenTK.Vector3(1100, 0, 0), false);
                        playerAdded = true;
                    }
                    if (!pair.ifAdded)
                        currentScene = new Scene(null, new OpenTK.Vector3(-900, 0, 0),
                            new OpenTK.Vector3(1100, 0, 0), true);
                }
            }
            if (!playerAdded)
                Console.WriteLine("No player in the scene");
        }
    }
}
