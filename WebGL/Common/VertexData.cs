using System.Numerics;

namespace WebGL.Common;

public struct VertexData
{
    public Vector3 Position { get; set; }
    public Vector3 Normal { get; set; }
    public Vector4 Color { get; set; }

    public VertexData(Vector3 position, Vector3 normal, Vector4 color)
    {
        Position = position;
        Normal = normal;
        Color = color;
    }

    public VertexData(float x, float y, float z, float nx, float ny, float nz, float r, float g, float b, float a)
    {
        Position = new Vector3(x, y, z);
        Normal = new Vector3(nx, ny, nz);
        Color = new Vector4(r, g, b, a);
    }
}
