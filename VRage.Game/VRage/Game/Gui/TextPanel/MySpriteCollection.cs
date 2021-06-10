// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.MySpriteCollection
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
  public struct MySpriteCollection
  {
    [ProtoMember(1)]
    [Nullable]
    public MySprite[] Sprites;

    public MySpriteCollection(MySprite[] sprites) => this.Sprites = sprites;

    public static implicit operator MySerializableSpriteCollection(
      MySpriteCollection collection)
    {
      MySerializableSprite[] sprites = (MySerializableSprite[]) null;
      if (collection.Sprites != null && collection.Sprites.Length != 0)
      {
        sprites = new MySerializableSprite[collection.Sprites.Length];
        for (int index = 0; index < sprites.Length; ++index)
          sprites[index] = (MySerializableSprite) collection.Sprites[index];
      }
      return new MySerializableSpriteCollection(sprites, sprites != null ? sprites.Length : 0);
    }

    public static implicit operator MySpriteCollection(
      MySerializableSpriteCollection collection)
    {
      MySprite[] sprites = (MySprite[]) null;
      if (collection.Sprites != null && collection.Sprites.Length != 0)
      {
        sprites = new MySprite[collection.Sprites.Length];
        for (int index = 0; index < sprites.Length; ++index)
          sprites[index] = (MySprite) collection.Sprites[index];
      }
      return new MySpriteCollection(sprites);
    }

    protected class VRage_Game_GUI_TextPanel_MySpriteCollection\u003C\u003ESprites\u003C\u003EAccessor : IMemberAccessor<MySpriteCollection, MySprite[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySpriteCollection owner, in MySprite[] value) => owner.Sprites = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySpriteCollection owner, out MySprite[] value) => value = owner.Sprites;
    }

    private class VRage_Game_GUI_TextPanel_MySpriteCollection\u003C\u003EActor : IActivator, IActivator<MySpriteCollection>
    {
      object IActivator.CreateInstance() => (object) new MySpriteCollection();

      MySpriteCollection IActivator<MySpriteCollection>.CreateInstance() => new MySpriteCollection();
    }
  }
}
