// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MultilineData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Gui
{
  [ProtoContract]
  public class MultilineData
  {
    [ProtoMember(5)]
    public bool Completed;
    [ProtoMember(10)]
    public bool IsObjective;
    [ProtoMember(15)]
    public string Data;
    [ProtoMember(20)]
    public int CharactersDisplayed;

    protected class VRage_Game_ObjectBuilders_Gui_MultilineData\u003C\u003ECompleted\u003C\u003EAccessor : IMemberAccessor<MultilineData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MultilineData owner, in bool value) => owner.Completed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MultilineData owner, out bool value) => value = owner.Completed;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MultilineData\u003C\u003EIsObjective\u003C\u003EAccessor : IMemberAccessor<MultilineData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MultilineData owner, in bool value) => owner.IsObjective = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MultilineData owner, out bool value) => value = owner.IsObjective;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MultilineData\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MultilineData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MultilineData owner, in string value) => owner.Data = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MultilineData owner, out string value) => value = owner.Data;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MultilineData\u003C\u003ECharactersDisplayed\u003C\u003EAccessor : IMemberAccessor<MultilineData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MultilineData owner, in int value) => owner.CharactersDisplayed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MultilineData owner, out int value) => value = owner.CharactersDisplayed;
    }

    private class VRage_Game_ObjectBuilders_Gui_MultilineData\u003C\u003EActor : IActivator, IActivator<MultilineData>
    {
      object IActivator.CreateInstance() => (object) new MultilineData();

      MultilineData IActivator<MultilineData>.CreateInstance() => new MultilineData();
    }
  }
}
