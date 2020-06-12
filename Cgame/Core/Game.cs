using Cgame.Core.Interfaces;

namespace Cgame.Core
{
    class Game : IGame
    {
        private readonly ISpaceUpdater spaceUpdater;
        private readonly IPainter painter;

        public Game(ISpaceUpdater spaceUpdater, IPainter painter)
        {
            this.spaceUpdater = spaceUpdater;
            this.painter = painter;
            GameContext.Init(spaceUpdater);
        }

        public void Resize(int width, int height)
        {
            spaceUpdater.Resize(width, height);
        }

        public void Start()
        {
            spaceUpdater.Start();
        }

        public void Update(float delayTime)
        {
            spaceUpdater.Update(delayTime);
            painter.Draw(spaceUpdater.GetSprites(), spaceUpdater.Camera);
        }

    }
}
