using Cgame.Core.Interfaces;
using System.Windows.Input;

namespace Cgame.Core
{
    /// <summary>
    /// Статический класс для управления пространством.
    /// </summary>
    static class GameContext
    {
        public static bool IsInitialized { get; private set; }
        /// <summary>
        /// Игровое пространство.
        /// </summary>
        public static ISpaceContext Space => s;
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        public static float DelayTime => Space.DelayTime;
        public static KeyboardDevice KeyboardDevice => Keyboard.PrimaryDevice;
        public static MouseDevice MouseDevice => Mouse.PrimaryDevice;

        private static ISpace s;

        public static void Init(ISpace space)
        {
            s = space;

            IsInitialized = true;
        }
    }
}
