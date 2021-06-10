// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentSensor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentSensor : MyRenderComponent
  {
    private MySensorBase m_sensor;
    private float m_lastHighlight;
    protected Vector4 m_color;
    private bool DrawSensor = true;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_sensor = this.Container.Entity as MySensorBase;
    }

    public override void Draw()
    {
      if (!this.DrawSensor)
        return;
      this.SetHighlight();
      MatrixD matrixD = this.Container.Entity.PositionComp.WorldMatrixRef;
      if (MySession.Static.ControlledEntity != this)
        return;
      Vector4 vector4 = Color.Red.ToVector4();
      MySimpleObjectDraw.DrawLine(matrixD.Translation, matrixD.Translation + matrixD.Forward * (double) this.Container.Entity.PositionComp.LocalVolume.Radius * 1.20000004768372, new MyStringId?(), ref vector4, 0.05f);
    }

    protected void SetHighlight()
    {
      this.SetHighlight(new Vector4(0.0f, 0.0f, 0.0f, 0.3f));
      if (this.m_sensor.AnyEntityWithState(MySensorBase.EventType.Add))
        this.SetHighlight(new Vector4(1f, 0.0f, 0.0f, 0.3f), true);
      else if (this.m_sensor.AnyEntityWithState(MySensorBase.EventType.Delete))
        this.SetHighlight(new Vector4(1f, 0.0f, 1f, 0.3f), true);
      else if (this.m_sensor.HasAnyMoved())
      {
        this.SetHighlight(new Vector4(0.0f, 0.0f, 1f, 0.3f));
      }
      else
      {
        if (!this.m_sensor.AnyEntityWithState(MySensorBase.EventType.None))
          return;
        this.SetHighlight(new Vector4(0.4f, 0.4f, 0.4f, 0.3f));
      }
    }

    private void SetHighlight(Vector4 color, bool keepForMinimalTime = false)
    {
      if ((double) MySandboxGame.TotalGamePlayTimeInMilliseconds <= (double) this.m_lastHighlight + 300.0)
        return;
      this.m_color = color;
      if (!keepForMinimalTime)
        return;
      this.m_lastHighlight = (float) MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private class Sandbox_Game_Components_MyRenderComponentSensor\u003C\u003EActor : IActivator, IActivator<MyRenderComponentSensor>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentSensor();

      MyRenderComponentSensor IActivator<MyRenderComponentSensor>.CreateInstance() => new MyRenderComponentSensor();
    }
  }
}
