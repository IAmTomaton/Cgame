using Cgame.Core.Interfaces;
using Cgame.objects;
using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;

namespace Cgame.Core
{
    /// <summary>
    /// Класс хранящий логическое представление игры и взаимодействующий с ним.
    /// </summary>
    class Space : ISpace
    {
        public float DelayTime { get; private set; }
        public Camera Camera { get; private set; }

        private readonly Queue<GameObject> objectsToDelete = new Queue<GameObject>();
        private readonly Queue<GameObject> globalObjectsToDelete = new Queue<GameObject>();
        private readonly Queue<GameObject> localObjectsToDelete = new Queue<GameObject>();
        private readonly Queue<GameObject> globalObjectsToAdd = new Queue<GameObject>();
        private readonly Queue<GameObject> localObjectsToAdd = new Queue<GameObject>();

        private readonly List<GameObject> globalCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> localCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> globalNonCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> localNonCollidingObjects = new List<GameObject>();

        private IEnumerable<GameObject> AllObgects => globalCollidingObjects
            .Concat(localCollidingObjects)
            .Concat(globalNonCollidingObjects)
            .Concat(localNonCollidingObjects);
        private IEnumerable<GameObject> CollidingObjects => globalCollidingObjects
            .Concat(localCollidingObjects);
        private IEnumerable<GameObject> LocalObjects => localCollidingObjects
            .Concat(localNonCollidingObjects);
        private IEnumerable<GameObject> GlobalObjects => globalCollidingObjects
            .Concat(globalNonCollidingObjects);

        public Space(Camera camera)
        {
            Camera = camera;
            camera.Position = Vector3.UnitZ * 500;
            SceneLoader.LoadNextScene(this);
        }

        public void Update(float delayTime)
        {
            DelayTime = delayTime;
            MoveGameObjects();
            UpdateGameObjects();
            DeleteGameObjects();
            AddGameObjects();
            CollisionCheck();
        }

        public IEnumerable<Sprite> GetSprites()
        {
            return AllObgects
                .Where(obj => !(obj.Sprite is null))
                .Select(obj => obj.Sprite);
        }

        public void Resize(int width, int height)
        {
            Camera.Width = width;
            Camera.Height = height;
        }

        public void BindGameObjectToCamera(GameObject gameObject)
        {
            Camera.GameObject = gameObject;
        }

        /// <summary>
        /// Перемещает все игровые объекты в соответствии с их скоростью.
        /// </summary>
        private void MoveGameObjects()
        {
            foreach (var gameObject in AllObgects)
                MoveGameObject(gameObject);
        }

        /// <summary>
        /// Перемещает игровой объект в соответствии с его скоростью.
        /// </summary>
        private void MoveGameObject(GameObject gameObject)
        {
            gameObject.Position += new Vector3(gameObject.Velocity.X * DelayTime, gameObject.Velocity.Y * DelayTime, 0);
            //if (gameObject is Player)
            //    Console.WriteLine("after move " + gameObject.Position.ToString());
        }

        /// <summary>
        /// Обновляет все игровые объекты.
        /// </summary>
        private void UpdateGameObjects()
        {
            foreach (var gameObject in AllObgects)
                gameObject.Update();
            ConsoleListener.Update(this);
        }

        /// <summary>
        /// Проверяет столкновения для всех сталкиваемых игровых объектах.
        /// </summary>
        private void CollisionCheck()
        {
            var objects = CollidingObjects.ToList();
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
            /*if (gameObject is Player)
            {
                Console.WriteLine("ratio " + ratio.ToString()+"delta "+delta.ToString());
                Console.WriteLine("after collision " + gameObject.Position.ToString() + "reverse" + revers.ToString());
            }*/
        }

        public void AddLocalObject(GameObject gameObject) => localObjectsToAdd.Enqueue(gameObject);
        public void AddGlobalObject(GameObject gameObject) => globalObjectsToAdd.Enqueue(gameObject);
        public void AddLocalObjects(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
                localObjectsToAdd.Enqueue(gameObject);
        }
        public void AddGlobalObjects(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
                globalObjectsToAdd.Enqueue(gameObject);
        }

        private void AddGameObjects()
        {
            while (globalObjectsToAdd.Count > 0)
                AddObjectTo(globalObjectsToAdd.Dequeue(), globalCollidingObjects, globalNonCollidingObjects);
            while (localObjectsToAdd.Count > 0)
                AddObjectTo(localObjectsToAdd.Dequeue(), localCollidingObjects, localNonCollidingObjects);
        }

        private void AddObjectTo(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding)
        {
            if (LocalObjectExistence(gameObject) || GlobalObjectExistence(gameObject))
                return;
            if (gameObject.Collider is null)
                nonColliding.Add(gameObject);
            else
                colliding.Add(gameObject);
            gameObject.Start();
        }

        public bool LocalObjectExistence(GameObject gameObject) => LocalObjects.Contains(gameObject);
        public bool GlobalObjectExistence(GameObject gameObject) => GlobalObjects.Contains(gameObject);

        public IEnumerable<T> FindLocalObject<T>() => FindObjectIn<T>(LocalObjects);
        public IEnumerable<T> FindGlobalObject<T>() => FindObjectIn<T>(GlobalObjects);

        private IEnumerable<T> FindObjectIn<T>(IEnumerable<GameObject> objects)
        {
            return objects.Where(obj => obj is T).Cast<T>();
            //return objects.Cast<T>();
        }

        public void DeleteLocalObject(GameObject gameObject) => localObjectsToDelete.Enqueue(gameObject);
        public void DeleteGlobalObject(GameObject gameObject) => globalObjectsToDelete.Enqueue(gameObject);
        public void DeleteObject(GameObject gameObject) => objectsToDelete.Enqueue(gameObject);

        private void DeleteGameObjects()
        {
            while (globalObjectsToDelete.Count > 0)
                DeleteObjectFrom(globalObjectsToDelete.Dequeue(), globalCollidingObjects, globalNonCollidingObjects);
            while (localObjectsToDelete.Count > 0)
                DeleteObjectFrom(localObjectsToDelete.Dequeue(), localCollidingObjects, localNonCollidingObjects);
            while (objectsToDelete.Count > 0)
            {
                DeleteObjectFrom(objectsToDelete.Dequeue(), localCollidingObjects, localNonCollidingObjects);
                DeleteObjectFrom(objectsToDelete.Dequeue(), globalCollidingObjects, globalNonCollidingObjects);
            }
        }

        private void DeleteObjectFrom(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding)
        {
            if (colliding.Contains(gameObject))
            {
                colliding.Remove(gameObject);
                return;
            }
            else if (nonColliding.Contains(gameObject))
            {
                nonColliding.Remove(gameObject);
                return;
            }
        }
    }
}
