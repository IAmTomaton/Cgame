using Cgame.Core.Graphic;
using OpenTK;

namespace Cgame.Core
{
    public static class CameraExtension
    {
        public static void TransformToGameObject(this Camera camera, GameObject gameObject) =>
            camera.Position = gameObject is null ? camera.Position : new Vector3(camera.Position.Xy) + camera.Position;
    }
}
