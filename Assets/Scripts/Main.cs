using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MandelbrotMesh m = new MandelbrotMesh(0.0005);
        m.GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void MandelbrotCubes() {
        int result;
        double interval = 0.01;
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position   = new Vector3(0, -0.1F, 0);
        plane.transform.localScale = new Vector3(1, 0.1F, 1);

        for (double x = -2; x < 2; x += interval) {
            for (double y = -2; y < 2; y += interval) {
                if (Math.Sqrt(x*x+y*y) <= 2) {
                    result = MandelbrotGen.Iterate(x, y);
                    if (result != 0) {
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3((float)x, 0, (float)y);
                        cube.transform.localScale = new Vector3 ((float)interval, (float)(0.01 + result/100), (float)interval);
                    }
                }
            }
        }
    }
}
