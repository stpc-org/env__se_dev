// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.MySerializableSpriteCollection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.GUI.TextPanel
{
  [ProtoContract]
  [Serializable]
  public struct MySerializableSpriteCollection
  {
    [ProtoMember(1)]
    [Nullable]
    public MySerializableSprite[] Sprites;
    [ProtoMember(4)]
    public int Length;

    public MySerializableSpriteCollection(MySerializableSprite[] sprites, int length)
    {
      this.Sprites = sprites;
      this.Length = length;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection\u003C\u003ESprites\u003C\u003EAccessor : IMemberAccessor<MySerializableSpriteCollection, MySerializableSprite[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializableSpriteCollection owner,
        in MySerializableSprite[] value)
      {
        owner.Sprites = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializableSpriteCollection owner,
        out MySerializableSprite[] value)
      {
        value = owner.Sprites;
      }
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection\u003C\u003ELength\u003C\u003EAccessor : IMemberAccessor<MySerializableSpriteCollection, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSpriteCollection owner, in int value) => owner.Length = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSpriteCollection owner, out int value) => value = owner.Length;
    }

    private class VRage_Game_GUI_TextPanel_MySerializableSpriteCollection\u003C\u003EActor : IActivator, IActivator<MySerializableSpriteCollection>
    {
      object IActivator.CreateInstance() => (object) new MySerializableSpriteCollection();

      MySerializableSpriteCollection IActivator<MySerializableSpriteCollection>.CreateInstance() => new MySerializableSpriteCollection();
    }
  }
}
