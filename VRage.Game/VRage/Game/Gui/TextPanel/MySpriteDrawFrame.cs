// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.MySpriteDrawFrame
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace VRage.Game.GUI.TextPanel
{
  public struct MySpriteDrawFrame : IDisposable
  {
    private MyList<MySprite> m_sprites;
    private Action<MySpriteDrawFrame> m_submitFrameCallback;

    public MySpriteDrawFrame(Action<MySpriteDrawFrame> submitFrameCallback)
    {
      this.m_sprites = (MyList<MySprite>) null;
      this.m_submitFrameCallback = submitFrameCallback;
      if (submitFrameCallback == null)
        return;
      this.m_sprites = PoolManager.Get<MyList<MySprite>>();
    }

    public MySpriteDrawFrame.ClearClipToken Clip(
      int x,
      int y,
      int width,
      int height)
    {
      this.Add(MySprite.CreateClipRect(new Rectangle(x, y, width, height)));
      return new MySpriteDrawFrame.ClearClipToken(this);
    }

    public void Add(MySprite sprite) => this.m_sprites?.Add(sprite);

    public void AddRange(IEnumerable<MySprite> sprites) => this.m_sprites?.AddRange(sprites);

    public MySpriteCollection ToCollection()
    {
      if (this.m_sprites == null || this.m_sprites.Count == 0)
        return new MySpriteCollection();
      MySprite[] sprites = new MySprite[this.m_sprites.Count];
      for (int index = 0; index < this.m_sprites.Count; ++index)
        sprites[index] = this.m_sprites[index];
      return new MySpriteCollection(sprites);
    }

    public void AddToList(List<MySprite> list)
    {
      if (list == null || this.m_sprites == null)
        return;
      list.AddRange((IEnumerable<MySprite>) this.m_sprites);
    }

    public void Dispose()
    {
      if (this.m_submitFrameCallback == null)
        return;
      this.m_submitFrameCallback.InvokeIfNotNull<MySpriteDrawFrame>(this);
      this.m_submitFrameCallback = (Action<MySpriteDrawFrame>) null;
      this.m_sprites.ClearFast();
      PoolManager.Return<MyList<MySprite>>(ref this.m_sprites);
    }

    public struct ClearClipToken : IDisposable
    {
      private MySpriteDrawFrame m_frame;

      public ClearClipToken(MySpriteDrawFrame frame) => this.m_frame = frame;

      public void Dispose() => this.m_frame.Add(MySprite.CreateClearClipRect());
    }
  }
}
