using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cgame.Core;
using Cgame.Interfaces;
using OpenTK;
using OpenTK.Input;

namespace Cgame
{
    class Bullet : GameObject
    {
        private static float defaultSpeedX = 5f;
        private Vector3 startPosition;
        private float range;
        private Player player;

        public Bullet(Player player, Vector2 direction, Vector3 start, float range, float speed=2f) : base()
        {
            this.player = player;
            this.range = range;
            Sprite = new Sprite(this, "bullet");
            Layer = Layer.Player;
            Collider = new Collider(this, 16, 16);
            Mass = 0.05f;
            Position = start + new Vector3(50,0,0);
            var normalized = direction.Normalized();
            Velocity = new Vector2(normalized.X*speed, normalized.Y*speed);
        }

        public override void Start()
        {
            startPosition = Position;
            base.Start();
        }

        public override void Update()
        {
            if ((Position - startPosition).Length >= range)
            {
                player.isShooting = false;
                GameContext.Space.DeleteLocalObject(this);
            }
            base.Update();
        }

        public override void Collision(GameObject other)
        {
            if (other is IShootable)
                GameContext.Space.DeleteLocalObject(other);
            base.Collision(other);
        }
    }
}
