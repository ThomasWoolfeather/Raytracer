using OpenTK;
using RayTracer.Model;
using RayTracer.Model.LightSources;
using RayTracer.Utils;
using System;

namespace RayTracer.Service
{
    public class Raytracer
    {
        //The Scene contains all objects (i.e. the primitives and lightsources)
        public Scene Scene;

        //The Camera is our window into the 3D world through which all rays are casted.
        public Camera Camera;

        //The Surface is the same as the one given in Assignment 1 with the slightest adjustments.
        public Surface Screen;

        //The value of the offset of rays when they get reflected.
        private float epsilon = 0.001f;

        public int MaxDepth = 18;

        /// <summary>
        /// Initiate the scene, objects and camera. Gets called only once.
        /// </summary>
        public void Init()
        {
            Scene = new Scene();
            Camera = new Camera();
        }

        /// <summary>
        /// Render gets called once every frame. It cleans the screen and draws a new image.
        /// </summary>
        public void Render()
        {
            Cast();
        }

        /// <summary>
        /// Cast all the rays, one for every pixel.
        /// Note: Every pixels is half the width because of the debug window.
        /// </summary>
        private void Cast()
        {
            for (int y = 0; y < Screen.height; y++)
            {
                for (int x = 0; x < Screen.halfwidth; x++)
                {
                    Ray ray = CreateRay(x, y);
                    Vector3 Color = CastRay(ray);
                    //Plot the color of the ray on the pixel it was sent from.
                    Screen.Plot(Screen.halfwidth - 1 - x, Screen.height - 1 - y, Translator.CreateColor(Color));
                }
            }
        }

        /// <summary>
        /// Creates a ray that goes through the pixel at x and y.
        /// Note: The input requires screen coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the screen.</param>
        /// <param name="y">The Y coordinate of the screen.</param>
        /// <returns></returns>
        public Ray CreateRay(int x, int y)
        {
            //Calculate the origin of the ray, starting at the Camera (not the Eye).
            Vector3 Origin = PixelToCoordinate(x, y);
            Vector3 eye = Camera.GetEyePosition();
            //The direction of the ray is; from the eye through the screen and into the unknown.
            Vector3 Direction = (Origin - eye).Normalized();
            return new Ray(Origin, Direction);
        }

        /// <summary>
        /// Calculate the positions of the screencoordinates as worldcoordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the screen.</param>
        /// <param name="y">The Y coordinate of the screen.</param>
        /// <returns></returns>
        public Vector3 PixelToCoordinate(int x, int y)
        {
            float xpos = ((x / (float)(Screen.halfwidth) * Camera.ScreenDimensions.X) + Camera.ScreenOrigin.X);
            float ypos = ((y / (float)(Screen.height) * Camera.ScreenDimensions.Y) + Camera.ScreenOrigin.Y);
            return new Vector3(xpos * Camera.Perpendicular.X, xpos * Camera.Perpendicular.Y, ypos) + Camera.Position;
        }

        /// <summary>
        /// Casts the ray initially, setting the depth to zero.
        /// </summary>
        /// <param name="ray">The ray to be cast.</param>
        /// <returns>The color of the ray.</returns>
        public Vector3 CastRay(Ray ray)
        {
            return CastRay(ray, MaxDepth);
        }

        /// <summary>
        /// Casts the ray.
        /// </summary>
        /// <param name="ray">The ray to be cast.</param>
        /// <param name="depth">The current depth to make sure there is no overflow.</param>
        /// <returns>The color of the ray</returns>
        public Vector3 CastRay(Ray ray, int depth)
        {
            //Get the first intersection.
            if (ray.Intersection == null)
                Intersect(ray);
            //Check if there is an intersection to begin with...
            if (ray.Intersection != null)
                //If there is an intersection we're interested of the color of the thing we hit at the position we hit it.
                return GetColor(ray, depth);
            else
                //If there is no intersection it means we've hit nothing; so we return the backgroundcolor (as defined in Scene).
                return Scene.BackgroundColor(ray.Direction);
        }

        /// <summary>
        /// Calculates all the intersections of a ray in the scene and returns the closest one.
        /// Note: Result can be null!
        /// </summary>
        /// <param name="ray">The ray to be checked on intersections.</param>
        /// <returns>The first intersection</returns>
        public void Intersect(Ray ray)
        {
            foreach (Primitive primitive in Scene.Primitives)
            {
                if (!primitive.Active) continue;
                Intersection intersection = primitive.Intersect(ray);
                //If there is an intersection with this primitive we add it to the results, otherwise we ignore it.
                if (intersection != null)
                {
                    if (ray.Intersection == null)
                        ray.Intersection = intersection;
                    else if (intersection.Distance < ray.Intersection.Distance)
                        ray.Intersection = intersection;
                }
            }
        }

        /// <summary>
        /// Get the distance of the first intersection of a ray.
        /// </summary>
        /// <param name="ray">The ray to be checked.</param>
        /// <returns>The distance of the first intersection of a ray.</returns>
        public float GetIntersectionDistance(Ray ray)
        {
            //Get the intersections the usual way.
            if (ray.Intersection == null)
                Intersect(ray);
            //If there is no intersection the distance will be set to 0.
            if (ray.Intersection == null)
                return 0;
            //If there is an intersection we'll simply look at its distance.
            else
                return ray.Intersection.Distance;
        }

        /// <summary>
        /// Get the color of an intersection.
        /// Note: This is a recursive function.
        /// </summary>
        /// <param name="intersection">The intersection.</param>
        /// <param name="depth">The current depth to make sure there is no overflow.</param>
        /// <returns>The color at the intersection.</returns>
        public Vector3 GetColor(Ray ray, int depth)
        {
            Vector3 color = new Vector3(0, 0, 0);
            //The position of the intersection.
            Vector3 position = (ray.Intersection.Distance * ray.Direction) + ray.Origin;
            //The normal of the primitive at that position.
            Vector3 normal = ray.Intersection.Primitive.GetNormal(position);
            //The reflection direction using the normal.
            Vector3 reflection = (ray.Direction - (2 * Vector3.Dot(normal, ray.Direction)) * normal).Normalized();

            foreach (Light light in Scene.LightSources)
            {
                if (!light.Active) continue;
                //For every light source we'll check the distance, direction and length from the intersection point.
                Vector3 lightDistance = light.Position - position;
                Vector3 lightDirection = lightDistance.Normalized();
                //A shadow ray is cast to check if this point is in the shadow.
                Ray shadowRay = new Ray(position, lightDirection);
                //Check the distance of the intersection and use this distance to determine wether the point of intersection is in the shadows.
                float intersectionDistance = GetIntersectionDistance(shadowRay);
                bool shadow = !(intersectionDistance == 0 || intersectionDistance > lightDistance.Length);
                if (!shadow)
                {
                    //If the point is not in the shadows we can calculate the light intensity at the point
                    float illumination = Vector3.Dot(normal, lightDirection);
                    Vector3 lightColor = new Vector3(0, 0, 0);
                    float distance = lightDirection.Length;
                    float attenuation = 1 / (distance * distance);
                    if (illumination > 0)
                        lightColor = illumination * light.GetColor(position) * attenuation;
                    //Calculate the specular color
                    float specular = Vector3.Dot(lightDirection, reflection);
                    Vector3 specularColor = new Vector3(0, 0, 0);
                    if (specular > 0)
                        specularColor = light.GetColor(position) * (float)Math.Pow(specular, ray.Intersection.Primitive.Material.Roughness);
                    //Add them all together
                    color = color + (ray.Intersection.Primitive.Material.Diffuse(position) * lightColor) + (ray.Intersection.Primitive.Material.Specular(position) * specularColor);
                }
            }
            //If this is the final ray, just quit. Enough is enough.
            if (depth <= 0)
                return color;
            //The reflectiveness factor.
            float reflectiveness = ray.Intersection.Primitive.Material.Reflectiveness(position);
            if (reflectiveness > 0f)
            {
                ray.Intersection.ReflectedRay = new Ray(position + epsilon * reflection, reflection);
                Vector3 reflectedColor = reflectiveness * CastRay(ray.Intersection.ReflectedRay, depth - 1);
                return color + reflectedColor;
            }
            else
                return color;
        }
    }
}
