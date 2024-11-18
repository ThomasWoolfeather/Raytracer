using System;
using OpenTK;
using RayTracer.Model.LightSources;
using RayTracer.Model.Materials;
using RayTracer.Model.Primitives;
using System.Collections.Generic;

namespace RayTracer.Model
{
    public class Scene
    {
        private bool useSkydome = false;

        private Skydome skydome;

        private Vector3 backgroundColor;

        public List<Primitive> Primitives = new List<Primitive>();

        public List<Light> LightSources = new List<Light>();

        /// <summary>
        /// The scene is a collection of primitives and lightsources.
        /// </summary>
        public Scene()
        {
            backgroundColor = new Vector3(0.2f, 0.4f, 0.6f);
            skydome = new Skydome();

            //The floor.
            Primitives.Add(new Plane(new Vector3(0, 0, 1), 1)
            {
                Material = new Checkerboard()
                {
                    Color = new Vector3(1, 1, 1)
                }
            });
            //The blue sphere.
            Primitives.Add(new Sphere(new Vector3(-2.5f, 1, 0), 1)
            {
                Material = new Shiny()
                {
                    Color = new Vector3(0.0f, 0.0f, 1.0f)
                }
            });
            //The green sphere.
            Primitives.Add(new Sphere(new Vector3(0, 1, 0), 1)
            {
                Material = new Shiny()
                {
                    Color = new Vector3(0.0f, 1.0f, 0.0f)
                }
            });
            //The red sphere.
            Primitives.Add(new Sphere(new Vector3(2.5f, 1, 0), 1)
            {
                Material = new Shiny()
                {
                    Color = new Vector3(1.0f, 0.0f, 0.0f)
                }
            });

            //The triangular mirror in the background.
            Primitives.Add(new Triangle(new Vector3(-4, 4, -1), new Vector3(4, 4, -1), new Vector3(0, 4, 6))
            {
                Material = new Mirror()
                {
                    Color = new Vector3(0.05f, 0.05f, 0.05f)
                }
            });

            ////The grey wall.
            //Primitives.Add(new Quad(new Vector3(-10, 10, -1), new Vector3(10, 10, -1), new Vector3(-10, 10, 9), new Vector3(10, 10, 9))
            //{
            //    Material = new Mirror()
            //    {
            //        Color = new Vector3(0.2f, 0.4f, 0.6f)
            //    }
            //});

            //The pointlight.
            LightSources.Add(new Light()
            {
                Color = new Vector3(1f, 1f, 1f),
                Position = new Vector3(0, -2, 2)
            });

            //The spotlight pointed at the blue sphere.
            LightSources.Add(new SpotLight(new Vector3(-0.5f, 0.5f, -1), 10)
            {
                Color = new Vector3(0.5f, 0.5f, 0.5f),
                Position = new Vector3(0, -0.5f, 3),
                Active = false,
            });

            //The spotlight pointed at the green sphere.
            LightSources.Add(new SpotLight(new Vector3(0, 0.5f, -1), 10)
            {
                Color = new Vector3(0.5f, 0.5f, 0.5f),
                Position = new Vector3(0, -0.5f, 3),
                Active = false,
            });

            //The spotlight pointed at the red sphere.
            LightSources.Add(new SpotLight(new Vector3(0.5f, 0.5f, -1), 10)
            {
                Color = new Vector3(0.5f, 0.5f, 0.5f),
                Position = new Vector3(0, -0.5f, 3),
                Active = false,
            });
        }

        /// <summary>
        /// Get the background color at a certain direction.
        /// Note: In case of a skydome the direction is used, otherwise the background color is returned.
        /// </summary>
        /// <param name="Direction">The direction of the ray that had no intersections.</param>
        /// <returns>The backgroundcolor.</returns>
        public Vector3 BackgroundColor(Vector3 Direction)
        {
            if (useSkydome)
            {
                return skydome.GetColor(Direction);
            }
            else
                return backgroundColor;
        }

        /// <summary>
        /// Toggles all lights in the scene to on -&gt; off -&gt; on.
        /// </summary>
        public void ToggleLights()
        {
            foreach (Light light in LightSources)
            {
                light.Active = !light.Active;
            }
        }

        /// <summary>
        /// Toggles all primitives in the scene to visible -&gt; invisible -&gt; visible.
        /// </summary>
        public void TogglePrimitives()
        {
            foreach (Primitive primitive in Primitives)
            {
                primitive.Active = !primitive.Active;
            }
        }
    }
}
