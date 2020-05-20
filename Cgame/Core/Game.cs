using Cgame.Core.Interfaces;

namespace Cgame.Core
{
    class Game : IGame
    {
        private readonly ISpace space;
        private readonly IPainter painter;

        public Game(ISpace space, IPainter painter)
        {
            this.space = space;
            this.painter = painter;
            GameContext.Init(space);
        }

        public void Resize(int width, int height)
        {
            space.Resize(width, height);
        }

        public void Update(float delayTime)
        {
            space.Update(delayTime);
            painter.Draw(space.GetSprites(), space.Camera);
        }


    }
}
