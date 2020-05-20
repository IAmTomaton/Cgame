using Cgame.Core;
using Cgame.Core.Interfaces;
using Cgame.objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    //refactoring!!!!!!!!!!!!!!!!!!!!
    static class ConsoleListener
    {
        //should we use events in programm???
        //private IUpdateContext prevIUpdateContext = null;
        public static Stack<GameObject> gameObjectsStack = new Stack<GameObject>();
        private static Dictionary<string, Func<GameObjectParameter,GameObject>> creationDict 
            = new Dictionary<string, Func<GameObjectParameter,GameObject>>()
            {
                {"player", p=>new Player((PlayerObjectParameter)p)},
                {"platform", p=>new Platform(p)},
                { "obstacle", p=>new Obstacle(p)}
            };

        public static void Update(ISpaceContext updateContext)
        {
            //while (Console.KeyAvailable)
            //{
            //    var command = Console.ReadLine();
            //    Process(command, updateContext);
            //    //Console.WriteLine(updateContext.FindLocalObject<Obstacle>().Count());
            //}
            
        }

        public static void Process(string command, ISpaceContext uc)
        {
            var commandParts = command.Split();
            List<GameObject> listToAdd = null;
            var add = false;
            GameObject newObject = null;
            switch (commandParts[0])
            {
                case "add":
                    //listToAdd = uc.objectsToAdd;
                    add = true;
                    break;
                case "cancel":
                    var lastObj = gameObjectsStack.Pop();
                    //uc.objectsToDelete.Add(lastObj);
                    uc.DeleteObject(lastObj);
                    break;
                default:
                    //it is possible to throw exception here
                    Console.WriteLine("some mistake in scene command(add/delete)");
                    break;
            }
            //if (listToAdd != null)
            if (add)
            {
                try
                {
                    int x=0; int y=0; bool gr=true; bool jumps=true; 
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
                        .Skip(2)
                        .Select((s, i) => toInitPlayer[i](s))
                        .Aggregate((f, s) => f && s);
                    if (ifParsed)
                    {
                        if (commandParts[1]=="player")
                            newObject = creationDict[commandParts[1]]
                             (new PlayerObjectParameter(new Vector3(x, y, 0), gr, jumps, move, shoot));
                        else
                            newObject = creationDict[commandParts[1]]
                             (new GameObjectParameter(new Vector3(x, y, 0)));
                        gameObjectsStack.Push(newObject);
                    }
                    else
                        Console.WriteLine("some mistake in scene command(parameters of object)");
                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine("some mistake in scene command(type of object)");
                }
                uc.AddLocalObject(newObject);
            }
        }
    }
}
