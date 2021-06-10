// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyResourceSourceComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.Components
{
  public abstract class MyResourceSourceComponentBase : MyEntityComponentBase
  {
    public abstract float CurrentOutputByType(MyDefinitionId resourceTypeId);

    public abstract float MaxOutputByType(MyDefinitionId resourceTypeId);

    public abstract float DefinedOutputByType(MyDefinitionId resourceTypeId);

    public abstract bool ProductionEnabledByType(MyDefinitionId resourceTypeId);
  }
}
