using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ThreeBody : MonoBehaviour
{
    private const float G = 500f;
    GameObject[] body;
    BodyProperty[] bp;
    private int numberOfSphere = 300;
    TrailRenderer trailRenderer;
    // Start is called before the first frame update
    struct BodyProperty
    {
        public float mass;
        public Vector3 velocity;
        public Vector3 acceleration;
    }

    void Start()
    {
        bp = new BodyProperty[numberOfSphere];
        body = new GameObject[numberOfSphere];

        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Our gameobjects are created here:
            body[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.
            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html

            // initial conditions
            float r = 100f;
            body[i].transform.position = new Vector3(r * Mathf.Cos(Mathf.PI * 2 / numberOfSphere * i), r * Mathf.Sin(Mathf.PI * 2 / numberOfSphere * i), Random.Range(100, 200));
            bp[i].velocity = new Vector3(r / 10 * Mathf.Sin(Mathf.PI * 2 / 3 * i), r / 10 * Mathf.Cos(Mathf.PI * 2 / 3 * i), 0);
            bp[i].mass = 1;

            // trail
            trailRenderer = body[i].AddComponent<TrailRenderer>();
            // Configure the TrailRenderer's properties
            trailRenderer.time = 100.0f;  // Duration of the trail
            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start
            trailRenderer.endWidth = 0.1f;    // Width of the trail at the end
            // a material to the trail
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Set the trail color over time
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(new Color((Mathf.Sin(Mathf.PI * 2 / numberOfSphere * i)+ 1)/2f, (Mathf.Cos(Mathf.PI * 2 / numberOfSphere * i)+1)/2f, Mathf.Tan(Mathf.PI * 2 / numberOfSphere * i)), 0.80f), new GradientColorKey(new Color((Mathf.Cos(Mathf.PI * 2 / numberOfSphere * i) + 1) / 5f, (Mathf.Sin(Mathf.PI * 2 / numberOfSphere * i) + 1) / 3f, Mathf.Tan(Mathf.PI * 2 / numberOfSphere * i)), 0.80f) },
                new GradientAlphaKey[] { new GradientAlphaKey(7.0f, 0.0f), new GradientAlphaKey(0.0f, 8.0f) }
            );
            trailRenderer.colorGradient = gradient;

        }
    }

    void Update()
    {
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Important. Think about where this should be placed
            bp[i].acceleration = Vector3.zero; // what happens if you comment this out?
        }
        // Loop for N-body gravity
        // How should we design the loop?
        for (int i = 0; i < numberOfSphere; i++)
        {
            for (int j = i + 1; j < numberOfSphere; j++)
            {
                // Vector from i to j body. Make sure which vector you are getting.
                Vector3 distance = body[j].transform.position - body[i].transform.position;
                float m1 = bp[i].mass;
                float m2 = bp[j].mass;
                // Gravity
                Vector3 gravity = CalculateGravity(distance, m1, m2);
                // Apply Gravity
                // F = ma -> a = F/m
                // Gravity is push and pull with same amount. Force: m1 <-> m2
                bp[i].acceleration += gravity / m1; // why this is +?
                bp[j].acceleration -= gravity / m2; // why this is -? What decided the direction?
            }
            // velocity is sigma(Acceleration*time)
            bp[i].velocity += bp[i].acceleration * Time.deltaTime;
            body[i].transform.position += bp[i].velocity * Time.deltaTime;
        }

    }
    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        Vector3 gravity; // note this is also Vector3
        gravity = G * m1 * m2 / (distanceVector.sqrMagnitude) * distanceVector.normalized;
        return gravity;
    }
}


