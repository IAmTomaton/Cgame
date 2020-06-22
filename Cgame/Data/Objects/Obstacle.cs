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

        public Obstacle(GameObjectParameter parameter) : this(parameter.Position) 
        { }
    }
}
