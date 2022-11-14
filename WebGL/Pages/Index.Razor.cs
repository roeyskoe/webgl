using System;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices.JavaScript;
using WebGL.Common;
using System.Numerics;

namespace WebGL.Pages;

public partial class Index
{

    static string vertex = $$"""
        #version 300 es

        layout (location = 0) in vec3 vPos;
        layout (location = 1) in vec2 vUv;

        uniform mat4 uModel;
        uniform mat4 uView;
        uniform mat4 uProjection;

        out vec2 fUv;

        void main()
        {
            gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
            fUv = vUv;
        }
        """;

    static string fragment = $$"""
        #version 300 es
        
        precision highp float;

        in vec2 fUv;
        
        out vec4 FragColor;
        
        void main() {
          FragColor  = vec4(fUv.x, fUv.y, 0.5, 1);
        }
        """;

    static BufferObject<uint> Ebo;
    static BufferObject<float> Vbo;
    static VertexArrayObject<float, uint> Vao;
    static Common.Shader Shader;

    private static float[] Vertices =
{
            //X    Y      Z     U   V
             0.5f,  0.5f, 0.5f, 1f, 0f,
             0.5f, -0.5f, 0.5f, 1f, 1f,
            -0.5f, -0.5f, 0.5f, 0f, 1f,
            -0.5f,  0.5f, 0.5f, 0f, 0f,

             0.5f,  0.5f, -0.5f, 1f, 0f,
             0.5f, -0.5f, -0.5f, 1f, 1f,
            -0.5f, -0.5f, -0.5f, 0f, 1f,
            -0.5f,  0.5f, -0.5f, 0f, 0f
        };

    private static readonly uint[] Indices =
    {
            0, 1, 3,
            1, 2, 3,
            0, 4, 7,
            7, 3, 0,
            4, 5, 6,
            6, 7, 4,
            1, 5, 6,
            6, 2, 1,
            0, 4, 1,
            4, 5, 1,
            3, 6, 2,
            3, 7, 6
        };


    protected override unsafe void OnAfterRender(bool firstRender)
    {
        Js.Init();

        Shader = new Common.Shader(vertex, fragment);

        Ebo = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<float>(Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Vbo, Ebo);

        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        Js.Run(0);
    }

    [JSExport]
    public static void Update(float dt, float width, float height)
    {
        Shader.Use();
        Gl.ClearColor(0.5f, 0.9f, 0.9f, 1);
        Gl.Clear((int)(GLEnum.ColorBufferBit | GLEnum.DepthBufferBit));

        Gl.Enable((int)GLEnum.DepthTest);
        //Gl.Enable((int)GLEnum.CullFace);

        Vao.Bind();

        Vector3 CameraPosition = new Vector3(0, 0, 3.0f);
        Vector3 CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 CameraUp = Vector3.UnitY;
        Vector3 CameraDirection = Vector3.Zero;

        var model = Matrix4x4.CreateRotationY(dt / 2500) * Matrix4x4.CreateRotationX(dt / 3000) * Matrix4x4.CreateRotationZ(dt / 3500); ;
        var view = Matrix4x4.CreateLookAt(CameraPosition, Vector3.Zero, CameraUp);
        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI/4, width / height, 0.1f, 100.0f);

        Shader.SetUniform("uModel", model);
        Shader.SetUniform("uView", view);
        Shader.SetUniform("uProjection", projection);

        Gl.DrawElements((int)GLEnum.Triangles, Indices.Length, (int)GLEnum.UnsignedInt, 0);
    }

}
