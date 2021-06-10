// Decompiled with JetBrains decompiler
// Type: VRage.Game.CutsceneSequenceNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class CutsceneSequenceNode
  {
    [ProtoMember(28)]
    [XmlAttribute]
    public float Time;
    [ProtoMember(31)]
    [XmlAttribute]
    public string LookAt;
    [ProtoMember(34)]
    [XmlAttribute]
    public string Event;
    [ProtoMember(37)]
    [XmlAttribute]
    public float EventDelay;
    [ProtoMember(40)]
    [XmlAttribute]
    public string LockRotationTo;
    [ProtoMember(43)]
    [XmlAttribute]
    public string AttachTo;
    [ProtoMember(46)]
    [XmlAttribute]
    public string AttachPositionTo;
    [ProtoMember(49)]
    [XmlAttribute]
    public string AttachRotationTo;
    [ProtoMember(52)]
    [XmlAttribute]
    public string MoveTo;
    [ProtoMember(55)]
    [XmlAttribute]
    public string SetPositionTo;
    [ProtoMember(58)]
    [XmlAttribute]
    public float ChangeFOVTo;
    [ProtoMember(61)]
    [XmlAttribute]
    public string RotateTowards;
    [ProtoMember(64)]
    [XmlAttribute]
    public string SetRotationLike;
    [ProtoMember(67)]
    [XmlAttribute]
    public string RotateLike;
    [ProtoMember(70)]
    [XmlArrayItem("Waypoint")]
    public List<CutsceneSequenceNodeWaypoint> Waypoints;

    public string GetNodeSummary()
    {
      string str = string.IsNullOrEmpty(this.Event) ? "" : " - \"" + this.Event + "\" event" + ((double) this.EventDelay > 0.0 ? " (" + this.EventDelay.ToString() + "s delay)" : "");
      return this.Time.ToString() + "s" + str;
    }

    public string GetNodeDescription()
    {
      StringBuilder stringBuilder = new StringBuilder(this.Time.ToString() + "s");
      if (!string.IsNullOrEmpty(this.Event))
        stringBuilder.Append(", \"" + this.Event + "\" event" + ((double) this.EventDelay > 0.0 ? " (" + this.EventDelay.ToString() + "s delay)" : ""));
      if ((double) this.ChangeFOVTo > 0.0)
        stringBuilder.Append(", change FOV to " + this.ChangeFOVTo.ToString() + " over time");
      if (!string.IsNullOrEmpty(this.SetPositionTo))
        stringBuilder.Append(", set position to \"" + this.SetPositionTo + "\"");
      if (!string.IsNullOrEmpty(this.MoveTo))
        stringBuilder.Append(", move over time to \"" + this.MoveTo + "\"");
      if (!string.IsNullOrEmpty(this.LookAt))
        stringBuilder.Append(", look at \"" + this.LookAt + "\" instantly");
      if (!string.IsNullOrEmpty(this.RotateTowards))
        stringBuilder.Append(", look at \"" + this.RotateTowards + "\" over time");
      if (!string.IsNullOrEmpty(this.SetRotationLike))
        stringBuilder.Append(", set rotation like \"" + this.SetRotationLike + "\" instantly");
      if (!string.IsNullOrEmpty(this.RotateLike))
        stringBuilder.Append(", change rotation like \"" + this.RotateLike + "\" over time");
      if (this.LockRotationTo != null)
      {
        if (string.IsNullOrEmpty(this.LockRotationTo))
          stringBuilder.Append(", stop looking at target");
        else
          stringBuilder.Append(", look at \"" + this.LockRotationTo + "\" until disabled");
      }
      if (this.AttachTo != null)
      {
        if (string.IsNullOrEmpty(this.AttachTo))
          stringBuilder.Append(", stop attachment");
        else
          stringBuilder.Append(", attach to \"" + this.AttachTo + "\"");
      }
      else
      {
        if (this.AttachPositionTo != null)
        {
          if (string.IsNullOrEmpty(this.AttachPositionTo))
            stringBuilder.Append(", stop position attachment");
          else
            stringBuilder.Append(", attach position to \"" + this.AttachPositionTo + "\"");
        }
        if (this.AttachRotationTo != null)
        {
          if (string.IsNullOrEmpty(this.AttachRotationTo))
            stringBuilder.Append(", stop rotation attachment");
          else
            stringBuilder.Append(", attach rotation to \"" + this.AttachRotationTo + "\"");
        }
      }
      if (this.Waypoints != null && this.Waypoints.Count > 1)
        stringBuilder.Append(", movement spline over " + this.Waypoints.Count.ToString() + " points");
      return stringBuilder.ToString();
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ETime\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in float value) => owner.Time = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out float value) => value = owner.Time;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ELookAt\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.LookAt = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.LookAt;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EEvent\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.Event = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.Event;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EEventDelay\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in float value) => owner.EventDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out float value) => value = owner.EventDelay;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ELockRotationTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.LockRotationTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.LockRotationTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EAttachTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.AttachTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.AttachTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EAttachPositionTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.AttachPositionTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.AttachPositionTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EAttachRotationTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.AttachRotationTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.AttachRotationTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EMoveTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.MoveTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.MoveTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ESetPositionTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.SetPositionTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.SetPositionTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EChangeFOVTo\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in float value) => owner.ChangeFOVTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out float value) => value = owner.ChangeFOVTo;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ERotateTowards\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.RotateTowards = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.RotateTowards;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ESetRotationLike\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.SetRotationLike = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.SetRotationLike;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003ERotateLike\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CutsceneSequenceNode owner, in string value) => owner.RotateLike = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CutsceneSequenceNode owner, out string value) => value = owner.RotateLike;
    }

    protected class VRage_Game_CutsceneSequenceNode\u003C\u003EWaypoints\u003C\u003EAccessor : IMemberAccessor<CutsceneSequenceNode, List<CutsceneSequenceNodeWaypoint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref CutsceneSequenceNode owner,
        in List<CutsceneSequenceNodeWaypoint> value)
      {
        owner.Waypoints = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref CutsceneSequenceNode owner,
        out List<CutsceneSequenceNodeWaypoint> value)
      {
        value = owner.Waypoints;
      }
    }

    private class VRage_Game_CutsceneSequenceNode\u003C\u003EActor : IActivator, IActivator<CutsceneSequenceNode>
    {
      object IActivator.CreateInstance() => (object) new CutsceneSequenceNode();

      CutsceneSequenceNode IActivator<CutsceneSequenceNode>.CreateInstance() => new CutsceneSequenceNode();
    }
  }
}
