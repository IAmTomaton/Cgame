﻿using Cgame.Core.Graphic;
using Cgame.Core.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cgame.Core
{
    /// <summary>
    /// Класс хранящий логическое представление игры и взаимодействующий с ним.
    /// </summary>
    public class Space : ISpaceStore
    {
        public Camera Camera
        {
            get
            {
                if (!(cameraObject is null))
                    camera.Target = new Vector3(cameraObject.Position.Xy);
                return camera;
            }
        }

        
        private ISceneLoader sceneLoader;

        private Queue<GameObject> globalObjectsToAdd = new Queue<GameObject>();
        private Queue<GameObject> localObjectsToAdd = new Queue<GameObject>();

        private HashSet<GameObject> globalObjects = new HashSet<GameObject>();
        private HashSet<GameObject> localObjects = new HashSet<GameObject>();

        /// <summary>
        /// Объект к которому привязана камера.
        /// </summary>
        private GameObject cameraObject;
        private readonly Camera camera;

        public Space(Camera camera, ISceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
            this.camera = camera;
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
            cameraObject = gameObject;
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
        public IEnumerable<T> FindObject<T>() => FindObjectIn<T>(GetGameObjects());

        private IEnumerable<T> FindObjectIn<T>(IEnumerable<GameObject> objects)
        {
            return objects.Where(obj => obj is T).Cast<T>();
        }

        private void DeleteGameObjects()
        {
            localObjects.RemoveWhere(o => !o.Alive);
            globalObjects.RemoveWhere(o => !o.Alive);
        }

        public void Start()
        {
            sceneLoader.LoadNextScene();
        }
    }
}
