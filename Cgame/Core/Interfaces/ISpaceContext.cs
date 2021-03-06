﻿using System.Collections.Generic;
using System.Windows;

namespace Cgame.Core.Interfaces
{
    /// <summary>
    /// Коктекст пространства.
    /// Позволяет получитьь доступ к пространству из игровых объектов.
    /// </summary>
    public interface ISpaceContext
    {
        void ClearGlobals();
        void ClearLocals();
        /// <summary>
        /// Добавляет локальный объект в пространство.
        /// Локальные объекты удаляются при переходе между сценами.
        /// </summary>
        /// <param name="gameObject"></param>
        void AddLocalObject(GameObject gameObject);
        /// <summary>
        /// Добавляет локальные объекты из последовательности в пространство.
        /// Глобальные объекты не удаляются при переходе между сценами.
        /// </summary>
        /// <param name="gameObject"></param>
        void AddLocalObjects(IEnumerable<GameObject> gameObjects);
        /// <summary>
        /// Добавляет глобальный объект в пространство.
        /// Глобальные объекты не удаляются при переходе между сценами.
        /// </summary>
        /// <param name="gameObject"></param>
        void AddGlobalObject(GameObject gameObject);
        /// <summary>
        /// Добавляет глобальные объекты из последовательности в пространство.
        /// Глобальные объекты не удаляются при переходе между сценами.
        /// </summary>
        /// <param name="gameObject"></param>
        void AddGlobalObjects(IEnumerable<GameObject> gameObjects);
        /// <summary>
        /// Возвращает все локальные объекты указанного типа в пространстве.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> FindLocalObject<T>();
        /// <summary>
        /// Возвращает все глобальные объекты указанного типа в пространстве.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> FindGlobalObject<T>();
        /// <summary>
        /// Возвращает все объекты указанного типа в пространстве.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> FindObject<T>();
        /// <summary>
        /// Определяет, существует ли локальный объект в пространстве.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        bool LocalObjectExistence(GameObject gameObject);
        /// <summary>
        /// Определяет, существует ли глобальный объект в пространстве.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        bool GlobalObjectExistence(GameObject gameObject);
        /// <summary>
        /// Устанавливает объект, за ктоторым будет следить камера.
        /// </summary>
        /// <param name="gameObject"></param>
        void BindGameObjectToCamera(GameObject gameObject);
    }
}
