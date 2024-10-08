using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDraw : MonoBehaviour
{
    LineRenderer[] lineRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = new LineRenderer[Boids.numberOfSphere];
    }
    void Start()
    {
        // init draw
        for (int i = 0; i < lineRenderer.Length-1; i++)
        {
            lineRenderer[i] = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer[i].material = new Material(Shader.Find("Diffuse"));
            lineRenderer[i].startColor = new Color(0.1f, 0.7f, 0.9f);
            lineRenderer[i].endColor = new Color(0.1f, 0.7f, 0.9f);
            lineRenderer[i].startWidth = 0.1f;
            lineRenderer[i].endWidth = 0.1f;
            //lineRenderer[i].positionCount = 2;
            //lineRenderer[i].useWorldSpace = true;

            ////For drawing line in the world space, provide the x,y,z values
            //lineRenderer[i].SetPosition(0, Boids.body[i].transform.position); //starting point of the line
            //lineRenderer[i].SetPosition(1, Boids.body[i+1].transform.position); //end point of the line
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lineRenderer.Length; i++)
        {
            lineRenderer[i].startColor = new Color(0.1f, 0.7f, 0.9f);
            lineRenderer[i].endColor = new Color(0.1f, 0.7f, 0.9f);
            lineRenderer[i].startWidth = 0.1f;
            lineRenderer[i].endWidth = 0.1f;
            lineRenderer[i].positionCount = 2;
            lineRenderer[i].useWorldSpace = true;

            //For drawing line in the world space, provide the x,y,z values
            lineRenderer[i].SetPosition(0, Boids.body[(int)Random.Range(0, i)].transform.position); //starting point of the line
            lineRenderer[i].SetPosition(1, Boids.body[(int)Random.Range(0, i)].transform.position); //end point of the line
        }
    }
}
