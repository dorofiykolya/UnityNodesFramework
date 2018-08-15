using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodes.Editor.Data;

namespace Nodes.Editor.Core.Descriptions
{
  public class NodeDescription
  {
    private readonly Type _type;
    private readonly List<SocketDescription> _in = new List<SocketDescription>();
    private readonly List<SocketDescription> _out = new List<SocketDescription>();
    private readonly Attribute[] _attributes;

    public NodeDescription(Type type)
    {
      _type = type;
      _attributes = _type.GetCustomAttributes(true).Cast<Attribute>().ToArray();

      if (!_type.IsSubclassOf(typeof(Node)))
      {
        throw new ArgumentException();
      }

      ParseFields();
      ParseProperties();
    }

    public Attribute[] Attributes
    {
      get { return _attributes; }
    }

    public SocketDescription[] InSockets
    {
      get { return _in.ToArray(); }
    }

    public SocketDescription[] OutSockets
    {
      get { return _out.ToArray(); }
    }

    public SocketDescription[] GetSockets(SocketType type)
    {
      switch (type)
      {
        case SocketType.Input: return InSockets;
        case SocketType.Output: return OutSockets;
      }

      return null;
    }

    private void ParseFields()
    {
      var fields = _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.SetField);
      foreach (var fieldInfo in fields)
      {
        var attributes = fieldInfo.GetCustomAttributes(typeof(Socket), true);
        if (attributes.Length != 0)
        {
          ParseSocket(fieldInfo.FieldType);
        }
      }
    }

    private void ParseProperties()
    {
      var properties = _type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);
      foreach (var propertyInfo in properties)
      {
        var attributes = propertyInfo.GetCustomAttributes(typeof(Socket), true);
        if (attributes.Length != 0)
        {
          ParseSocket(propertyInfo.PropertyType);
        }
      }
    }

    private void ParseSocket(Type type)
    {
      Type matcherType = null;
      if (type.IsSubclassOf(typeof(SocketIn<>)))
      {
        matcherType = type.GetGenericArguments()[0];
        _in.Add(new SocketDescription(this, matcherType, SocketType.Input));
      }
      else if (type.IsSubclassOf(typeof(SocketOut<>)))
      {
        matcherType = type.GetGenericArguments()[0];
        _out.Add(new SocketDescription(this, matcherType, SocketType.Output));
      }
    }
  }
}
