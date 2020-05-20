namespace Cgame.Core.Interfaces
{
    /// <summary>
    /// Объект хранящий всё остальное.
    /// </summary>
    interface IGame
    {
        /// <summary>
        /// Обновляет игру.
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
