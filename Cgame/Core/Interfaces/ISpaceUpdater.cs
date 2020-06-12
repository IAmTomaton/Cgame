using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.Core.Interfaces
{
    interface ISpaceUpdater
    {
        /// <summary>
        /// Текущаяя камера пространства.
        /// </summary>
        Camera Camera { get; }
        /// <summary>
        /// Текущее пространство.
        /// </summary>
        ISpaceStore Space { get; }
        /// <summary>
        /// Возвращает последовательность спрайтов для отрисовки.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Sprite> GetSprites();
        /// <summary>
        /// Обновляет внутренне представление пространства.
        /// </summary>
        /// <param name="delayTime">Промежуток времени прошедший с последнего обновления.</param>
        void Update(float delayTime);
        /// <summary>
        /// Запустить пространство.
        /// </summary>
        void Start();
        /// <summary>
        /// Устанавливает размеры камеры
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Resize(int width, int height);
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        float DelayTime { get; }
    }
}
