using System;

namespace Nodes.Editor.Core
{
  public interface IMatcher
  {
    bool Match(Type type);
    T Convert<T>(object value);
    Type[] Types { get; }
  }

  public class AnyType : IMatcher
  {
    private static readonly Type[] _types = new Type[] { typeof(object) };
    public static readonly AnyType Default = new AnyType();

    public bool Match(Type type)
    {
      return true;
    }

    public T Convert<T>(object value)
    {
      return (T)System.Convert.ChangeType(value, typeof(T));
    }

    public Type[] Types
    {
      get { return _types; }
    }
  }
}
