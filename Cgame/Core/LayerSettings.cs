using System;

namespace Cgame.Core
{
    /// <summary>
    /// Слои столкновений.
    /// Обекты сталкиваюся на основании их слоя и правил в LayerSettings.
    /// </summary>
    public enum Layer
    {
        Base,
        Background,
        Platform,
        Enemy,
        Player,
        Object
    }
    /// <summary>
    /// Класс определяет правила столкновения объектов на основании их слоя.
    /// </summary>
    static class LayerSettings
    {
        private static readonly bool[,] collisionRules;

        static LayerSettings()
        {
            var layersCount = Enum.GetNames(typeof(Layer)).Length;
            collisionRules = new bool[layersCount, layersCount];
            InitRules();
        }

        /// <summary>
        /// В данном методе нужно добавлять правила столкновений в соответствии с шаблоном:
        /// AddCollisionRule(Layers.НазваниеПервогоСлоя, Layers.НазваниеВторгоСлоя)
        /// Прорядок указания слоёв не важен.
        /// </summary>
        private static void InitRules()
        {
            AddCollisionRule(Layer.Player, Layer.Platform);
            AddCollisionRule(Layer.Player, Layer.Object);
            AddCollisionRule(Layer.Object, Layer.Platform);
            //AddCollisionRule(Layers.Object, Layers.Object);
        }

        /// <summary>
        /// Проверяет должны ли объекты из указанных слоёв сталкиваться.
        /// </summary>
        /// <param name="firstLayer"></param>
        /// <param name="secondLayer"></param>
        /// <returns></returns>
        public static bool CheckCollision(Layer firstLayer, Layer secondLayer)
        {
            return collisionRules[(int)secondLayer, (int)firstLayer];
        }


        /// <summary>
        /// Добавляет правило столкновения указанных слоёв.
        /// </summary>
        /// <param name="firstLayer"></param>
        /// <param name="secondLayer"></param>
        /// <param name="collision">Указывает должны ли объекты сталкиваться.</param>
        public static void AddCollisionRule(Layer firstLayer, Layer secondLayer, bool collision = true)
        {
            collisionRules[(int)firstLayer, (int)secondLayer] = collision;
            collisionRules[(int)secondLayer, (int)firstLayer] = collision;
        }
    }
}
