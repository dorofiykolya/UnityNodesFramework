using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

namespace Nodes.Editor.Utils
{
  public class MethodAttributeUtil
  {
    public static MethodInfo[] GetMethods(Type type, Type attributeType)
    {
      IsSubclassOf(attributeType, typeof(Attribute));

      var result = new List<MethodInfo>();
      var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      foreach (var method in methods)
      {
        var attributes = method.GetCustomAttributes(attributeType, true);
        if (attributes.Length > 0)
        {
          result.Add(method);
        }
      }
      return result.ToArray();
    }

    public static void IsSubclassOf(Type type, Type parentType, string message = null)
    {
      Assert.IsNotNull(type);
      Assert.IsNotNull(parentType);
      if (!type.IsSubclassOf(parentType))
      {
        if (string.IsNullOrEmpty(message))
        {
          message = string.Format("{0}, , this argument is not a subclass of {1}", type, parentType);
        }
        throw new ArgumentException(message);
      }
    }
  }
}
