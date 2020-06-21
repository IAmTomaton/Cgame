using System.Collections.Generic;

namespace Cgame.Core.Interfaces
{
    interface ISpaceStore : ISpaceContext
    {
        /// <summary>
        /// Текущаяя камера пространства.
        /// </summary>
        Camera Camera { get; }
        /// <summary>
        /// Возвращает все объекты пространства.
        /// </summary>
        /// <returns></returns>
        IEnumerable<GameObject> GetGameObjects();
        /// <summary>
        /// Устанавливает размеры камеры
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Resize(int width, int height);
        /// <summary>
        /// Обновляет внутренне представление пространства.
        /// </summary>
        void Update();
        /// <summary>
        /// Запустить пространство.
        /// </summary>
        void Start();
    }
}
