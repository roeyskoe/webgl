using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Common;

// Who needs type safety? :D
public partial class Gl
{
    // ============== Basic stuff ==================

    [JSImport("globalThis.gl.clear")]
    public static partial void Clear(int val);

    [JSImport("globalThis.gl.clearColor")]
    public static partial void ClearColor(float r, float g, float b, float a);

    [JSImport("globalThis.gl.enable")]
    public static partial void Enable(int val);

    [JSImport("globalThis.gl.getError")]
    public static partial int GetError();

    [JSImport("globalThis.gl.viewport")]
    public static partial void Viewport(int x, int y, int width, int height);

    // ============== Drawing ==================

    [JSImport("globalThis.gl.drawArrays")]
    public static partial void DrawArrays(int promitiveType, int offset, int count);

    [JSImport("globalThis.gl.drawElements")]
    public static partial void DrawElements(int promitiveType, int count, int type, int offset);

    // ============== Shaders ==================

    [JSImport("globalThis.gl.attachShader")]
    public static partial JSObject AttachShader(JSObject program, JSObject shader);

    [JSImport("globalThis.gl.compileShader")]
    public static partial void CompileShader(JSObject shader);

    [JSImport("globalThis.gl.createProgram")]
    public static partial JSObject CreateProgram();

    [JSImport("globalThis.gl.createShader")]
    public static partial JSObject CreateShader(int type);

    [JSImport("globalThis.gl.deleteShader")]
    public static partial void DeleteShader(JSObject shader);

    [JSImport("globalThis.gl.detachShader")]
    public static partial void DetachShader(JSObject program, JSObject shader);

    [JSImport("globalThis.gl.getAttribLocation")]
    public static partial int GetAttribLocation(JSObject program, string attrib);

    [JSImport("globalThis.gl.getProgramInfoLog")]
    public static partial string GetProgramInfoLog(JSObject program);

    [JSImport("globalThis.gl.getShaderInfoLog")]
    public static partial string GetShaderInfoLog(JSObject shader);

    [JSImport("globalThis.gl.getUniformLocation")]
    public static partial JSObject GetUniformLocation(JSObject program, string name);

    [JSImport("globalThis.gl.linkProgram")]
    public static partial JSObject LinkProgram(JSObject program);

    [JSImport("globalThis.gl.shaderSource")]
    public static partial void ShaderSource(JSObject shader, string source);

    [JSImport("globalThis.gl.useProgram")]
    public static partial void UseProgram(JSObject program);

    [JSImport("globalThis.gl.uniform1f")]
    public static partial void Uniform1(JSObject location, float value);

    [JSImport("globalThis.uniformMatrix2fv")]
    public static partial void UniformMatrix2(JSObject location, int ptr);

    [JSImport("globalThis.uniformMatrix3fv")]
    public static partial void UniformMatrix3(JSObject location, int ptr);

    [JSImport("globalThis.uniformMatrix4fv")]
    public static partial void UniformMatrix4(JSObject location, int ptr);

    [JSImport("globalThis.uniform4fv")]
    public static partial void Uniform4(JSObject location, int ptr);

    // ============== Buffers etc ==================

    [JSImport("globalThis.gl.bindBuffer")]
    public static partial JSObject BindBuffer(int type, JSObject buffer);

    [JSImport("globalThis.gl.bindVertexArray")]
    public static partial void BindVertexArray(JSObject array);

    [JSImport("globalThis.bufferData")]
    public static partial JSObject BufferData(int type, int size, int bufferptr, int usage);

    [JSImport("globalThis.gl.createBuffer")]
    public static partial JSObject CreateBuffer();

    [JSImport("globalThis.gl.createVertexArray")]
    public static partial JSObject CreateVertexArray();

    [JSImport("globalThis.gl.enableVertexAttribArray")]
    public static partial void EnableVertexAttribArray(int pos);

    [JSImport("globalThis.gl.vertexAttribPointer")]
    public static partial void VertexAttribPointer(int position, int size, int type, bool normalize, int stride, int offset);
}
