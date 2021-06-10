// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.UseObject.IMyUseObject
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Entity.UseObject
{
  public interface IMyUseObject
  {
    IMyEntity Owner { get; }

    MyModelDummy Dummy { get; }

    float InteractiveDistance { get; }

    MatrixD ActivationMatrix { get; }

    MatrixD WorldMatrix { get; }

    uint RenderObjectID { get; }

    int InstanceID { get; }

    bool ShowOverlay { get; }

    UseActionEnum SupportedActions { get; }

    UseActionEnum PrimaryAction { get; }

    UseActionEnum SecondaryAction { get; }

    bool ContinuousUsage { get; }

    void Use(UseActionEnum actionEnum, IMyEntity user);

    MyActionDescription GetActionInfo(UseActionEnum actionEnum);

    bool HandleInput();

    void OnSelectionLost();

    void SetRenderID(uint id);

    void SetInstanceID(int id);

    bool PlayIndicatorSound { get; }
  }
}
