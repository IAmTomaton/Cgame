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
        public Player Player { get; private set; }
        public bool IsEnded => Player.Position.X > endPosition.X;
        public bool IsLast { get; private set; }

        public Scene(Player player, Vector3 startOfScene, 
            Vector3 endOfScene, bool last)
        {
            IsLast = last;
            this.Player = player;
            startPosition = startOfScene;
            endPosition = endOfScene;
        }
    }
}
