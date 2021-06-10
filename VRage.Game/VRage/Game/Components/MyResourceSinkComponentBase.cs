// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyResourceSinkComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Collections;
using VRage.ModAPI;

namespace VRage.Game.Components
{
  public abstract class MyResourceSinkComponentBase : MyEntityComponentBase
  {
    public abstract ListReader<MyDefinitionId> AcceptedResources { get; }

    public abstract float CurrentInputByType(MyDefinitionId resourceTypeId);

    public abstract bool IsPowerAvailable(MyDefinitionId resourceTypeId, float power);

    public abstract bool IsPoweredByType(MyDefinitionId resourceTypeId);

    public abstract float MaxRequiredInputByType(MyDefinitionId resourceTypeId);

    public abstract void SetMaxRequiredInputByType(
      MyDefinitionId resourceTypeId,
      float newMaxRequiredInput);

    public abstract float RequiredInputByType(MyDefinitionId resourceTypeId);

    public abstract void SetInputFromDistributor(
      MyDefinitionId resourceTypeId,
      float newResourceInput,
      bool isAdaptible,
      bool fireEvents = true);

    public abstract void SetRequiredInputByType(
      MyDefinitionId resourceTypeId,
      float newRequiredInput);

    public abstract void SetRequiredInputFuncByType(
      MyDefinitionId resourceTypeId,
      Func<float> newRequiredInputFunc);

    public abstract float SuppliedRatioByType(MyDefinitionId resourceTypeId);

    public abstract IMyEntity TemporaryConnectedEntity { get; set; }
  }
}
