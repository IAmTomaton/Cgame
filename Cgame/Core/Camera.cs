using System;
using System.Windows;
using OpenTK;

namespace Cgame.Core
{
    /// <summary>
    /// Объект камеры для отрисовки игровых объектов.
    /// Хранит информацию о своём положении.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Объект к которому привязана камера.
        /// </summary>
        public Vector3 Target { get; set; } = Vector3.Zero;
        /// <summary>
        /// Возвращает позицию камеры в глобальной системе координат.
        /// Задаёт позицию камеры относительно объекта привязки. Если он не задан, то задаёт позицию камеры в глобальной системе координат.
        /// </summary>
        public Vector3 Position
        {
            get { return position + Target; }
            set { position = value; }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public float WidthScale { get; }

        public float AspectRatio => Width / (float)Height;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(yaw);
            set
            {
                yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                fov = MathHelper.DegreesToRadians(angle);
            }
        }

        private Vector3 position;

        public Vector3 Front => front;
        public Vector3 Up => up;
        public Vector3 Right => right;

        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private float pitch;
        private float yaw = -MathHelper.PiOver2;
        private float fov = MathHelper.PiOver2;

        /// <summary>
        /// Создаёт экземмпляр камеры с указанной позицией, и разрешинием в пикселях.
        /// При масштабировании изображения постоянным отаётся только разрешение по высоте.
        /// </summary>
        /// <param name="position">Начальная позиция камеры.</param>
        /// <param name="width">Разрешение в пикселях по ширине.</param>
        /// <param name="height">Разрешение в пикселях по высоте.</param>
        public Camera(Vector3 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
            WidthScale = 2 / (float)Width;
        }

        public Camera(int width, int height) : this(Vector3.Zero, width, height) { }

        public Camera(Size size) : this((int)size.Width, (int)size.Height) { }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + front, up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreateScale(WidthScale) * Matrix4.CreatePerspectiveFieldOfView(fov, AspectRatio, 0.01f, 100f);
        }

        private void UpdateVectors()
        {
            front.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
            front.Y = (float)Math.Sin(pitch);
            front.Z = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

            front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }
    }
}