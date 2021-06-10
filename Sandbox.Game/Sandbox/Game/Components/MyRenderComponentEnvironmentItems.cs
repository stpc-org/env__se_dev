// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentEnvironmentItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.EnvironmentItems;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentEnvironmentItems : MyRenderComponent
  {
    internal readonly MyEnvironmentItems EnvironmentItems;

    internal MyRenderComponentEnvironmentItems(MyEnvironmentItems environmentItems) => this.EnvironmentItems = environmentItems;

    public override void AddRenderObjects()
    {
    }

    public override void RemoveRenderObjects()
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.EnvironmentItems.Sectors)
        sector.Value.UnloadRenderObjects();
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.EnvironmentItems.Sectors)
        sector.Value.ClearInstanceData();
    }

    private class Sandbox_Game_Components_MyRenderComponentEnvironmentItems\u003C\u003EActor
    {
    }
  }
}
