using Cgame.Core;
using Cgame.Core.Interfaces;
using Cgame.objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    static class SceneProcesser
    {
        private static Stack<GameObject> gameObjectsStack = new Stack<GameObject>();
        private static Dictionary<string, Func<GameObjectParameter, GameObject>> creationDict
            = new Dictionary<string, Func<GameObjectParameter, GameObject>>()
            {
                {"player", p=>new Player((PlayerObjectParameter)p)},
                {"platform", p=>new Platform(p)},
                { "obstacle", p=>new Obstacle(p)}
            };

        public static GameObject CreateObjectToAdd(string[] commandParts, ISpaceContext uc)
        {
            GameObject newObject = null;
            int x = 0; int y = 0; bool gr = true; bool jumps = true;
            MovementType move = MovementType.Continuos; ShootType shoot = ShootType.CreateBullet;
            var toInitPlayer = new List<Func<string, bool>>()
                    {
                        s=>int.TryParse(s, out x),
                        s=>int.TryParse(s, out y),
                        s=>bool.TryParse(s, out gr),
                        s=>bool.TryParse(s, out jumps),
                        s=>Enum.TryParse(s, out move),
                        s=>Enum.TryParse(s, out shoot)
                    };
            bool ifParsed = commandParts
                .Skip(3)
                .Select((s, i) => toInitPlayer[i](s))
                .Aggregate((f, s) => f && s);
            if (ifParsed)
            {
                if (commandParts[2] == "player")
                    newObject = creationDict[commandParts[2]]
                     (new PlayerObjectParameter(new Vector3(x, y, 0), gr, jumps, move, shoot));
                else
                    newObject = creationDict[commandParts[2]]
                     (new GameObjectParameter(new Vector3(x, y, 0)));
                gameObjectsStack.Push(newObject);
            }
            else
                Console.WriteLine("some mistake in scene command(parameters of object)");
            return newObject;
        }

        public static (bool ifAdded, GameObject newObject) Process(string command, ISpaceContext uc)
        {
            GameObject newObject = null;
            var commandParts = command.Split();
            var add = false;
            switch (commandParts[0])
            {
                case "add":
                    add = true;
                    break;
                case "cancel":
                    if (gameObjectsStack.Count() != 0)
                    {
                        var lastObj = gameObjectsStack.Pop();
                        uc.DeleteObject(lastObj);
                    }
                    else Console.WriteLine("some mistake in scene command(add/delete)");
                    break;
                default:
                    Console.WriteLine("some mistake in scene command(add/delete)");
                    break;
            }
            if (add)
            {
                try
                {
                    newObject = CreateObjectToAdd(commandParts, uc);
                    if (newObject != null)
                    {
                        if (commandParts[1] == "local")
                            uc.AddLocalObject(newObject);
                        else if (commandParts[1] == "global")
                            uc.AddGlobalObject(newObject);
                        else
                            Console.WriteLine("wrong command.need local/global");
                    }
                    else Console.WriteLine("null object created");

                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine("some mistake in scene command(type of object)");
                }
            }
            return (add, newObject);
        }
    }
}
