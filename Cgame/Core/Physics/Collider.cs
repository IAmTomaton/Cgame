﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cgame.Core
{
    /// <summary>
    /// Содержит информации для просчёта столкновений объекта.
    /// </summary>
    public class Collider
    {
        /// <summary>
        /// Указывает должел ли данный коллайдер участвовать в столкновениях.
        /// </summary>
        public bool IsColliding { get; set; } = true;
        /// <summary>
        /// Угол поворока коллайдера, относительно его центра.
        /// </summary>
        public float Angle { get; private set; }
        /// <summary>
        /// Позиция центра коллайдера в глобальной системе координат.
        /// </summary>
        public Vector2 Position => (new Vector3(position) * RotateObject + gameObject.Position).Xy;
        /// <summary>
        /// Радиус коллайдера. Указывает минимальный радиус круга с центром в центре коллайдера и содержащий все его точки.
        /// </summary>
        public float Radius { get; }
        /// <summary>
        /// Указывает является ли коллайдер триггером. Триггеры не участвуют в столкновениях, но вызов метода Collision происходит.
        /// </summary>
        public bool IsTrigger { get; }
        /// <summary>
        /// Список всех вершин коллайдера в глобальной системе координат.
        /// </summary>
        public List<Vector2> Vertices => vertices
                    .Select(vert => vert * RotateCollider)
                    .Select(vert => vert + new Vector3(Position.X, Position.Y, 0))
                    .Select(vert => vert.Xy)
                    .ToList();

        private Matrix3 RotateCollider => Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(Angle));
        private Matrix3 RotateObject => Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(gameObject.Angle));
        private GameObject gameObject;
        private Vector2 position;
        private readonly List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        /// Создаёт круглый коллайдер с указанным радиусом.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="radius">Радиус коллайдера.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        public Collider(GameObject gameObject, float radius, Vector2 position = default)
        {
            Init(gameObject, position, 0);
            Radius = radius;
        }

        /// <summary>
        /// Создаёт примоугольный коллайдер с указанными высотой и шириной.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="height">Высота.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(GameObject gameObject, float height, float width, Vector2 position = default, float angle = 0)
        {
            Init(gameObject, position, angle);
            vertices = new List<Vector3>
            {
                new Vector3(-width / 2, height / 2, 0),
                new Vector3(width / 2, height / 2, 0),
                new Vector3(width / 2, -height / 2, 0),
                new Vector3(-width / 2, -height / 2, 0)
            };
            Radius = vertices[0].Length;
        }

        /// <summary>
        /// Создаёт коллайдер с указанным набором вершин.
        /// </summary>
        /// <param name="gameObject">Обект-родитель коллайдера.</param>
        /// <param name="vertices">Список вершин. Перечисление по часовой стрелке.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(GameObject gameObject, List<Vector2> vertices, Vector2 position = default, float angle = 0)
        {
            Init(gameObject, position, angle);
            this.vertices = vertices
                .Select(vert => new Vector3(vert.X, vert.Y, 0))
                .ToList();
            Radius = vertices
                .Select(vert => vert.Length)
                .Max();
        }

        /// <summary>
        /// Возвращает список нормалей ко всем граням коллайдера.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vector2> GetNornals()
        {
            var vertices = Vertices;
            for (var i = 0; i < vertices.Count; i++)
            {
                yield return (vertices[i] - vertices[(i + 1) % vertices.Count]).PerpendicularLeft.Normalized();
            }
        }

        private void Init(GameObject gameObject, Vector2 position, float angle)
        {
            this.gameObject = gameObject;
            this.position = position;
            Angle = angle;
        }

        public static Collision Collide(Collider firstCollider, Collider secondCollider)
        {
            if (firstCollider.Radius + secondCollider.Radius <= Vector2.Distance(firstCollider.Position, secondCollider.Position))
                return Collision.FalseCollision;
            if (firstCollider.Vertices.Count == 0 && secondCollider.Vertices.Count == 0)
            {
                return new Collision(
                    (firstCollider.Position - secondCollider.Position).Normalized(),
                    Math.Abs(
                        firstCollider.Radius +
                        secondCollider.Radius -
                        Vector2.Distance(firstCollider.Position, secondCollider.Position)));
            }
            return CollisionPolygons(firstCollider, secondCollider);
        }

        private static Collision CollisionPolygons(Collider firstCollider, Collider secondCollider)
        {
            var mtv = default(Vector2);
            var minMTVLength = 0f;
            var first = true;

            foreach (var normal in firstCollider.GetNornals().Concat(secondCollider.GetNornals()))
            {
                Vector2 firstProjection = GetProjection(normal, firstCollider);
                Vector2 secondProjection = GetProjection(normal, secondCollider);

                if (firstProjection.X < secondProjection.Y || secondProjection.X < firstProjection.Y)
                    return Collision.FalseCollision;

                if (first)
                {
                    first = false;
                    mtv = normal.Normalized();
                    minMTVLength = GetIntersectionLength(firstProjection, secondProjection);
                    continue;
                }

                float mtvLength = GetIntersectionLength(firstProjection, secondProjection);
                if (Math.Abs(mtvLength) < Math.Abs(minMTVLength))
                {
                    mtv = normal.Normalized();
                    minMTVLength = mtvLength;
                }
            }

            return new Collision(mtv, Math.Abs(minMTVLength));
        }

        private static Vector2 GetProjection(Vector2 vector, Collider collider)
        {
            var vertices = collider.Vertices;
            Vector2 result = default;

            if (vertices.Count == 0)
            {
                var projection = Vector2.Dot(vector, collider.Position);
                return new Vector2(projection + collider.Radius, projection - collider.Radius);
            }

            for (var i = 0; i < vertices.Count; i++)
            {
                var projection = Vector2.Dot(vector, vertices[i]);

                if (i == 0)
                    result = new Vector2(projection, projection);

                if (projection > result.X)
                    result.X = projection;

                if (projection < result.Y)
                    result.Y = projection;
            }

            return result;
        }

        private static float GetIntersectionLength(Vector2 firstProjection, Vector2 secondProjection)
        {
            return secondProjection.Y - firstProjection.X > 0
              ? secondProjection.Y - firstProjection.X
              : firstProjection.Y - secondProjection.X;
        }
    }
}
