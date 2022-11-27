using Silk.NET.OpenGL;
using System.Numerics;

namespace WebGL.Common;

public unsafe class Drawable
{
    public BufferObject<uint> Ebo;
    public BufferObject<VertexPositionColor> Vbo;
    public VertexArrayObject<VertexPositionColor, uint> Vao;

    public VertexPositionColor[] Vertices;

    public readonly uint[] Indices;


    public Drawable(VertexPositionColor[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;

        Ebo = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<VertexPositionColor>(Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<VertexPositionColor, uint>(Vbo, Ebo);
        Vao.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, sizeof(VertexPositionColor), 0);
        Vao.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, sizeof(VertexPositionColor), sizeof(Vector3));
        Gl.BindVertexArray(null);
    }
}
