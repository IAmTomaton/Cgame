using Cgame.Core.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Cgame.Core
{
    class GUIManager : IGUIManager
    {
        private readonly Grid Grid;

        public GUIManager(Grid grid)
        {
            Grid = grid;
        }

        public void AddUIElement(UIElement element)
        {
            if (element is null) throw new ArgumentNullException("Добавляемый UI элемент не может быть null.");
            if (!Grid.Children.Contains(element)) Grid.Children.Add(element);
        }

        public void RemoveUIElement(UIElement element)
        {
            if (element is null) throw new ArgumentNullException("Удаляемый UI элемент не может быть null.");
            if (Grid.Children.Contains(element)) Grid.Children.Remove(element);
        }
    }
}
