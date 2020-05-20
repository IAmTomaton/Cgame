using OpenTK;

namespace Cgame.Core
{
    public abstract class GameObject
    {
        //GameObject Copy();
        /// <summary>
        /// Спрайт объекта.
        /// </summary>
        public Sprite Sprite { get;set; }
        /// <summary>
        /// Масса обекта.
        /// Если Mass = 0, то объект считается статичным и не смещается при коллизии.
        /// </summary>
        public float Mass { get;set; }
        /// <summary>
        /// Указывает слой для столкновений.
        /// default: Layers.Base
        /// </summary>
        public Layer Layer { get; set; }
        /// <summary>
        /// Коллайдер объекта.
        /// Если экземпляр объекта был добавлен в пространство без коллайдера, то он никогда не будет участвовать в столкновениях.
        /// </summary>
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        /// <summary>
        /// На данный момент не нужен, но чтобы не дописывать его вызов во всех потомках когда он понадобится он тут есть.
        /// </summary>
        public GameObject() { }

        /// <summary>
        /// Вызывается единственный раз для экземпляра, сразу после добавления в пространство.
        /// </summary>
        /// <param name="updateContext"></param>
        public virtual void Start() { }

        /// <summary>
        /// Вызывается на каждом обновлении пространства.
        /// </summary>
        /// <param name="updateContext"></param>
        public virtual void Update() { }

        /// <summary>
        /// Вызывается если произошла коллизия.
        /// </summary>
        /// <param name="updateContext"></param>
        /// <param name="other">Объект с которым произошла коллизия.</param>
        public virtual void Collision(GameObject other) { }
    }
}
