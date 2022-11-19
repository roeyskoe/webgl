using Silk.NET.OpenGL;

namespace WebGL.Common;

public class Drawable
{
    public BufferObject<uint> Ebo;
    public BufferObject<float> Vbo;
    public VertexArrayObject<float, uint> Vao;

    public float[] Vertices;

    public readonly uint[] Indices;


    public Drawable(float[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;

        Ebo = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<float>(Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Vbo, Ebo);
        Vao.Bind();
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 6, 0);
        Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 6, 3);
        Gl.BindVertexArray(null);
    }
}
