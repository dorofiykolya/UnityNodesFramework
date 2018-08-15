using System;

namespace Nodes.Editor.Core
{
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public class SocketAttribute : Attribute
  {
    public uint Color;
  }
}
