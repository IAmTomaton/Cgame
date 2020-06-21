using System.Windows;

namespace Cgame.Core.Interfaces
{
    interface IGUIManager
    {
        /// <summary>
        /// Добавляет элемент UI к корневому Grid.
        /// </summary>
        /// <param name="element"></param>
        void AddUIElement(UIElement element);
        /// <summary>
        /// Удаляет элемент UI из корневого Grid.
        /// </summary>
        /// <param name="element"></param>
        void RemoveUIElement(UIElement element);
    }
}
