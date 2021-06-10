// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyIdentity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MyIdentity : IMyIdentity
  {
    private MyBlockLimits m_blockLimits;
    private static readonly Dictionary<string, short> EmptyBlockTypeLimitDictionary = new Dictionary<string, short>();
    private List<MyIdentity.BuildPlanItem> m_buildPlanner = new List<MyIdentity.BuildPlanItem>();
    public List<long> RespawnShips = new List<long>();
    public List<long> ActiveContracts = new List<long>();

    public long IdentityId { get; private set; }

    public string DisplayName { get; private set; }

    public MyCharacter Character { get; private set; }

    public HashSet<long> SavedCharacters { get; private set; }

    public string Model { get; private set; }

    public Vector3? ColorMask { get; private set; }

    public bool IsDead { get; private set; }

    public Vector3D? LastDeathPosition { get; private set; }

    public TimeSpan LastRespawnTime { get; private set; }

    public bool FirstSpawnDone { get; private set; }

    public DateTime LastLoginTime { get; set; }

    public DateTime LastLogoutTime { get; set; }

    public MyBlockLimits BlockLimits
    {
      get
      {
        if (MySession.Static.Players.IdentityIsNpc(this.IdentityId))
          return MySession.Static.NPCBlockLimits;
        switch (MySession.Static.BlockLimitsEnabled)
        {
          case MyBlockLimitsEnabledEnum.GLOBALLY:
            return MySession.Static.GlobalBlockLimits;
          case MyBlockLimitsEnabledEnum.PER_FACTION:
            return MySession.Static.Factions.TryGetPlayerFaction(this.IdentityId) is MyFaction playerFaction ? playerFaction.BlockLimits : MyBlockLimits.Empty;
          default:
            return this.m_blockLimits;
        }
      }
    }

    public event Action<MyCharacter, MyCharacter> CharacterChanged;

    public event Action<MyFaction, MyFaction> FactionChanged;

    private MyIdentity(
      string name,
      MyEntityIdentifier.ID_OBJECT_TYPE identityType,
      string model = null,
      Vector3? colorMask = null)
    {
      this.IdentityId = MyEntityIdentifier.AllocateId(identityType, MyEntityIdentifier.ID_ALLOCATION_METHOD.SERIAL_START_WITH_1);
      this.Init(name, this.IdentityId, model, colorMask);
    }

    private MyIdentity(string name, long identityId, string model, Vector3? colorMask)
    {
      identityId = MyEntityIdentifier.FixObsoleteIdentityType(identityId);
      this.Init(name, identityId, model, colorMask);
      MyEntityIdentifier.MarkIdUsed(identityId);
    }

    private MyIdentity(MyObjectBuilder_Identity objectBuilder)
    {
      string displayName = objectBuilder.DisplayName;
      long identityId = MyEntityIdentifier.FixObsoleteIdentityType(objectBuilder.IdentityId);
      string model = objectBuilder.Model;
      SerializableVector3? colorMask1 = objectBuilder.ColorMask;
      Vector3? colormask = colorMask1.HasValue ? new Vector3?((Vector3) colorMask1.GetValueOrDefault()) : new Vector3?();
      int blockLimitModifier = objectBuilder.BlockLimitModifier;
      int transferedPcuDelta = objectBuilder.TransferedPCUDelta;
      DateTime? loginTime = new DateTime?(objectBuilder.LastLoginTime);
      DateTime? logoutTime = new DateTime?(objectBuilder.LastLogoutTime);
      this.Init(displayName, identityId, model, colormask, blockLimitModifier, transferedPcuDelta, loginTime, logoutTime);
      MyEntityIdentifier.MarkIdUsed(this.IdentityId);
      if (objectBuilder.ColorMask.HasValue)
      {
        SerializableVector3? colorMask2 = objectBuilder.ColorMask;
        this.ColorMask = colorMask2.HasValue ? new Vector3?((Vector3) colorMask2.GetValueOrDefault()) : new Vector3?();
      }
      this.IsDead = true;
      MyEntity entity;
      MyEntities.TryGetEntityById(objectBuilder.CharacterEntityId, out entity);
      if (entity is MyCharacter)
        this.Character = entity as MyCharacter;
      if (objectBuilder.SavedCharacters != null)
        this.SavedCharacters = objectBuilder.SavedCharacters;
      if (objectBuilder.RespawnShips != null)
        this.RespawnShips = objectBuilder.RespawnShips;
      if (objectBuilder.ActiveContracts != null)
        this.ActiveContracts = objectBuilder.ActiveContracts;
      this.LastDeathPosition = objectBuilder.LastDeathPosition;
    }

    public MyObjectBuilder_Identity GetObjectBuilder()
    {
      MyObjectBuilder_Identity objectBuilderIdentity1 = new MyObjectBuilder_Identity();
      objectBuilderIdentity1.IdentityId = this.IdentityId;
      objectBuilderIdentity1.DisplayName = this.DisplayName;
      objectBuilderIdentity1.CharacterEntityId = this.Character == null ? 0L : this.Character.EntityId;
      objectBuilderIdentity1.Model = this.Model;
      MyObjectBuilder_Identity objectBuilderIdentity2 = objectBuilderIdentity1;
      Vector3? colorMask = this.ColorMask;
      SerializableVector3? nullable = colorMask.HasValue ? new SerializableVector3?((SerializableVector3) colorMask.GetValueOrDefault()) : new SerializableVector3?();
      objectBuilderIdentity2.ColorMask = nullable;
      objectBuilderIdentity1.BlockLimitModifier = this.BlockLimits.BlockLimitModifier;
      objectBuilderIdentity1.TransferedPCUDelta = this.BlockLimits.TransferedDelta;
      objectBuilderIdentity1.LastLoginTime = this.LastLoginTime;
      objectBuilderIdentity1.LastLogoutTime = this.LastLogoutTime;
      objectBuilderIdentity1.SavedCharacters = new HashSet<long>();
      foreach (long savedCharacter in this.SavedCharacters)
      {
        if (MyEntities.GetEntityById(savedCharacter) != null)
          objectBuilderIdentity1.SavedCharacters.Add(savedCharacter);
      }
      objectBuilderIdentity1.RespawnShips = this.RespawnShips;
      objectBuilderIdentity1.LastDeathPosition = this.LastDeathPosition;
      objectBuilderIdentity1.ActiveContracts = this.ActiveContracts;
      return objectBuilderIdentity1;
    }

    private void Init(
      string name,
      long identityId,
      string model,
      Vector3? colormask,
      int blockLimitModifier = 0,
      int transferedPCUDelta = 0,
      DateTime? loginTime = null,
      DateTime? logoutTime = null)
    {
      this.DisplayName = name;
      this.IdentityId = identityId;
      this.IsDead = true;
      this.Model = model;
      this.ColorMask = colormask;
      this.m_blockLimits = new MyBlockLimits(this.GetInitialPCU(), blockLimitModifier, transferedPCUDelta);
      DateTime? nullable;
      if (MySession.Static.Players.IdentityIsNpc(identityId))
      {
        this.LastLoginTime = DateTime.Now;
      }
      else
      {
        nullable = loginTime;
        this.LastLoginTime = nullable ?? DateTime.Now;
      }
      nullable = logoutTime;
      this.LastLogoutTime = nullable ?? DateTime.Now;
      this.SavedCharacters = new HashSet<long>();
    }

    public int GetInitialPCU() => MyBlockLimits.GetInitialPCU(this.IdentityId);

    public int GetMaxPCU() => MyBlockLimits.GetMaxPCU(this);

    public void SetColorMask(Vector3 color) => this.ColorMask = new Vector3?(color);

    public void ChangeCharacter(MyCharacter character)
    {
      MyCharacter character1 = this.Character;
      if (character1 != null)
      {
        character1.OnClosing -= new Action<MyEntity>(this.OnCharacterClosing);
        character1.CharacterDied -= new Action<MyCharacter>(this.OnCharacterDied);
      }
      this.Character = character;
      if (character != null)
      {
        character.OnClosing += new Action<MyEntity>(this.OnCharacterClosing);
        character.CharacterDied += new Action<MyCharacter>(this.OnCharacterDied);
        this.SaveModelAndColorFromCharacter();
        this.IsDead = character.IsDead;
        if (!this.SavedCharacters.Contains(character.EntityId))
        {
          this.SavedCharacters.Add(character.EntityId);
          character.OnClosing += new Action<MyEntity>(this.OnSavedCharacterClosing);
        }
      }
      this.CharacterChanged.InvokeIfNotNull<MyCharacter, MyCharacter>(character1, this.Character);
    }

    private void OnCharacterDied(MyCharacter character) => this.LastDeathPosition = new Vector3D?(character.PositionComp.GetPosition());

    private void SaveModelAndColorFromCharacter()
    {
      this.Model = this.Character.ModelName;
      this.ColorMask = new Vector3?(this.Character.ColorMask);
    }

    public void SetDead(bool dead) => this.IsDead = dead;

    public void PerformFirstSpawn() => this.FirstSpawnDone = true;

    public void LogRespawnTime() => this.LastRespawnTime = MySession.Static.ElapsedGameTime;

    public void SetDisplayName(string name) => this.DisplayName = name;

    private void OnCharacterClosing(MyEntity character)
    {
      this.Character.OnClosing -= new Action<MyEntity>(this.OnCharacterClosing);
      this.Character.CharacterDied -= new Action<MyCharacter>(this.OnCharacterDied);
      this.Character = (MyCharacter) null;
    }

    private void OnSavedCharacterClosing(MyEntity character)
    {
      character.OnClosing -= new Action<MyEntity>(this.OnSavedCharacterClosing);
      this.SavedCharacters.Remove(character.EntityId);
    }

    public void RaiseFactionChanged(MyFaction oldFaction, MyFaction newFaction) => this.FactionChanged.InvokeIfNotNull<MyFaction, MyFaction>(oldFaction, newFaction);

    public IReadOnlyList<MyIdentity.BuildPlanItem> BuildPlanner => (IReadOnlyList<MyIdentity.BuildPlanItem>) this.m_buildPlanner;

    internal bool AddToBuildPlanner(
      MyCubeBlockDefinition block,
      int index = -1,
      List<MyIdentity.BuildPlanItem.Component> components = null)
    {
      if (this.m_buildPlanner.Count >= 8 && (index == -1 || index > 7))
      {
        MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerCapacityReached);
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        return false;
      }
      MyIdentity.BuildPlanItem buildPlanItem = new MyIdentity.BuildPlanItem();
      buildPlanItem.BlockDefinition = block;
      buildPlanItem.IsInProgress = false;
      if (components == null)
      {
        buildPlanItem.Components = new List<MyIdentity.BuildPlanItem.Component>();
        foreach (MyCubeBlockDefinition.Component component in block.Components)
          buildPlanItem.Components.Add(new MyIdentity.BuildPlanItem.Component()
          {
            ComponentDefinition = component.Definition,
            Count = component.Count
          });
      }
      else
      {
        buildPlanItem.Components = components;
        buildPlanItem.IsInProgress = true;
      }
      if (index == -1 || index >= this.m_buildPlanner.Count)
        this.m_buildPlanner.Add(buildPlanItem);
      else if (index >= 0 && index < this.m_buildPlanner.Count)
      {
        this.m_buildPlanner.RemoveAt(index);
        this.m_buildPlanner.Insert(index, buildPlanItem);
      }
      return true;
    }

    internal void RemoveAtBuildPlanner(int index)
    {
      if (index < 0 || index >= this.m_buildPlanner.Count)
        return;
      this.m_buildPlanner.RemoveAt(index);
    }

    internal void RemoveLastFromBuildPlanner()
    {
      if (this.m_buildPlanner.Count == 0)
        return;
      this.m_buildPlanner.RemoveAt(this.m_buildPlanner.Count - 1);
    }

    internal void CleanFinishedBuildPlanner() => this.m_buildPlanner.RemoveAll((Predicate<MyIdentity.BuildPlanItem>) (x => x.Components.Count == 0));

    internal void LoadBuildPlanner(
      MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
    {
      this.m_buildPlanner = new List<MyIdentity.BuildPlanItem>();
      foreach (MyObjectBuilder_Character.BuildPlanItem buildPlanItem1 in buildPlanner)
      {
        MyIdentity.BuildPlanItem buildPlanItem2 = new MyIdentity.BuildPlanItem();
        buildPlanItem2.BlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) buildPlanItem1.BlockId);
        buildPlanItem2.IsInProgress = buildPlanItem1.IsInProgress;
        buildPlanItem2.Components = new List<MyIdentity.BuildPlanItem.Component>();
        foreach (MyObjectBuilder_Character.ComponentItem component in buildPlanItem1.Components)
          buildPlanItem2.Components.Add(new MyIdentity.BuildPlanItem.Component()
          {
            ComponentDefinition = MyDefinitionManager.Static.GetComponentDefinition((MyDefinitionId) component.ComponentId),
            Count = component.Count
          });
        this.m_buildPlanner.Add(buildPlanItem2);
      }
    }

    private static MyBlueprintDefinitionBase MakeBlueprintFromBuildPlanItem(
      MyIdentity.BuildPlanItem buildPlanItem)
    {
      MyObjectBuilder_CompositeBlueprintDefinition newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
      newObject.Id = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintDefinition), buildPlanItem.BlockDefinition.Id.ToString().Replace("MyObjectBuilder_", "BuildPlanItem_"));
      Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
      foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
      {
        MyDefinitionId id = component.ComponentDefinition.Id;
        if (!dictionary.ContainsKey(id))
          dictionary[id] = (MyFixedPoint) 0;
        dictionary[id] += (MyFixedPoint) component.Count;
      }
      newObject.Blueprints = new BlueprintItem[dictionary.Count];
      int index = 0;
      foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> keyValuePair in dictionary)
      {
        MyBlueprintDefinitionBase definitionByResultId;
        if ((definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(keyValuePair.Key)) == null)
          return (MyBlueprintDefinitionBase) null;
        newObject.Blueprints[index] = new BlueprintItem()
        {
          Id = new SerializableDefinitionId(definitionByResultId.Id.TypeId, definitionByResultId.Id.SubtypeName),
          Amount = keyValuePair.Value.ToString()
        };
        ++index;
      }
      newObject.Icons = buildPlanItem.BlockDefinition.Icons;
      newObject.DisplayName = buildPlanItem.BlockDefinition.DisplayNameEnum.HasValue ? buildPlanItem.BlockDefinition.DisplayNameEnum.Value.ToString() : buildPlanItem.BlockDefinition.DisplayNameText;
      newObject.Public = buildPlanItem.BlockDefinition.Public;
      MyCompositeBlueprintDefinition blueprintDefinition = new MyCompositeBlueprintDefinition();
      blueprintDefinition.Init((MyObjectBuilder_DefinitionBase) newObject, MyModContext.BaseGame);
      blueprintDefinition.Postprocess();
      return (MyBlueprintDefinitionBase) blueprintDefinition;
    }

    private MyObjectBuilder_Character.BuildPlanItem[] SaveBuildPlanner()
    {
      List<MyObjectBuilder_Character.BuildPlanItem> buildPlanItemList = new List<MyObjectBuilder_Character.BuildPlanItem>();
      foreach (MyIdentity.BuildPlanItem buildPlanItem1 in (IEnumerable<MyIdentity.BuildPlanItem>) this.BuildPlanner)
      {
        MyObjectBuilder_Character.BuildPlanItem buildPlanItem2 = new MyObjectBuilder_Character.BuildPlanItem();
        buildPlanItem2.BlockId = (SerializableDefinitionId) buildPlanItem1.BlockDefinition.Id;
        buildPlanItem2.IsInProgress = buildPlanItem1.IsInProgress;
        buildPlanItem2.Components = new List<MyObjectBuilder_Character.ComponentItem>();
        foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem1.Components)
          buildPlanItem2.Components.Add(new MyObjectBuilder_Character.ComponentItem()
          {
            ComponentId = (SerializableDefinitionId) component.ComponentDefinition.Id,
            Count = component.Count
          });
        buildPlanItemList.Add(buildPlanItem2);
      }
      return buildPlanItemList.ToArray();
    }

    event Action<IMyCharacter, IMyCharacter> IMyIdentity.CharacterChanged
    {
      add => this.CharacterChanged += this.GetDelegate(value);
      remove => this.CharacterChanged -= this.GetDelegate(value);
    }

    private Action<MyCharacter, MyCharacter> GetDelegate(
      Action<IMyCharacter, IMyCharacter> value)
    {
      return (Action<MyCharacter, MyCharacter>) Delegate.CreateDelegate(typeof (Action<MyCharacter, MyCharacter>), value.Target, value.Method);
    }

    long IMyIdentity.PlayerId => this.IdentityId;

    long IMyIdentity.IdentityId => this.IdentityId;

    string IMyIdentity.DisplayName => this.DisplayName;

    string IMyIdentity.Model => this.Model;

    Vector3? IMyIdentity.ColorMask => this.ColorMask;

    bool IMyIdentity.IsDead => this.IsDead;

    public class Friend
    {
      public virtual MyIdentity CreateNewIdentity(
        string name,
        string model = null,
        Vector3? colorMask = null)
      {
        return new MyIdentity(name, MyEntityIdentifier.ID_OBJECT_TYPE.IDENTITY, model, colorMask);
      }

      public virtual MyIdentity CreateNewIdentity(
        string name,
        long identityId,
        string model,
        Vector3? colorMask)
      {
        return new MyIdentity(name, identityId, model, colorMask);
      }

      public virtual MyIdentity CreateNewIdentity(MyObjectBuilder_Identity objectBuilder) => new MyIdentity(objectBuilder);
    }

    public class BuildPlanItem
    {
      public MyCubeBlockDefinition BlockDefinition;
      public bool IsInProgress;
      public List<MyIdentity.BuildPlanItem.Component> Components;

      public MyIdentity.BuildPlanItem Clone()
      {
        MyIdentity.BuildPlanItem buildPlanItem = new MyIdentity.BuildPlanItem();
        buildPlanItem.Components = new List<MyIdentity.BuildPlanItem.Component>();
        buildPlanItem.IsInProgress = this.IsInProgress;
        buildPlanItem.BlockDefinition = this.BlockDefinition;
        foreach (MyIdentity.BuildPlanItem.Component component in this.Components)
          buildPlanItem.Components.Add(component.Clone());
        return buildPlanItem;
      }

      public class Component
      {
        public MyComponentDefinition ComponentDefinition;
        public int Count;

        public MyIdentity.BuildPlanItem.Component Clone() => new MyIdentity.BuildPlanItem.Component()
        {
          ComponentDefinition = this.ComponentDefinition,
          Count = this.Count
        };
      }
    }
  }
}
