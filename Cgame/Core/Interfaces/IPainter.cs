using System.Collections.Generic;

namespace Cgame.Core.Interfaces
{
    interface IPainter
    {
        /// <summary>
        /// Отрисовывает последовательность спрайтов относительно камеры.
        /// </summary>
        /// <param name="sprites"></param>
        /// <param name="camera"></param>
        void Draw(IEnumerable<Sprite> sprites, Camera camera);
    }
}
