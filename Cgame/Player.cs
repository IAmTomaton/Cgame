using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cgame.Core;
using Cgame.Interfaces;
using OpenTK;
using OpenTK.Input;
using System.Windows.Input;
using Key = System.Windows.Input.Key;
using System.Windows.Controls;
using Cgame.Core.Graphic;

namespace Cgame
{
    public enum MovementType
    {
        NoAcceleration,//no force
        Continuos,//just move forward all the time
        UserAcceleration//user makes player to move forward
    }

    public enum ShootType
    {
        Raycast,
        CreateBullet
    }

    public class Player : GameObject
    {
        public bool IsAlive { get; private set; }
        private Vector2 vert_acc = new Vector2(0, -10);
        private Vector2 horiz_acc = new Vector2(-10, 0);
        private static Vector2 up = Vector2.UnitY;
        private static Vector2 right = Vector2.UnitX;
        private bool isJumpingUp = false;
        private bool isFallingAfterJump = false;
        public bool isShooting = false;
        private static float defaultSpeedX = 100;

        private bool hasGravity = true;
        private bool hasJumps = true;
        private MovementType movementType = MovementType.Continuos;
        private ShootType shootType=ShootType.CreateBullet;

        public Player() : base()
        {
            IsAlive = true;
            Sprite = new Sprite(new[] { "player1", "player2"});
            Collider = new Collider(48, 48);
            Layer = Layer.Player;
            Mass = 10;
            Velocity = new Vector2(defaultSpeedX,0);
        }

        public Player(Vector3 pos, bool hasGravity=true, bool hasJumps=true, 
            MovementType movementType=MovementType.Continuos, 
            ShootType shootType=ShootType.CreateBullet) : this()
        {
            Position = pos;
            this.movementType = movementType;
            this.hasGravity = hasGravity;
            this.shootType = shootType;
            if (movementType == MovementType.NoAcceleration || movementType == MovementType.UserAcceleration)
                defaultSpeedX = 0;
        }

        public Player(int x, int y) : this()
        {
            Position = new Vector3(x, y, 0);
        }

        public Player(PlayerObjectParameter parameters)
            : this(parameters.Position, parameters.HasGravity, parameters.HasJumps,
                  parameters.MovementType, parameters.ShootType)
        {}

        public override void Start()
        {
            GameContext.Space.BindGameObjectToCamera(this);
            base.Start();
        }

        public override void OnCollision(GameObject other)
        {
            if (other.Mass == 0)
            {
                if (isFallingAfterJump)
                {
                    isFallingAfterJump = false;
                }
            }
            if (other is IKilling)
            {
                Sprite.StepForward();
                IsAlive = false;
                Collider = null;
                Velocity = vert_acc = horiz_acc = new Vector2(0,0);
            }

            base.OnCollision(other);
        }

        /// <summary>
        /// Finds projection of the p to line. {p1;p2} defines line
        /// </summary>
        /// <param name="p">Point to project</param>
        /// <param name="p1">First point of line segment</param>
        /// <param name="p2">Second point of line segment</param>
        /// <returns></returns>
        public static Vector2 GetProjection(Vector2 p, Vector2 p1, Vector2 p2)
        {
            float fDenominator = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            if (fDenominator == 0) // p1 and p2 are the same
                return p1;
            float t = (p.X * (p2.X - p1.X) - (p2.X - p1.X) * p1.X 
                + p.Y * (p2.Y - p1.Y) - (p2.Y - p1.Y) * p1.Y) / fDenominator;
            return new Vector2(p1.X + (p2.X - p1.X) * t, p1.Y + (p2.Y - p1.Y) * t);
        }


        /// <summary>
        /// Raycast way of shooting
        /// </summary>
        public void ShootStraight(Vector2 shootDirection)
        {
            var start = new Vector2(this.Position.X, this.Position.Y<0?0:this.Position.Y);
            if (GameContext.Space.FindLocalObject<IShootable>().Count() != 0)
            {
                var objectsToShoot = GameContext.Space.FindLocalObject<IShootable>().Cast<GameObject>();
                var circle = objectsToShoot.ElementAt(0).Collider;
                var projection = GetProjection(circle.Position, start, start + shootDirection);
                var x = projection.X;
                var y = projection.Y;
                //right way too
                /*var t = (circle.Position.X - start.X) / (rs.X - rs.Y);
                var x = start.X + rs.X * t;
                var y = start.Y + rs.Y * t;*/
                var d = Math.Sqrt((x - circle.Position.X) * (x - circle.Position.X) +
                    (y - circle.Position.Y) * (y - circle.Position.Y));
                var n = Math.Sqrt(Math.Abs(circle.Radius * circle.Radius - d * d));
                Vector2 res = new Vector2(x, y) - new Vector2((float)n * shootDirection.X, (float)n * shootDirection.Y);
                Console.WriteLine(res.ToString());
                if (res.Length <= 300)
                {
                    objectsToShoot.ElementAt(0).Destroy();
                }
            }
        }

        public void SendBullet(Vector2 shootDirection,float range, float speed)
        {
            GameContext.Space.AddLocalObject(new Bullet(this, shootDirection, this.Position, range, speed));
            isShooting = true;
        }

        public override void Update()
        {
            var input = GameContext.KeyboardDevice;
            var dt = GameContext.DelayTime;
            if (IsAlive)
            {
                if (input.IsKeyDown(Key.Z) && !isShooting)
                {
                    switch (shootType)
                    {
                        case ShootType.CreateBullet:
                            SendBullet(new Vector2(1, 0), 300, defaultSpeedX*10);
                            break;
                        case ShootType.Raycast:
                            ShootStraight(new Vector2(1, 0));
                            break;
                    }
                }
                if (hasJumps)
                {
                    if (input.IsKeyDown(Key.W) && !isJumpingUp && !isFallingAfterJump)
                    {
                        Velocity += 550 * up;
                        isJumpingUp = true;
                    }
                    if (isJumpingUp && Velocity.Y == 0)
                    {
                        isJumpingUp = false;
                        isFallingAfterJump = true;
                    }
                }
                if (movementType == MovementType.NoAcceleration)
                {
                    if (input.IsKeyDown(Key.A))
                        Velocity -= 200 * right;
                    if (input.IsKeyDown(Key.D))
                        Velocity += 200 * right;
                }
                if (Velocity.Y < -100)
                {
                    Velocity = new Vector2(defaultSpeedX, -100);
                }
                if (movementType == MovementType.UserAcceleration)
                {
                    if (input.IsKeyDown(Key.D))
                    {
                        Velocity += 40 * right;
                    }
                    if (Velocity.X > 0)
                        Velocity += horiz_acc;
                }
                if (hasGravity)
                    Velocity += vert_acc;
                Position += new Vector3(Velocity.X * dt, Velocity.Y * dt, 0);
                if (Position.Y < -500)
                {
                    Sprite.StepForward();
                    IsAlive = false;
                }
            }
            base.Update();
        }
    }
}
