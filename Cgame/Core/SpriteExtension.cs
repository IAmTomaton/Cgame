using Cgame.Core.Graphic;
using OpenTK;

namespace Cgame.Core
{
    public static class SpriteExtension
    {
        public static void TransformToGameObject(this Sprite sprite, GameObject gameObject)
        {
            sprite.Position = gameObject.Position;
            sprite.Angle = MathHelper.DegreesToRadians(gameObject.Angle);
        }
    }
}
