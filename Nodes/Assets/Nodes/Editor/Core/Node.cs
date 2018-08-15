using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodes.Editor.Data;
using Nodes.Editor.Utils;

namespace Nodes.Editor.Core
{
  [Serializable]
  public abstract class Node
  {
    private readonly Dictionary<string, Socket> _sockets = new Dictionary<string, Socket>();
    private readonly List<SocketIn> _in = new List<SocketIn>();
    private readonly List<SocketOut> _out = new List<SocketOut>();
    private ExternalDataProvider _externalDataProvider;

    public Guid Guid { get; private set; }

    public Socket GetSocket(string name)
    {
      return _sockets[name];
    }

    public Socket[] Sockets
    {
      get { return _sockets.Values.ToArray(); }
    }

    public void GetSockets(List<Socket> sockets, SocketType type)
    {
      switch (type)
      {
        case SocketType.Input:
          {
            foreach (var socketIn in _in)
            {
              sockets.Add(socketIn);
            }
            break;
          }
        case SocketType.Output:
          {
            foreach (var socketOut in _out)
            {
              sockets.Add(socketOut);
            }
            break;
          }
      }
    }

    public SocketIn[] InSockets
    {
      get { return _in.ToArray(); }
    }

    public SocketOut[] OutSockets
    {
      get { return _out.ToArray(); }
    }

    public Socket[] GetSockets(SocketType type)
    {
      switch (type)
      {
        case SocketType.Input: return InSockets;
        case SocketType.Output: return OutSockets;
      }

      return null;
    }

    public ExternalDataProvider ExternalDataProvider
    {
      get { return _externalDataProvider; }
    }

    public abstract void Calculate(ICalculateContext context);

    public CalculateContext Calculate(NodeCoroutine nodeCoroutine)
    {
      var context = new CalculateContext(nodeCoroutine, null, this);
      CalculateInternal(nodeCoroutine, context);
      return context;
    }

    private void CalculateInternal(NodeCoroutine nodeCoroutine, CalculateContext context)
    {
      context.Process();
      foreach (var socketIn in InSockets)
      {
        if (socketIn.Connected)
        {
          socketIn.Connection.Out.Owner.CalculateInternal(nodeCoroutine, new CalculateContext(nodeCoroutine, context, socketIn.Connection.Out.Owner));
        }
      }
    }

    [PostCreate]
    private void PostCreate(ExternalDataProvider externalDataProvider, Guid guid)
    {
      _externalDataProvider = externalDataProvider;
      Guid = guid;

      var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField |
                                       BindingFlags.SetField);
      var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                               BindingFlags.SetProperty);

      foreach (var fieldInfo in fields)
      {
        var attributes = fieldInfo.GetCustomAttributes(typeof(SocketAttribute), true);
        if (attributes.Length != 0 && fieldInfo.FieldType.IsSubclassOf(typeof(Socket)))
        {
          var value = (Socket)fieldInfo.GetValue(this);
          if (value == null)
          {
            value = (Socket)Activator.CreateInstance(fieldInfo.FieldType, new object[] { fieldInfo.Name, this });
            fieldInfo.SetValue(this, value);
          }

          switch (value.Type)
          {
            case SocketType.Input:
              _in.Add((SocketIn)value);
              break;
            case SocketType.Output:
              _out.Add((SocketOut)value);
              break;
          }

          _sockets.Add(fieldInfo.Name, value);

          MethodInvoker<Socket, PostCreateAttribute>.Invoke(value);
        }
      }

      foreach (var propertyInfo in properties)
      {
        var attributes = propertyInfo.GetCustomAttributes(typeof(SocketAttribute), true);
        if (attributes.Length != 0 && propertyInfo.PropertyType.IsSubclassOf(typeof(Socket)))
        {
          var value = (Socket)propertyInfo.GetValue(this, null);
          if (value == null)
          {
            value = (Socket)Activator.CreateInstance(propertyInfo.PropertyType, new object[] { propertyInfo.Name, this });
            propertyInfo.SetValue(this, value, null);
          }

          switch (value.Type)
          {
            case SocketType.Input:
              _in.Add((SocketIn)value);
              break;
            case SocketType.Output:
              _out.Add((SocketOut)value);
              break;
          }

          _sockets.Add(propertyInfo.Name, value);

          MethodInvoker<Socket, PostCreateAttribute>.Invoke(value);
        }
      }
    }
  }
}
