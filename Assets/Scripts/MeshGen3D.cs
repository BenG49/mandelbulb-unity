using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen3D
{
    // cube side length
    private int totalSideLength, chunkCount;
    private bool[] data;

    /*
        Preconditions: data.Length must be totalSideLength^3
    */
    public MeshGen3D(int totalSideLength, bool[] data) {
        this.totalSideLength = totalSideLength;
        this.data = data;
    }

    public void GenMesh() {
        Vector3[] edges = GenerateEdges();

        for (int i = 0; i < edges.Length; i++) {

        }
    }

    /*
    Goes through every point within 1 block, and if it has an empty space
    in that area, it is part of the edge

    OPTIMIZE BY MAKING CACHE BEFOREHAND?  
    */
    private Vector3[] GenerateEdges() {
        List<Vector3> output = new List<Vector3>();

        for (int i = 0; i < data.Length; i++) {
            Vector3 point = getPoint(i, totalSideLength);

            if (data[i]) {
                for (int z = -1; z < 1; z++) {
                    for (int y = -1; y < 1; y++) {
                        for (int x = -1; x < 1; x++) {
                            if (!data[getIndex(
                                (point + new Vector3(x, y, z)),
                                totalSideLength
                            )])
                                output.Add(point);
                        }
                    }
                }
            }
        }
        return output.ToArray();
    }

    public Vector3 getPoint(int i, int sideLength) {
        return new Vector3(
            i%sideLength,
            (int)(i/Mathf.Pow(sideLength, 2)),
            (int)(Mathf.Ceil((float)(i/totalSideLength))%totalSideLength)
        );
    }

    public int getIndex(Vector3 point, int sideLength) {
        return (int)(point.x+point.y*sideLength+point.z*Mathf.Pow(sideLength, 2));
    }
}
