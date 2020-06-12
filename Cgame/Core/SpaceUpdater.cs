using Cgame.Core.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.Core
{
    class SpaceUpdater : ISpaceUpdater
    {
        public float DelayTime { get; private set; }
        public ISpaceStore Space { get; }

        public SpaceUpdater(ISpaceStore space)
        {
            Space = space;
        }

        public Camera Camera => Space.Camera;

        public IEnumerable<Sprite> GetSprites() => Space.GetGameObjects().Select(o => o.Sprite);

        public void Resize(int width, int height) => Space.Resize(width, height);

        public void Update(float delayTime)
        {
            DelayTime = delayTime;
            MoveGameObjects(delayTime);
            UpdateGameObjects();
            Space.Update();
            CollisionCheck();
        }

        /// <summary>
        /// Перемещает все игровые объекты в соответствии с их скоростью.
        /// </summary>
        private void MoveGameObjects(float delayTime)
        {
            foreach (var gameObject in Space.GetGameObjects())
                MoveGameObject(gameObject, delayTime);
        }

        /// <summary>
        /// Перемещает игровой объект в соответствии с его скоростью.
        /// </summary>
        private void MoveGameObject(GameObject gameObject, float delayTime)
        {
            gameObject.Position += new Vector3(gameObject.Velocity.X * delayTime, gameObject.Velocity.Y * delayTime, 0);
        }

        /// <summary>
        /// Обновляет все игровые объекты.
        /// </summary>
        private void UpdateGameObjects()
        {
            foreach (var gameObject in Space.GetGameObjects())
                gameObject.Update();
            ConsoleControl.Update();
            ConsoleListener.Update();
            SceneLoader.Update();
        }

        /// <summary>
        /// Проверяет столкновения для всех сталкиваемых игровых объектах.
        /// </summary>
        private void CollisionCheck()
        {
            var objects = Space.GetCollidingObjects().ToList();
            for (var i = 0; i < objects.Count; i++)
                for (var j = i + 1; j < objects.Count; j++)
                {
                    if (!LayerSettings.CheckCollision(objects[i].Layer, objects[j].Layer))
                        continue;
                    if (objects[i].Collider is null || objects[j].Collider is null)
                        continue;
                    var collision = Collider.Collide(objects[i].Collider, objects[j].Collider);
                    if (!collision.Collide)
                        continue;
                    if (!objects[i].Collider.IsTrigger && !objects[j].Collider.IsTrigger)
                    {
                        var massSum = objects[i].Mass + objects[j].Mass;
                        DisplacementObjectAfterCollision(objects[i], massSum, collision, 1);
                        DisplacementObjectAfterCollision(objects[j], massSum, collision, -1);
                    }
                    objects[i].Collision(objects[j]);
                    objects[j].Collision(objects[i]);
                }
        }

        /// <summary>
        /// Перемещает столкнувшийся объект.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="massSum"></param>
        /// <param name="collision"></param>
        /// <param name="revers"></param>
        private void DisplacementObjectAfterCollision(GameObject gameObject, float massSum, Collision collision, int revers)
        {
            if (gameObject.Mass == 0)
                return;
            var ratio = massSum == gameObject.Mass ? 1 : (massSum - gameObject.Mass) / massSum;
            var delta = collision.Mtv * ratio * collision.MtvLength;
            gameObject.Position += new Vector3(delta) * revers;
        }

        public void Start()
        {
            Space.Start();
        }
    }
}
