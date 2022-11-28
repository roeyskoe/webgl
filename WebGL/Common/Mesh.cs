using Silk.NET.OpenGL;
using System.Numerics;

namespace WebGL.Common;

public unsafe class Mesh
{
    public BufferObject<uint> Ebo;
    public BufferObject<VertexData> Vbo;
    public VertexArrayObject<VertexData, uint> Vao;

    public VertexData[] Vertices;

    public readonly uint[] Indices;

    public string MaterialName { get; set; }

    public Mesh(VertexData[] vertices, uint[] indices, string materialName)
    {
        Vertices = vertices;
        Indices = indices;
        MaterialName = materialName;

        Ebo = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<VertexData>(Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<VertexData, uint>(Vbo, Ebo);
        Vao.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, sizeof(VertexData), 0);
        Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, sizeof(VertexData), sizeof(Vector3));
        Vao.VertexAttributePointer(2, 4, VertexAttribPointerType.Float, sizeof(VertexData), sizeof(Vector3) * 2);
        Gl.BindVertexArray(null);
    }

    public Mesh(VertexData[] vertices, string materialName)
    {
        Vertices = vertices;
        MaterialName = materialName;

        Vbo = new BufferObject<VertexData>(Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<VertexData, uint>(Vbo, null);
        Vao.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, sizeof(VertexData), 0);
        Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, sizeof(VertexData), sizeof(Vector3));
        Vao.VertexAttributePointer(2, 4, VertexAttribPointerType.Float, sizeof(VertexData), sizeof(Vector3) * 2);
        Gl.BindVertexArray(null);
    }
}
