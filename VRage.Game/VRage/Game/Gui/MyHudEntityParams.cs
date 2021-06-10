// Decompiled with JetBrains decompiler
// Type: VRage.Game.Gui.MyHudEntityParams
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.Gui
{
  public struct MyHudEntityParams
  {
    private IMyEntity m_entity;
    private Vector3D m_position;

    public IMyEntity Entity
    {
      get => this.m_entity;
      set
      {
        this.m_entity = value;
        if (value == null)
          return;
        this.EntityId = value.EntityId;
      }
    }

    public long EntityId { get; set; }

    public Vector3D Position
    {
      get => this.m_entity == null || this.m_entity.PositionComp == null ? this.m_position : this.m_entity.PositionComp.GetPosition();
      set => this.m_position = value;
    }

    public StringBuilder Text { get; set; }

    public MyHudIndicatorFlagsEnum FlagsEnum { get; set; }

    public long Owner { get; set; }

    public MyOwnershipShareModeEnum Share { get; set; }

    public float BlinkingTime { get; set; }

    public Func<bool> ShouldDraw { get; set; }

    public MyHudEntityParams(StringBuilder text, long Owner, MyHudIndicatorFlagsEnum flagsEnum)
      : this()
    {
      this.Text = text;
      this.FlagsEnum = flagsEnum;
      this.Owner = Owner;
    }

    public MyHudEntityParams(MyObjectBuilder_HudEntityParams builder)
      : this()
    {
      this.m_entity = (IMyEntity) null;
      this.m_position = builder.Position;
      this.EntityId = builder.EntityId;
      this.Text = new StringBuilder(builder.Text);
      this.FlagsEnum = builder.FlagsEnum;
      this.Owner = builder.Owner;
      this.Share = builder.Share;
      this.BlinkingTime = builder.BlinkingTime;
    }

    public MyObjectBuilder_HudEntityParams GetObjectBuilder() => new MyObjectBuilder_HudEntityParams()
    {
      EntityId = this.EntityId,
      Position = this.Position,
      Text = this.Text.ToString(),
      FlagsEnum = this.FlagsEnum,
      Owner = this.Owner,
      Share = this.Share,
      BlinkingTime = this.BlinkingTime
    };
  }
}
