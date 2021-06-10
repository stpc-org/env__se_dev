// Decompiled with JetBrains decompiler
// Type: VRage.Game.Cutscene
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class Cutscene
  {
    [ProtoMember(4)]
    public string Name = "";
    [ProtoMember(7)]
    public string StartEntity = "";
    [ProtoMember(10)]
    public string StartLookAt = "";
    [ProtoMember(13)]
    public string NextCutscene = "";
    [ProtoMember(16)]
    public float StartingFOV = 70f;
    [ProtoMember(19)]
    public bool CanBeSkipped = true;
    [ProtoMember(22)]
    public bool FireEventsDuringSkip = true;
    [ProtoMember(25)]
    [XmlArrayItem("Node")]
    public List<CutsceneSequenceNode> SequenceNodes;

    protected class VRage_Game_Cutscene\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<Cutscene, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_Cutscene\u003C\u003EStartEntity\u003C\u003EAccessor : IMemberAccessor<Cutscene, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in string value) => owner.StartEntity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out string value) => value = owner.StartEntity;
    }

    protected class VRage_Game_Cutscene\u003C\u003EStartLookAt\u003C\u003EAccessor : IMemberAccessor<Cutscene, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in string value) => owner.StartLookAt = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out string value) => value = owner.StartLookAt;
    }

    protected class VRage_Game_Cutscene\u003C\u003ENextCutscene\u003C\u003EAccessor : IMemberAccessor<Cutscene, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in string value) => owner.NextCutscene = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out string value) => value = owner.NextCutscene;
    }

    protected class VRage_Game_Cutscene\u003C\u003EStartingFOV\u003C\u003EAccessor : IMemberAccessor<Cutscene, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in float value) => owner.StartingFOV = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out float value) => value = owner.StartingFOV;
    }

    protected class VRage_Game_Cutscene\u003C\u003ECanBeSkipped\u003C\u003EAccessor : IMemberAccessor<Cutscene, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in bool value) => owner.CanBeSkipped = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out bool value) => value = owner.CanBeSkipped;
    }

    protected class VRage_Game_Cutscene\u003C\u003EFireEventsDuringSkip\u003C\u003EAccessor : IMemberAccessor<Cutscene, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in bool value) => owner.FireEventsDuringSkip = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out bool value) => value = owner.FireEventsDuringSkip;
    }

    protected class VRage_Game_Cutscene\u003C\u003ESequenceNodes\u003C\u003EAccessor : IMemberAccessor<Cutscene, List<CutsceneSequenceNode>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Cutscene owner, in List<CutsceneSequenceNode> value) => owner.SequenceNodes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Cutscene owner, out List<CutsceneSequenceNode> value) => value = owner.SequenceNodes;
    }

    private class VRage_Game_Cutscene\u003C\u003EActor : IActivator, IActivator<Cutscene>
    {
      object IActivator.CreateInstance() => (object) new Cutscene();

      Cutscene IActivator<Cutscene>.CreateInstance() => new Cutscene();
    }
  }
}
