// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyRuntimeObjectBuilderId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.ObjectBuilders
{
  [ProtoContract]
  public struct MyRuntimeObjectBuilderId
  {
    public static readonly MyRuntimeObjectBuilderIdComparer Comparer = new MyRuntimeObjectBuilderIdComparer();
    [ProtoMember(1)]
    public readonly ushort Value;

    public MyRuntimeObjectBuilderId(ushort value) => this.Value = value;

    public bool IsValid => this.Value > (ushort) 0;

    public override string ToString() => string.Format("{0}: {1}", (object) this.Value, (object) (MyObjectBuilderType) this);

    protected class VRage_ObjectBuilders_MyRuntimeObjectBuilderId\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyRuntimeObjectBuilderId, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRuntimeObjectBuilderId owner, in ushort value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRuntimeObjectBuilderId owner, out ushort value) => value = owner.Value;
    }

    private class VRage_ObjectBuilders_MyRuntimeObjectBuilderId\u003C\u003EActor : IActivator, IActivator<MyRuntimeObjectBuilderId>
    {
      object IActivator.CreateInstance() => (object) new MyRuntimeObjectBuilderId();

      MyRuntimeObjectBuilderId IActivator<MyRuntimeObjectBuilderId>.CreateInstance() => new MyRuntimeObjectBuilderId();
    }
  }
}
