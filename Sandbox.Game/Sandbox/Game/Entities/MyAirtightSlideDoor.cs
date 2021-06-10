// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAirtightSlideDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_AirtightSlideDoor))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyAirtightSlideDoor), typeof (Sandbox.ModAPI.Ingame.IMyAirtightSlideDoor)})]
  public class MyAirtightSlideDoor : MyAirtightDoorGeneric, Sandbox.ModAPI.IMyAirtightSlideDoor, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase, Sandbox.ModAPI.Ingame.IMyAirtightSlideDoor
  {
    private bool hadEnoughPower;

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      MyAirtightDoorGeneric.m_emissiveTextureNames = new string[1]
      {
        "Emissive"
      };
      base.Init(builder, cubeGrid);
    }

    protected override void UpdateDoorPosition()
    {
      if (this.m_subparts.Count == 0)
        return;
      float num1 = (float) Math.Sqrt(1.13750004768372);
      float num2 = this.m_currOpening * 1.75f;
      float num3 = this.m_currOpening * 1.570796f;
      float num4;
      if ((double) num2 < (double) num1)
      {
        num4 = (float) Math.Asin((double) num2 / 1.20000004768372);
      }
      else
      {
        float num5 = (float) ((1.75 - (double) num2) / (1.75 - (double) num1));
        num4 = (float) (1.57079601287842 - (double) num5 * (double) num5 * (1.57079601287842 - Math.Asin((double) num1 / 1.20000004768372)));
      }
      float z = num2 - 1f;
      MyGridPhysics physics1 = this.CubeGrid.Physics;
      bool flag1 = !Sync.IsServer;
      int index = 0;
      bool flag2 = true;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart != null)
        {
          Matrix result1;
          Matrix.CreateRotationY(flag2 ? num4 : -num4, out result1);
          result1.Translation = new Vector3(flag2 ? -1.2f : 1.2f, 0.0f, z);
          Matrix renderLocal = result1 * this.PositionComp.LocalMatrixRef;
          MyPhysicsComponentBase physics2 = subpart.Physics;
          if (flag1 && physics2 != null)
          {
            Matrix result2 = Matrix.Identity;
            result2.Translation = new Vector3(flag2 ? -0.55f : 0.55f, 0.0f, 0.0f);
            Matrix.Multiply(ref result2, ref result1, out result2);
            subpart.PositionComp.SetLocalMatrix(ref result2);
          }
          subpart.PositionComp.SetLocalMatrix(ref result1, (object) physics2, true, ref renderLocal, true);
          if (physics1 != null && physics2 != null && this.m_subpartConstraintsData.Count > index)
          {
            physics1.RigidBody.Activate();
            physics2.RigidBody.Activate();
            result1 = Matrix.Invert(result1);
            this.m_subpartConstraintsData[index].SetInBodySpace(this.PositionComp.LocalMatrixRef, result1, (MyPhysicsBody) physics1, (MyPhysicsBody) physics2);
          }
        }
        flag2 = !flag2;
        ++index;
      }
    }

    protected override void FillSubparts()
    {
      this.m_subparts.Clear();
      MyEntitySubpart myEntitySubpart;
      if (this.Subparts.TryGetValue("DoorLeft", out myEntitySubpart))
        this.m_subparts.Add(myEntitySubpart);
      if (!this.Subparts.TryGetValue("DoorRight", out myEntitySubpart))
        return;
      this.m_subparts.Add(myEntitySubpart);
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    protected override void UpdateEmissivity(bool force)
    {
      Color color = Color.Red;
      float emissivity = 1f;
      if (this.IsWorking)
      {
        color = Color.Green;
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(((MyCubeBlock) this).BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
          color = result.EmissiveColor;
      }
      else if (this.IsFunctional)
      {
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(((MyCubeBlock) this).BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
          color = result.EmissiveColor;
        if (!this.IsEnoughPower())
          emissivity = 0.0f;
      }
      else
      {
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(((MyCubeBlock) this).BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
          color = result.EmissiveColor;
        emissivity = 0.0f;
      }
      this.SetEmissive(color, emissivity, force);
    }

    public override void ContactCallbackInternal() => base.ContactCallbackInternal();

    public override bool EnableContactCallbacks() => false;

    public override bool IsClosing() => !(bool) this.m_open && (double) this.OpenRatio > 0.0;

    private class Sandbox_Game_Entities_MyAirtightSlideDoor\u003C\u003EActor : IActivator, IActivator<MyAirtightSlideDoor>
    {
      object IActivator.CreateInstance() => (object) new MyAirtightSlideDoor();

      MyAirtightSlideDoor IActivator<MyAirtightSlideDoor>.CreateInstance() => new MyAirtightSlideDoor();
    }
  }
}
