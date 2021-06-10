// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyUpgradeModuleInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  public struct MyUpgradeModuleInfo
  {
    [ProtoMember(1)]
    public string UpgradeType { get; set; }

    [ProtoMember(4)]
    public float Modifier { get; set; }

    [ProtoMember(7)]
    public MyUpgradeModifierType ModifierType { get; set; }

    protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo\u003C\u003EUpgradeType\u003C\u003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUpgradeModuleInfo owner, in string value) => owner.UpgradeType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUpgradeModuleInfo owner, out string value) => value = owner.UpgradeType;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo\u003C\u003EModifier\u003C\u003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUpgradeModuleInfo owner, in float value) => owner.Modifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUpgradeModuleInfo owner, out float value) => value = owner.Modifier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo\u003C\u003EModifierType\u003C\u003EAccessor : IMemberAccessor<MyUpgradeModuleInfo, MyUpgradeModifierType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyUpgradeModuleInfo owner, in MyUpgradeModifierType value) => owner.ModifierType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyUpgradeModuleInfo owner, out MyUpgradeModifierType value) => value = owner.ModifierType;
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyUpgradeModuleInfo\u003C\u003EActor : IActivator, IActivator<MyUpgradeModuleInfo>
    {
      object IActivator.CreateInstance() => (object) new MyUpgradeModuleInfo();

      MyUpgradeModuleInfo IActivator<MyUpgradeModuleInfo>.CreateInstance() => new MyUpgradeModuleInfo();
    }
  }
}
