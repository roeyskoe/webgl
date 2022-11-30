
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace WebGL.Common;

public static class ObjReader
{
    public static Drawable Parse(string objName, string materialName = null)
    {
        List<Material> materials = null;

        if (materialName != null)
            materials = LoadMaterials(materialName);

        string data = Load(objName);
        string[] lines = data.Split('\n');

        List<VertexData> vertices = new List<VertexData>();
        List<Vector4> vertexColors = new List<Vector4>();
        List<Vector3> vertexNormals = new List<Vector3>();
        List<Vector2> vertexTextures = new List<Vector2>();

        List<VertexData> vertexData = new List<VertexData>();

        List<Mesh> meshes = new List<Mesh>();

        Material mat = null;

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
                Vector3 position = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
                
                Vector4 color = Vector4.One;
                if(split.Length > 4)
                {
                    if (split.Length > 7)
                        color = new Vector4(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]), float.Parse(split[7]));
                    else
                        color = new Vector4(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]), 1f);
                }
                vertices.Add(new VertexData(position, Vector3.Zero, color));
            }
            else if (split[0].Equals("f"))
            {
                foreach (var item in split[1..])
                {
                    string[] indexSplit = item.Split("/");
                    int vertexIndex = int.Parse(indexSplit[0]) - 1; // Indexing starts at 1 :D
                    
                    if (!string.IsNullOrEmpty(indexSplit[1]))
                    {
                        int textureIndex = int.Parse(indexSplit[1]) - 1;
                    }
                    
                    int normalIndex = int.Parse(indexSplit[2]) - 1;

                    VertexData vData = vertices[vertexIndex];
                    vData.Normal = vertexNormals[normalIndex];
                    vertexData.Add(vData);
                }
            }
            else if (split[0].Equals("usemtl"))
            {
                Mesh m = new Mesh(vertexData.ToArray(), mat);
                meshes.Add(m);
                mat = materials?.First(t => t.Name.Equals(split[1]));
            }
        }
        Mesh mesh = new Mesh(vertexData.ToArray(), mat);
        meshes.Add(mesh);

        return new Drawable(meshes);
    }

    public static List<Material> LoadMaterials(string name)
    {
        string data = Load(name);
        string[] lines = data.Split('\n');

        List<Material> materials = new List<Material>();
        Material mat = new Material();

        foreach (var line in lines)
        {
            string[] split = line.Split(" ");
            if (split[0].Equals("newmtl"))
            {
                mat = new Material();
                mat.Name = split[1];
                materials.Add(mat);
            }
            else if (split[0].Equals("Ns"))
            {
                mat.Shininess = float.Parse(split[1]);
            }
            else if (split[0].Equals("Ka"))
            {
                mat.AmbientColor = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else if (split[0].Equals("Kd"))
            {
                mat.DiffuseColor = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else if (split[0].Equals("Ks"))
            {
                mat.SpecularColor = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else if (split[0].Equals("Ke"))
            {
                mat.EmissiveColor = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else if (split[0].Equals("Ni"))
            {
                mat.OpticalDensity = float.Parse(split[1]);
            }
            else if (split[0].Equals("d"))
            {
                mat.Opacity = float.Parse(split[1]);
            }
            else if (split[0].Equals("illum"))
            {
                mat.Illumination = int.Parse(split[1]);
            }
        }

        return materials;
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
