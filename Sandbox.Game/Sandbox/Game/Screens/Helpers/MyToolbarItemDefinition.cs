// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyToolbarItemDefinition : MyToolbarItem
  {
    public MyDefinitionBase Definition;

    public MyToolbarItemDefinition()
    {
      int num = (int) this.SetEnabled(true);
      this.WantsToBeActivated = true;
    }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is MyToolbarItemDefinition toolbarItemDefinition && this.Definition != null && this.Definition.Id.Equals(toolbarItemDefinition.Definition.Id);
    }

    public override sealed int GetHashCode() => this.Definition.Id.GetHashCode();

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
    {
      if (this.Definition == null)
        return (MyObjectBuilder_ToolbarItem) null;
      MyObjectBuilder_ToolbarItemDefinition objectBuilder = (MyObjectBuilder_ToolbarItemDefinition) MyToolbarItemFactory.CreateObjectBuilder((MyToolbarItem) this);
      objectBuilder.DefinitionId = (SerializableDefinitionId) this.Definition.Id;
      return (MyObjectBuilder_ToolbarItem) objectBuilder;
    }

    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) ((MyObjectBuilder_ToolbarItemDefinition) data).DefinitionId, out this.Definition) || !this.Definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
        return false;
      int num1 = (int) this.SetDisplayName(this.Definition.DisplayNameText);
      int num2 = (int) this.SetIcons(this.Definition.Icons);
      return true;
    }
  }
}
