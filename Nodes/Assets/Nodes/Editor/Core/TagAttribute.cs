using System;

namespace Nodes.Editor.Core
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class TagAttribute : Attribute
  {
    private readonly string _tag;

    public TagAttribute(string tag)
    {
      _tag = tag;
    }

    public string Tag
    {
      get { return _tag; }
    }
  }
}
