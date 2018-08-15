using System;
using Nodes.Editor.Data;

namespace Nodes.Editor.Core
{
  [Serializable]
  public abstract class SocketIn : Socket
  {
    protected SocketIn(string name, Node owner) : base(name, SocketType.Input)
    {
      Owner = owner;
    }

    public T GetValue<T>()
    {
      return Convert<T>(GetValue());
    }
  }

  [Serializable]
  public sealed class SocketIn<T> : SocketIn where T : IMatcher, new()
  {
    public SocketIn(string name, Node owner) : base(name, owner)
    {
      SetMatcher(new T());
    }
  }
}
