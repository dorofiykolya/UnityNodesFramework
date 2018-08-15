using System;

namespace Nodes.Editor.Core
{
  public class TypeMatcher<T> : IMatcher
  {
    private static readonly Type[] _types = new Type[] { typeof(T) };

    public bool Match(Type type)
    {
      return type.IsSubclassOf(typeof(T));
    }

    public T1 Convert<T1>(object value)
    {
      return (T1)value;
    }

    public Type[] Types
    {
      get { return _types; }
    }
  }
}
