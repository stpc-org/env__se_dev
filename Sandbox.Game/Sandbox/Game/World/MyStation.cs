// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyStation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World.Generator;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyStation
  {
    public static readonly float SAFEZONE_SIZE = 150f;
    private MatrixD m_transformation;
    private MyStationTypeEnum m_type;
    private bool m_isDeepSpaceStation;

    public List<MyStoreItem> StoreItems { get; private set; }

    internal long SafeZoneEntityId { get; set; }

    public bool IsOnPlanetWithAtmosphere { get; set; }

    internal MyStationResourcesGenerator ResourcesGenerator { get; private set; }

    public long Id { get; private set; }

    public long StationEntityId { get; set; }

    public string PrefabName { get; private set; }

    public long FactionId { get; private set; }

    public MyStationTypeEnum Type => this.m_type;

    public Vector3D Position => this.m_transformation.Translation;

    public Vector3D Up => this.m_transformation.Up;

    public Vector3D Forward => this.m_transformation.Forward;

    public bool IsDeepSpaceStation => this.m_isDeepSpaceStation;

    public MyStation(
      long id,
      Vector3D position,
      MyStationTypeEnum type,
      MyFaction faction,
      string prefabName,
      MyDefinitionId? generatedItemsContainerTypeId,
      Vector3? up = null,
      Vector3? forward = null,
      bool isDeep = false)
    {
      this.Id = id;
      this.m_type = type;
      this.m_isDeepSpaceStation = isDeep;
      this.FactionId = faction.FactionId;
      this.PrefabName = prefabName;
      this.m_transformation = new MatrixD();
      this.m_transformation.Translation = position;
      this.ResourcesGenerator = new MyStationResourcesGenerator(generatedItemsContainerTypeId);
      if (up.HasValue)
      {
        if (forward.HasValue)
        {
          this.m_transformation.Up = (Vector3D) up.Value;
          this.m_transformation.Forward = (Vector3D) forward.Value;
          this.m_transformation.Left = (Vector3D) Vector3.Cross((Vector3) this.m_transformation.Up, (Vector3) this.m_transformation.Forward);
        }
        else
        {
          this.m_transformation.Up = (Vector3D) up.Value;
          this.m_transformation.Forward = (Vector3D) Vector3.CalculatePerpendicularVector((Vector3) this.m_transformation.Up);
          this.m_transformation.Left = (Vector3D) Vector3.Cross((Vector3) this.m_transformation.Up, (Vector3) this.m_transformation.Forward);
        }
      }
      else if (forward.HasValue)
      {
        this.m_transformation.Forward = (Vector3D) forward.Value;
        this.m_transformation.Up = (Vector3D) Vector3.CalculatePerpendicularVector((Vector3) this.m_transformation.Forward);
        this.m_transformation.Left = (Vector3D) Vector3.Cross((Vector3) this.m_transformation.Up, (Vector3) this.m_transformation.Forward);
      }
      else
      {
        this.m_transformation.Up = (Vector3D) Vector3.Normalize(MyUtils.GetRandomVector3());
        this.m_transformation.Forward = (Vector3D) Vector3.CalculatePerpendicularVector((Vector3) this.m_transformation.Up);
        this.m_transformation.Left = (Vector3D) Vector3.Cross((Vector3) this.m_transformation.Up, (Vector3) this.m_transformation.Forward);
      }
      this.StoreItems = new List<MyStoreItem>();
    }

    internal MySafeZone CreateSafeZone(IMyFaction faction)
    {
      MySafeZone mySafeZone = MySessionComponentSafeZones.CrateSafeZone(this.m_transformation, MySafeZoneShape.Sphere, MySafeZoneAccess.Whitelist, (long[]) null, (long[]) null, MyStation.SAFEZONE_SIZE, true, false, new Vector3(0.0f, 0.09f, 0.196f), "SafeZone_Texture_Aura") as MySafeZone;
      mySafeZone.DisplayName = mySafeZone.Name = mySafeZone.DisplayName = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), (object) faction.Tag, (object) this.Id);
      this.SafeZoneEntityId = mySafeZone.EntityId;
      return mySafeZone;
    }

    public MyStation(MyObjectBuilder_Station obj)
    {
      this.m_transformation = MatrixD.CreateWorld((Vector3D) obj.Position, (Vector3) obj.Forward, (Vector3) obj.Up);
      this.Id = obj.Id;
      this.m_type = obj.StationType;
      this.m_isDeepSpaceStation = obj.IsDeepSpaceStation;
      this.StationEntityId = obj.StationEntityId;
      this.FactionId = obj.FactionId;
      this.PrefabName = obj.PrefabName;
      this.SafeZoneEntityId = obj.SafeZoneEntityId;
      this.IsOnPlanetWithAtmosphere = obj.IsOnPlanetWithAtmosphere;
      if (obj.StoreItems != null)
      {
        this.StoreItems = new List<MyStoreItem>(obj.StoreItems.Count);
        foreach (MyObjectBuilder_StoreItem storeItem in obj.StoreItems)
          this.StoreItems.Add(new MyStoreItem(storeItem));
      }
      else
        this.StoreItems = new List<MyStoreItem>();
      this.ResourcesGenerator = new MyStationResourcesGenerator(MyStationGenerator.GetStationTypeDefinition(this.Type).GeneratedItemsContainerType);
    }

    public MyObjectBuilder_Station GetObjectBuilder()
    {
      MyObjectBuilder_Station objectBuilderStation = new MyObjectBuilder_Station();
      objectBuilderStation.Position = (SerializableVector3D) this.m_transformation.Translation;
      objectBuilderStation.Up = new SerializableVector3((float) this.m_transformation.Up.X, (float) this.m_transformation.Up.Y, (float) this.m_transformation.Up.Z);
      objectBuilderStation.Forward = new SerializableVector3((float) this.m_transformation.Forward.X, (float) this.m_transformation.Forward.Y, (float) this.m_transformation.Forward.Z);
      objectBuilderStation.Id = this.Id;
      objectBuilderStation.StationType = this.m_type;
      objectBuilderStation.IsDeepSpaceStation = this.m_isDeepSpaceStation;
      objectBuilderStation.StationEntityId = this.StationEntityId;
      objectBuilderStation.FactionId = this.FactionId;
      objectBuilderStation.PrefabName = this.PrefabName;
      objectBuilderStation.SafeZoneEntityId = this.SafeZoneEntityId;
      objectBuilderStation.IsOnPlanetWithAtmosphere = this.IsOnPlanetWithAtmosphere;
      if (this.StoreItems != null)
      {
        objectBuilderStation.StoreItems = new List<MyObjectBuilder_StoreItem>(this.StoreItems.Count);
        foreach (MyStoreItem storeItem in this.StoreItems)
          objectBuilderStation.StoreItems.Add(storeItem.GetObjectBuilder());
      }
      else
        objectBuilderStation.StoreItems = new List<MyObjectBuilder_StoreItem>();
      return objectBuilderStation;
    }

    internal void StationGridSpawned() => MySession.Static.GetComponent<MySessionComponentContractSystem>().StationGridSpawned(this);

    internal MyStoreItem GetStoreItemById(long id)
    {
      foreach (MyStoreItem storeItem in this.StoreItems)
      {
        if (storeItem.Id == id)
          return storeItem;
      }
      return (MyStoreItem) null;
    }

    internal void RemoveStoreItem(MyStoreItem storeItem) => this.StoreItems.Remove(storeItem);

    internal void Update(MyFaction faction) => this.ResourcesGenerator.UpdateStation(this.StationEntityId);
  }
}
