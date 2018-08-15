using UnityEngine;

namespace Nodes.Editor.Utils
{
  public static class ColorExtension
  {
    public static uint ToUInt(this Color color)
    {
      uint result = 0;
      var c = (Color32)color;
      result = (uint)c.a << 24 | (uint)c.r << 16 | (uint)c.g << 8 | (uint)c.b;
      return result;
    }

    public static Color ToColor(this uint color)
    {
      var result = new Color32
      {
        a = (byte)(color >> 24),
        r = (byte)(color >> 16),
        g = (byte)(color >> 8),
        b = (byte)(color)
      };
      return (Color)result;
    }
  }
}
