using Silk.NET.OpenGL;
using System.Numerics;

namespace WebGL.Common;

public unsafe class Material
{
    public string Name { get; set; }
    public float Shininess { get; set; }
    public Vector3 AmbientColor { get; set; }
    public Vector3 DiffuseColor { get; set; }
    public Vector3 SpecularColor { get; set; }
    public Vector3 EmissiveColor { get; set; }
    public float OpticalDensity { get; set; }
    public float Opacity { get; set; }
    public int Illumination { get; set; }
}
