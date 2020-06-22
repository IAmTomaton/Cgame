using OpenTK;
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
        public float Angle { get; set; }
        /// <summary>
        /// Позиция центра коллайдера в глобальной системе координат.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Радиус коллайдера. Указывает минимальный радиус круга с центром в центре коллайдера и содержащий все его точки.
        /// </summary>
        public float Radius { get; }
        /// <summary>
        /// Указывает является ли коллайдер триггером. Триггеры не участвуют в столкновениях, но вызов метода Collision происходит.
        /// </summary>
        public bool IsTrigger { get; set; }
        /// <summary>
        /// Список всех вершин коллайдера в глобальной системе координат.
        /// </summary>
        public List<Vector2> Vertices => vertices
            .Select(vert => vert.Xy)
            .ToList();

        private readonly List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        /// Создаёт круглый коллайдер с указанным радиусом.
        /// </summary>
        /// <param name="radius">Радиус коллайдера.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        public Collider(float radius, Vector2 position = default, bool trigger = false)
        {
            Position = position;
            IsTrigger = trigger;
            Angle = 0;
            Radius = radius;
        }

        /// <summary>
        /// Создаёт примоугольный коллайдер с указанными высотой и шириной.
        /// </summary>
        /// <param name="height">Высота.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(float height, float width, Vector2 position = default, float angle = 0, bool trigger = false)
        {
            Position = position;
            IsTrigger = trigger;
            Angle = angle;
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
        /// <param name="vertices">Список вершин. Перечисление по часовой стрелке.</param>
        /// <param name="position">Позиция центра коллайдера в системе координат объекта.</param>
        /// <param name="angle">Угол поворотаколлайдера относительно его центра.</param>
        public Collider(List<Vector2> vertices, Vector2 position = default, float angle = 0, bool trigger = false)
        {
            Position = position;
            IsTrigger = trigger;
            Angle = angle;
            this.vertices = vertices
                .Select(vert => new Vector3(vert.X, vert.Y, 0))
                .ToList();
            Radius = vertices
                .Select(vert => vert.Length)
                .Max();
        }

        private Collider(List<Vector3> vertices, Vector2 position, float radius, float angle, bool trigger = false)
        {
            Position = position;
            this.vertices = vertices;
            IsTrigger = trigger;
            Radius = radius;
            Angle = angle;
        }

        /// <summary>
        /// Копирует коллайдер.
        /// </summary>
        /// <returns></returns>
        public Collider Copy() => Transform(Vector2.Zero, 0);

        /// <summary>
        /// Возвращает новый коллайдер перемещённый на вектор move и повёрнутый вокруг этой точки.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="rotate"></param>
        /// <returns></returns>
        public Collider Transform(Vector2 move, float rotate)
        {
            var rotateCollider = Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(Angle));
            var rotateObject = Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(rotate));
            var objectPosition = new Vector3(move.X, move.Y, 0);
            return new Collider(vertices
                .Select(vert => vert * rotateCollider)
                .Select(vert => vert + new Vector3(Position.X, Position.Y, 0))
                .Select(vert => vert * rotateObject)
                .Select(vert => vert + objectPosition)
                .ToList(),
                (new Vector3(Position) * rotateObject + new Vector3(move)).Xy,
                Radius,
                Angle);
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

        public static Vector2 GetProjection(Vector2 vector, Collider collider)
        {
            Vector2 result = default;

            if (collider.Vertices.Count == 0)
            {
                var projection = Vector2.Dot(vector, collider.Position);
                return new Vector2(projection + collider.Radius, projection - collider.Radius);
            }

            for (var i = 0; i < collider.Vertices.Count; i++)
            {
                var projection = Vector2.Dot(vector, collider.Vertices[i]);

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
