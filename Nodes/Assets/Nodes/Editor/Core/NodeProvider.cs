using System;
using System.Collections.Generic;
using System.Reflection;
using Nodes.Editor.Core.Descriptions;

namespace Nodes.Editor.Core
{
  public abstract class NodeProvider
  {
    private readonly Dictionary<Type, NodeDescription> _mapDescriptions;
    private readonly Type[] _types;

    protected NodeProvider(Type[] availableTypes)
    {
      _mapDescriptions = new Dictionary<Type, NodeDescription>();
      _types = availableTypes;
      foreach (var type in _types)
      {
        _mapDescriptions.Add(type, null);
      }
    }

    public Type[] AvailableTypes
    {
      get { return _types; }
    }

    public NodeDescription GetDescription(Type type)
    {
      if (!type.IsSubclassOf(typeof(Node))) throw new ArgumentException();

      NodeDescription description;
      if (!_mapDescriptions.TryGetValue(type, out description) || description == null)
      {
        description = new NodeDescription(type);
        _mapDescriptions[type] = description;
      }

      return description;
    }

    public static NodeProvider Create(string[] tags = null, bool includeGlobal = false)
    {
      return Create(AppDomain.CurrentDomain.GetAssemblies(), tags, includeGlobal);
    }

    public static NodeProvider Create(Assembly[] assemblies, string[] tags = null, bool includeGlobal = false)
    {
      var nodes = new List<Type>();

      foreach (var assembly in assemblies)
      {
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
          if (type.IsSubclassOf(typeof(Node)))
          {
            if (tags == null || tags.Length == 0)
            {
              nodes.Add(type);
            }
            else
            {
              var attributes = type.GetCustomAttributes(typeof(TagAttribute), false);
              if (attributes.Length != 0)
              {
                var tag = (TagAttribute)attributes[0];
                if (Array.IndexOf(tags, tag.Tag) != -1)
                {
                  nodes.Add(type);
                }
              }
              else if (includeGlobal)
              {
                nodes.Add(type);
              }
            }
          }
        }
      }
      var nodeProvider = new AssemblyNodeProvider(nodes.ToArray());
      return nodeProvider;
    }

    private class AssemblyNodeProvider : NodeProvider
    {
      public AssemblyNodeProvider(Type[] availableTypes) : base(availableTypes)
      {
      }
    }
  }
}
