using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGenerator
{
    static List<Vector3> verts;
    static List<Vector3> norms;
    static List<int> trian;
    static List<Vector2> uv;

    public static GameObject Create(float radius, float height, int resolution, Material material, bool inSide)
    {
        GameObject obj = new GameObject();
        obj.name = "ring";

        CalcVertecies(radius, height, resolution, inSide);

        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = trian.ToArray();
        mesh.normals = norms.ToArray();
        mesh.uv = uv.ToArray();

        obj.AddComponent<MeshFilter>();
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshFilter>().sharedMesh.name = obj.name;
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = material;

        //CapsuleCollider collider = obj.AddComponent<CapsuleCollider>();
        //collider.height = 2;

        return obj;
    }

    static void CalcVertecies(float radius, float height, int resolution, bool inSide)
    {
        verts = new List<Vector3>();
        norms = new List<Vector3>();
        trian = new List<int>();
        uv = new List<Vector2>();

        float deltaTheta = Mathf.PI * 2f / (float)resolution;
        if (inSide)
        {
            deltaTheta = -deltaTheta;
        }

        Vector3 p0 = Vector3.zero;
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.zero;

        float aspectRatio = height / (2f * Mathf.PI * radius); 

        int index = 0;
        for (int i = 0; i <= resolution; i++)
        {
            p0 = p2;
            p1 = p3;

            float theta = (float)i * deltaTheta;
            p2 = new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta)) * radius;
            p3 = new Vector3(Mathf.Cos(theta), height, Mathf.Sin(theta)) * radius * 1.02f;

            verts.Add(p2);
            verts.Add(p3);
            norms.Add(Vector3.up);
            norms.Add(Vector3.up);
            uv.Add(new Vector2((float)i / (float)resolution, 0.5f - aspectRatio));
            uv.Add(new Vector2((float)i / (float)resolution, 0.5f));

            if (i == 0)
            {
                continue;
            }

            trian.Add(index + 0);
            trian.Add(index + 1);
            trian.Add(index + 2);
            trian.Add(index + 1);
            trian.Add(index + 3);
            trian.Add(index + 2);

            index += 2;
        }
    }



}