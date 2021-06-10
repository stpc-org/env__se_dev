// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyParameterValue
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyParameterValue
  {
    [ProtoMember(1)]
    public string ParameterName;
    [ProtoMember(5)]
    public string Value;

    public MyParameterValue()
    {
      this.ParameterName = string.Empty;
      this.Value = string.Empty;
    }

    public MyParameterValue(string paramName) => this.ParameterName = paramName;

    protected class VRage_Game_MyParameterValue\u003C\u003EParameterName\u003C\u003EAccessor : IMemberAccessor<MyParameterValue, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyParameterValue owner, in string value) => owner.ParameterName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyParameterValue owner, out string value) => value = owner.ParameterName;
    }

    protected class VRage_Game_MyParameterValue\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyParameterValue, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyParameterValue owner, in string value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyParameterValue owner, out string value) => value = owner.Value;
    }

    private class VRage_Game_MyParameterValue\u003C\u003EActor : IActivator, IActivator<MyParameterValue>
    {
      object IActivator.CreateInstance() => (object) new MyParameterValue();

      MyParameterValue IActivator<MyParameterValue>.CreateInstance() => new MyParameterValue();
    }
  }
}
