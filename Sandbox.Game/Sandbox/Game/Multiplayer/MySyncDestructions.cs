// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MySyncDestructions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  [PreloadRequired]
  public static class MySyncDestructions
  {
    public static void AddDestructionEffect(
      string effectName,
      Vector3D position,
      Vector3 direction,
      float scale)
    {
      MyMultiplayer.RaiseStaticEvent<string, Vector3D, Vector3, float>((Func<IMyEventOwner, Action<string, Vector3D, Vector3, float>>) (s => new Action<string, Vector3D, Vector3, float>(MySyncDestructions.OnAddDestructionEffectMessage)), effectName, position, direction, scale);
    }

    [Event(null, 43)]
    [Server]
    [Broadcast]
    private static void OnAddDestructionEffectMessage(
      string effectName,
      Vector3D position,
      Vector3 direction,
      float scale)
    {
      MyGridPhysics.CreateDestructionEffect(effectName, position, direction, scale);
    }

    public static void CreateFracturePiece(MyObjectBuilder_FracturedPiece fracturePiece) => MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_FracturedPiece>((Func<IMyEventOwner, Action<MyObjectBuilder_FracturedPiece>>) (s => new Action<MyObjectBuilder_FracturedPiece>(MySyncDestructions.OnCreateFracturePieceMessage)), fracturePiece);

    [Event(null, 55)]
    [Reliable]
    [Broadcast]
    private static void OnCreateFracturePieceMessage([Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FracturedPiece fracturePiece)
    {
      MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(fracturePiece.EntityId, true);
      try
      {
        pieceFromPool.Init((MyObjectBuilder_EntityBase) fracturePiece);
        Sandbox.Game.Entities.MyEntities.Add((MyEntity) pieceFromPool);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Cannot add fracture piece: " + ex.Message);
        if (pieceFromPool == null)
          return;
        MyFracturedPiecesManager.Static.RemoveFracturePiece(pieceFromPool, 0.0f, true, false);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (MyObjectBuilder_FracturedPiece.Shape shape in fracturePiece.Shapes)
          stringBuilder.Append(shape.Name).Append(" ");
        MyLog.Default.WriteLine("Received fracture piece not added, no shape found. Shapes: " + stringBuilder.ToString());
      }
    }

    public static void RemoveFracturePiece(long entityId, float blendTime) => MyMultiplayer.RaiseStaticEvent<long, float>((Func<IMyEventOwner, Action<long, float>>) (s => new Action<long, float>(MySyncDestructions.OnRemoveFracturePieceMessage)), entityId, blendTime);

    [Event(null, 90)]
    [Reliable]
    [Broadcast]
    private static void OnRemoveFracturePieceMessage(long entityId, float blendTime)
    {
      MyFracturedPiece entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyFracturedPiece>(entityId, out entity))
        return;
      MyFracturedPiecesManager.Static.RemoveFracturePiece(entity, blendTime, true, false);
    }

    [Conditional("DEBUG")]
    public static void FPManagerDbgMessage(long createdId, long removedId) => MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MySyncDestructions.OnFPManagerDbgMessage)), createdId, removedId);

    [Event(null, 112)]
    [Reliable]
    [Server]
    private static void OnFPManagerDbgMessage(long createdId, long removedId) => MyFracturedPiecesManager.Static.DbgCheck(createdId, removedId);

    public static void CreateFracturedBlock(
      MyObjectBuilder_FracturedBlock fracturedBlock,
      long gridId,
      Vector3I position)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3I, MyObjectBuilder_FracturedBlock>((Func<IMyEventOwner, Action<long, Vector3I, MyObjectBuilder_FracturedBlock>>) (s => new Action<long, Vector3I, MyObjectBuilder_FracturedBlock>(MySyncDestructions.OnCreateFracturedBlockMessage)), gridId, position, fracturedBlock);
    }

    [Event(null, 125)]
    [Reliable]
    [Broadcast]
    private static void OnCreateFracturedBlockMessage(
      long gridId,
      Vector3I position,
      [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FracturedBlock fracturedBlock)
    {
      MyCubeGrid entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(gridId, out entity))
        return;
      entity.CreateFracturedBlock(fracturedBlock, position);
    }

    public static void CreateFractureComponent(
      long gridId,
      Vector3I position,
      ushort compoundBlockId,
      MyObjectBuilder_FractureComponentBase component)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3I, ushort, MyObjectBuilder_FractureComponentBase>((Func<IMyEventOwner, Action<long, Vector3I, ushort, MyObjectBuilder_FractureComponentBase>>) (s => new Action<long, Vector3I, ushort, MyObjectBuilder_FractureComponentBase>(MySyncDestructions.OnCreateFractureComponentMessage)), gridId, position, compoundBlockId, component);
    }

    [Event(null, 145)]
    [Reliable]
    [Broadcast]
    private static void OnCreateFractureComponentMessage(
      long gridId,
      Vector3I position,
      ushort compoundBlockId,
      [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FractureComponentBase component)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(gridId, out entity))
        return;
      MySlimBlock cubeBlock = (entity as MyCubeGrid).GetCubeBlock(position);
      if (cubeBlock == null || cubeBlock.FatBlock == null)
        return;
      if (cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        MySlimBlock block = fatBlock.GetBlock(compoundBlockId);
        if (block == null)
          return;
        MySyncDestructions.AddFractureComponent(component, (MyEntity) block.FatBlock);
      }
      else
        MySyncDestructions.AddFractureComponent(component, (MyEntity) cubeBlock.FatBlock);
    }

    private static void AddFractureComponent(
      MyObjectBuilder_FractureComponentBase obFractureComponent,
      MyEntity entity)
    {
      if (!(MyComponentFactory.CreateInstanceByTypeId(obFractureComponent.TypeId) is MyFractureComponentBase instanceByTypeId))
        return;
      try
      {
        if (entity.Components.Has<MyFractureComponentBase>())
          return;
        entity.Components.Add<MyFractureComponentBase>(instanceByTypeId);
        instanceByTypeId.Deserialize((MyObjectBuilder_ComponentBase) obFractureComponent);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Cannot add received fracture component: " + ex.Message);
        if (entity.Components.Has<MyFractureComponentBase>())
        {
          if (entity is MyCubeBlock myCubeBlock && myCubeBlock.SlimBlock != null)
            myCubeBlock.SlimBlock.RemoveFractureComponent();
          else
            entity.Components.Remove<MyFractureComponentBase>();
        }
        StringBuilder stringBuilder = new StringBuilder();
        foreach (MyObjectBuilder_FractureComponentBase.FracturedShape shape in obFractureComponent.Shapes)
          stringBuilder.Append(shape.Name).Append(" ");
        MyLog.Default.WriteLine("Received fracture component not added, no shape found. Shapes: " + stringBuilder.ToString());
      }
    }

    public static void RemoveShapeFromFractureComponent(
      long gridId,
      Vector3I position,
      ushort compoundBlockId,
      string shapeName)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3I, ushort, string[]>((Func<IMyEventOwner, Action<long, Vector3I, ushort, string[]>>) (s => new Action<long, Vector3I, ushort, string[]>(MySyncDestructions.OnRemoveShapeFromFractureComponentMessage)), gridId, position, compoundBlockId, new string[1]
      {
        shapeName
      });
    }

    public static void RemoveShapesFromFractureComponent(
      long gridId,
      Vector3I position,
      ushort compoundBlockId,
      List<string> shapeNames)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3I, ushort, string[]>((Func<IMyEventOwner, Action<long, Vector3I, ushort, string[]>>) (s => new Action<long, Vector3I, ushort, string[]>(MySyncDestructions.OnRemoveShapeFromFractureComponentMessage)), gridId, position, compoundBlockId, shapeNames.ToArray());
    }

    [Event(null, 234)]
    [Reliable]
    [Broadcast]
    private static void OnRemoveShapeFromFractureComponentMessage(
      long gridId,
      Vector3I position,
      ushort compoundBlockId,
      string[] shapeNames)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(gridId, out entity))
        return;
      MySlimBlock cubeBlock = (entity as MyCubeGrid).GetCubeBlock(position);
      if (cubeBlock == null || cubeBlock.FatBlock == null)
        return;
      if (cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        MySlimBlock block = fatBlock.GetBlock(compoundBlockId);
        if (block == null)
          return;
        MySyncDestructions.RemoveFractureComponentChildShapes(block, shapeNames);
      }
      else
        MySyncDestructions.RemoveFractureComponentChildShapes(cubeBlock, shapeNames);
    }

    private static void RemoveFractureComponentChildShapes(MySlimBlock block, string[] shapeNames)
    {
      MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
      if (fractureComponent != null)
        fractureComponent.RemoveChildShapes((IEnumerable<string>) shapeNames);
      else
        MyLog.Default.WriteLine("Cannot remove child shapes from fracture component, fracture component not found in block, BlockDefinition: " + block.BlockDefinition.Id.ToString() + ", Shapes: " + string.Join(", ", shapeNames));
    }

    public static void RemoveFracturedPiecesRequest(ulong userId, Vector3D center, float radius)
    {
      if (Sync.IsServer)
        MyFracturedPiecesManager.Static.RemoveFracturesInSphere(center, radius);
      else
        MyMultiplayer.RaiseStaticEvent<ulong, Vector3D, float>((Func<IMyEventOwner, Action<ulong, Vector3D, float>>) (s => new Action<ulong, Vector3D, float>(MySyncDestructions.OnRemoveFracturedPiecesMessage)), userId, center, radius);
    }

    [Event(null, 294)]
    [Reliable]
    [Server]
    private static void OnRemoveFracturedPiecesMessage(ulong userId, Vector3D center, float radius)
    {
      if (!MySession.Static.IsUserAdmin(userId))
        return;
      MyFracturedPiecesManager.Static.RemoveFracturesInSphere(center, radius);
    }

    protected sealed class OnAddDestructionEffectMessage\u003C\u003ESystem_String\u0023VRageMath_Vector3D\u0023VRageMath_Vector3\u0023System_Single : ICallSite<IMyEventOwner, string, Vector3D, Vector3, float, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string effectName,
        in Vector3D position,
        in Vector3 direction,
        in float scale,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnAddDestructionEffectMessage(effectName, position, direction, scale);
      }
    }

    protected sealed class OnCreateFracturePieceMessage\u003C\u003EVRage_Game_MyObjectBuilder_FracturedPiece : ICallSite<IMyEventOwner, MyObjectBuilder_FracturedPiece, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_FracturedPiece fracturePiece,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnCreateFracturePieceMessage(fracturePiece);
      }
    }

    protected sealed class OnRemoveFracturePieceMessage\u003C\u003ESystem_Int64\u0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in float blendTime,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnRemoveFracturePieceMessage(entityId, blendTime);
      }
    }

    protected sealed class OnFPManagerDbgMessage\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long createdId,
        in long removedId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnFPManagerDbgMessage(createdId, removedId);
      }
    }

    protected sealed class OnCreateFracturedBlockMessage\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023VRage_Game_MyObjectBuilder_FracturedBlock : ICallSite<IMyEventOwner, long, Vector3I, MyObjectBuilder_FracturedBlock, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in Vector3I position,
        in MyObjectBuilder_FracturedBlock fracturedBlock,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnCreateFracturedBlockMessage(gridId, position, fracturedBlock);
      }
    }

    protected sealed class OnCreateFractureComponentMessage\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023System_UInt16\u0023VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase : ICallSite<IMyEventOwner, long, Vector3I, ushort, MyObjectBuilder_FractureComponentBase, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in Vector3I position,
        in ushort compoundBlockId,
        in MyObjectBuilder_FractureComponentBase component,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnCreateFractureComponentMessage(gridId, position, compoundBlockId, component);
      }
    }

    protected sealed class OnRemoveShapeFromFractureComponentMessage\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023System_UInt16\u0023System_String\u003C\u0023\u003E : ICallSite<IMyEventOwner, long, Vector3I, ushort, string[], DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in Vector3I position,
        in ushort compoundBlockId,
        in string[] shapeNames,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnRemoveShapeFromFractureComponentMessage(gridId, position, compoundBlockId, shapeNames);
      }
    }

    protected sealed class OnRemoveFracturedPiecesMessage\u003C\u003ESystem_UInt64\u0023VRageMath_Vector3D\u0023System_Single : ICallSite<IMyEventOwner, ulong, Vector3D, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong userId,
        in Vector3D center,
        in float radius,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDestructions.OnRemoveFracturedPiecesMessage(userId, center, radius);
      }
    }
  }
}
