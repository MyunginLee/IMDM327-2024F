using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public static Vector3 CameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        CameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraPosition = transform.position;
    }
}
