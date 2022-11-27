using System;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices.JavaScript;
using WebGL.Common;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WebGL.Pages;

public partial class Index
{
    static Common.Shader Shader;
    static Camera Camera;

    static List<Drawable> Drawables = new List<Drawable>();

    public string Load(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = assembly.GetName().Name;
        using (Stream stream = assembly.GetManifestResourceStream($"{name}.{path}"))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Js.Init();

            Shader = new Common.Shader(Load("Content.vert.gls"), Load("Content.frag.gls"));
            
            Drawables.Add(new Drawable(new VertexPositionColor[]
                {
                                              //X     Y     Z    R   G   B   A
                     new VertexPositionColor(0.5f,  0.5f, 0.5f, 1f, 0f, 0f, 1f),
                     new VertexPositionColor(0.5f, -0.5f, 0.5f, 1f, 1f, 1f, 1f),
                     new VertexPositionColor(-0.5f, -0.5f, 0.5f, 0f, 1f, 1f, 1f),
                     new VertexPositionColor(-0.5f,  0.5f, 0.5f, 0f, 1f, 0f, 1f),

                     new VertexPositionColor(0.5f,  0.5f, -0.5f, 1f, 0f, 0f, 1f),
                     new VertexPositionColor(0.5f, -0.5f, -0.5f, 1f, 1f, 1f, 1f),
                     new VertexPositionColor(-0.5f, -0.5f, -0.5f, 0f, 1f, 1f, 1f),
                     new VertexPositionColor(-0.5f,  0.5f, -0.5f, 0f, 1f, 0f, 1f)
                }, new uint[]
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
                }
            ));

            Drawables.Add(new Drawable(new VertexPositionColor[]
                { 
                                             //X     Y    Z     R    G      B    A
                      new VertexPositionColor(50f,  -5f, 50f,  0.2f, 0.2f, 0.2f, 1f),
                      new VertexPositionColor(50f,  -5f, -50f, 0.2f, 0.2f, 0.2f, 1f),
                      new VertexPositionColor(-50f, -5f, -50f, 0.2f, 0.2f, 0.2f, 1f),
                      new VertexPositionColor(-50f, -5f, 50f,  0.2f, 0.2f, 0.2f, 1f)
                }, new uint[]
                {
                    0, 1, 3,
                    1, 2, 3,
                }
            ));

            Camera = new Camera(new Vector3(0,0,5), new Vector3(0.0f, 0.0f, -1.0f), Vector3.UnitY, 1);

            Js.Run(0);
        }


    }

    static MouseState CurrMouseState;
    static MouseState PrevMouseState;


    [JSExport]
    public static void MouseMove(int x, int y)
    {
        CurrMouseState.Position += new Vector2(x, y);

        //Console.WriteLine($"Move x:{x}, y:{y}");
    }

    [JSExport]
    public static void MouseDown(int x, int y)
    {
        //Console.WriteLine($"Down x:{x}, y:{y}");
    }

    [JSExport]
    public static void MouseUp(int x, int y)
    {
        //Console.WriteLine($"Up x:{x}, y:{y}");
    }

    [JSExport]
    public static void KeyDown(string key)
    {
        KeysDown.Add(key);
        //Console.WriteLine($"keyDown:{key}");
    }

    [JSExport]
    public static void KeyUp(string key)
    {
        KeysDown.Remove(key);
    }

    static HashSet<string> KeysDown = new HashSet<string>();

    [JSExport]
    public static void Update(float dt, float width, float height)
    {
        HandleKeys(dt);
        HandleMouse(dt);

        PrevMouseState = CurrMouseState;

        Shader.Use();
        Gl.ClearColor(0.5f, 0.9f, 0.9f, 1);
        Gl.Clear((int)(GLEnum.ColorBufferBit | GLEnum.DepthBufferBit));

        Gl.Enable((int)GLEnum.DepthTest);
        //Gl.Enable((int)GLEnum.CullFace);

        Camera.AspectRatio = width / height;

        var model = Matrix4x4.CreateRotationX(0); //Matrix4x4.CreateRotationY(dt / 2500) * Matrix4x4.CreateRotationX(dt / 3000) * Matrix4x4.CreateRotationZ(dt / 3500); ;

        Shader.SetUniform("uModel", model);
        Shader.SetUniform("uView", Camera.GetViewMatrix());
        Shader.SetUniform("uProjection", Camera.GetProjectionMatrix());

        foreach (var item in Drawables)
        {
            item.Vao.Bind();
            Gl.DrawElements((int)GLEnum.Triangles, item.Indices.Length, (int)GLEnum.UnsignedInt, 0);
        }

    }

    public static void HandleKeys(float dt)
    {
        float step = dt / 500000;
        foreach (var key in KeysDown)
        {
            if (key == "ArrowLeft")
                Camera.Position -= Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * step;
            if (key == "ArrowRight")
                Camera.Position += Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * step;
            if (key == "ArrowUp")
                Camera.Position += Vector3.Normalize(Camera.Front) * step;
            if (key == "ArrowDown")
                Camera.Position -= Vector3.Normalize(Camera.Front) * step;
        }
    }



    public static void HandleMouse(float dt)
    {
        float step = dt / 500000;
        Vector2 change = CurrMouseState.Position - PrevMouseState.Position;

        Camera.ModifyDirection(change.X * step, change.Y * step);
    }

}
