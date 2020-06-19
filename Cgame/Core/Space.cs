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

        private Queue<GameObject> objectsToDelete = new Queue<GameObject>();
        private Queue<GameObject> globalObjectsToDelete = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToDelete = new Queue<GameObject>();
        private Queue<GameObject> globalObjectsToAdd = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToAdd = new Queue<GameObject>();

        private List<GameObject> globalObjects = new List<GameObject>();
        private List<GameObject> localObjects = new List<GameObject>();

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
            localObjectsToDelete = new Queue<GameObject>();
            localObjectsToAdd = new Queue<GameObject>();
            localObjects = new List<GameObject>();
        }

        public void ClearGlobals()
        {
            globalObjectsToDelete = new Queue<GameObject>();
            globalObjectsToAdd = new Queue<GameObject>();
            globalObjects = new List<GameObject>();
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
                AddObjectTo(globalObjectsToAdd.Dequeue(), globalObjects);
            while (localObjectsToAdd.Count > 0)
                AddObjectTo(localObjectsToAdd.Dequeue(), localObjects);
        }

        private void AddObjectTo(GameObject gameObject, List<GameObject> gameObjects)
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

        public void DeleteLocalObject(GameObject gameObject) => localObjectsToDelete.Enqueue(gameObject);
        public void DeleteGlobalObject(GameObject gameObject) => globalObjectsToDelete.Enqueue(gameObject);
        public void DeleteObject(GameObject gameObject) => objectsToDelete.Enqueue(gameObject);

        private void DeleteGameObjects()
        {
            while (globalObjectsToDelete.Count > 0)
                DeleteObjectFrom(globalObjectsToDelete.Dequeue(), globalObjects);
            while (localObjectsToDelete.Count > 0)
                DeleteObjectFrom(localObjectsToDelete.Dequeue(), localObjects);
            while (objectsToDelete.Count > 0)
            {
                var toDelete = objectsToDelete.Dequeue();
                DeleteObjectFrom(toDelete, localObjects);
                DeleteObjectFrom(toDelete, globalObjects);
            }
        }

        private void DeleteObjectFrom(GameObject gameObject, List<GameObject> gameObjects)
        {
            if (gameObjects.Contains(gameObject))
                gameObjects.Remove(gameObject);
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
