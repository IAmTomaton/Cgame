using OpenTK;

namespace Cgame.Core.Graphic
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
        public Vector3 Position { get; set; }
        /// <summary>
        /// Угол поворота спайта вокруг его центра. Совпадает с углом поворота объекта-родителя.
        /// </summary>
        public float Angle { get; set; }
        /// <summary>
        /// Текущий индекс текстуры в списке текстур спрайта.
        /// </summary>
        public int CurrentIndex { get; private set; }
        /// <summary>
        /// Количество текстур в спрайте.
        /// </summary>
        public int Count => textures.Length;

        private readonly string[] textures;

        /// <summary>
        /// Создаёт спрайт с одной указанной текстурой.
        /// </summary>
        /// <param name="texture">Имя текстуры в библиотеке текстур.</param>
        public Sprite(string texture)
        {
            textures = new string[] { texture };
        }

        /// <summary>
        /// Создаёт спрайт с указанным списом текстур.
        /// </summary>
        /// <param name="textures">Массив имён текстур в библиотеке текстур</param>
        public Sprite(string[] textures)
        {
            this.textures = textures;
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
