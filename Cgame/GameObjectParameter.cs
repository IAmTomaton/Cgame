using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class GameObjectParameter
    {
        public Vector3 Position;
        public GameObjectParameter(Vector3 pos)
        {
            Position = pos;
        }
    }

    class PlayerObjectParameter : GameObjectParameter
    {
        public bool hasGravity;
        public bool hasJumps;
        public MovementType movementType;
        public ShootType shootType;
        public PlayerObjectParameter(Vector3 pos, bool hasGravity = true, bool hasJumps = true,
            MovementType movementType = MovementType.Continuos, 
            ShootType shootType = ShootType.CreateBullet) : base(pos)
        {
            this.hasGravity = hasGravity;
            this.hasJumps = hasJumps;
            this.movementType = movementType;
            this.shootType = shootType;
        }
    }
}
