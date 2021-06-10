// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyStatControlBase : IMyStatControl
  {
    private Vector2 m_position;
    private Vector2 m_size;
    private MyStatControls m_parent;

    public float StatCurrent { get; set; }

    public float StatMaxValue { get; set; }

    public float StatMinValue { get; set; }

    public string StatString { get; set; }

    public uint FadeInTimeMs { get; set; }

    public uint FadeOutTimeMs { get; set; }

    public uint MaxOnScreenTimeMs { get; set; }

    public uint SpentInStateTimeMs { get; set; }

    public MyStatControlState State { get; set; }

    public VisualStyleCategory Category { get; set; }

    public Vector4 ColorMask { get; set; }

    public MyAlphaBlinkBehavior BlinkBehavior { get; private set; }

    public MyStatControls Parent => this.m_parent;

    public Vector2 Position
    {
      get => this.m_position;
      set
      {
        Vector2 position = this.m_position;
        this.m_position = value;
        this.OnPositionChanged(position, value);
      }
    }

    public Vector2 Size
    {
      get => this.m_size;
      set
      {
        Vector2 size = this.m_size;
        this.m_size = value;
        this.OnSizeChanged(size, value);
      }
    }

    protected MyStatControlBase(MyStatControls parent)
    {
      this.ColorMask = Vector4.One;
      this.BlinkBehavior = new MyAlphaBlinkBehavior();
      this.FadeInTimeMs = 0U;
      this.FadeOutTimeMs = 0U;
      this.SpentInStateTimeMs = 0U;
      this.State = MyStatControlState.Invisible;
      this.m_parent = parent;
    }

    public virtual void Draw(float transitionAlpha)
    {
    }

    protected virtual void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
    {
    }

    protected virtual void OnSizeChanged(Vector2 oldSize, Vector2 newSize)
    {
    }
  }
}
