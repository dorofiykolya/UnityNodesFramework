using System;
using System.Collections.Generic;

namespace Nodes.Editor.Core
{
  public class ExternalDataProvider
  {
    private readonly Dictionary<Type, object> _map = new Dictionary<Type, object>();
    private readonly Dictionary<string, object> _keyMap = new Dictionary<string, object>();

    public void SetValue<T>(T value)
    {
      _map[typeof(T)] = value;
    }

    public T GetValue<T>()
    {
      object value;
      _map.TryGetValue(typeof(T), out value);
      return (T)value;
    }

    public object this[string key]
    {
      get
      {
        object value;
        _keyMap.TryGetValue(key, out value);
        return value;
      }
      set { _keyMap[key] = value; }
    }
  }
}
