using OpenTK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public class Scene
    {
        private Vector3 startPosition;
        private Vector3 endPosition;
        private Player player;
        public bool IsEnded => player.Position.X > endPosition.X;

        public Scene(Player player, Vector3 startOfScene, Vector3 endOfScene)
        {
            this.player = player;
            startPosition = startOfScene;
            endPosition = endOfScene;
        }
    }
}
