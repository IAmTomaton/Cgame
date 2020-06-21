using OpenTK;

namespace Cgame.Core
{
    /// <summary>
    /// Класс для проверки коллизии между объектами и хранении информации о ней.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// Указывает вектор нормали к какой-то грани. Вдоль этого вектора нужно перемещать объекты.
        /// </summary>
        public Vector2 Mtv { get; }
        /// <summary>
        /// Указывает растояние на которое суммарно нужно раздвинуть объекты.
        /// </summary>
        public float MtvLength { get; }
        /// <summary>
        /// Указывает роизошла ли коллизия.
        /// </summary>
        public bool Collide { get; }
        /// <summary>
        /// Возвращает ложную коллизию.
        /// </summary>
        public static Collision FalseCollision => new Collision();

        /// <summary>
        /// Возвращает коллизию с указанными параметрами.
        /// </summary>
        public Collision(Vector2 mtv, float mtvLength)
        {
            Mtv = mtv;
            MtvLength = mtvLength;
            Collide = true;
        }

        /// <summary>
        /// Возвращает ложную коллизию.
        /// </summary>
        public Collision() { }
    }
}
