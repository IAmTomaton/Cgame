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
        public static ISpaceContext Space => IsInitialized ? SpaceUpdater.Space : null;
        /// <summary>
        /// Апдейтер игрового пространства.
        /// </summary>
        public static ISpaceUpdater SpaceUpdater { get; private set; }
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        public static float DelayTime => IsInitialized ? SpaceUpdater.DelayTime : 0 ;
        public static KeyboardDevice KeyboardDevice => Keyboard.PrimaryDevice;
        public static MouseDevice MouseDevice => Mouse.PrimaryDevice;

        public static void Init(ISpaceUpdater spaceUpdater)
        {
            SpaceUpdater = spaceUpdater;
            IsInitialized = true;
        }
    }
}
