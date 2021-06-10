// Decompiled with JetBrains decompiler
// Type: VRage.Game.CutsceneSequenceNodeWaypoint
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class CutsceneSequenceNodeWaypoint
  {
    [ProtoMember(73)]
    [XmlAttribute]
    public string Name = "";
    [ProtoMember(76)]
    [XmlAttribute]
    public float Time;

    protected class VRage_Game_CutsceneSequenceNodeWaypoint\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNodeWaypoint, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNodeWaypoint owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNodeWaypoint owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_CutsceneSequenceNodeWaypoint\u003C\u003ETime\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNodeWaypoint, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNodeWaypoint owner, in float value) => owner.Time = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNodeWaypoint owner, out float value) => value = owner.Time;
    }

    private class VRage_Game_CutsceneSequenceNodeWaypoint\u003C\u003EActor : IActivator, IActivator<CutsceneSequenceNodeWaypoint>
    {
      object IActivator.CreateInstance() => (object) new CutsceneSequenceNodeWaypoint();

      CutsceneSequenceNodeWaypoint IActivator<CutsceneSequenceNodeWaypoint>.CreateInstance() => new CutsceneSequenceNodeWaypoint();
    }
  }
}
