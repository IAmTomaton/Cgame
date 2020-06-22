using Cgame.Core;
using Cgame.Core.Interfaces;
using Cgame.Core.Graphic;
using Cgame.Interfaces;
using Cgame.objects;
using Ninject;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Ninject.Extensions.Factory;

namespace Cgame
{
    public interface IGameObjectFactory
    {
        GameObject CreateGameObject(string name);
    }

    public class SceneProcesser
    {
        private static Stack<GameObject> gameObjectsStack = new Stack<GameObject>();

        public GameObject CreateObjectToAdd(string[] commandParts)
        {
            GameObject newObject = null;
            int x = 0; int y = 0;
            var toInit = new List<Func<string, bool>>()
            {
                s=>int.TryParse(s, out x),
                s=>int.TryParse(s, out y)
            };
            bool ifParsed = commandParts.Skip(3)
                .Select((s, i) => toInit[i](s))
                .Aggregate((f, s) => f && s);
            if (ifParsed)
            {
                var factory = MainWindow.Conteiner.Get<IGameObjectFactory>();
                var name = commandParts[2];
                try
                {
                    newObject = factory.CreateGameObject(name);
                    newObject.Position = new Vector3(x, y, 0);
                }
                catch (Ninject.ActivationException e)
                {
                    Console.WriteLine("nonexistent gameobject");
                }
                gameObjectsStack.Push(newObject);
            }
            else Console.WriteLine("some mistake in scene command(parameters of object)");
            return newObject;
        }

        public GameObject AddActions(string[] commandParts, GameObject newObject)
        {
            newObject = CreateObjectToAdd(commandParts);
            if (newObject != null)
            {
                if (commandParts[1] == "local")
                    GameContext.Space.AddLocalObject(newObject);
                else if (commandParts[1] == "global")
                    GameContext.Space.AddGlobalObject(newObject);
                else
                    Console.WriteLine("wrong command.need local/global");
            }
            else Console.WriteLine("null object created");
            return newObject;
        }

        public void CancelActions()
        {
            if (gameObjectsStack.Count() != 0)
            {
                var lastObj = gameObjectsStack.Pop();
                lastObj.Destroy();
            }
            else Console.WriteLine("no actions to cancel");
        }

        public  (bool ifAdded, GameObject newObject) Process(string command)
        {
            GameObject newObject = null;
            var commandParts = command.Split();
            switch (commandParts[0])
            {
                case "add":
                    var obj = AddActions(commandParts,newObject);
                    return (true, obj);
                case "cancel":
                    CancelActions();
                    return (false, newObject);
                default:
                    Console.WriteLine("some mistake in scene command(add/cancel)");
                    return (false, newObject);
            }
        }
    }
}
