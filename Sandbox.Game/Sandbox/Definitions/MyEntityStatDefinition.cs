// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEntityStatDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EntityStatDefinition), null)]
  public class MyEntityStatDefinition : MyDefinitionBase
  {
    public float MinValue;
    public float MaxValue;
    public float DefaultValue;
    public bool EnabledInCreative;
    public string Name;
    public MyEntityStatDefinition.GuiDefinition GuiDef;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EntityStatDefinition entityStatDefinition = builder as MyObjectBuilder_EntityStatDefinition;
      this.MinValue = entityStatDefinition.MinValue;
      this.MaxValue = entityStatDefinition.MaxValue;
      this.DefaultValue = entityStatDefinition.DefaultValue;
      this.EnabledInCreative = entityStatDefinition.EnabledInCreative;
      this.Name = entityStatDefinition.Name;
      if (float.IsNaN(this.DefaultValue))
        this.DefaultValue = this.MaxValue;
      this.GuiDef = new MyEntityStatDefinition.GuiDefinition();
      if (entityStatDefinition.GuiDef == null)
        return;
      this.GuiDef.HeightMultiplier = entityStatDefinition.GuiDef.HeightMultiplier;
      this.GuiDef.Priority = entityStatDefinition.GuiDef.Priority;
      this.GuiDef.Color = (Vector3I) entityStatDefinition.GuiDef.Color;
      this.GuiDef.CriticalRatio = entityStatDefinition.GuiDef.CriticalRatio;
      this.GuiDef.DisplayCriticalDivider = entityStatDefinition.GuiDef.DisplayCriticalDivider;
      this.GuiDef.CriticalColorFrom = (Vector3I) entityStatDefinition.GuiDef.CriticalColorFrom;
      this.GuiDef.CriticalColorTo = (Vector3I) entityStatDefinition.GuiDef.CriticalColorTo;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_EntityStatDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_EntityStatDefinition;
      objectBuilder.MinValue = this.MinValue;
      objectBuilder.MaxValue = this.MaxValue;
      objectBuilder.DefaultValue = this.DefaultValue;
      objectBuilder.EnabledInCreative = this.EnabledInCreative;
      objectBuilder.Name = this.Name;
      objectBuilder.GuiDef = new MyObjectBuilder_EntityStatDefinition.GuiDefinition();
      objectBuilder.GuiDef.HeightMultiplier = this.GuiDef.HeightMultiplier;
      objectBuilder.GuiDef.Priority = this.GuiDef.Priority;
      objectBuilder.GuiDef.Color = (SerializableVector3I) this.GuiDef.Color;
      objectBuilder.GuiDef.CriticalRatio = this.GuiDef.CriticalRatio;
      objectBuilder.GuiDef.DisplayCriticalDivider = this.GuiDef.DisplayCriticalDivider;
      objectBuilder.GuiDef.CriticalColorFrom = (SerializableVector3I) this.GuiDef.CriticalColorFrom;
      objectBuilder.GuiDef.CriticalColorTo = (SerializableVector3I) this.GuiDef.CriticalColorTo;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    public struct GuiDefinition
    {
      public float HeightMultiplier;
      public int Priority;
      public Vector3I Color;
      public float CriticalRatio;
      public bool DisplayCriticalDivider;
      public Vector3I CriticalColorFrom;
      public Vector3I CriticalColorTo;
    }

    private class Sandbox_Definitions_MyEntityStatDefinition\u003C\u003EActor : IActivator, IActivator<MyEntityStatDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEntityStatDefinition();

      MyEntityStatDefinition IActivator<MyEntityStatDefinition>.CreateInstance() => new MyEntityStatDefinition();
    }
  }
}
