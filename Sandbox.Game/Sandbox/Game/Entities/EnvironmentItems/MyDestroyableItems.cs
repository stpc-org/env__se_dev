// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.EnvironmentItems.MyDestroyableItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.EnvironmentItems
{
  [MyEntityType(typeof (MyObjectBuilder_Bushes), false)]
  [MyEntityType(typeof (MyObjectBuilder_DestroyableItems), true)]
  public class MyDestroyableItems : MyEnvironmentItems
  {
    public override void DoDamage(
      float damage,
      int instanceId,
      Vector3D position,
      Vector3 normal,
      MyStringHash type)
    {
      if (!Sync.IsServer)
        return;
      this.RemoveItem(instanceId, true);
    }

    protected override MyEntity DestroyItem(int itemInstanceId)
    {
      this.RemoveItem(itemInstanceId, true);
      return (MyEntity) null;
    }

    private class Sandbox_Game_Entities_EnvironmentItems_MyDestroyableItems\u003C\u003EActor : IActivator, IActivator<MyDestroyableItems>
    {
      object IActivator.CreateInstance() => (object) new MyDestroyableItems();

      MyDestroyableItems IActivator<MyDestroyableItems>.CreateInstance() => new MyDestroyableItems();
    }
  }
}
