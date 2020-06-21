using Cgame.Core.Interfaces;
using System;
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
        public static ISpaceContext Space
        {
            get { return IsInitialized ? space : throw new Exception(exceptionText + "Space"); }
        }
        /// <summary>
        /// GUI.
        /// </summary>
        public static IGUIManager GUI
        {
            get { return IsInitialized ? gui : throw new Exception(exceptionText + "GUI"); }
        }
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        public static float DelayTime { get; private set; }
        public static KeyboardDevice KeyboardDevice => Keyboard.PrimaryDevice;
        public static MouseDevice MouseDevice => Mouse.PrimaryDevice;

        private static ISpaceContext space;
        private static IGUIManager gui;
        private static readonly string exceptionText =
            "Доступ к полю GameContext, до его инициализации. Убедитесь что вы обращаетесь к полю только внутри методов Update или Start. Поле: ";

        public static void Init(ISpaceContext spaceContext, IGUIManager GUIManager)
        {
            space = spaceContext;
            gui = GUIManager;
            IsInitialized = true;
        }

        public static void Update(float delayTime)
        {
            DelayTime = delayTime;
        }
    }
}
