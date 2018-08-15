using System;

namespace Nodes.Editor.Core
{
  public class TitleAttribute : Attribute
  {
    public string[] Categories;
    public string Name;
    public string Description;

    public TitleAttribute(params string[] categories)
    {
      Categories = categories;
    }
  }
}
