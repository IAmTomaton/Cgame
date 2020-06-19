using Cgame.Core.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cgame.Core
{
    /// <summary>
    /// Класс хранящий логическое представление игры и взаимодействующий с ним.
    /// </summary>
    class Space : ISpaceStore
    {
        public Camera Camera { get; private set; }
        public Grid GUI { get; private set; }

        private Queue<GameObject> globalObjectsToAdd = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToAdd = new Queue<GameObject>();

        private HashSet<GameObject> globalObjects = new HashSet<GameObject>();
        private HashSet<GameObject> localObjects = new HashSet<GameObject>();

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

        public IEnumerable<GameObject> GetGameObjects() => globalObjects.Concat(localObjects);

        public void ClearLocals()
        {
            localObjectsToAdd = new Queue<GameObject>();
            localObjects = new HashSet<GameObject>();
        }

        public void ClearGlobals()
        {
            globalObjectsToAdd = new Queue<GameObject>();
            globalObjects = new HashSet<GameObject>();
        }

        public void Resize(int width, int height)
        {
            Camera.Width = width;
            Camera.Height = height;
        }

        public void BindGameObjectToCamera(GameObject gameObject)
        {
            if (gameObject is null) throw new ArgumentNullException("GameObject к которому можно прикрепить камеру не может быть null.");
            Camera.GameObject = gameObject;
        }

        public void AddLocalObject(GameObject gameObject) => AddGameObjectTo(gameObject, localObjectsToAdd);
        public void AddGlobalObject(GameObject gameObject) => AddGameObjectTo(gameObject, globalObjectsToAdd);

        public void AddLocalObjects(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
                AddGameObjectTo(gameObject, localObjectsToAdd);
        }

        public void AddGlobalObjects(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
                AddGameObjectTo(gameObject, globalObjectsToAdd);
        }

        private void AddGameObjectTo(GameObject gameObject, Queue<GameObject> gameObjects)
        {
            if (gameObject is null) throw new ArgumentNullException("Добавляемый GameObject не может быть null.");
            gameObjects.Enqueue(gameObject);
        }

        private void AddGameObjects()
        {
            while (globalObjectsToAdd.Count > 0)
                AddObjectTo(globalObjectsToAdd.Dequeue(), globalObjects);
            while (localObjectsToAdd.Count > 0)
                AddObjectTo(localObjectsToAdd.Dequeue(), localObjects);
        }

        private void AddObjectTo(GameObject gameObject, HashSet<GameObject> gameObjects)
        {
            if (LocalObjectExistence(gameObject) || GlobalObjectExistence(gameObject))
                return;
            gameObjects.Add(gameObject);
            gameObject.Start();
        }

        public bool LocalObjectExistence(GameObject gameObject) => localObjects.Contains(gameObject);
        public bool GlobalObjectExistence(GameObject gameObject) => globalObjects.Contains(gameObject);

        public IEnumerable<T> FindLocalObject<T>() => FindObjectIn<T>(localObjects);
        public IEnumerable<T> FindGlobalObject<T>() => FindObjectIn<T>(globalObjects);

        private IEnumerable<T> FindObjectIn<T>(IEnumerable<GameObject> objects)
        {
            return objects.Where(obj => obj is T).Cast<T>();
        }

        private void DeleteGameObjects()
        {
            localObjects.RemoveWhere(o => !o.Alive);
            globalObjects.RemoveWhere(o => !o.Alive);
        }

        public void AddUIElement(UIElement element)
        {
            if (element is null) throw new ArgumentNullException("Добавляемый UI элемент не может быть null.");
            if (!GUI.Children.Contains(element)) GUI.Children.Add(element);
        }

        public void RemoveUIElement(UIElement element)
        {
            if (element is null) throw new ArgumentNullException("Удаляемый UI элемент не может быть null.");
            if (GUI.Children.Contains(element)) GUI.Children.Remove(element);
        }

        public void Start()
        {
            SceneLoader.LoadNextScene();
        }
    }
}
