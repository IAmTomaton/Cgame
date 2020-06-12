using Cgame.Core.Interfaces;
using Cgame.objects;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Cgame.Core
{
    /// <summary>
    /// Класс хранящий логическое представление игры и взаимодействующий с ним.
    /// </summary>
    class Space : ISpaceStore
    {
        public Camera Camera { get; private set; }
        public Grid GUI { get; private set; }

        private readonly Queue<GameObject> objectsToDelete = new Queue<GameObject>();
        private readonly Queue<GameObject> globalObjectsToDelete = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToDelete = new Queue<GameObject>();
        private readonly Queue<GameObject> globalObjectsToAdd = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToAdd = new Queue<GameObject>();

        private readonly List<GameObject> globalCollidingObjects = new List<GameObject>();
        private List<GameObject> localCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> globalNonCollidingObjects = new List<GameObject>();
        private List<GameObject> localNonCollidingObjects = new List<GameObject>();

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

        public Space(Camera camera, Grid gui)
        {
            Camera = camera;
            GUI = gui;
            camera.Position = Vector3.UnitZ * 500;
        }

        public void Update()
        {
            DeleteGameObjects();
            AddGameObjects();
        }

        public IEnumerable<GameObject> GetGameObjects() => AllObgects;

        public IEnumerable<GameObject> GetCollidingObjects() => CollidingObjects;

        public void ClearLocals()
        {
            localObjectsToDelete = new Queue<GameObject>();
            localObjectsToAdd = new Queue<GameObject>();
            localCollidingObjects = new List<GameObject>();
            localNonCollidingObjects = new List<GameObject>();
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
                var toDelete = objectsToDelete.Dequeue();
                DeleteObjectFrom(toDelete, localCollidingObjects, localNonCollidingObjects);
                DeleteObjectFrom(toDelete, globalCollidingObjects, globalNonCollidingObjects);
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

        public void AddUIElement(UIElement element)
        {
            if (!GUI.Children.Contains(element))
                GUI.Children.Add(element);
        }

        public void RemoveUIElement(UIElement element)
        {
            if (GUI.Children.Contains(element))
                GUI.Children.Remove(element);
        }

        public void Start()
        {
            SceneLoader.LoadNextScene();
        }
    }
}
