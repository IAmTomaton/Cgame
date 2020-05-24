using OpenTK.Input;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Cgame.Core.Interfaces
{
    /// <summary>
    /// Коктекст пространства.
    /// Позволяет получитьь доступ к пространству из игровых объектов.
    /// </summary>
    public interface ISpaceContext
    {
        //IUpdateContext Copy();
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
        /// Удаляет локальный объект из пространства.
        /// </summary>
        /// <param name="gameObject"></param>
        void DeleteLocalObject(GameObject gameObject);
        /// <summary>
        /// Удаляет глобальный объект из пространства.
        /// </summary>
        /// <param name="gameObject"></param>
        void DeleteGlobalObject(GameObject gameObject);
        /// <summary>
        /// Удаляет объект из пространства.
        /// </summary>
        /// <param name="gameObject"></param>
        void DeleteObject(GameObject gameObject);
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
        /// <summary>
        /// Промежуток времени прошедший с последнего обновления.
        /// </summary>
        float DelayTime { get; }
        /// <summary>
        /// Добавляет элемент UI к корневому Grid.
        /// </summary>
        /// <param name="element"></param>
        void AddUIElement(UIElement element);
        /// <summary>
        /// Удаляет элемент UI из корневого Grid.
        /// </summary>
        /// <param name="element"></param>
        void RemoveUIElement(UIElement element);
    }
}
