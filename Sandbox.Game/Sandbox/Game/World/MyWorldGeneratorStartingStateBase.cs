// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyWorldGeneratorStartingStateBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World
{
  [GenerateActivator]
  public abstract class MyWorldGeneratorStartingStateBase
  {
    public string FactionTag;

    public abstract Vector3D? GetStartingLocation();

    public abstract void SetupCharacter(MyWorldGenerator.Args generatorArgs);

    public virtual void Init(
      MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
    {
      this.FactionTag = builder.FactionTag;
    }

    public virtual MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
    {
      MyObjectBuilder_WorldGeneratorPlayerStartingState objectBuilder = MyWorldGenerator.StartingStateFactory.CreateObjectBuilder(this);
      objectBuilder.FactionTag = this.FactionTag;
      return objectBuilder;
    }

    protected Vector3D FixPositionToVoxel(Vector3D position)
    {
      map = (MyVoxelMap) null;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyVoxelMap map)
          break;
      }
      float maxVertDistance = 2048f;
      if (map != null)
        position = map.GetPositionOnVoxel(position, maxVertDistance);
      return position;
    }

    protected virtual void CreateAndSetPlayerFaction()
    {
      if (!Sync.IsServer || this.FactionTag == null || MySession.Static.LocalHumanPlayer == null)
        return;
      MySession.Static.Factions.TryGetOrCreateFactionByTag(this.FactionTag).AcceptJoin(MySession.Static.LocalHumanPlayer.Identity.IdentityId);
    }
  }
}
