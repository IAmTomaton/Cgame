using Cgame.Core;
using OpenTK;

namespace Cgame.objects
{
    class Platform : GameObject
    {
        public Platform()
        {
            Sprite = new Sprite(this, "platform");
            Collider = new Collider(64, 256);
            Layer = Layer.Object;
            Mass = 0;
        }

        public Platform(Vector3 pos) : this()
        {
            Position = pos;
        }

        public Platform(int x, int y) : this()
        {
            Position = new Vector3(x,y,0);
        }
    }
}
