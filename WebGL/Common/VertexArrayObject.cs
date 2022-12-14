using Silk.NET.OpenGL;
using System;
using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Common;

public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
    where TVertexType : unmanaged
    where TIndexType : unmanaged
{
    private JSObject _handle;

    public VertexArrayObject(BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
    {
        _handle = Gl.CreateVertexArray();
        Bind();
        vbo.Bind();
        ebo?.Bind();
    }

    public unsafe void VertexAttributePointer(int index, int count, VertexAttribPointerType type, int vertexSizeBytes, int offSetBytes)
    {
        Gl.VertexAttribPointer(index, count, (int)type, false, vertexSizeBytes, (int)(void*)(offSetBytes));
        Gl.EnableVertexAttribArray(index);
    }

    public void Bind()
    {
        Gl.BindVertexArray(_handle);
    }

    public void Dispose()
    {
        //Gl.DeleteVertexArray(_handle);
    }
}
