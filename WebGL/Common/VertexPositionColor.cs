using System.Numerics;

namespace WebGL.Common;

public struct VertexPositionColor
{
    public Vector3 Position { get; set; }
    public Vector4 Color { get; set; }

    public VertexPositionColor(Vector3 position, Vector4 color)
    {
        Position = position;
        Color = color;
    }

    public VertexPositionColor(float x, float y, float z, float r, float g, float b, float a)
    {
        Position = new Vector3(x, y, z);
        Color = new Vector4(r, g, b, a);
    }
}
