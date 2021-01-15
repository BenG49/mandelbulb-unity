using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshGen : MonoBehaviour
{
    private int totalWidth, totalLength, chunkCount, chunkSideLength;
    private int[] z;
    private float scale;

    /*
        Precondition: z.Length must be width*length
    */
    // public MeshGen(int width, int length, int[] z, float scale) {
    public MeshGen(int width, int length, int[] z) {
        totalWidth = width;
        totalLength = length;
        this.z = z;
        // this.scale = scale;
    }

    public void GenerateMesh() {
        int meshCount;
        // this is is because doubles do not have enough precision
        if ((totalWidth*totalLength)%65535 == 0)
            meshCount = (int)(totalWidth*totalLength)/65535;
        else {
            if ((totalWidth*totalLength)/65535 >= 1)
                meshCount = (int)Math.Floor((double)((totalWidth*totalLength)/65535))+1;
            else
                meshCount = 1;
        }

        // find closest square to become the number of chunks
        if (meshCount != 0) {
            for (int e = 1;; e++) {
                // if the mesh count is greater than the last square <= this square
                if (meshCount > Math.Pow(e-1, 2) && meshCount <= e*e) {
                    chunkCount = e*e;
                    break;
                }
            }
        } else
            chunkCount = 1;
        
        chunkSideLength = (int)Math.Sqrt(chunkCount);

        GameObject baseObject = new GameObject();
        // renderer
        baseObject.AddComponent<MeshRenderer>();
        baseObject.GetComponent<MeshRenderer>().sharedMaterial =
            (Material)Resources.Load("MeshMaterial", typeof(Material));
        // filter
        baseObject.AddComponent<MeshFilter>();

        // initializes each chunk
        for (int i = 0; i < chunkCount; i++) {
            GenCoords(i, baseObject);
        }
    }

    private void GenCoords(int chunkIndex, GameObject baseObject) {
        // INIT
        GameObject gameObject = GameObject.Instantiate(baseObject);
        gameObject.name = "mesh" + chunkIndex.ToString();

        tIndex = 0;
        Vector3[] vertices;
        int[] triangles;
        int width = 254;
        int length = 254;
        int chunkX = (int)(chunkIndex%Math.Sqrt(chunkCount));
        int chunkY = (int)Math.Ceiling((double)((chunkIndex)/chunkSideLength));

        // if an edge chunk (first is x, second is y)
        if ((chunkIndex+1)%Math.Sqrt(chunkCount) == 0)
            width++;
        if (chunkIndex+1+Math.Sqrt(chunkCount) > chunkCount+1)
            length++;

        vertices = new Vector3[width*length];
        triangles = new int[(width-1)*(length-1)*6];



        // WRITE COORDINATES
        for (int i = 0; i < vertices.Length; i++) {
            int x = i%width;
            int y = (int)Math.Ceiling((double)(i/width));
            int zPlt = 0;

            // if the current vertex is within the given width and height, and
            // not the z=0 chunk extensions, otherwise defaults to 0
            if (x+(chunkX*254) < totalWidth && y+(chunkY*254) < totalLength) {
                int index = ((y+chunkY*254)*totalWidth)+x+(chunkX*254);
                zPlt = z[index];
            }

            vertices[i] = new Vector3(x, zPlt, y);
            
            // adds triangle if not in bottom right edge vertices
            if (x < width-1 && y < length-1) {
                addTriangle(triangles, i, (i+width), (i+width+1));
                addTriangle(triangles, i, (i+width+1), (i+1));
            }
        }

        writeMesh(vertices, triangles, gameObject, chunkX, chunkY);
    }

    private void writeMesh(Vector3[] vertices, int[] triangles, GameObject gameObject, int chunkX, int chunkY) {
        MeshFilter meshFilter;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // idk if its necessary, but if given game object does not have mesh filter
        try {
            meshFilter = gameObject.GetComponent<MeshFilter>();
        } catch {
            gameObject.AddComponent<MeshFilter>();
            meshFilter = gameObject.GetComponent<MeshFilter>();
        }
            
        meshFilter.sharedMesh = mesh;
        meshFilter.transform.Translate(chunkX*253, 0, chunkY*253);
    }

    private int tIndex = 0;
    private void addTriangle(int[] triangleIn, int a, int b, int c) {
        triangleIn[tIndex] = a;
        triangleIn[tIndex+1] = b;
        triangleIn[tIndex+2] = c;

        tIndex += 3;
    }
}