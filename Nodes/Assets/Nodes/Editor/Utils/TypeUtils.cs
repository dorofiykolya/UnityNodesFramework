using System;

namespace Nodes.Editor.Utils
{
  public static class TypeUtils
  {
    public static bool IsSubclassOf(this Type type, Type baseclass)
    {
      return baseclass.IsAssignableFrom(type);
    }
  }
}
