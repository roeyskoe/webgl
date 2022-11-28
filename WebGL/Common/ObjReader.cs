
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace WebGL.Common;

public static class ObjReader
{
    public static Drawable Parse(string name)
    {
        string data = Load(name);
        string[] lines = data.Split('\n');

        List<Vector3> vertices = new List<Vector3>();
        List<Vector4> vertexColors = new List<Vector4>();
        List<Vector3> vertexNormals = new List<Vector3>();
        List<Vector2> vertexTextures = new List<Vector2>();

        List<VertexData> vertexData = new List<VertexData>();

        List<Mesh> meshes = new List<Mesh>();

        foreach (var line in lines)
        {
            string[] split = line.Split(" ");
            if (split[0].Equals("vt"))
            {
                vertexTextures.Add(new Vector2(float.Parse(split[1]), float.Parse(split[2])));
            }
            else if (split[0].Equals("vn"))
            {
                vertexNormals.Add(new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3])));
            }
            else if (split[0].Equals("v"))
            {
                vertices.Add(new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3])));
                if(split.Length > 4)
                    if(split.Length > 7)
                        vertexColors.Add(new Vector4(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]), float.Parse(split[7])));
                    else
                        vertexColors.Add(new Vector4(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]), 1f));
            }
            else if (split[0].Equals("f"))
            {
                foreach (var item in split[1..])
                {
                    string[] indexSplit = item.Split("/");
                    int vertexIndex = int.Parse(indexSplit[0]) - 1; // Indexing starts at 1 :D
                    int textureIndex = int.Parse(indexSplit[1]) - 1;
                    int normalIndex = int.Parse(indexSplit[2]) - 1;

                    Vector4 col;

                    if (vertexColors.Count > 0)
                        col = vertexColors[vertexIndex];
                    else
                        col = new Vector4(0.5f, 0.5f, 0.5f, 1f);
                    VertexData vData = new VertexData(vertices[vertexIndex], vertexNormals[normalIndex], col);
                    vertexData.Add(vData);
                }
            }
            else if (split[0].Equals("usemtl"))
            {
                meshes.Add(new Mesh(vertexData.ToArray(), split[1]));
            }
        }
        Mesh mesh = new Mesh(vertexData.ToArray(), "");
        meshes.Add(mesh);
        return new Drawable(meshes);
    }

    public static string Load(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = assembly.GetName().Name;
        using (Stream stream = assembly.GetManifestResourceStream($"{name}.{path}"))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}
