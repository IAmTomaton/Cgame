using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    public class GameObjectParameter
    {
        public Vector3 Position { get; private set; }
        public GameObjectParameter(Vector3 pos)
        {
            Position = pos;
        }
    }

    public class PlayerObjectParameter : GameObjectParameter
    {
        public bool HasGravity { get; private set; }
        public bool HasJumps { get; private set; }
        public MovementType MovementType { get; private set; }
        public ShootType ShootType { get; private set; }

        public PlayerObjectParameter(Vector3 pos, bool hasGravity = true, bool hasJumps = true,
            MovementType movementType = MovementType.Continuos, 
            ShootType shootType = ShootType.CreateBullet) : base(pos)
        {
            this.HasGravity = hasGravity;
            this.HasJumps = hasJumps;
            this.MovementType = movementType;
            this.ShootType = shootType;
        }
    }
}
