using Microsoft.Xna.Framework;
using System;

namespace CollisionDetection
{
    public class Circle : ICollider
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center; Radius = radius;
        }

        virtual public bool CollidesWith(Circle other)
        {
            float dist1 = (Center - other.Center).LengthSquared();
            float dist2 = (float)Math.Pow(Radius + other.Radius, 2f);
            return dist2 >= dist1;
        }

        virtual public bool CollidesWith(OBB other)
        {
            return other.CollidesWith(this);
        }

        public bool CollidesWith(ICollider other)
        {
            ICollider collider = other.GetCollider();
            switch (collider)
            {
                case OBB o:
                    return CollidesWith(o);
                case Circle c:
                    return CollidesWith(c);
                default:
                    return false;
            }
        }

        public string Name() { return "undef"; }
        public void CollisionWith(ICollider other) { }

        public ICollider GetCollider()
        {
            return this;
        }
    }
}