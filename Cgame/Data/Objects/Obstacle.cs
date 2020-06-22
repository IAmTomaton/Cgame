using Cgame.Core;
using Cgame.Core.Graphic;
using Cgame.Interfaces;
using OpenTK;

namespace Cgame.objects
{
    class Obstacle : GameObject, IKilling, IShootable
    {
        public Obstacle()
        {
            Sprite = new Sprite("triangle");
            Collider = new Collider(48, 48);
            Layer = Layer.Object;
            Mass = 1;
        }

        public Obstacle(Vector3 pos):this()
        {
            Position = pos;
        }

        public Obstacle(int x, int y) : this()
        {
            Position = new Vector3(x, y, 0);
        }

        public Obstacle(GameObjectParameter parameter) : this(parameter.Position) 
        { }
    }
}
