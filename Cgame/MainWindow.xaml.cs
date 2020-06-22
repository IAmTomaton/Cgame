﻿using Cgame.Core;
using Cgame.Core.Graphic;
using Cgame.Core.Interfaces;
using Ninject;
using OpenTK;
using System.Timers;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cgame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private readonly Timer timer = new Timer(10);

        public MainWindow()
        {
            InitializeComponent();
            ConsoleControl.HideWindow();
        }

        private StandardKernel GetConteiner()
        {
            var conteiner = new StandardKernel();
            conteiner.Bind<Game>().To<Game>();
            conteiner.Bind<Size>().ToConstant(new Size(gLControl.Width, gLControl.Height));
            conteiner.Bind<Shader>().ToConstant(new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/shader.frag"));
            conteiner.Bind<ITextureLibrary>().To<TextureLibrary>();
            conteiner.Bind<GLControl>().ToConstant(gLControl);
            conteiner.Bind<Grid>().ToConstant(GUI);
            conteiner.Bind<IGUIManager>().To<GUIManager>();
            conteiner.Bind<ISpaceUpdater>().To<SpaceUpdater>();
            conteiner.Bind<IPainter>().To<Painter>();
            conteiner.Bind<ISpaceStore>().To<Space>().InSingletonScope();
            conteiner.Bind<Camera>().ToSelf();
            return conteiner;
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            gLControl.MakeCurrent();
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            game = GetConteiner().Get<Game>();
            game.Start();
            timer.Elapsed += (_, __) => gLControl.Invalidate();
            timer.Start();
        }

        private void GLControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            game.Update((float)timer.Interval / 1000);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (game != null)
                game.Resize(gLControl.Width, gLControl.Height);
        }
    }
}
