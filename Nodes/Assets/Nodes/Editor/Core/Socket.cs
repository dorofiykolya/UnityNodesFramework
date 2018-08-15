using System;
using Nodes.Editor.Data;
using Nodes.Editor.Utils;

namespace Nodes.Editor.Core
{
  [Serializable]
  public abstract class Socket
  {
    private object _value;
    private IMatcher _matcher = AnyType.Default;

    public string Name { get; private set; }
    public Guid Guid { get; set; }

    protected Socket(string name, SocketType type)
    {
      Name = name;
      Type = type;
    }

    public SocketType Type { get; private set; }

    public Connection Connection { get; private set; }

    public Connection Connect(Socket otherSocket, Guid guid)
    {
      if (Type == SocketType.Input && otherSocket is SocketIn) throw new ArgumentException();
      if (Type == SocketType.Output && otherSocket is SocketOut) throw new ArgumentException();
      if (!NodeUtils.CanConnect(this, otherSocket)) throw new ArgumentException();

      Connection = null;

      var @in = Type == SocketType.Input ? (SocketIn)this : (SocketIn)otherSocket;
      var @out = Type == SocketType.Output ? (SocketOut)this : (SocketOut)otherSocket;
      Connection = new Connection(guid, @in, @out);
      otherSocket.Connection = Connection;

      Connection.In.ResetValue();
      Connection.Out.ResetValue();

      return Connection;
    }

    public void Disconnect()
    {
      if (Connection != null)
      {
        var connection = Connection;
        connection.In.Connection = null;
        connection.In.ResetValue();
        connection.Out.Connection = null;
        connection.Out.ResetValue();
      }
    }

    public Node Owner { get; protected set; }

    protected void SetMatcher(IMatcher matcher)
    {
      _matcher = matcher;
    }

    public Type[] MatchTypes
    {
      get { return _matcher.Types; }
    }

    public bool Match(Type type)
    {
      return _matcher.Match(type);
    }

    public bool Connected
    {
      get { return Connection != null; }
    }

    public T Convert<T>(object value)
    {
      return _matcher.Convert<T>(value);
    }

    protected void SetValue(object value)
    {
      _value = value;
      if (Connected)
      {
        if (Connection.Out == this)
        {
          Connection.In.SetValue(value);
        }
      }
    }

    protected object GetValue()
    {
      return _value;
    }

    public void ResetValue()
    {
      _value = null;
    }

    public object Value
    {
      get { return _value; }
    }

    [PostCreate]
    private void PostCreate()
    {
      Guid = Guid.NewGuid();
    }
  }
}
