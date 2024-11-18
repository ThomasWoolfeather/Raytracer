using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Model
{
    public abstract class Primitive
    {
        public string Name;
        public Vector3 Color;
        public bool IsStatic = true;

        public abstract Intersection Intersect(Ray ray);
    }
}
