using System;
using Nodes.Editor.Data;

namespace Nodes.Editor.Core.Descriptions
{
  public class SocketDescription
  {
    private readonly NodeDescription _owner;
    private readonly Type _matcherType;
    private readonly SocketType _type;
    private readonly IMatcher _matcher;

    public SocketDescription(NodeDescription owner, Type matcherType, SocketType type)
    {
      _owner = owner;
      _matcherType = matcherType;
      _type = type;
      _matcher = (IMatcher)Activator.CreateInstance(matcherType);
    }

    public SocketType Type
    {
      get { return _type; }
    }

    public IMatcher Matcher
    {
      get { return _matcher; }
    }

    public NodeDescription Owner
    {
      get { return _owner; }
    }

    public Type MatcherType
    {
      get { return _matcherType; }
    }
  }
}
