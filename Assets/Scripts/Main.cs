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
        // MandelbrotGen.CreateMesh(500);
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

public static class MandelbrotGen
{
    private static int maxIterations = 150;

    public static void SetMaxIterations(int maxIterationsIn) {
        maxIterations = maxIterationsIn;
    }


    public static int Iterate(double xIn, double yIn) {
        int output = 0;
        double x = 0;
        double y = 0;

        for (int i = 0; i < maxIterations; i++) {
            if (x*x + y*y >= 4) {
                output = i;
                break;
            }

            double tempX = x*x - y*y + xIn;
            y = 2*x*y + yIn;
            x = tempX;
        }

        return output;
    }

    /*
        scale: larger number = larger and higher detail
    */
    public static void CreateMesh(int scale) {
        int sideLength = (int)(4*scale+1);
        int[] z = new int[sideLength*sideLength];

        for (int y = 0; y < sideLength; y++) {
            for (int x = 0; x < sideLength; x++) {
                int index = x+(y*sideLength);
                double mandelbrotX = -2.0+(double)x*(1.0/scale);
                double mandelbrotY = -2.0+(double)y*(1.0/scale);

                z[index] = MandelbrotGen.Iterate(mandelbrotX, mandelbrotY);

                if (z[index] != 1)
                    z[index] *= -1;
            }
        }

        MeshGen gen = new MeshGen(sideLength, sideLength, z);
        gen.GenerateMesh();
    }
}