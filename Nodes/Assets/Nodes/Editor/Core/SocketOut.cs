using System;
using Nodes.Editor.Data;

namespace Nodes.Editor.Core
{
  [Serializable]
  public abstract class SocketOut : Socket
  {
    protected SocketOut(string name, Node owner) : base(name, SocketType.Output)
    {
      Owner = owner;
    }

    public void SetValue<T>(T value)
    {
      if (!Match(typeof(T)))
      {
        throw new ArgumentException();
      }
      base.SetValue(value);
    }
  }

  [Serializable]
  public sealed class SocketOut<T> : SocketOut where T : IMatcher
  {
    public SocketOut(string name, Node owner) : base(name, owner)
    {
    }
  }
}
