using Cgame.Core.Interfaces;
using System.Linq;

namespace Cgame.Core
{
    class Game
    {
        private readonly ISpaceUpdater spaceUpdater;
        private readonly ISpaceStore spaceStore;
        private readonly IPainter painter;

        public Game(ISpaceUpdater spaceUpdater, ISpaceStore spaceStore, IPainter painter)
        {
            this.spaceUpdater = spaceUpdater;
            this.spaceStore = spaceStore;
            this.painter = painter;
        }

        public void Resize(int width, int height)
        {
            spaceStore.Resize(width, height);
        }

        public void Start()
        {
            GameContext.Init(spaceStore);
            spaceStore.Start();
        }

        public void Update(float delayTime)
        {
            GameContext.Update(delayTime);
            spaceStore.Update();
            spaceUpdater.Update(spaceStore.GetGameObjects().ToList(), delayTime);
            var sprites = spaceStore.GetGameObjects().Where(o => o.Sprite != null).Select(o => o.Sprite);
            painter.Draw(sprites, spaceStore.Camera);
        }
    }
}
