// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyCubeBlockDecayScript
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Systems;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  [MyScriptedSystem("DecayBlocks")]
  public class MyCubeBlockDecayScript : MyGroupScriptBase
  {
    private HashSet<MyStringHash> m_tmpSubtypes;

    public MyCubeBlockDecayScript() => this.m_tmpSubtypes = new HashSet<MyStringHash>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);

    public override void ProcessObjects(ListReader<MyDefinitionId> objects)
    {
      MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
      this.m_tmpSubtypes.Clear();
      foreach (MyDefinitionId myDefinitionId in objects)
        this.m_tmpSubtypes.Add(myDefinitionId.SubtypeId);
      foreach (MyEntity myEntity in entities)
      {
        if (myEntity is MyFloatingObject myFloatingObject && this.m_tmpSubtypes.Contains(myFloatingObject.Item.Content.GetObjectId().SubtypeId))
          MyFloatingObjects.RemoveFloatingObject(myFloatingObject, true);
      }
    }
  }
}
