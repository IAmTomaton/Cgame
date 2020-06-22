using Cgame.Core;
using NUnit.Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestProject
{
    class ColliderTests
    {
        [Test]
        public void TestСircularCollider()
        {
            var first = new Collider(10);
            var second = new Collider(10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(20, result.MtvLength);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestСircularColliderMove()
        {
            var first = new Collider(10);
            var second = new Collider(10).Transform(new Vector2(10, 0), 0);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(10, result.MtvLength);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestСircularColliderGetNormals()
        {
            var normals = new Collider(10).GetNormals().ToList();

            Assert.IsEmpty(normals);
        }

        [Test]
        public void TestRectangularCollider()
        {
            var first = new Collider(10, 10);
            var second = new Collider(10, 10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(10, result.MtvLength);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestRectangularColliderGetNormals()
        {
            var normals = new Collider(10, 10).GetNormals().ToList();

            Assert.AreEqual(new List<Vector2>() { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) }, normals);
        }

        [Test]
        public void TestRectangularColliderMove()
        {
            var first = new Collider(10, 10).Transform(new Vector2(5, 0), 0);
            var second = new Collider(10, 10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(5, result.MtvLength);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestRectangularColliderRotation()
        {
            var first = new Collider(10, 10).Transform(Vector2.Zero, 45);
            var second = first.Transform(new Vector2(10, 0), 0);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(2.9, result.MtvLength, 0.1);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestArbitraryCollider()
        {
            var first = new Collider(new List<Vector2>() { new Vector2(10, 10), new Vector2(10, -10), new Vector2(0, 10) }, Vector2.Zero);
            var second = new Collider(new List<Vector2>() { new Vector2(10, 10), new Vector2(10, -10), new Vector2(0, 10) }, Vector2.Zero);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(8.9, result.MtvLength, 0.1);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestRectangularColliderCollideZeroLen()
        {
            var first = new Collider(10, 10).Transform(new Vector2(10, 0), 0);
            var second = new Collider(10, 10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(0, result.MtvLength);
            Assert.IsTrue(result.Collide);
        }

        [Test]
        public void TestArbitraryColliderGetNormals()
        {
            var collider = new Collider(new List<Vector2>() { new Vector2(10, 10), new Vector2(10, -10), new Vector2(0, 10) }, Vector2.Zero);

            var normals = collider.GetNormals().Select(n => new Vector2((float)Math.Round(n.X, 1), (float)Math.Round(n.Y, 1))).ToList();

            Assert.AreEqual(new List<Vector2>() { new Vector2(-1, 0), new Vector2(0.9f, 0.4f), new Vector2(0, -1) }, normals);
        }
    }
}
