using System;
using Nodes.Editor.Core;

namespace Nodes.Editor.Math
{
  public class NumberMatcher : IMatcher
  {
    private static readonly Type[] _types = { typeof(int), typeof(uint), typeof(float), typeof(long), typeof(short), typeof(double), typeof(bool), typeof(byte), typeof(sbyte), typeof(char), typeof(decimal), typeof(ulong), typeof(ushort) };

    public bool Match(Type type)
    {
      return Array.IndexOf(_types, type) != -1;
    }

    public T Convert<T>(object value)
    {
      return AnyType.Default.Convert<T>(value);
    }

    public Type[] Types
    {
      get { return _types; }
    }
  }
}
