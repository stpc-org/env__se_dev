// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTextSurfaceScriptBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  public abstract class MyTextSurfaceScriptBase : IMyTextSurfaceScript, IDisposable
  {
    public static readonly Color DEFAULT_BACKGROUND_COLOR = new Color(0, 88, 151);
    public static readonly Color DEFAULT_FONT_COLOR = new Color(179, 237, (int) byte.MaxValue);
    protected IMyTextSurface m_surface;
    protected IMyCubeBlock m_block;
    protected Vector2 m_size;
    protected Vector2 m_halfSize;
    protected Vector2 m_scale;
    protected Color m_backgroundColor = MyTextSurfaceScriptBase.DEFAULT_BACKGROUND_COLOR;
    protected Color m_foregroundColor = MyTextSurfaceScriptBase.DEFAULT_FONT_COLOR;

    public IMyTextSurface Surface => this.m_surface;

    public IMyCubeBlock Block => this.m_block;

    public Color ForegroundColor => this.m_foregroundColor;

    public Color BackgroundColor => this.m_backgroundColor;

    public abstract ScriptUpdate NeedsUpdate { get; }

    protected MyTextSurfaceScriptBase(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
    {
      this.m_surface = surface;
      this.m_block = block;
      this.m_size = size;
      this.m_halfSize = size / 2f;
      this.m_scale = size / 512f;
    }

    public virtual void Run()
    {
    }

    public virtual void Dispose()
    {
      this.m_surface = (IMyTextSurface) null;
      this.m_block = (IMyCubeBlock) null;
    }

    public static void FitRect(Vector2 texture, ref Vector2 rect)
    {
      float num = Math.Min(texture.X / rect.X, texture.Y / rect.Y);
      rect *= num;
    }
  }
}
