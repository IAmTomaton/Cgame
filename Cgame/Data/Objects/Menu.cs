using Cgame.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cgame
{
    class Menu:GameObject
    {
        private List<UIElement> menuElements = new List<UIElement>();
        private Label backGroundLabel;

        private void AddStart()
        {
            var startButton = new Button();
            startButton.Width = 300;
            startButton.Height = 100;
            startButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            startButton.Margin = new Thickness(0, 20, 0, 20);
            menuElements.Add(startButton);
            startButton.Content = "START";
            startButton.Click += (sender, args) =>
            {
                foreach (var element in menuElements)
                    GameContext.GUI.RemoveUIElement(element);
                var menus = GameContext.Space.FindObject<Menu>();
                foreach (var m in menus)
                    m.Destroy();
            };
            GameContext.GUI.AddUIElement(startButton);
        }

        private void AddEnd()
        {
            var endButton = new Button();
            endButton.Width = 300;
            endButton.Height = 100;
            endButton.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            endButton.Margin = new Thickness(0, 20, 0, 20);
            menuElements.Add(endButton);
            endButton.Content = "EXIT";
            endButton.Click += (sender, args) =>
            {
                MainWindow mainWindow =
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow.Close();
            };
            GameContext.GUI.AddUIElement(endButton);
        }

        private void AddLabel()
        {
            backGroundLabel = new Label();
            backGroundLabel.Background = System.Windows.Media.Brushes.Teal;
            backGroundLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            backGroundLabel.VerticalContentAlignment = VerticalAlignment.Top;
            backGroundLabel.Foreground = System.Windows.Media.Brushes.White;
            backGroundLabel.FontFamily = new System.Windows
                .Media.FontFamily("Verdana");
            backGroundLabel.FontSize = 40;
            menuElements.Add(backGroundLabel);
            GameContext.GUI.AddUIElement(backGroundLabel);
        }

        public Menu()
        {
            AddLabel();
            AddStart();
            AddEnd();
        }

        public Menu(bool start, bool exit=false, string text=null)
        {
            AddLabel();
            if (start)
                AddStart();
            if (exit)
                AddEnd();
            if (text != null)
                backGroundLabel.Content = text;
        }
    }
}
