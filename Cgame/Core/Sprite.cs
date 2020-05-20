using OpenTK;

namespace Cgame.Core
{
    /// <summary>
    /// Содержит информации для отрисовки объекта.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Текущее имя текстуры в библиотеке текстур.
        /// </summary>
        public string Texture { get { return textures[CurrentIndex]; } }
        /// <summary>
        /// Позиция спайта в глобальной системе координат. Совпадает с координатами объекта-родителя.
        /// </summary>
        public Vector3 Position => gameObject.Position;
        /// <summary>
        /// Угол поворота спайта вокруг его центра. Совпадает с углом поворота объекта-родителя.
        /// </summary>
        public float Angle => MathHelper.DegreesToRadians(gameObject.Angle);
        /// <summary>
        /// Текущий индекс текстуры в списке текстур спрайта.
        /// </summary>
        public int CurrentIndex { get; private set; }
        /// <summary>
        /// Количество текстур в спрайте.
        /// </summary>
        public int Count => textures.Length;

        private readonly GameObject gameObject;
        private readonly string[] textures;

        /// <summary>
        /// Создаёт спрайт с одной указанной текстурой.
        /// </summary>
        /// <param name="gameObject">Объект-родитель.</param>
        /// <param name="texture">Имя текстуры в библиотеке текстур.</param>
        public Sprite(GameObject gameObject, string texture)
        {
            textures = new string[] { texture };
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Создаёт спрайт с указанным списом текстур.
        /// </summary>
        /// <param name="gameObject">Объект-родитель.</param>
        /// <param name="textures">Массив имён текстур в библиотеке текстур</param>
        public Sprite(GameObject gameObject, string[] textures)
        {
            this.textures = textures;
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Устанавливает следующую текстуру из списка текстур как текущую.
        /// </summary>
        public void StepForward() => Step(1);

        /// <summary>
        /// Устанавливает предыдущую текстуру из списка текстур как текущую.
        /// </summary>
        public void StepBack() => Step(-1);

        /// <summary>
        /// Сдвигает индекс текущей текстура на указанное число.
        /// </summary>
        public void Step(int count) => SetIndex(CurrentIndex + count);

        /// <summary>
        /// Устанавливает текстуру из списка текстур с указанным индексом как текущую.
        /// </summary>
        public void SetIndex(int index)
        {
            CurrentIndex = (index % Count + Count) % Count;
        }
    }
}
