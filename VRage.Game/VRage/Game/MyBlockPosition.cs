// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBlockPosition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBlockPosition
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    public Vector2I Position;

    protected class VRage_Game_MyBlockPosition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyBlockPosition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBlockPosition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBlockPosition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyBlockPosition\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyBlockPosition, Vector2I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBlockPosition owner, in Vector2I value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBlockPosition owner, out Vector2I value) => value = owner.Position;
    }

    private class VRage_Game_MyBlockPosition\u003C\u003EActor : IActivator, IActivator<MyBlockPosition>
    {
      object IActivator.CreateInstance() => (object) new MyBlockPosition();

      MyBlockPosition IActivator<MyBlockPosition>.CreateInstance() => new MyBlockPosition();
    }
  }
}
