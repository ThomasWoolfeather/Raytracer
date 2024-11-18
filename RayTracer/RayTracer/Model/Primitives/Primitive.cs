using OpenTK;

namespace RayTracer.Model
{
    public abstract class Primitive
    {
        public bool Active = true;

        protected Material material = new Material();

        public Service.Surface Texture;

        public Material Material
        {
            get
            {
                return material;
            }

            set
            {
                SetMaterial(value);
            }
        }

        /// <summary>
        /// Check wether the ray intersects this primitive.
        /// </summary>
        /// <param name="ray">The ray to check.</param>
        /// <returns>An intersection if one is found.</returns>
        public abstract Intersection Intersect(Ray ray);

        /// <summary>
        /// Get the normal vector of an intersected point on the surface of the primitive.
        /// </summary>
        /// <param name="position">
        /// The Location on the surface where the normal vector should be calculated for.
        /// </param>
        /// <returns>The normal.</returns>
        public abstract Vector3 GetNormal(Vector3 position);

        /// <summary>
        /// Set the material of this primitive.
        /// Note: This is required to prevent a certain material + primitive matchup.
        /// </summary>
        /// <param name="material">the material.</param>
        internal abstract void SetMaterial(Material material);

        /// <summary>
        /// Get the texture at a position on the primitive surface.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The texture at that position.</returns>
        public abstract Vector3 GetTexture(Vector3 position);

        /// <summary>
        /// Set the texture to a texture given an image path.
        /// </summary>
        /// <param name="texture">The image path.</param>
        public void SetTexture(string texture)
        {
            Texture = new Service.Surface(texture);
        }
    }
}
