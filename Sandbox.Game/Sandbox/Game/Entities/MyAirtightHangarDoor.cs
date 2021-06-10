// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAirtightHangarDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_AirtightHangarDoor))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyAirtightHangarDoor), typeof (Sandbox.ModAPI.Ingame.IMyAirtightHangarDoor)})]
  public class MyAirtightHangarDoor : MyAirtightDoorGeneric, Sandbox.ModAPI.IMyAirtightHangarDoor, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase, Sandbox.ModAPI.Ingame.IMyAirtightHangarDoor
  {
    protected override void UpdateDoorPosition()
    {
      if (this.CubeGrid.Physics == null)
        return;
      bool flag = !Sync.IsServer;
      if (this.m_subpartConstraints.Count == 0 && !flag)
        return;
      float num1 = (this.m_currOpening - 1f) * (float) this.m_subparts.Count * this.m_subpartMovementDistance;
      float num2 = 0.0f;
      int index = 0;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        num2 -= this.m_subpartMovementDistance;
        if (subpart != null && subpart.Physics != null)
        {
          float y = (double) num1 < (double) num2 ? num2 : num1;
          Vector3 position = new Vector3(0.0f, y, 0.0f);
          Matrix result;
          Matrix.CreateTranslation(ref position, out result);
          Matrix renderLocal = result * this.PositionComp.LocalMatrixRef;
          subpart.PositionComp.SetLocalMatrix(ref result, flag ? (object) (MyPhysicsComponentBase) null : (object) subpart.Physics, true, ref renderLocal, true);
          if (this.m_subpartConstraintsData.Count > 0)
          {
            if (this.CubeGrid.Physics != null)
              this.CubeGrid.Physics.RigidBody.Activate();
            subpart.Physics.RigidBody.Activate();
            position = new Vector3(0.0f, -y, 0.0f);
            Matrix.CreateTranslation(ref position, out result);
            this.m_subpartConstraintsData[index].SetInBodySpace(this.PositionComp.LocalMatrixRef, result, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) subpart.Physics);
          }
          ++index;
        }
      }
    }

    protected override void FillSubparts()
    {
      this.m_subparts.Clear();
      MyEntitySubpart myEntitySubpart;
      for (int index = 1; this.Subparts.TryGetValue("HangarDoor_door" + (object) index, out myEntitySubpart); ++index)
        this.m_subparts.Add(myEntitySubpart);
    }

    public override void ContactCallbackInternal() => base.ContactCallbackInternal();

    public override bool EnableContactCallbacks() => false;

    public override bool IsClosing() => !(bool) this.m_open && (double) this.OpenRatio > 0.0;

    private class Sandbox_Game_Entities_MyAirtightHangarDoor\u003C\u003EActor : IActivator, IActivator<MyAirtightHangarDoor>
    {
      object IActivator.CreateInstance() => (object) new MyAirtightHangarDoor();

      MyAirtightHangarDoor IActivator<MyAirtightHangarDoor>.CreateInstance() => new MyAirtightHangarDoor();
    }
  }
}
