// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.EnvironmentItems.MyEnvironmentItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.EnvironmentItems
{
  [MyEntityType(typeof (MyObjectBuilder_EnvironmentItems), true)]
  public class MyEnvironmentItems : MyEntity, IMyEventProxy, IMyEventOwner
  {
    private readonly MyInstanceFlagsEnum m_instanceFlags;
    protected readonly Dictionary<int, MyEnvironmentItems.MyEnvironmentItemData> m_itemsData = new Dictionary<int, MyEnvironmentItems.MyEnvironmentItemData>();
    protected readonly Dictionary<int, int> m_physicsShapeInstanceIdToLocalId = new Dictionary<int, int>();
    protected readonly Dictionary<int, int> m_localIdToPhysicsShapeInstanceId = new Dictionary<int, int>();
    protected static readonly Dictionary<MyStringHash, int> m_subtypeToModels = new Dictionary<MyStringHash, int>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    protected readonly Dictionary<Vector3I, MyEnvironmentSector> m_sectors = new Dictionary<Vector3I, MyEnvironmentSector>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    protected List<HkdShapeInstanceInfo> m_childrenTmp = new List<HkdShapeInstanceInfo>();
    private HashSet<Vector3I> m_updatedSectorsTmp = new HashSet<Vector3I>();
    private List<HkdBreakableBodyInfo> m_tmpBodyInfos = new List<HkdBreakableBodyInfo>();
    protected static List<HkBodyCollision> m_tmpResults = new List<HkBodyCollision>();
    protected static List<MyEnvironmentSector> m_tmpSectors = new List<MyEnvironmentSector>();
    private List<int> m_tmpToDisable = new List<int>();
    private MyEnvironmentItemsDefinition m_definition;
    private List<MyEnvironmentItems.AddItemData> m_batchedAddItems = new List<MyEnvironmentItems.AddItemData>();
    private List<MyEnvironmentItems.ModifyItemData> m_batchedModifyItems = new List<MyEnvironmentItems.ModifyItemData>();
    private List<MyEnvironmentItems.RemoveItemData> m_batchedRemoveItems = new List<MyEnvironmentItems.RemoveItemData>();
    private float m_batchTime;
    private const float BATCH_DEFAULT_TIME = 10f;
    public Vector3 BaseColor;
    public Vector2 ColorSpread;
    private Vector3D m_cellsOffset;

    public Dictionary<Vector3I, MyEnvironmentSector> Sectors => this.m_sectors;

    public MyEnvironmentItemsDefinition Definition => this.m_definition;

    public event Action<MyEnvironmentItems, MyEnvironmentItems.ItemInfo> ItemAdded;

    public event Action<MyEnvironmentItems, MyEnvironmentItems.ItemInfo> ItemRemoved;

    public event Action<MyEnvironmentItems, MyEnvironmentItems.ItemInfo> ItemModified;

    public bool IsBatching => (double) this.m_batchTime > 0.0;

    public float BatchTime => this.m_batchTime;

    public event Action<MyEnvironmentItems> BatchEnded;

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    static MyEnvironmentItems()
    {
      foreach (MyEnvironmentItemDefinition environmentItemDefinition in MyDefinitionManager.Static.GetEnvironmentItemDefinitions())
        MyEnvironmentItems.CheckModelConsistency(environmentItemDefinition);
    }

    public MyEnvironmentItems()
    {
      this.m_instanceFlags = MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask;
      this.m_definition = (MyEnvironmentItemsDefinition) null;
      this.Render = (MyRenderComponentBase) new MyRenderComponentEnvironmentItems(this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyEnvironmentItems.MyEnviromentItemsDebugDraw(this));
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      this.Init((StringBuilder) null, (string) null, (MyEntity) null, new float?());
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      Dictionary<MyStringHash, HkShape> subtypeIdToShape = new Dictionary<MyStringHash, HkShape>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      HkStaticCompoundShape sectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
      MyObjectBuilder_EnvironmentItems environmentItems = (MyObjectBuilder_EnvironmentItems) objectBuilder;
      MyDefinitionId defId = new MyDefinitionId(environmentItems.TypeId, environmentItems.SubtypeId);
      this.CellsOffset = environmentItems.CellsOffset;
      if (environmentItems.SubtypeId == MyStringHash.NullOrEmpty)
      {
        switch (objectBuilder)
        {
          case MyObjectBuilder_Bushes _:
            defId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_DestroyableItems), "Bushes");
            break;
          case MyObjectBuilder_TreesMedium _:
            defId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Trees), "TreesMedium");
            break;
          case MyObjectBuilder_Trees _:
            defId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Trees), "Trees");
            break;
        }
      }
      if (!MyDefinitionManager.Static.TryGetDefinition<MyEnvironmentItemsDefinition>(defId, out this.m_definition))
        return;
      if (environmentItems.Items != null)
      {
        foreach (MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData environmentItemData in environmentItems.Items)
        {
          MyStringHash orCompute = MyStringHash.GetOrCompute(environmentItemData.SubtypeName);
          if (this.m_definition.ContainsItemDefinition(orCompute))
          {
            MatrixD matrix = environmentItemData.PositionAndOrientation.GetMatrix();
            this.AddItem(this.m_definition.GetItemDefinition(orCompute), ref matrix, ref invalid);
          }
        }
      }
      this.PrepareItemsPhysics(sectorRootShape, ref invalid, subtypeIdToShape);
      this.PrepareItemsGraphics();
      foreach (KeyValuePair<MyStringHash, HkShape> keyValuePair in subtypeIdToShape)
        keyValuePair.Value.RemoveReference();
      sectorRootShape.Base.RemoveReference();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EnvironmentItems objectBuilder = (MyObjectBuilder_EnvironmentItems) base.GetObjectBuilder(copy);
      objectBuilder.SubtypeName = this.Definition.Id.SubtypeName;
      if (this.IsBatching)
        this.EndBatch(true);
      int length = 0;
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.Enabled)
          ++length;
      }
      objectBuilder.Items = new MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[length];
      int index = 0;
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.Enabled)
        {
          objectBuilder.Items[index].SubtypeName = keyValuePair.Value.SubtypeId.ToString();
          objectBuilder.Items[index].PositionAndOrientation = new MyPositionAndOrientation(keyValuePair.Value.Transform.TransformMatrix);
          ++index;
        }
      }
      objectBuilder.CellsOffset = this.CellsOffset;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public static MyEnvironmentItems.MyEnvironmentItemsSpawnData BeginSpawn(
      MyEnvironmentItemsDefinition itemsDefinition,
      bool addToScene = true,
      long withEntityId = 0)
    {
      MyObjectBuilder_EnvironmentItems newObject = MyObjectBuilderSerializer.CreateNewObject(itemsDefinition.Id.TypeId, itemsDefinition.Id.SubtypeName) as MyObjectBuilder_EnvironmentItems;
      newObject.EntityId = withEntityId;
      newObject.PersistentFlags |= (MyPersistentEntityFlags2) (2 | (addToScene ? 16 : 0) | 4);
      MyEnvironmentItems environmentItems = !addToScene ? MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) newObject, true) as MyEnvironmentItems : MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) newObject, true) as MyEnvironmentItems;
      return new MyEnvironmentItems.MyEnvironmentItemsSpawnData()
      {
        EnvironmentItems = environmentItems
      };
    }

    public static bool SpawnItem(
      MyEnvironmentItems.MyEnvironmentItemsSpawnData spawnData,
      MyEnvironmentItemDefinition itemDefinition,
      Vector3D position,
      Vector3D up,
      int userdata = -1,
      bool silentOverlaps = true)
    {
      if (!MyFakes.ENABLE_ENVIRONMENT_ITEMS)
        return true;
      if (spawnData == null || spawnData.EnvironmentItems == null || itemDefinition == null)
        return false;
      Vector3D perpendicularVector = MyUtils.GetRandomPerpendicularVector(ref up);
      MatrixD world = MatrixD.CreateWorld(position, perpendicularVector, up);
      return spawnData.EnvironmentItems.AddItem(itemDefinition, ref world, ref spawnData.AabbWorld, userdata, silentOverlaps);
    }

    public static void EndSpawn(
      MyEnvironmentItems.MyEnvironmentItemsSpawnData spawnData,
      bool updateGraphics = true,
      bool updatePhysics = true)
    {
      if (updatePhysics)
      {
        spawnData.EnvironmentItems.PrepareItemsPhysics(spawnData);
        spawnData.SubtypeToShapes.Clear();
        foreach (KeyValuePair<MyStringHash, HkShape> subtypeToShape in spawnData.SubtypeToShapes)
          subtypeToShape.Value.RemoveReference();
        spawnData.SubtypeToShapes.Clear();
      }
      if (updateGraphics)
        spawnData.EnvironmentItems.PrepareItemsGraphics();
      spawnData.EnvironmentItems.UpdateGamePruningStructure();
    }

    public void UnloadGraphics()
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_sectors)
        sector.Value.UnloadRenderObjects();
    }

    public void ClosePhysics(
      MyEnvironmentItems.MyEnvironmentItemsSpawnData data)
    {
      if (this.Physics == null)
        return;
      this.Physics.Close();
      this.Physics = (MyPhysicsBody) null;
    }

    public static string GetModelName(MyStringHash itemSubtype) => MyModel.GetById(MyEnvironmentItems.GetModelId(itemSubtype));

    public static int GetModelId(MyStringHash subtypeId) => MyEnvironmentItems.m_subtypeToModels[subtypeId];

    private bool AddItem(
      MyEnvironmentItemDefinition itemDefinition,
      ref MatrixD worldMatrix,
      ref BoundingBoxD aabbWorld,
      int userData = -1,
      bool silentOverlaps = false)
    {
      if (!MyFakes.ENABLE_ENVIRONMENT_ITEMS)
        return true;
      if (!this.m_definition.ContainsItemDefinition(itemDefinition) || itemDefinition.Model == null)
        return false;
      int modelId = MyEnvironmentItems.GetModelId(itemDefinition.Id.SubtypeId);
      MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(modelId));
      if (modelOnlyData == null)
        return false;
      MyEnvironmentItems.CheckModelConsistency(itemDefinition);
      int hashCode = worldMatrix.Translation.GetHashCode();
      if (this.m_itemsData.ContainsKey(hashCode))
      {
        if (!silentOverlaps)
          MyLog.Default.WriteLine("WARNING: items are on the same place.");
        return false;
      }
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData = new MyEnvironmentItems.MyEnvironmentItemData()
      {
        Id = hashCode,
        SubtypeId = itemDefinition.Id.SubtypeId,
        Transform = new MyTransformD(ref worldMatrix),
        Enabled = true,
        SectorInstanceId = -1,
        Model = modelOnlyData,
        UserData = userData
      };
      aabbWorld.Include(modelOnlyData.BoundingBox.Transform(worldMatrix));
      MatrixD transformMatrix = environmentItemData.Transform.TransformMatrix;
      float sectorSize = MyFakes.ENVIRONMENT_ITEMS_ONE_INSTANCEBUFFER ? 20000f : this.m_definition.SectorSize;
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(transformMatrix.Translation - this.CellsOffset, sectorSize);
      MyEnvironmentSector environmentSector;
      if (!this.m_sectors.TryGetValue(sectorId, out environmentSector))
      {
        environmentSector = new MyEnvironmentSector(sectorId, sectorId * sectorSize + this.CellsOffset);
        this.m_sectors.Add(sectorId, environmentSector);
      }
      MatrixD translation = MatrixD.CreateTranslation(-sectorId * sectorSize - this.CellsOffset);
      MatrixD matrixD = environmentItemData.Transform.TransformMatrix * translation;
      Matrix localMatrix = (Matrix) ref matrixD;
      Color color = (Color) this.BaseColor;
      if ((double) this.ColorSpread.LengthSquared() > 0.0)
      {
        float randomFloat1 = MyUtils.GetRandomFloat(0.0f, this.ColorSpread.X);
        float randomFloat2 = MyUtils.GetRandomFloat(0.0f, this.ColorSpread.Y);
        color = (double) MyUtils.GetRandomSign() > 0.0 ? Color.Lighten(color, (double) randomFloat1) : Color.Darken(color, (double) randomFloat2);
      }
      Vector3 hsvdX11 = color.ColorToHSVDX11();
      environmentItemData.SectorInstanceId = environmentSector.AddInstance(itemDefinition.Id.SubtypeId, modelId, hashCode, ref localMatrix, modelOnlyData.BoundingBox, this.m_instanceFlags, this.m_definition.MaxViewDistance, hsvdX11);
      environmentItemData.Transform = new MyTransformD(transformMatrix);
      this.m_itemsData.Add(hashCode, environmentItemData);
      if (this.ItemAdded != null)
        this.ItemAdded(this, new MyEnvironmentItems.ItemInfo()
        {
          LocalId = hashCode,
          SubtypeId = environmentItemData.SubtypeId,
          Transform = environmentItemData.Transform
        });
      return true;
    }

    private static void CheckModelConsistency(MyEnvironmentItemDefinition itemDefinition)
    {
      if (MyEnvironmentItems.m_subtypeToModels.TryGetValue(itemDefinition.Id.SubtypeId, out int _) || itemDefinition.Model == null)
        return;
      MyEnvironmentItems.m_subtypeToModels.Add(itemDefinition.Id.SubtypeId, MyModel.GetId(itemDefinition.Model));
    }

    public void PrepareItemsGraphics()
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_sectors)
      {
        sector.Value.UpdateRenderInstanceData();
        sector.Value.UpdateRenderEntitiesData(this.WorldMatrix);
      }
    }

    public void PrepareItemsPhysics(
      MyEnvironmentItems.MyEnvironmentItemsSpawnData spawnData)
    {
      spawnData.SectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
      spawnData.EnvironmentItems.PrepareItemsPhysics(spawnData.SectorRootShape, ref spawnData.AabbWorld, spawnData.SubtypeToShapes);
    }

    private void PrepareItemsPhysics(
      HkStaticCompoundShape sectorRootShape,
      ref BoundingBoxD aabbWorld,
      Dictionary<MyStringHash, HkShape> subtypeIdToShape)
    {
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.Enabled)
        {
          MatrixD transformMatrix = keyValuePair.Value.Transform.TransformMatrix;
          int physicsShapeInstanceId;
          if (this.AddPhysicsShape(keyValuePair.Value.SubtypeId, keyValuePair.Value.Model, ref transformMatrix, sectorRootShape, subtypeIdToShape, out physicsShapeInstanceId))
          {
            this.m_physicsShapeInstanceIdToLocalId[physicsShapeInstanceId] = keyValuePair.Value.Id;
            this.m_localIdToPhysicsShapeInstanceId[keyValuePair.Value.Id] = physicsShapeInstanceId;
          }
        }
      }
      this.PositionComp.WorldAABB = aabbWorld;
      if (sectorRootShape.InstanceCount <= 0)
        return;
      MyPhysicsBody myPhysicsBody = new MyPhysicsBody((IMyEntity) this, RigidBodyFlag.RBF_STATIC);
      myPhysicsBody.MaterialType = this.m_definition.Material;
      myPhysicsBody.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
      myPhysicsBody.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
      myPhysicsBody.IsStaticForCluster = true;
      this.Physics = myPhysicsBody;
      sectorRootShape.Bake();
      HkMassProperties hkMassProperties = new HkMassProperties();
      MatrixD translation = MatrixD.CreateTranslation(this.CellsOffset);
      this.Physics.CreateFromCollisionObject((HkShape) sectorRootShape, Vector3.Zero, translation, new HkMassProperties?(hkMassProperties));
      if (Sync.IsServer)
      {
        this.Physics.ContactPointCallback += new MyPhysicsBody.PhysicsContactHandler(this.Physics_ContactPointCallback);
        this.Physics.RigidBody.ContactPointCallbackEnabled = true;
        this.Physics.RigidBody.IsEnvironment = true;
      }
      this.Physics.Enabled = true;
    }

    public bool IsValidPosition(Vector3D position) => !this.m_itemsData.ContainsKey(position.GetHashCode());

    public void BeginBatch(bool sync)
    {
      this.m_batchTime = 10f;
      if (!sync)
        return;
      MySyncEnvironmentItems.SendBeginBatchAddMessage(this.EntityId);
    }

    public void BatchAddItem(Vector3D position, MyStringHash subtypeId, bool sync)
    {
      if (!this.m_definition.ContainsItemDefinition(subtypeId))
        return;
      this.m_batchedAddItems.Add(new MyEnvironmentItems.AddItemData()
      {
        Position = position,
        SubtypeId = subtypeId
      });
      if (!sync)
        return;
      MySyncEnvironmentItems.SendBatchAddItemMessage(this.EntityId, position, subtypeId);
    }

    public void BatchModifyItem(int localId, MyStringHash subtypeId, bool sync)
    {
      if (!this.m_itemsData.ContainsKey(localId))
        return;
      this.m_batchedModifyItems.Add(new MyEnvironmentItems.ModifyItemData()
      {
        LocalId = localId,
        SubtypeId = subtypeId
      });
      if (!sync)
        return;
      MySyncEnvironmentItems.SendBatchModifyItemMessage(this.EntityId, localId, subtypeId);
    }

    public void BatchRemoveItem(int localId, bool sync)
    {
      if (!this.m_itemsData.ContainsKey(localId))
        return;
      this.m_batchedRemoveItems.Add(new MyEnvironmentItems.RemoveItemData()
      {
        LocalId = localId
      });
      if (!sync)
        return;
      MySyncEnvironmentItems.SendBatchRemoveItemMessage(this.EntityId, localId);
    }

    public void EndBatch(bool sync)
    {
      this.m_batchTime = 0.0f;
      if (this.m_batchedAddItems.Count > 0 || this.m_batchedModifyItems.Count > 0 || this.m_batchedRemoveItems.Count > 0)
        this.ProcessBatch();
      this.m_batchedAddItems.Clear();
      this.m_batchedModifyItems.Clear();
      this.m_batchedRemoveItems.Clear();
      if (!sync)
        return;
      MySyncEnvironmentItems.SendEndBatchAddMessage(this.EntityId);
    }

    private void ProcessBatch()
    {
      foreach (MyEnvironmentItems.RemoveItemData batchedRemoveItem in this.m_batchedRemoveItems)
        this.RemoveItem(batchedRemoveItem.LocalId, false, false);
      foreach (MyEnvironmentItems.ModifyItemData batchedModifyItem in this.m_batchedModifyItems)
        this.ModifyItemModel(batchedModifyItem.LocalId, batchedModifyItem.SubtypeId, false, false);
      if (this.Physics != null)
      {
        if (Sync.IsServer)
          this.Physics.ContactPointCallback -= new MyPhysicsBody.PhysicsContactHandler(this.Physics_ContactPointCallback);
        this.Physics.Close();
        this.Physics = (MyPhysicsBody) null;
      }
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      Dictionary<MyStringHash, HkShape> subtypeIdToShape = new Dictionary<MyStringHash, HkShape>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      HkStaticCompoundShape sectorRootShape = new HkStaticCompoundShape(HkReferencePolicy.None);
      this.m_physicsShapeInstanceIdToLocalId.Clear();
      this.m_localIdToPhysicsShapeInstanceId.Clear();
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.Enabled)
        {
          MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(MyEnvironmentItems.m_subtypeToModels[keyValuePair.Value.SubtypeId]));
          MatrixD transformMatrix = keyValuePair.Value.Transform.TransformMatrix;
          invalid.Include(modelOnlyData.BoundingBox.Transform(transformMatrix));
        }
      }
      foreach (MyEnvironmentItems.AddItemData batchedAddItem in this.m_batchedAddItems)
      {
        MatrixD world = MatrixD.CreateWorld(batchedAddItem.Position, Vector3D.Forward, Vector3D.Up);
        this.AddItem(this.m_definition.GetItemDefinition(batchedAddItem.SubtypeId), ref world, ref invalid);
      }
      this.PrepareItemsPhysics(sectorRootShape, ref invalid, subtypeIdToShape);
      this.PrepareItemsGraphics();
      foreach (KeyValuePair<MyStringHash, HkShape> keyValuePair in subtypeIdToShape)
        keyValuePair.Value.RemoveReference();
      subtypeIdToShape.Clear();
    }

    public bool ModifyItemModel(
      int itemInstanceId,
      MyStringHash newSubtypeId,
      bool updateSector,
      bool sync)
    {
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData;
      if (!this.m_itemsData.TryGetValue(itemInstanceId, out environmentItemData))
        return false;
      int modelId1 = MyEnvironmentItems.GetModelId(environmentItemData.SubtypeId);
      int modelId2 = MyEnvironmentItems.GetModelId(newSubtypeId);
      if (environmentItemData.Enabled)
      {
        MatrixD transformMatrix = environmentItemData.Transform.TransformMatrix;
        Matrix localMatrix = (Matrix) ref transformMatrix;
        Vector3I sectorId = MyEnvironmentSector.GetSectorId(localMatrix.Translation - this.CellsOffset, this.Definition.SectorSize);
        MyModel modelOnlyData = MyModels.GetModelOnlyData(MyModel.GetById(modelId1));
        MyEnvironmentSector sector = this.Sectors[sectorId];
        MatrixD sectorMatrix = sector.SectorMatrix;
        Matrix matrix = Matrix.Invert((Matrix) ref sectorMatrix);
        localMatrix *= matrix;
        sector.DisableInstance(environmentItemData.SectorInstanceId, modelId1);
        int num = sector.AddInstance(newSubtypeId, modelId2, itemInstanceId, ref localMatrix, modelOnlyData.BoundingBox, this.m_instanceFlags, this.m_definition.MaxViewDistance);
        environmentItemData.SubtypeId = newSubtypeId;
        environmentItemData.SectorInstanceId = num;
        this.m_itemsData[itemInstanceId] = environmentItemData;
        if (updateSector)
        {
          sector.UpdateRenderInstanceData();
          sector.UpdateRenderEntitiesData(this.WorldMatrix);
        }
        if (this.ItemModified != null)
          this.ItemModified(this, new MyEnvironmentItems.ItemInfo()
          {
            LocalId = environmentItemData.Id,
            SubtypeId = environmentItemData.SubtypeId,
            Transform = environmentItemData.Transform
          });
        if (sync)
          MySyncEnvironmentItems.SendModifyModelMessage(this.EntityId, itemInstanceId, newSubtypeId);
      }
      return true;
    }

    public bool TryGetItemInfoById(int itemId, out MyEnvironmentItems.ItemInfo result)
    {
      result = new MyEnvironmentItems.ItemInfo();
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData;
      if (!this.m_itemsData.TryGetValue(itemId, out environmentItemData) || !environmentItemData.Enabled)
        return false;
      result = new MyEnvironmentItems.ItemInfo()
      {
        LocalId = itemId,
        SubtypeId = environmentItemData.SubtypeId,
        Transform = environmentItemData.Transform
      };
      return true;
    }

    public void GetPhysicalItemsInRadius(
      Vector3D position,
      float radius,
      List<MyEnvironmentItems.ItemInfo> result)
    {
      double num = (double) radius * (double) radius;
      if (this.Physics == null || !((HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null))
        return;
      HkStaticCompoundShape shape = (HkStaticCompoundShape) this.Physics.RigidBody.GetShape();
      HkShapeContainerIterator iterator = shape.GetIterator();
      while (iterator.IsValid)
      {
        uint currentShapeKey = iterator.CurrentShapeKey;
        int instanceId;
        shape.DecomposeShapeKey(currentShapeKey, out instanceId, out uint _);
        int key;
        MyEnvironmentItems.MyEnvironmentItemData environmentItemData;
        if (this.m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out key) && this.m_itemsData.TryGetValue(key, out environmentItemData) && (environmentItemData.Enabled && Vector3D.DistanceSquared(environmentItemData.Transform.Position, position) < num))
          result.Add(new MyEnvironmentItems.ItemInfo()
          {
            LocalId = key,
            SubtypeId = environmentItemData.SubtypeId,
            Transform = environmentItemData.Transform
          });
        iterator.Next();
      }
    }

    public void GetAllItemsInRadius(
      Vector3D point,
      float radius,
      List<MyEnvironmentItems.ItemInfo> output)
    {
      this.GetSectorsInRadius(point, radius, MyEnvironmentItems.m_tmpSectors);
      foreach (MyEnvironmentSector tmpSector in MyEnvironmentItems.m_tmpSectors)
        tmpSector.GetItemsInRadius((Vector3) point, radius, output);
      MyEnvironmentItems.m_tmpSectors.Clear();
    }

    public void GetItemsInSector(Vector3I sectorId, List<MyEnvironmentItems.ItemInfo> output)
    {
      if (!this.m_sectors.ContainsKey(sectorId))
        return;
      this.m_sectors[sectorId].GetItems(output);
    }

    public int GetItemsCount(MyStringHash id)
    {
      int num = 0;
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.SubtypeId == id)
          ++num;
      }
      return num;
    }

    public void GetSectorsInRadius(
      Vector3D position,
      float radius,
      List<MyEnvironmentSector> sectors)
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_sectors)
      {
        if (sector.Value.IsValid)
        {
          BoundingBoxD sectorWorldBox = sector.Value.SectorWorldBox;
          sectorWorldBox.Inflate((double) radius);
          if (sectorWorldBox.Contains(position) == ContainmentType.Contains)
            sectors.Add(sector.Value);
        }
      }
    }

    public void GetSectorIdsInRadius(Vector3D position, float radius, List<Vector3I> sectorIds)
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_sectors)
      {
        if (sector.Value.IsValid)
        {
          BoundingBoxD sectorWorldBox = sector.Value.SectorWorldBox;
          sectorWorldBox.Inflate((double) radius);
          if (sectorWorldBox.Contains(position) == ContainmentType.Contains)
            sectorIds.Add(sector.Key);
        }
      }
    }

    public void RemoveItemsAroundPoint(Vector3D point, double radius)
    {
      double radiusSq = radius * radius;
      if (this.Physics != null && (HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null)
      {
        HkStaticCompoundShape shape = (HkStaticCompoundShape) this.Physics.RigidBody.GetShape();
        HkShapeContainerIterator iterator = shape.GetIterator();
        while (iterator.IsValid)
        {
          uint currentShapeKey = iterator.CurrentShapeKey;
          int instanceId;
          shape.DecomposeShapeKey(currentShapeKey, out instanceId, out uint _);
          int itemInstanceId;
          if (this.m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out itemInstanceId) && this.DisableRenderInstanceIfInRadius(point, radiusSq, itemInstanceId, true))
          {
            shape.EnableInstance(instanceId, false);
            this.m_tmpToDisable.Add(itemInstanceId);
          }
          iterator.Next();
        }
      }
      else
      {
        foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
        {
          if (keyValuePair.Value.Enabled && this.DisableRenderInstanceIfInRadius(point, radiusSq, keyValuePair.Key))
            this.m_tmpToDisable.Add(keyValuePair.Key);
        }
      }
      foreach (int key in this.m_tmpToDisable)
      {
        MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[key];
        environmentItemData.Enabled = false;
        this.m_itemsData[key] = environmentItemData;
      }
      this.m_tmpToDisable.Clear();
      foreach (Vector3I key in this.m_updatedSectorsTmp)
        this.Sectors[key].UpdateRenderInstanceData();
      this.m_updatedSectorsTmp.Clear();
    }

    public bool RemoveItem(int itemInstanceId, bool sync, bool immediateUpdate = true)
    {
      int physicsInstanceId;
      if (this.m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out physicsInstanceId))
        return this.RemoveItem(itemInstanceId, physicsInstanceId, sync, immediateUpdate);
      return this.m_itemsData.ContainsKey(itemInstanceId) && this.RemoveNonPhysicalItem(itemInstanceId, sync, immediateUpdate);
    }

    protected bool RemoveItem(
      int itemInstanceId,
      int physicsInstanceId,
      bool sync,
      bool immediateUpdate)
    {
      this.m_physicsShapeInstanceIdToLocalId.Remove(physicsInstanceId);
      this.m_localIdToPhysicsShapeInstanceId.Remove(itemInstanceId);
      if (!this.m_itemsData.ContainsKey(itemInstanceId))
        return false;
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData1 = this.m_itemsData[itemInstanceId];
      this.m_itemsData.Remove(itemInstanceId);
      if (this.Physics != null)
        ((HkStaticCompoundShape) this.Physics.RigidBody.GetShape()).EnableInstance(physicsInstanceId, false);
      MatrixD transformMatrix = environmentItemData1.Transform.TransformMatrix;
      Matrix matrix = (Matrix) ref transformMatrix;
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(matrix.Translation - this.m_cellsOffset, this.Definition.SectorSize);
      int modelId = MyEnvironmentItems.GetModelId(environmentItemData1.SubtypeId);
      MyEnvironmentSector environmentSector;
      if (this.Sectors.TryGetValue(sectorId, out environmentSector))
        environmentSector.DisableInstance(environmentItemData1.SectorInstanceId, modelId);
      foreach (KeyValuePair<int, MyEnvironmentItems.MyEnvironmentItemData> keyValuePair in this.m_itemsData)
      {
        if (keyValuePair.Value.SectorInstanceId == this.Sectors[sectorId].SectorItemCount)
        {
          MyEnvironmentItems.MyEnvironmentItemData environmentItemData2 = keyValuePair.Value;
          environmentItemData2.SectorInstanceId = environmentItemData1.SectorInstanceId;
          this.m_itemsData[keyValuePair.Key] = environmentItemData2;
          break;
        }
      }
      if (immediateUpdate && environmentSector != null)
        environmentSector.UpdateRenderInstanceData(modelId);
      this.OnRemoveItem(itemInstanceId, ref matrix, environmentItemData1.SubtypeId, environmentItemData1.UserData);
      if (sync)
        MySyncEnvironmentItems.RemoveEnvironmentItem(this.EntityId, itemInstanceId);
      return true;
    }

    protected bool RemoveNonPhysicalItem(int itemInstanceId, bool sync, bool immediateUpdate)
    {
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[itemInstanceId];
      environmentItemData.Enabled = false;
      this.m_itemsData[itemInstanceId] = environmentItemData;
      MatrixD transformMatrix = environmentItemData.Transform.TransformMatrix;
      Matrix matrix = (Matrix) ref transformMatrix;
      Vector3I sectorId = MyEnvironmentSector.GetSectorId((Vector3D) matrix.Translation, this.Definition.SectorSize);
      int modelId = MyEnvironmentItems.GetModelId(environmentItemData.SubtypeId);
      this.Sectors[sectorId].DisableInstance(environmentItemData.SectorInstanceId, modelId);
      if (immediateUpdate)
        this.Sectors[sectorId].UpdateRenderInstanceData(modelId);
      this.OnRemoveItem(itemInstanceId, ref matrix, environmentItemData.SubtypeId, environmentItemData.UserData);
      if (sync)
        MySyncEnvironmentItems.RemoveEnvironmentItem(this.EntityId, itemInstanceId);
      return true;
    }

    public void RemoveItemsOfSubtype(HashSet<MyStringHash> subtypes)
    {
      this.BeginBatch(true);
      foreach (int num in new List<int>((IEnumerable<int>) this.m_itemsData.Keys))
      {
        MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[num];
        if (environmentItemData.Enabled && subtypes.Contains(environmentItemData.SubtypeId))
          this.BatchRemoveItem(num, true);
      }
      this.EndBatch(true);
    }

    protected virtual void OnRemoveItem(
      int localId,
      ref Matrix matrix,
      MyStringHash myStringId,
      int userData)
    {
      if (this.ItemRemoved == null)
        return;
      this.ItemRemoved(this, new MyEnvironmentItems.ItemInfo()
      {
        LocalId = localId,
        SubtypeId = myStringId,
        Transform = new MyTransformD((MatrixD) ref matrix),
        UserData = userData
      });
    }

    private bool DisableRenderInstanceIfInRadius(
      Vector3D center,
      double radiusSq,
      int itemInstanceId,
      bool hasPhysics = false)
    {
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[itemInstanceId];
      if (Vector3D.DistanceSquared(new Vector3D((Vector3) environmentItemData.Transform.Position), center) <= radiusSq)
      {
        bool flag = false;
        int key;
        if (this.m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out key))
        {
          this.m_physicsShapeInstanceIdToLocalId.Remove(key);
          this.m_localIdToPhysicsShapeInstanceId.Remove(itemInstanceId);
          flag = true;
        }
        if (!hasPhysics | flag)
        {
          MatrixD transformMatrix = environmentItemData.Transform.TransformMatrix;
          Vector3I sectorId = MyEnvironmentSector.GetSectorId(((Matrix) ref transformMatrix).Translation - this.m_cellsOffset, this.m_definition.SectorSize);
          if (this.Sectors.TryGetValue(sectorId, out MyEnvironmentSector _) && this.Sectors[sectorId].DisableInstance(environmentItemData.SectorInstanceId, MyEnvironmentItems.GetModelId(environmentItemData.SubtypeId)))
            this.m_updatedSectorsTmp.Add(sectorId);
          return true;
        }
      }
      return false;
    }

    public virtual void DoDamage(
      float damage,
      int instanceId,
      Vector3D position,
      Vector3 normal,
      MyStringHash type)
    {
    }

    private void Physics_ContactPointCallback(ref MyPhysics.MyContactPointEvent e)
    {
      float num1 = Math.Abs(e.ContactPointEvent.SeparatingVelocity);
      IMyEntity otherEntity = e.ContactPointEvent.GetOtherEntity((IMyEntity) this);
      if (otherEntity == null || otherEntity.Physics == null || (otherEntity is MyFloatingObject || otherEntity is IMyHandheldGunObject<MyDeviceBase>) || (HkReferenceObject) otherEntity.Physics.RigidBody != (HkReferenceObject) null && otherEntity.Physics.RigidBody.Layer == 20)
        return;
      float num2 = MyDestructionHelper.MassFromHavok(otherEntity.Physics.Mass);
      if (otherEntity is MyCharacter)
        num2 = otherEntity.Physics.Mass;
      double energy = (double) num1 * (double) num1 * (double) num2;
      if (energy <= 200000.0)
        return;
      int bodyIdx = 0;
      Vector3 normal = e.ContactPointEvent.ContactPoint.Normal;
      if (e.ContactPointEvent.Base.BodyA.GetEntity(0U) != this)
      {
        bodyIdx = 1;
        normal *= -1f;
      }
      uint shapeKey = e.ContactPointEvent.GetShapeKey(bodyIdx);
      if (shapeKey == uint.MaxValue)
        return;
      int instanceId;
      ((HkStaticCompoundShape) this.Physics.RigidBody.GetShape()).DecomposeShapeKey(shapeKey, out instanceId, out uint _);
      int itemId;
      if (!this.m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out itemId))
        return;
      this.DestroyItemAndCreateDebris(this.Physics.ClusterToWorld(e.ContactPointEvent.ContactPoint.Position), normal, energy, itemId);
    }

    public void DestroyItemAndCreateDebris(
      Vector3D position,
      Vector3 normal,
      double energy,
      int itemId)
    {
      if (MyPerGameSettings.Destruction)
      {
        this.DoDamage(100f, itemId, position, normal, MyStringHash.NullOrEmpty);
      }
      else
      {
        MyEntity myEntity = this.DestroyItem(itemId);
        if (myEntity == null || myEntity.Physics == null)
          return;
        MyParticlesManager.TryCreateParticleEffect("Tree Destruction", MatrixD.CreateTranslation(position), out MyParticleEffect _);
        float mass = myEntity.Physics.Mass;
        Vector3 vector3 = (float) (Math.Sqrt(energy / (double) mass) / (0.0166666675359011 * (double) MyFakes.SIMULATION_SPEED) * 0.800000011920929) * normal;
        Vector3D vector3D = myEntity.Physics.CenterOfMassWorld + 0.5 * Vector3D.Dot(position - myEntity.Physics.CenterOfMassWorld, myEntity.WorldMatrix.Up) * myEntity.WorldMatrix.Up;
        myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(vector3), new Vector3D?(vector3D), new Vector3?());
      }
    }

    protected virtual MyEntity DestroyItem(int itemInstanceId) => (MyEntity) null;

    private void DestructionBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
    {
      e.GetNewBodies(this.m_tmpBodyInfos);
      foreach (HkdBreakableBodyInfo tmpBodyInfo in this.m_tmpBodyInfos)
      {
        Matrix rigidBodyMatrix = tmpBodyInfo.Body.GetRigidBody().GetRigidBodyMatrix();
        Vector3 translation = rigidBodyMatrix.Translation;
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(rigidBodyMatrix.GetOrientation());
        this.Physics.HavokWorld.GetPenetrationsShape(tmpBodyInfo.Body.BreakableShape.GetShape(), ref translation, ref fromRotationMatrix, MyEnvironmentItems.m_tmpResults, 15);
        foreach (HkBodyCollision tmpResult in MyEnvironmentItems.m_tmpResults)
        {
          if (tmpResult.GetCollisionEntity() is MyVoxelMap)
          {
            tmpBodyInfo.Body.GetRigidBody().Quality = HkCollidableQualityType.Fixed;
            break;
          }
        }
        MyEnvironmentItems.m_tmpResults.Clear();
        tmpBodyInfo.Body.GetRigidBody();
        tmpBodyInfo.Body.Dispose();
      }
    }

    private bool AddPhysicsShape(
      MyStringHash subtypeId,
      MyModel model,
      ref MatrixD worldMatrix,
      HkStaticCompoundShape sectorRootShape,
      Dictionary<MyStringHash, HkShape> subtypeIdToShape,
      out int physicsShapeInstanceId)
    {
      physicsShapeInstanceId = 0;
      HkShape shape;
      if (!subtypeIdToShape.TryGetValue(subtypeId, out shape))
      {
        HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
        if (havokCollisionShapes == null || havokCollisionShapes.Length == 0)
          return false;
        shape = havokCollisionShapes[0];
        shape.AddReference();
        subtypeIdToShape[subtypeId] = shape;
      }
      if (shape.ReferenceCount == 0)
        return false;
      MatrixD matrixD = worldMatrix * MatrixD.CreateTranslation(-this.CellsOffset);
      Matrix transform = (Matrix) ref matrixD;
      physicsShapeInstanceId = sectorRootShape.AddInstance(shape, transform);
      return true;
    }

    public void GetItems(ref Vector3D point, List<Vector3D> output)
    {
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, this.m_definition.SectorSize);
      MyEnvironmentSector environmentSector = (MyEnvironmentSector) null;
      if (!this.m_sectors.TryGetValue(sectorId, out environmentSector))
        return;
      environmentSector.GetItems(output);
    }

    public void GetItemsInRadius(ref Vector3D point, float radius, List<Vector3D> output)
    {
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, this.m_definition.SectorSize);
      MyEnvironmentSector environmentSector = (MyEnvironmentSector) null;
      if (!this.m_sectors.TryGetValue(sectorId, out environmentSector))
        return;
      environmentSector.GetItemsInRadius(point, radius, output);
    }

    public bool HasItem(int localId) => this.m_itemsData.ContainsKey(localId) && this.m_itemsData[localId].Enabled;

    public void GetAllItems(List<MyEnvironmentItems.ItemInfo> output)
    {
      foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_sectors)
        sector.Value.GetItems(output);
    }

    public void GetItemsInSector(ref Vector3D point, List<MyEnvironmentItems.ItemInfo> output)
    {
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(point, this.m_definition.SectorSize);
      MyEnvironmentSector environmentSector = (MyEnvironmentSector) null;
      if (!this.m_sectors.TryGetValue(sectorId, out environmentSector))
        return;
      environmentSector.GetItems(output);
    }

    public MyEnvironmentSector GetSector(ref Vector3D worldPosition)
    {
      Vector3I sectorId = MyEnvironmentSector.GetSectorId(worldPosition, this.m_definition.SectorSize);
      MyEnvironmentSector environmentSector = (MyEnvironmentSector) null;
      return this.m_sectors.TryGetValue(sectorId, out environmentSector) ? environmentSector : (MyEnvironmentSector) null;
    }

    public MyEnvironmentSector GetSector(ref Vector3I sectorId)
    {
      MyEnvironmentSector environmentSector = (MyEnvironmentSector) null;
      return this.m_sectors.TryGetValue(sectorId, out environmentSector) ? environmentSector : (MyEnvironmentSector) null;
    }

    public Vector3I GetSectorId(ref Vector3D worldPosition) => MyEnvironmentSector.GetSectorId(worldPosition, this.m_definition.SectorSize);

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!Sync.IsServer || !this.IsBatching)
        return;
      this.m_batchTime -= 1.666667f;
      if ((double) this.m_batchTime <= 0.0)
        this.EndBatch(true);
      if (this.BatchEnded == null)
        return;
      this.BatchEnded(this);
    }

    protected override void ClampToWorld()
    {
    }

    public int GetItemInstanceId(uint shapeKey)
    {
      HkStaticCompoundShape shape = (HkStaticCompoundShape) this.Physics.RigidBody.GetShape();
      if (shapeKey == uint.MaxValue)
        return -1;
      int instanceId;
      shape.DecomposeShapeKey(shapeKey, out instanceId, out uint _);
      int num;
      return !this.m_physicsShapeInstanceIdToLocalId.TryGetValue(instanceId, out num) ? -1 : num;
    }

    public bool IsItemEnabled(int localId) => this.m_itemsData[localId].Enabled;

    public MyStringHash GetItemSubtype(int localId) => this.m_itemsData[localId].SubtypeId;

    public MyEnvironmentItemDefinition GetItemDefinition(
      int itemInstanceId)
    {
      return MyDefinitionManager.Static.GetEnvironmentItemDefinition(new MyDefinitionId(this.m_definition.ItemDefinitionType, this.m_itemsData[itemInstanceId].SubtypeId));
    }

    public MyEnvironmentItemDefinition GetItemDefinitionFromShapeKey(
      uint shapeKey)
    {
      int itemInstanceId = this.GetItemInstanceId(shapeKey);
      return itemInstanceId == -1 ? (MyEnvironmentItemDefinition) null : MyDefinitionManager.Static.GetEnvironmentItemDefinition(new MyDefinitionId(this.m_definition.ItemDefinitionType, this.m_itemsData[itemInstanceId].SubtypeId));
    }

    public bool GetItemWorldMatrix(int itemInstanceId, out MatrixD worldMatrix)
    {
      worldMatrix = MatrixD.Identity;
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[itemInstanceId];
      worldMatrix = environmentItemData.Transform.TransformMatrix;
      return true;
    }

    public Vector3D CellsOffset
    {
      set
      {
        this.m_cellsOffset = value;
        this.PositionComp.SetPosition(this.m_cellsOffset);
      }
      get => this.m_cellsOffset;
    }

    protected struct MyEnvironmentItemData
    {
      public int Id;
      public MyTransformD Transform;
      public MyStringHash SubtypeId;
      public bool Enabled;
      public int SectorInstanceId;
      public int UserData;
      public MyModel Model;
    }

    public class MyEnvironmentItemsSpawnData
    {
      public MyEnvironmentItems EnvironmentItems;
      public Dictionary<MyStringHash, HkShape> SubtypeToShapes = new Dictionary<MyStringHash, HkShape>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      public HkStaticCompoundShape SectorRootShape;
      public BoundingBoxD AabbWorld = BoundingBoxD.CreateInvalid();
    }

    public struct ItemInfo
    {
      public int LocalId;
      public MyTransformD Transform;
      public MyStringHash SubtypeId;
      public int UserData;
    }

    private struct AddItemData
    {
      public Vector3D Position;
      public MyStringHash SubtypeId;
    }

    private struct ModifyItemData
    {
      public int LocalId;
      public MyStringHash SubtypeId;
    }

    private struct RemoveItemData
    {
      public int LocalId;
    }

    private class MyEnviromentItemsDebugDraw : MyDebugRenderComponentBase
    {
      private MyEnvironmentItems m_items;

      public MyEnviromentItemsDebugDraw(MyEnvironmentItems items) => this.m_items = items;

      public override void DebugDraw()
      {
        if (!MyDebugDrawSettings.DEBUG_DRAW_ENVIRONMENT_ITEMS)
          return;
        foreach (KeyValuePair<Vector3I, MyEnvironmentSector> sector in this.m_items.Sectors)
        {
          sector.Value.DebugDraw(sector.Key, this.m_items.m_definition.SectorSize);
          if (sector.Value.IsValid)
          {
            Vector3D worldCoord = sector.Value.SectorBox.Center + sector.Value.SectorMatrix.Translation;
            if (Vector3D.Distance(MySector.MainCamera.Position, worldCoord) < 1000.0)
              MyRenderProxy.DebugDrawText3D(worldCoord, this.m_items.Definition.Id.SubtypeName + " Sector: " + (object) sector.Key, Color.SaddleBrown, 1f, true);
          }
        }
      }

      public override void DebugDrawInvalidTriangles()
      {
      }
    }

    private class Sandbox_Game_Entities_EnvironmentItems_MyEnvironmentItems\u003C\u003EActor : IActivator, IActivator<MyEnvironmentItems>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentItems();

      MyEnvironmentItems IActivator<MyEnvironmentItems>.CreateInstance() => new MyEnvironmentItems();
    }
  }
}
