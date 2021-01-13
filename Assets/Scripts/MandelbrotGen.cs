using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class MandelbrotMesh
{
    private MeshFilter meshFilter;
    private Vector3[] vertices;
    private int[] triangles;
    private int width;
    private int height;
    private double interval;

    public MandelbrotMesh(double interval) {
        this.interval = interval;

        GameObject gameObject = new GameObject();
        gameObject.name = "mesh";

        // renderer
        gameObject.AddComponent<MeshRenderer>();
        MeshRenderer r = gameObject.GetComponent<MeshRenderer>();
        Material mat = (Material)Resources.Load("MeshMaterial", typeof(Material));
        r.sharedMaterial = mat;

        // filter
        gameObject.AddComponent<MeshFilter>();
        meshFilter = gameObject.GetComponent<MeshFilter>();

        width  = (int)(4/interval+1);
        height = (int)(4/interval+1);

        vertices = new Vector3[width * height];
        triangles = new int[(width-1)*(height-1)*6];
        triangleIndex = 0;
    }

    public void GenerateMesh() {

        for (int i = 0; i < vertices.Length; i++) {
            int x = i%width;
            int y = (int)(i/width);
            double pltX = -2 + (i%width)*interval;
            double pltY = -2 + (int)(i/width)*interval;
            int z = -3;

            if (Math.Sqrt(pltX*pltX+pltY*pltY) <= 2)
                z = 2-MandelbrotGen.Iterate(pltX, pltY);

            vertices[i] = new Vector3((float)(pltX-1), z/10, (float)(pltY-1));

            if (x < width-1 && y < height-1) {
                addTriangle(i, (i+width), (i+width+1));
                addTriangle(i, (i+width+1), (i+1));
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    private int triangleIndex = 0;
    void addTriangle(int a, int b, int c) {
        triangles[triangleIndex]   = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;

        triangleIndex+=3;
    }
}