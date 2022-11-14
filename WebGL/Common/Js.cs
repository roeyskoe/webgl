using System.Runtime.InteropServices.JavaScript;

namespace WebGL.Common;

public static partial class Js
{
    [JSImport("globalThis.init")]
    public static partial void Init();

    [JSImport("globalThis.run")]
    public static partial void Run(double dt);

    [JSImport("globalThis.debugprint")]
    public static partial void Debugprint(JSObject pro, string buffer);
}
