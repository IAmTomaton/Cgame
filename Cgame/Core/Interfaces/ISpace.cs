using OpenTK.Input;
using System.Collections.Generic;

namespace Cgame.Core.Interfaces
{
    interface ISpace : ISpaceContext
    {
        /// <summary>
        /// Текущаяя камера пространства.
        /// </summary>
        Camera Camera { get; }
        /// <summary>
        /// Возвращает последовательность спрайтов для отрисовки.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Sprite> GetSprites();
        /// <summary>
        /// Обновляет внутренне представление пространства.
        /// </summary>
        /// <param name="delayTime">Промежуток времени прошедший с последнего обновления.</param>
        /// <param name="keyboardState"></param>
        /// <param name="mouseState"></param>
        void Update(float delayTime);
        /// <summary>
        /// Устанавливает размеры камеры
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Resize(int width, int height);
    }
}
