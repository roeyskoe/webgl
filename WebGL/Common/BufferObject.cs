using Silk.NET.OpenGL;
using System;
using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Common;

public class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
{
    private JSObject _handle;
    private BufferTargetARB _bufferType;

    public unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
    {
        _bufferType = bufferType;

        _handle = Gl.CreateBuffer();
        Bind();
        fixed (void* d = data)
        {
            Gl.BufferData((int)bufferType, (int)(data.Length * sizeof(TDataType)), (int)d, (int)BufferUsageARB.StaticDraw);
        }
        Js.Debugprint(_handle);
    }

    public void Bind()
    {
        Gl.BindBuffer((int)_bufferType, _handle);
    }

    public void Dispose()
    {
        //Gl.DeleteBuffer(_handle);
    }
}
