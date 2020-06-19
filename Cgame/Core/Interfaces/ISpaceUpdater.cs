using System.Collections.Generic;

namespace Cgame.Core.Interfaces
{
    interface ISpaceUpdater
    {
        /// <summary>
        /// Обновляет внутренне представление пространства.
        /// </summary>
        /// <param name="gameObjects">Объекты для обновления</param>
        /// <param name="delayTime">Промежуток времени прошедший с последнего обновления.</param>
        void Update(List<GameObject> gameObjects, float delayTime);
    }
}
