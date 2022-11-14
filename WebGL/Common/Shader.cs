using Silk.NET.OpenGL;
using System;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Common;

public class Shader : IDisposable
{
    private JSObject _handle;
    private GL _gl;

    public Shader(string vertexData, string fragmentData)
    {
        JSObject vertex = LoadShader(ShaderType.VertexShader, vertexData);
        JSObject fragment = LoadShader(ShaderType.FragmentShader, fragmentData);
        _handle = Gl.CreateProgram();
        Gl.AttachShader(_handle, vertex);
        Gl.AttachShader(_handle, fragment);
        Gl.LinkProgram(_handle);
        
        Console.WriteLine(Gl.GetProgramInfoLog(_handle));

        Gl.DetachShader(_handle, vertex);
        Gl.DetachShader(_handle, fragment);
        Gl.DeleteShader(vertex);
        Gl.DeleteShader(fragment);
    }

    public void Use()
    {
        Gl.UseProgram(_handle);
    }

    public void SetUniform(string name, int value)
    {
        JSObject location = Gl.GetUniformLocation(_handle, name);
        //if (location == -1)
        //{
        //    throw new Exception($"{name} uniform not found on shader.");
        //}
        Gl.Uniform1(location, value);
    }

    public unsafe void SetUniform(string name, Matrix4x4 value)
    {
        JSObject location = Gl.GetUniformLocation(_handle, name);
        //if (location == -1)
        //{
        //    throw new Exception($"{name} uniform not found on shader.");
        //}
        Gl.UniformMatrix4(location, (int)(float*)&value);
    }

    public void SetUniform(string name, float value)
    {
        JSObject location = Gl.GetUniformLocation(_handle, name);
        //if (location == -1)
        //{
        //    throw new Exception($"{name} uniform not found on shader.");
        //}
        Gl.Uniform1(location, value);
    }

    public void Dispose()
    {
        //Gl.DeleteProgram(_handle);
    }

    private JSObject LoadShader(ShaderType type, string data)
    {
        JSObject handle = Gl.CreateShader((int)type);
        Gl.ShaderSource(handle, data);
        Gl.CompileShader(handle);
        string infoLog = Gl.GetShaderInfoLog(handle);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return handle;
    }
}
