using Cgame.Core;
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
using Ninject.Extensions.Factory;
using Ninject.Parameters;
using System.Reflection;
using Cgame.objects;
using Cgame.Interfaces;
using System.ComponentModel;

namespace Cgame
{
    public class UseFirstArgumentAsNameInstanceProvider : StandardInstanceProvider
    {
        protected override string GetName(System.Reflection.MethodInfo methodInfo,
            object[] arguments)
        {
            return (string)arguments[0];
        }

        protected override IConstructorArgument[]
            GetConstructorArguments(MethodInfo methodInfo, object[] arguments)
        {
            var parameters = methodInfo.GetParameters();
            var constructorArguments =
                new ConstructorArgument[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                constructorArguments[i] =
                    new ConstructorArgument
                        (parameters[i].Name, arguments[i], true);
            }
            var resArray = constructorArguments.Skip(1).ToArray();
            return resArray;
        }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private readonly Timer timer = new Timer(10);
        public static StandardKernel Conteiner { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ConsoleControl.HideWindow();
        }

        public StandardKernel GetConteiner()
        {
            var conteiner = new StandardKernel();
            conteiner.Bind<Game>().To<Game>();
            conteiner.Bind<Size>().ToConstant(new Size(gLControl.Width, gLControl.Height));
            conteiner.Bind<Shader>().ToConstant(new Shader("Resources/Shaders/shader.vert",
                "Resources/Shaders/shader.frag"));
            conteiner.Bind<ITextureLibrary>().To<TextureLibrary>();
            conteiner.Bind<GLControl>().ToConstant(gLControl);
            conteiner.Bind<Grid>().ToConstant(GUI);
            conteiner.Bind<IGUIManager>().To<GUIManager>();
            conteiner.Bind<ISpaceUpdater>().To<SpaceUpdater>();
            conteiner.Bind<IPainter>().To<Painter>();
            conteiner.Bind<ISpaceStore>().To<Space>().InSingletonScope();
            conteiner.Bind<Camera>().ToSelf();
            conteiner.Bind<GameObject>().To<Player>().Named("Player");
            conteiner.Bind<GameObject>().To<Obstacle>().Named("Obstacle");
            conteiner.Bind<GameObject>().To<Platform>().Named("Platform");
            conteiner.Bind<IGameObjectFactory>()
                .ToFactory(() => new UseFirstArgumentAsNameInstanceProvider());
            Conteiner = conteiner;
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
