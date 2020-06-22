namespace Cgame.Core
{
    public static class ColliderExtension
    {
        /// <summary>
        /// Возвращает новый коллайдер который перемещён аналогично объекту.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Collider TransformToGameObject(this Collider collider, GameObject gameObject) =>
            collider.Transform(gameObject.Position.Xy, gameObject.Angle);
    }
}
