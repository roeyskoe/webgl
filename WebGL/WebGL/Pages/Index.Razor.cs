using System;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Pages;

public partial class Index
{

    static string vertex = $$"""
        #version 300 es

        layout (location = 0) in vec3 vPos;
        layout (location = 1) in vec2 vUv;

        out vec2 fUv;

        void main()
        {
            gl_Position = vec4(vPos, 1.0);
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

    static JSObject Ebo;
    static JSObject Vbo;
    static JSObject Vao;
    static JSObject Shader;

    private static float[] Vertices =
{
            //X    Y      Z     U   V
             0.5f,  0.5f, 0.0f, 1f, 0f,
             0.5f, -0.5f, 0.0f, 1f, 1f,
            -0.5f, -0.5f, 0.0f, 0f, 1f,
            -0.5f,  0.5f, 0.5f, 0f, 0f
        };

    private static uint[] Indices =
    {
            0, 1, 3,
            1, 2, 3
        };


    protected override unsafe void OnAfterRender(bool firstRender)
    {
        Js.init();

        var vert = Gl.createShader((int)GLEnum.VertexShader);
        Gl.shaderSource(vert, vertex);
        Gl.compileShader(vert);
        Console.WriteLine(Gl.getShaderInfoLog(vert));

        var frag = Gl.createShader((int)GLEnum.FragmentShader);
        Gl.shaderSource(frag, fragment);
        Gl.compileShader(frag);
        Console.WriteLine(Gl.getShaderInfoLog(frag));

        Shader = Gl.createProgram();
        Gl.attachShader(Shader, vert);
        Gl.attachShader(Shader, frag);

        Gl.linkProgram(Shader);
        Console.WriteLine(Gl.getProgramInfoLog(Shader));


        Ebo = Gl.createBuffer();
        Gl.bindBuffer((int)GLEnum.ElementArrayBuffer, Ebo);
        fixed (void* d = &Indices[0])
        {
            Gl.bufferData((int)GLEnum.ElementArrayBuffer, sizeof(uint) * Indices.Length, (int)d, (int)GLEnum.StaticDraw);
        }

        Vbo = Gl.createBuffer();
        Gl.bindBuffer((int)GLEnum.ArrayBuffer, Vbo);
        fixed (void* d = &Vertices[0])
        {
            Gl.bufferData((int)GLEnum.ArrayBuffer, sizeof(float) * Vertices.Length, (int)d, (int)GLEnum.StaticDraw);
        }

        Vao = Gl.createVertexArray();
        Gl.bindVertexArray(Vao);
        Gl.bindBuffer((int)GLEnum.ArrayBuffer, Vbo);
        Gl.bindBuffer((int)GLEnum.ElementArrayBuffer, Ebo);

        Gl.vertexAttribPointer(0, 3, (int)VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        Gl.enableVertexAttribArray(0);
        Gl.vertexAttribPointer(1, 2, (int)VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        Gl.enableVertexAttribArray(1);

        Js.run(0);
    }

    [JSExport]
    public static void Update(float dt)
    {
        Gl.useProgram(Shader);
        Gl.clearColor(0.6f, 0.4f, 0.2f, 1);
        Gl.clear(16384);

        Gl.bindVertexArray(Vao);

        Gl.DrawElements((int)GLEnum.Triangles, Indices.Length, (int)GLEnum.UnsignedInt, 0);
    }

}


public partial class Gl
{
    [JSImport("globalThis.gl.clear")]
    public static partial void clear(int val);

    [JSImport("globalThis.gl.clearColor")]
    public static partial void clearColor(float r, float g, float b, float a);

    [JSImport("globalThis.gl.createShader")]
    public static partial JSObject createShader(int type);

    [JSImport("globalThis.gl.shaderSource")]
    public static partial void shaderSource(JSObject shader, string source);

    [JSImport("globalThis.gl.compileShader")]
    public static partial void compileShader(JSObject shader);

    [JSImport("globalThis.gl.createProgram")]
    public static partial JSObject createProgram();

    [JSImport("globalThis.gl.attachShader")]
    public static partial JSObject attachShader(JSObject program, JSObject shader);

    [JSImport("globalThis.gl.linkProgram")]
    public static partial JSObject linkProgram(JSObject program);

    [JSImport("globalThis.gl.getShaderInfoLog")]
    public static partial string getShaderInfoLog(JSObject shader);

    [JSImport("globalThis.gl.getProgramInfoLog")]
    public static partial string getProgramInfoLog(JSObject program);

    [JSImport("globalThis.gl.getAttribLocation")]
    public static partial int getAttribLocation(JSObject program, string attrib);

    [JSImport("globalThis.gl.createBuffer")]
    public static partial JSObject createBuffer();

    [JSImport("globalThis.gl.bindBuffer")]
    public static partial JSObject bindBuffer(int type, JSObject buffer);

    [JSImport("globalThis.bufferData")]
    public static partial JSObject bufferData(int type, int size, int bufferptr, int usage);

    [JSImport("globalThis.gl.genBuffer")]
    public static partial JSObject GenBuffer();

    [JSImport("globalThis.gl.createVertexArray")]
    public static partial JSObject createVertexArray();

    [JSImport("globalThis.gl.bindVertexArray")]
    public static partial void bindVertexArray(JSObject array);

    [JSImport("globalThis.gl.enableVertexAttribArray")]
    public static partial void enableVertexAttribArray(int pos);

    [JSImport("globalThis.gl.vertexAttribPointer")]
    public static partial void vertexAttribPointer(int position, int size, int type, bool normalize, int stride, int offset);

    [JSImport("globalThis.gl.viewport")]
    public static partial void viewport(int x, int y, int width, int height);

    [JSImport("globalThis.gl.useProgram")]
    public static partial void useProgram(JSObject program);

    [JSImport("globalThis.gl.drawArrays")]
    public static partial void drawArrays(int promitiveType, int offset, int count);

    [JSImport("globalThis.gl.drawElements")]
    public static partial void DrawElements(int promitiveType, int count, int type, int offset);

    [JSImport("globalThis.gl.getError")]
    public static partial int getError();
}
public static partial class Js
{
    [JSImport("globalThis.init")]
    public static partial void init();

    [JSImport("globalThis.run")]
    public static partial void run(double dt);

    [JSImport("globalThis.debugprint")]
    public static partial void debugprint(int buffer);
}