// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MyHighlightData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Gui
{
  [ProtoContract]
  [Serializable]
  public struct MyHighlightData
  {
    [ProtoMember(5)]
    public long EntityId;
    [ProtoMember(10)]
    public Color? OutlineColor;
    [ProtoMember(15)]
    public int Thickness;
    [ProtoMember(20)]
    public ulong PulseTimeInFrames;
    [ProtoMember(25)]
    public long PlayerId;
    [ProtoMember(30)]
    public bool IgnoreUseObjectData;
    [ProtoMember(35)]
    public string SubPartNames;

    public MyHighlightData(
      long entityId = 0,
      int thickness = -1,
      ulong pulseTimeInFrames = 0,
      Color? outlineColor = null,
      bool ignoreUseObjectData = false,
      long playerId = -1,
      string subPartNames = null)
    {
      this.EntityId = entityId;
      this.Thickness = thickness;
      this.OutlineColor = outlineColor;
      this.PulseTimeInFrames = pulseTimeInFrames;
      this.PlayerId = playerId;
      this.IgnoreUseObjectData = ignoreUseObjectData;
      this.SubPartNames = subPartNames;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out long value) => value = owner.EntityId;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EOutlineColor\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, Color?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in Color? value) => owner.OutlineColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out Color? value) => value = owner.OutlineColor;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EThickness\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in int value) => owner.Thickness = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out int value) => value = owner.Thickness;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EPulseTimeInFrames\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in ulong value) => owner.PulseTimeInFrames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out ulong value) => value = owner.PulseTimeInFrames;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EIgnoreUseObjectData\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in bool value) => owner.IgnoreUseObjectData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out bool value) => value = owner.IgnoreUseObjectData;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003ESubPartNames\u003C\u003EAccessor : IMemberAccessor<MyHighlightData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHighlightData owner, in string value) => owner.SubPartNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHighlightData owner, out string value) => value = owner.SubPartNames;
    }

    private class VRage_Game_ObjectBuilders_Gui_MyHighlightData\u003C\u003EActor : IActivator, IActivator<MyHighlightData>
    {
      object IActivator.CreateInstance() => (object) new MyHighlightData();

      MyHighlightData IActivator<MyHighlightData>.CreateInstance() => new MyHighlightData();
    }
  }
}
