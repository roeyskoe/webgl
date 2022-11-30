using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WebGL.Common;

public unsafe class Drawable
{
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }

    public List<Mesh> Meshes { get; set; } = new List<Mesh>();

    public Drawable(Vector3 pos, Vector3 rot)
    {
        Position = pos;
        Rotation = rot;
    }

    public Drawable(List<Mesh> meshes)
    {
        Meshes.AddRange(meshes);
    }

    public void Draw()
    {

        Shader shader = WebGL.Pages.Index.ShaderMaterial;

        var model = Matrix4x4.CreateRotationX(Rotation.X)
                        * Matrix4x4.CreateRotationY(Rotation.Y)
                        * Matrix4x4.CreateRotationZ(Rotation.Z)
                        * Matrix4x4.CreateTranslation(Position);

        shader.SetUniform("uModel", model);


        foreach (var mesh in Meshes)
        {
            if (mesh.Material == null) continue;
            shader.SetUniform("uDiffuse", mesh.Material.DiffuseColor);
            shader.SetUniform("uAmbient", mesh.Material.AmbientColor);
            shader.SetUniform("uEmissive", mesh.Material.EmissiveColor);
            shader.SetUniform("uSpecular", mesh.Material.SpecularColor);
            shader.SetUniform("uShininess", mesh.Material.Shininess);
            shader.SetUniform("uOpacity", mesh.Material.Opacity);

            mesh.Vao.Bind();
            if (mesh.Indices != null)
                Gl.DrawElements((int)GLEnum.Triangles, mesh.Indices.Length, (int)GLEnum.UnsignedInt, 0);
            else
                Gl.DrawArrays((int)GLEnum.Triangles, 0, mesh.Vertices.Length);
        }

    }
}
