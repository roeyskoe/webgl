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


            // https://sketchfab.com/3d-models/book-vertex-chameleon-study-51b0b3bdcd844a9e951a9ede6f192da8
            Drawable d1 = ObjReader.Parse("Content.book.obj");
            d1.Position = new Vector3(0, 1, 0);
            Drawables.Add(d1);

            Drawable d3 = ObjReader.Parse("Content.test.obj");
            d3.Position = new Vector3(1, -5, 0); // TODO: Normalize size
            Drawables.Add(d3);
            
            Camera = new Camera(new Vector3(0, 0, 5), new Vector3(0.0f, 0.0f, -1.0f), Vector3.UnitY, 1);

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

        Shader.SetUniform("uView", Camera.GetViewMatrix());
        Shader.SetUniform("uProjection", Camera.GetProjectionMatrix());
        Shader.SetUniform("uDiffuse", Vector4.One);

        foreach (var item in Drawables)
        {
            item.Draw(Shader);
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
