using Cgame.Core;
using Cgame.Core.Graphic;
using OpenTK;

namespace Cgame.objects
{
    class Platform : GameObject
    {
        public Platform()
        {
            Sprite = new Sprite("platform");
            Collider = new Collider(64, 256);
            Layer = Layer.Object;
            Mass = 0;
        }

        public Platform(Vector3 pos) : this()
        {
            Position = pos;
        }

        public Platform(GameObjectParameter parameter) : this(parameter.Position) { }
    }
}
