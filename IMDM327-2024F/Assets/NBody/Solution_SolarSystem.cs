// Unity Solar system gravity simulation
// UMD IMDM327. Instructor: Myungin Lee
// Date: 2023.09.11.
// Contact: myungin@umd.edu

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class gravity : MonoBehaviour
{
    const int planetCount = 9; // Sun and 8 planets
    const float G = 6.674e-11f; // Gravitational constant
    public GameObject[] planets;
    private float initAngle;
    // Generic unity don't have float vector, but we need. refer "Vector3.cs"
    public Vector3[] velocity;
    public Vector3[] acceleration;
    public Vector3[] real_position; // True position in meter
    public Vector3[] direction;
    public Quantity[] planetProfile;
    TrailRenderer trailRenderer;
    // Structure to hold quantity of solar system
    public struct Quantity
    {
        public string name;
        public float mass;     // kilograms
        public float radius;   // meters
        public float distance; // meters
        public float speed; // meters/second      
    }

    void Awake()
    {
        planets = new GameObject[planetCount];
        velocity = new Vector3[planetCount];
        acceleration = new Vector3[planetCount];
        real_position = new Vector3[planetCount];
        direction = new Vector3[planetCount];
        planetProfile = new Quantity[planetCount];

        // Physical quantity of solar system. source: https://en.wikipedia.org/wiki/List_of_Solar_System_objects_by_size
        planetProfile[0].name = "sun"; planetProfile[0].mass = 1989100000e21f; planetProfile[0].distance = 0; planetProfile[0].speed = 0; planetProfile[0].radius = 695508e3f;
        planetProfile[1].name = "mercury"; planetProfile[1].mass = 330.11e21f; planetProfile[1].distance = 57.9e9f; planetProfile[1].speed = 47.9e3f; planetProfile[1].radius = 2439.4e3f;
        planetProfile[2].name = "venus"; planetProfile[2].mass = 4867.5e21f; planetProfile[2].distance = 108e9f; planetProfile[2].speed = 35e3f; planetProfile[2].radius = 6052e3f;
        planetProfile[3].name = "earth"; planetProfile[3].mass = 5972.4e21f; planetProfile[3].distance = 149e9f; planetProfile[3].speed = 29.8e3f; planetProfile[3].radius = 6371e3f;
        planetProfile[4].name = "mars"; planetProfile[4].mass = 641.71e21f; planetProfile[4].distance = 228e9f; planetProfile[4].speed = 24.1e3f; planetProfile[4].radius = 3389.5e3f;
        planetProfile[5].name = "jupiter"; planetProfile[5].mass = 1898187e21f; planetProfile[5].distance = 778e9f; planetProfile[5].speed = 13.1e3f; planetProfile[5].radius = 69911e3f;
        planetProfile[6].name = "saturn"; planetProfile[6].mass = 568317e21f; planetProfile[6].distance = 1427e9f; planetProfile[6].speed = 9.7e3f; planetProfile[6].radius = 58232e3f;
        planetProfile[7].name = "uranus"; planetProfile[7].mass = 86813e21f; planetProfile[7].distance = 2867e9f; planetProfile[7].speed = 6.8e3f; planetProfile[7].radius = 25362e3f;
        planetProfile[8].name = "neptune"; planetProfile[8].mass = 102413e21f; planetProfile[8].distance = 4515e9f; planetProfile[8].speed = 5.4e3f; planetProfile[8].radius = 24622e3f;

        // Initialize the condition of the solar system
        for (int i = 0; i < planetCount; i++)
        {
            // Find and match GameObject and name of the planets
            planets[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.
            // Planet radius to scale the sphere gameobject but real scale is huge, so scale downed.
            float scaleSize = Mathf.Sqrt((float)planetProfile[i].radius / 1e7f);
            //  float scaleSize = 5;
            planets[i].transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

            // Where should the planets be spawned? Randomize the angular position
            initAngle = UnityEngine.Random.Range(-2 * Mathf.PI, Mathf.PI);
            // and assign distance. cos*cos + sin*sin = 1.
            real_position[i].x = Mathf.Cos(initAngle) * planetProfile[i].distance;
            real_position[i].y = 0;
            real_position[i].z = Mathf.Sin(initAngle) * planetProfile[i].distance;
            // Assign initial velocity. The direction it should be perpendicular to the initial angle.
            velocity[i].x = Mathf.Cos(initAngle + Mathf.PI / 2) * planetProfile[i].speed;
            velocity[i].y = 0;
            velocity[i].z = Mathf.Sin(initAngle + Mathf.PI / 2) * planetProfile[i].speed;

            // trail
            trailRenderer = planets[i].AddComponent<TrailRenderer>();
            // Configure the TrailRenderer's properties
            trailRenderer.time = 20.0f;  // Duration of the trail
            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start
            trailRenderer.endWidth = 0.0f;    // Width of the trail at the end
            // a material to the trail
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Set the trail color over time
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            trailRenderer.colorGradient = gradient;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Acceleration should be neutralized every update
        for (int i = 0; i < planetCount; i++)
        {
            acceleration[i] = Vector3.zero; // important
        }
        // Newtonian physics
        for (int i = 0; i < planetCount; i++)
        {
            for (int j = i + 1; j < planetCount; j++) // note this loop.
            {
                Vector3 diff = real_position[j] - real_position[i]; // distance vector
                float dist = diff.magnitude;
                Vector3 gravDirection = diff.normalized;
                float m1 = planetProfile[i].mass;
                float m2 = planetProfile[j].mass;
                // Gravity equation
                float gravity = G * (m1 * m2) / (dist * dist);
                // Gravity is push and pull with same amount. Force: m1 <-> m2
                acceleration[i] += (gravDirection * gravity) / m1;  // f = m * a. m1 got pushed.
                acceleration[j] -= (gravDirection * gravity) / m2;  // f = m * a. m2 got pulled
            }
            velocity[i] += acceleration[i] * 5000; // fast forward
            real_position[i] += velocity[i] * 5000; // fast forward
            // Solar system is very sparse. So we make a meta_position to squeeze our universe to make a better observation.
            direction[i] = real_position[i].normalized;
            float scaledDistance = Mathf.Sqrt((float)(real_position[i].magnitude / 1e8f));
            planets[i].transform.position = new Vector3((float)(direction[i].x * scaledDistance),
                                                                                ((float)direction[i].y * scaledDistance),
                                                                                ((float)direction[i].z * scaledDistance));
        }

    }
}

