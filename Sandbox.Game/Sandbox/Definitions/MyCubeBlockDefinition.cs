// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCubeBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CubeBlockDefinition), null)]
  public class MyCubeBlockDefinition : MyPhysicalModelDefinition
  {
    public MyCubeSize CubeSize;
    public MyBlockTopology BlockTopology = MyBlockTopology.TriangleMesh;
    public Vector3I Size;
    public Vector3 ModelOffset;
    public bool UseModelIntersection;
    public MyCubeDefinition CubeDefinition;
    public bool SilenceableByShipSoundSystem;
    public MyCubeBlockDefinition.Component[] Components;
    public ushort CriticalGroup;
    public float CriticalIntegrityRatio;
    public float OwnershipIntegrityRatio;
    public float MaxIntegrityRatio;
    public float MaxIntegrity;
    public int? DamageEffectID;
    public string DamageEffectName = string.Empty;
    public string DestroyEffect = "";
    public Vector3? DestroyEffectOffset;
    public MySoundPair DestroySound = MySoundPair.Empty;
    public CubeBlockEffectBase[] Effects;
    public MyCubeBlockDefinition.MountPoint[] MountPoints;
    public Dictionary<Vector3I, Dictionary<Vector3I, MyCubeBlockDefinition.MyCubePressurizationMark>> IsCubePressurized;
    public MyBlockNavigationDefinition NavigationDefinition;
    public Color Color;
    public List<MyCubeBlockDefinition> Variants = new List<MyCubeBlockDefinition>();
    public MyCubeBlockDefinition UniqueVersion;
    public MyPhysicsOption PhysicsOption;
    public MyStringId? DisplayNameVariant;
    public string BlockPairName;
    public bool UsesDeformation;
    public float DeformationRatio;
    public float IntegrityPointsPerSec;
    public string EdgeType;
    public List<VRage.Game.BoneInfo> Skeleton;
    public Dictionary<Vector3I, Vector3> Bones;
    public bool? IsAirTight;
    public bool IsStandAlone = true;
    public bool HasPhysics = true;
    public bool UseNeighbourOxygenRooms;
    public MyStringId BuildType;
    public string BuildMaterial;
    public MyDefinitionId[] GeneratedBlockDefinitions;
    public MyStringId GeneratedBlockType;
    public float BuildProgressToPlaceGeneratedBlocks;
    public bool CreateFracturedPieces;
    public MyStringHash EmissiveColorPreset = MyStringHash.NullOrEmpty;
    public string[] CompoundTemplates;
    public bool CompoundEnabled;
    public string MultiBlock;
    public Dictionary<string, MyDefinitionId> SubBlockDefinitions;
    [System.Obsolete("Use new block variant group system")]
    public MyDefinitionId[] BlockStages;
    public MyCubeBlockDefinition.BuildProgressModel[] BuildProgressModels;
    private Vector3I m_center;
    private MySymmetryAxisEnum m_symmetryX;
    private MySymmetryAxisEnum m_symmetryY;
    private MySymmetryAxisEnum m_symmetryZ;
    private StringBuilder m_displayNameTextCache;
    public float DisassembleRatio;
    public MyAutorotateMode AutorotateMode;
    private string m_mirroringBlock;
    public MySoundPair PrimarySound;
    public MySoundPair ActionSound;
    public MySoundPair DamagedSound;
    public int PCU;
    public static readonly int PCU_CONSTRUCTION_STAGE_COST = 1;
    public bool PlaceDecals;
    public Vector3? DepressurizationEffectOffset;
    public List<uint> TieredUpdateTimes = new List<uint>();
    public Dictionary<string, MyObjectBuilder_ComponentBase> EntityComponents;
    public VoxelPlacementOverride? VoxelPlacement;
    public float GeneralDamageMultiplier;
    public MyBlockVariantGroup BlockVariantsGroup;
    private static Matrix[] m_mountPointTransforms = new Matrix[6]
    {
      Matrix.CreateFromDir(Vector3.Right, Vector3.Up) * Matrix.CreateScale(1f, 1f, -1f),
      Matrix.CreateFromDir(Vector3.Up, Vector3.Forward) * Matrix.CreateScale(-1f, 1f, 1f),
      Matrix.CreateFromDir(Vector3.Forward, Vector3.Up) * Matrix.CreateScale(-1f, 1f, 1f),
      Matrix.CreateFromDir(Vector3.Left, Vector3.Up) * Matrix.CreateScale(1f, 1f, -1f),
      Matrix.CreateFromDir(Vector3.Down, Vector3.Backward) * Matrix.CreateScale(-1f, 1f, 1f),
      Matrix.CreateFromDir(Vector3.Backward, Vector3.Up) * Matrix.CreateScale(-1f, 1f, 1f)
    };
    private static Vector3[] m_mountPointWallOffsets = new Vector3[6]
    {
      new Vector3(1f, 0.0f, 1f),
      new Vector3(0.0f, 1f, 1f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 1f)
    };
    private static int[] m_mountPointWallIndices = new int[6]
    {
      2,
      5,
      3,
      0,
      1,
      4
    };
    private const float OFFSET_CONST = 0.001f;
    private const float THICKNESS_HALF = 0.0004f;
    private static List<int> m_tmpIndices = new List<int>();
    private static List<MyObjectBuilder_CubeBlockDefinition.MountPoint> m_tmpMounts = new List<MyObjectBuilder_CubeBlockDefinition.MountPoint>();
    private static readonly List<string> m_stringList = new List<string>();
    private static readonly HashSet<MyCubeBlockDefinition> m_preloadedDefinitions = new HashSet<MyCubeBlockDefinition>();

    public MyBlockDirection Direction { get; private set; }

    public MyBlockRotation Rotation { get; private set; }

    public bool IsGeneratedBlock => this.GeneratedBlockType != MyStringId.NullOrEmpty;

    public Vector3I Center => this.m_center;

    public MySymmetryAxisEnum SymmetryX => this.m_symmetryX;

    public MySymmetryAxisEnum SymmetryY => this.m_symmetryY;

    public MySymmetryAxisEnum SymmetryZ => this.m_symmetryZ;

    public string MirroringBlock => this.m_mirroringBlock;

    public override string DisplayNameText
    {
      get
      {
        if (!this.DisplayNameVariant.HasValue)
          return base.DisplayNameText;
        if (this.m_displayNameTextCache == null)
          this.m_displayNameTextCache = new StringBuilder();
        this.m_displayNameTextCache.Clear();
        return this.m_displayNameTextCache.Append(base.DisplayNameText).Append(' ').Append(MyTexts.GetString(this.DisplayNameVariant.Value)).ToString();
      }
    }

    public bool GuiVisible { get; set; }

    public bool Mirrored { get; private set; }

    public bool RandomRotation { get; private set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CubeBlockDefinition cubeBlockDefinition = builder as MyObjectBuilder_CubeBlockDefinition;
      this.Size = (Vector3I) cubeBlockDefinition.Size;
      this.Model = cubeBlockDefinition.Model;
      this.UseModelIntersection = cubeBlockDefinition.UseModelIntersection;
      this.CubeSize = cubeBlockDefinition.CubeSize;
      this.ModelOffset = (Vector3) cubeBlockDefinition.ModelOffset;
      this.BlockTopology = cubeBlockDefinition.BlockTopology;
      this.PhysicsOption = cubeBlockDefinition.PhysicsOption;
      this.BlockPairName = cubeBlockDefinition.BlockPairName;
      this.m_center = (Vector3I) (cubeBlockDefinition.Center ?? (SerializableVector3I) ((this.Size - 1) / 2));
      this.m_symmetryX = cubeBlockDefinition.MirroringX;
      this.m_symmetryY = cubeBlockDefinition.MirroringY;
      this.m_symmetryZ = cubeBlockDefinition.MirroringZ;
      this.UsesDeformation = cubeBlockDefinition.UsesDeformation;
      this.DeformationRatio = cubeBlockDefinition.DeformationRatio;
      this.SilenceableByShipSoundSystem = cubeBlockDefinition.SilenceableByShipSoundSystem;
      this.EdgeType = cubeBlockDefinition.EdgeType;
      this.AutorotateMode = cubeBlockDefinition.AutorotateMode;
      this.m_mirroringBlock = cubeBlockDefinition.MirroringBlock;
      this.MultiBlock = cubeBlockDefinition.MultiBlock;
      this.GuiVisible = cubeBlockDefinition.GuiVisible;
      this.Rotation = cubeBlockDefinition.Rotation;
      this.Direction = cubeBlockDefinition.Direction;
      this.Mirrored = cubeBlockDefinition.Mirrored;
      this.RandomRotation = cubeBlockDefinition.RandomRotation;
      this.BuildType = MyStringId.GetOrCompute(cubeBlockDefinition.BuildType != null ? cubeBlockDefinition.BuildType.ToLower() : (string) null);
      this.BuildMaterial = cubeBlockDefinition.BuildMaterial != null ? cubeBlockDefinition.BuildMaterial.ToLower() : (string) null;
      this.BuildProgressToPlaceGeneratedBlocks = cubeBlockDefinition.BuildProgressToPlaceGeneratedBlocks;
      this.GeneratedBlockType = MyStringId.GetOrCompute(cubeBlockDefinition.GeneratedBlockType != null ? cubeBlockDefinition.GeneratedBlockType.ToLower() : (string) null);
      this.CompoundEnabled = cubeBlockDefinition.CompoundEnabled;
      this.CreateFracturedPieces = cubeBlockDefinition.CreateFracturedPieces;
      this.EmissiveColorPreset = cubeBlockDefinition.EmissiveColorPreset != null ? MyStringHash.GetOrCompute(cubeBlockDefinition.EmissiveColorPreset) : MyStringHash.NullOrEmpty;
      this.VoxelPlacement = cubeBlockDefinition.VoxelPlacement;
      this.GeneralDamageMultiplier = cubeBlockDefinition.GeneralDamageMultiplier;
      if (cubeBlockDefinition.PhysicalMaterial != null)
        this.PhysicalMaterial = MyDefinitionManager.Static.GetPhysicalMaterialDefinition(cubeBlockDefinition.PhysicalMaterial);
      if (cubeBlockDefinition.Effects != null)
      {
        this.Effects = new CubeBlockEffectBase[cubeBlockDefinition.Effects.Length];
        for (int index1 = 0; index1 < cubeBlockDefinition.Effects.Length; ++index1)
        {
          this.Effects[index1] = new CubeBlockEffectBase(cubeBlockDefinition.Effects[index1].Name, cubeBlockDefinition.Effects[index1].ParameterMin, cubeBlockDefinition.Effects[index1].ParameterMax);
          if (cubeBlockDefinition.Effects[index1].ParticleEffects != null && cubeBlockDefinition.Effects[index1].ParticleEffects.Length != 0)
          {
            this.Effects[index1].ParticleEffects = new CubeBlockEffect[cubeBlockDefinition.Effects[index1].ParticleEffects.Length];
            for (int index2 = 0; index2 < cubeBlockDefinition.Effects[index1].ParticleEffects.Length; ++index2)
              this.Effects[index1].ParticleEffects[index2] = new CubeBlockEffect(cubeBlockDefinition.Effects[index1].ParticleEffects[index2]);
          }
          else
            this.Effects[index1].ParticleEffects = (CubeBlockEffect[]) null;
        }
      }
      if (cubeBlockDefinition.DamageEffectId != 0)
        this.DamageEffectID = new int?(cubeBlockDefinition.DamageEffectId);
      if (!string.IsNullOrEmpty(cubeBlockDefinition.DamageEffectName))
        this.DamageEffectName = cubeBlockDefinition.DamageEffectName;
      if (cubeBlockDefinition.DestroyEffect != null && cubeBlockDefinition.DestroyEffect.Length > 0)
        this.DestroyEffect = cubeBlockDefinition.DestroyEffect;
      if (cubeBlockDefinition.DestroyEffectOffset.HasValue)
        this.DestroyEffectOffset = new Vector3?(cubeBlockDefinition.DestroyEffectOffset.Value);
      if (cubeBlockDefinition.DepressurizationEffectOffset.HasValue)
        this.DepressurizationEffectOffset = new Vector3?((Vector3) cubeBlockDefinition.DepressurizationEffectOffset.Value);
      this.InitEntityComponents(cubeBlockDefinition.EntityComponents);
      this.CompoundTemplates = cubeBlockDefinition.CompoundTemplates;
      if (cubeBlockDefinition.SubBlockDefinitions != null)
      {
        this.SubBlockDefinitions = new Dictionary<string, MyDefinitionId>();
        foreach (MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition subBlockDefinition in cubeBlockDefinition.SubBlockDefinitions)
        {
          MyDefinitionId id;
          if (!this.SubBlockDefinitions.TryGetValue(subBlockDefinition.SubBlock, out id))
          {
            id = (MyDefinitionId) subBlockDefinition.Id;
            this.SubBlockDefinitions.Add(subBlockDefinition.SubBlock, id);
          }
        }
      }
      if (cubeBlockDefinition.BlockVariants != null)
      {
        MyLog.Default.Warning("BlockVariants are obsolete. Use block groups instead for block: " + (object) this.Id);
        this.BlockStages = new MyDefinitionId[cubeBlockDefinition.BlockVariants.Length];
        for (int index = 0; index < cubeBlockDefinition.BlockVariants.Length; ++index)
          this.BlockStages[index] = (MyDefinitionId) cubeBlockDefinition.BlockVariants[index];
      }
      MyObjectBuilder_CubeBlockDefinition.PatternDefinition cubeDefinition = cubeBlockDefinition.CubeDefinition;
      if (cubeDefinition != null)
      {
        MyCubeDefinition myCubeDefinition = new MyCubeDefinition();
        myCubeDefinition.CubeTopology = cubeDefinition.CubeTopology;
        myCubeDefinition.ShowEdges = cubeDefinition.ShowEdges;
        MyObjectBuilder_CubeBlockDefinition.Side[] sides = cubeDefinition.Sides;
        myCubeDefinition.Model = new string[sides.Length];
        myCubeDefinition.PatternSize = new Vector2I[sides.Length];
        myCubeDefinition.ScaleTile = new Vector2I[sides.Length];
        for (int index = 0; index < sides.Length; ++index)
        {
          MyObjectBuilder_CubeBlockDefinition.Side side = sides[index];
          myCubeDefinition.Model[index] = side.Model;
          myCubeDefinition.PatternSize[index] = (Vector2I) side.PatternSize;
          myCubeDefinition.ScaleTile[index] = new Vector2I(side.ScaleTileU, side.ScaleTileV);
        }
        this.CubeDefinition = myCubeDefinition;
      }
      MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] components = cubeBlockDefinition.Components;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      this.MaxIntegrityRatio = 1f;
      if (components != null && components.Length != 0)
      {
        this.Components = new MyCubeBlockDefinition.Component[components.Length];
        float num4 = 0.0f;
        int num5 = 0;
        for (int index = 0; index < components.Length; ++index)
        {
          MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent cubeBlockComponent = components[index];
          MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(new MyDefinitionId(cubeBlockComponent.Type, cubeBlockComponent.Subtype));
          MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
          if (!cubeBlockComponent.DeconstructId.IsNull() && !MyDefinitionManager.Static.TryGetPhysicalItemDefinition((MyDefinitionId) cubeBlockComponent.DeconstructId, out definition))
            definition = (MyPhysicalItemDefinition) componentDefinition;
          if (definition == null)
            definition = (MyPhysicalItemDefinition) componentDefinition;
          MyCubeBlockDefinition.Component component = new MyCubeBlockDefinition.Component()
          {
            Definition = componentDefinition,
            Count = (int) cubeBlockComponent.Count,
            DeconstructItem = definition
          };
          if (cubeBlockComponent.Type == typeof (MyObjectBuilder_Component) && cubeBlockComponent.Subtype == "Computer" && (double) num3 == 0.0)
            num3 = num4 + (float) component.Definition.MaxIntegrity;
          num4 += (float) (component.Count * component.Definition.MaxIntegrity);
          if (cubeBlockComponent.Type == cubeBlockDefinition.CriticalComponent.Type && cubeBlockComponent.Subtype == cubeBlockDefinition.CriticalComponent.Subtype)
          {
            if (num5 == cubeBlockDefinition.CriticalComponent.Index)
            {
              this.CriticalGroup = (ushort) index;
              num2 = num4 - (float) component.Definition.MaxIntegrity;
            }
            ++num5;
          }
          num1 += (float) component.Count * component.Definition.Mass;
          this.Components[index] = component;
        }
        this.MaxIntegrity = num4;
        this.IntegrityPointsPerSec = this.MaxIntegrity / cubeBlockDefinition.BuildTimeSeconds;
        this.DisassembleRatio = cubeBlockDefinition.DisassembleRatio;
        if (cubeBlockDefinition.MaxIntegrity != 0)
        {
          this.MaxIntegrityRatio = (float) cubeBlockDefinition.MaxIntegrity / this.MaxIntegrity;
          this.DeformationRatio /= this.MaxIntegrityRatio;
        }
        if (!MyPerGameSettings.Destruction)
          this.Mass = num1;
      }
      else if (cubeBlockDefinition.MaxIntegrity != 0)
        this.MaxIntegrity = (float) cubeBlockDefinition.MaxIntegrity;
      this.CriticalIntegrityRatio = num2 / this.MaxIntegrity;
      this.OwnershipIntegrityRatio = num3 / this.MaxIntegrity;
      if (cubeBlockDefinition.BuildProgressModels != null)
      {
        cubeBlockDefinition.BuildProgressModels.Sort((Comparison<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>) ((a, b) => a.BuildPercentUpperBound.CompareTo(b.BuildPercentUpperBound)));
        this.BuildProgressModels = new MyCubeBlockDefinition.BuildProgressModel[cubeBlockDefinition.BuildProgressModels.Count];
        for (int index = 0; index < this.BuildProgressModels.Length; ++index)
        {
          MyObjectBuilder_CubeBlockDefinition.BuildProgressModel buildProgressModel = cubeBlockDefinition.BuildProgressModels[index];
          if (!string.IsNullOrEmpty(buildProgressModel.File))
            this.BuildProgressModels[index] = new MyCubeBlockDefinition.BuildProgressModel()
            {
              BuildRatioUpperBound = (double) this.CriticalIntegrityRatio > 0.0 ? buildProgressModel.BuildPercentUpperBound * this.CriticalIntegrityRatio : buildProgressModel.BuildPercentUpperBound,
              File = buildProgressModel.File,
              RandomOrientation = buildProgressModel.RandomOrientation
            };
        }
      }
      if (cubeBlockDefinition.GeneratedBlocks != null)
      {
        this.GeneratedBlockDefinitions = new MyDefinitionId[cubeBlockDefinition.GeneratedBlocks.Length];
        for (int index = 0; index < cubeBlockDefinition.GeneratedBlocks.Length; ++index)
        {
          SerializableDefinitionId generatedBlock = cubeBlockDefinition.GeneratedBlocks[index];
          this.GeneratedBlockDefinitions[index] = (MyDefinitionId) generatedBlock;
        }
      }
      this.Skeleton = cubeBlockDefinition.Skeleton;
      if (this.Skeleton != null)
      {
        this.Bones = new Dictionary<Vector3I, Vector3>(cubeBlockDefinition.Skeleton.Count);
        foreach (VRage.Game.BoneInfo boneInfo in this.Skeleton)
          this.Bones[(Vector3I) boneInfo.BonePosition] = Vector3UByte.Denormalize((Vector3UByte) boneInfo.BoneOffset, MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize));
      }
      this.IsAirTight = cubeBlockDefinition.IsAirTight;
      this.IsStandAlone = cubeBlockDefinition.IsStandAlone;
      this.HasPhysics = cubeBlockDefinition.HasPhysics;
      this.UseNeighbourOxygenRooms = cubeBlockDefinition.UseNeighbourOxygenRooms;
      this.InitMountPoints(cubeBlockDefinition);
      this.InitPressurization();
      this.InitNavigationInfo(cubeBlockDefinition, cubeBlockDefinition.NavigationDefinition);
      this.PrimarySound = new MySoundPair(cubeBlockDefinition.PrimarySound);
      this.ActionSound = new MySoundPair(cubeBlockDefinition.ActionSound);
      if (!string.IsNullOrEmpty(cubeBlockDefinition.DamagedSound))
        this.DamagedSound = new MySoundPair(cubeBlockDefinition.DamagedSound);
      if (!string.IsNullOrEmpty(cubeBlockDefinition.DestroySound))
        this.DestroySound = new MySoundPair(cubeBlockDefinition.DestroySound);
      this.PCU = (MySession.Static == null || !MySession.Static.Settings.UseConsolePCU ? new int?() : cubeBlockDefinition.PCUConsole) ?? cubeBlockDefinition.PCU;
      this.PlaceDecals = cubeBlockDefinition.PlaceDecals;
      foreach (uint tieredUpdateTime in (List<uint>) cubeBlockDefinition.TieredUpdateTimes)
        this.TieredUpdateTimes.Add(tieredUpdateTime);
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_CubeBlockDefinition objectBuilder = (MyObjectBuilder_CubeBlockDefinition) base.GetObjectBuilder();
      objectBuilder.Size = (SerializableVector3I) this.Size;
      objectBuilder.Model = this.Model;
      objectBuilder.UseModelIntersection = this.UseModelIntersection;
      objectBuilder.CubeSize = this.CubeSize;
      objectBuilder.SilenceableByShipSoundSystem = this.SilenceableByShipSoundSystem;
      objectBuilder.ModelOffset = (SerializableVector3) this.ModelOffset;
      objectBuilder.BlockTopology = this.BlockTopology;
      objectBuilder.PhysicsOption = this.PhysicsOption;
      objectBuilder.BlockPairName = this.BlockPairName;
      objectBuilder.Center = new SerializableVector3I?((SerializableVector3I) this.m_center);
      objectBuilder.MirroringX = this.m_symmetryX;
      objectBuilder.MirroringY = this.m_symmetryY;
      objectBuilder.MirroringZ = this.m_symmetryZ;
      objectBuilder.UsesDeformation = this.UsesDeformation;
      objectBuilder.DeformationRatio = this.DeformationRatio;
      objectBuilder.EdgeType = this.EdgeType;
      objectBuilder.AutorotateMode = this.AutorotateMode;
      objectBuilder.MirroringBlock = this.m_mirroringBlock;
      objectBuilder.MultiBlock = this.MultiBlock;
      objectBuilder.GuiVisible = this.GuiVisible;
      objectBuilder.Rotation = this.Rotation;
      objectBuilder.Direction = this.Direction;
      objectBuilder.Mirrored = this.Mirrored;
      objectBuilder.BuildType = this.BuildType.ToString();
      objectBuilder.BuildMaterial = this.BuildMaterial;
      objectBuilder.GeneratedBlockType = this.GeneratedBlockType.ToString();
      objectBuilder.DamageEffectName = this.DamageEffectName;
      objectBuilder.DestroyEffect = this.DestroyEffect.Length > 0 ? this.DestroyEffect : "";
      objectBuilder.DestroyEffectOffset = this.DestroyEffectOffset;
      objectBuilder.Icons = this.Icons;
      objectBuilder.VoxelPlacement = this.VoxelPlacement;
      objectBuilder.GeneralDamageMultiplier = this.GeneralDamageMultiplier;
      if (this.PhysicalMaterial != null)
        objectBuilder.PhysicalMaterial = this.PhysicalMaterial.Id.SubtypeName;
      objectBuilder.CompoundEnabled = this.CompoundEnabled;
      objectBuilder.PCU = this.PCU;
      throw new NotSupportedException("ConsolePCU");
    }

    public bool RatioEnoughForOwnership(float ratio) => (double) ratio >= (double) this.OwnershipIntegrityRatio;

    public bool RatioEnoughForDamageEffect(float ratio) => (double) ratio < (double) this.CriticalIntegrityRatio;

    public bool ModelChangeIsNeeded(float was, float now)
    {
      if ((double) was == (double) now)
        return false;
      if ((double) now == 0.0 || (double) now >= 1.0)
        return true;
      return this.BuildProgressModels != null && this.GetBuildProgressModelIndex(was) != this.GetBuildProgressModelIndex(now);
    }

    public int GetBuildProgressModelIndex(float percentageA)
    {
      int index = 0;
      while (index < this.BuildProgressModels.Length && (double) percentageA > (double) this.BuildProgressModels[index].BuildRatioUpperBound)
        ++index;
      return index;
    }

    public float FinalModelThreshold() => this.BuildProgressModels == null || this.BuildProgressModels.Length == 0 ? 0.0f : this.BuildProgressModels[this.BuildProgressModels.Length - 1].BuildRatioUpperBound;

    [Conditional("DEBUG")]
    private void CheckBuildProgressModels()
    {
      if (this.BuildProgressModels == null)
        return;
      foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in this.BuildProgressModels)
      {
        if (buildProgressModel != null)
        {
          string file = buildProgressModel.File;
          if (!Path.IsPathRooted(file))
            Path.Combine(MyFileSystem.ContentPath, file);
        }
      }
    }

    internal static void TransformMountPointPosition(
      ref Vector3 position,
      int wallIndex,
      Vector3I cubeSize,
      out Vector3 result)
    {
      Vector3.Transform(ref position, ref MyCubeBlockDefinition.m_mountPointTransforms[wallIndex], out result);
      result += MyCubeBlockDefinition.m_mountPointWallOffsets[wallIndex] * cubeSize;
    }

    internal static void UntransformMountPointPosition(
      ref Vector3 position,
      int wallIndex,
      Vector3I cubeSize,
      out Vector3 result)
    {
      Vector3 position1 = position - MyCubeBlockDefinition.m_mountPointWallOffsets[wallIndex] * cubeSize;
      Matrix matrix = Matrix.Invert(MyCubeBlockDefinition.m_mountPointTransforms[wallIndex]);
      Vector3.Transform(ref position1, ref matrix, out result);
    }

    public static int GetMountPointWallIndex(Base6Directions.Direction direction) => MyCubeBlockDefinition.m_mountPointWallIndices[(int) direction];

    public Vector3 MountPointLocalToBlockLocal(
      Vector3 coord,
      Base6Directions.Direction mountPointDirection)
    {
      Vector3 result = new Vector3();
      int mountPointWallIndex = MyCubeBlockDefinition.m_mountPointWallIndices[(int) mountPointDirection];
      MyCubeBlockDefinition.TransformMountPointPosition(ref coord, mountPointWallIndex, this.Size, out result);
      return result - (Vector3) this.Center;
    }

    public Vector3 MountPointLocalNormalToBlockLocal(
      Vector3 normal,
      Base6Directions.Direction mountPointDirection)
    {
      Vector3 result = new Vector3();
      int mountPointWallIndex = MyCubeBlockDefinition.m_mountPointWallIndices[(int) mountPointDirection];
      Vector3.TransformNormal(ref normal, ref MyCubeBlockDefinition.m_mountPointTransforms[mountPointWallIndex], out result);
      return result;
    }

    public static BlockSideEnum NormalToBlockSide(Vector3I normal)
    {
      for (int index = 0; index < MyCubeBlockDefinition.m_mountPointTransforms.Length; ++index)
      {
        Vector3I vector3I = new Vector3I(MyCubeBlockDefinition.m_mountPointTransforms[index].Forward);
        if (normal == vector3I)
          return (BlockSideEnum) index;
      }
      return BlockSideEnum.Right;
    }

    private void InitMountPoints(MyObjectBuilder_CubeBlockDefinition def)
    {
      if (this.MountPoints != null)
        return;
      Vector3I vector3I = (this.Size - 1) / 2;
      if (!this.Context.IsBaseGame && def.MountPoints != null && def.MountPoints.Length == 0)
      {
        def.MountPoints = (MyObjectBuilder_CubeBlockDefinition.MountPoint[]) null;
        MyDefinitionErrors.Add(this.Context, "Obsolete default definition of mount points in " + (object) def.Id, TErrorSeverity.Warning);
      }
      if (def.MountPoints == null)
      {
        List<MyCubeBlockDefinition.MountPoint> mountPointList1 = new List<MyCubeBlockDefinition.MountPoint>(6);
        Vector3I result1;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[0], out result1);
        Vector3I result2;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[1], out result2);
        Vector3I result3;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[2], out result3);
        Vector3I result4;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[3], out result4);
        Vector3I result5;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[4], out result5);
        Vector3I result6;
        Vector3I.TransformNormal(ref Vector3I.Forward, ref MyCubeBlockDefinition.m_mountPointTransforms[5], out result6);
        Vector3 position1 = new Vector3(1f / 1000f, 1f / 1000f, 0.0004f);
        Vector3 position2 = new Vector3((float) this.Size.Z - 1f / 1000f, (float) this.Size.Y - 1f / 1000f, -0.0004f);
        Vector3 result7;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position1, 0, this.Size, out result7);
        Vector3 result8;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position2, 0, this.Size, out result8);
        Vector3 result9;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position1, 3, this.Size, out result9);
        Vector3 result10;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position2, 3, this.Size, out result10);
        List<MyCubeBlockDefinition.MountPoint> mountPointList2 = mountPointList1;
        MyCubeBlockDefinition.MountPoint mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result7;
        mountPoint1.End = result8;
        mountPoint1.Normal = result1;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint2 = mountPoint1;
        mountPointList2.Add(mountPoint2);
        List<MyCubeBlockDefinition.MountPoint> mountPointList3 = mountPointList1;
        mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result9;
        mountPoint1.End = result10;
        mountPoint1.Normal = result4;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint3 = mountPoint1;
        mountPointList3.Add(mountPoint3);
        Vector3 position3 = new Vector3(1f / 1000f, 1f / 1000f, 0.0004f);
        Vector3 position4 = new Vector3((float) this.Size.X - 1f / 1000f, (float) this.Size.Z - 1f / 1000f, -0.0004f);
        Vector3 result11;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position3, 1, this.Size, out result11);
        Vector3 result12;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position4, 1, this.Size, out result12);
        Vector3 result13;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position3, 4, this.Size, out result13);
        Vector3 result14;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position4, 4, this.Size, out result14);
        List<MyCubeBlockDefinition.MountPoint> mountPointList4 = mountPointList1;
        mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result11;
        mountPoint1.End = result12;
        mountPoint1.Normal = result2;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint4 = mountPoint1;
        mountPointList4.Add(mountPoint4);
        List<MyCubeBlockDefinition.MountPoint> mountPointList5 = mountPointList1;
        mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result13;
        mountPoint1.End = result14;
        mountPoint1.Normal = result5;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint5 = mountPoint1;
        mountPointList5.Add(mountPoint5);
        Vector3 position5 = new Vector3(1f / 1000f, 1f / 1000f, 0.0004f);
        Vector3 position6 = new Vector3((float) this.Size.X - 1f / 1000f, (float) this.Size.Y - 1f / 1000f, -0.0004f);
        Vector3 result15;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position5, 2, this.Size, out result15);
        Vector3 result16;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position6, 2, this.Size, out result16);
        Vector3 result17;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position5, 5, this.Size, out result17);
        Vector3 result18;
        MyCubeBlockDefinition.TransformMountPointPosition(ref position6, 5, this.Size, out result18);
        List<MyCubeBlockDefinition.MountPoint> mountPointList6 = mountPointList1;
        mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result15;
        mountPoint1.End = result16;
        mountPoint1.Normal = result3;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint6 = mountPoint1;
        mountPointList6.Add(mountPoint6);
        List<MyCubeBlockDefinition.MountPoint> mountPointList7 = mountPointList1;
        mountPoint1 = new MyCubeBlockDefinition.MountPoint();
        mountPoint1.Start = result17;
        mountPoint1.End = result18;
        mountPoint1.Normal = result6;
        mountPoint1.Enabled = true;
        mountPoint1.PressurizedWhenOpen = true;
        MyCubeBlockDefinition.MountPoint mountPoint7 = mountPoint1;
        mountPointList7.Add(mountPoint7);
        this.MountPoints = mountPointList1.ToArray();
      }
      else
      {
        this.SetMountPoints(ref this.MountPoints, def.MountPoints, MyCubeBlockDefinition.m_tmpMounts);
        if (def.BuildProgressModels != null)
        {
          for (int index = 0; index < def.BuildProgressModels.Count; ++index)
          {
            MyCubeBlockDefinition.BuildProgressModel buildProgressModel1 = this.BuildProgressModels[index];
            if (buildProgressModel1 != null)
            {
              MyObjectBuilder_CubeBlockDefinition.BuildProgressModel buildProgressModel2 = def.BuildProgressModels[index];
              if (buildProgressModel2.MountPoints != null)
              {
                foreach (MyObjectBuilder_CubeBlockDefinition.MountPoint mountPoint in buildProgressModel2.MountPoints)
                {
                  int sideId = (int) mountPoint.Side;
                  if (!MyCubeBlockDefinition.m_tmpIndices.Contains(sideId))
                  {
                    MyCubeBlockDefinition.m_tmpMounts.RemoveAll((Predicate<MyObjectBuilder_CubeBlockDefinition.MountPoint>) (mount => mount.Side == (BlockSideEnum) sideId));
                    MyCubeBlockDefinition.m_tmpIndices.Add(sideId);
                  }
                  MyCubeBlockDefinition.m_tmpMounts.Add(mountPoint);
                }
                MyCubeBlockDefinition.m_tmpIndices.Clear();
                buildProgressModel1.MountPoints = new MyCubeBlockDefinition.MountPoint[MyCubeBlockDefinition.m_tmpMounts.Count];
                this.SetMountPoints(ref buildProgressModel1.MountPoints, MyCubeBlockDefinition.m_tmpMounts.ToArray(), (List<MyObjectBuilder_CubeBlockDefinition.MountPoint>) null);
              }
            }
          }
        }
        MyCubeBlockDefinition.m_tmpMounts.Clear();
      }
    }

    private void SetMountPoints(
      ref MyCubeBlockDefinition.MountPoint[] mountPoints,
      MyObjectBuilder_CubeBlockDefinition.MountPoint[] mpBuilders,
      List<MyObjectBuilder_CubeBlockDefinition.MountPoint> addedMounts)
    {
      if (mountPoints == null)
        mountPoints = new MyCubeBlockDefinition.MountPoint[mpBuilders.Length];
      for (int index = 0; index < mountPoints.Length; ++index)
      {
        MyObjectBuilder_CubeBlockDefinition.MountPoint mpBuilder = mpBuilders[index];
        addedMounts?.Add(mpBuilder);
        Vector3 result1 = new Vector3(Vector2.Min((Vector2) mpBuilder.Start, (Vector2) mpBuilder.End) + 1f / 1000f, 0.0004f);
        Vector3 result2 = new Vector3(Vector2.Max((Vector2) mpBuilder.Start, (Vector2) mpBuilder.End) - 1f / 1000f, -0.0004f);
        int side = (int) mpBuilder.Side;
        Vector3I result3 = Vector3I.Forward;
        MyCubeBlockDefinition.TransformMountPointPosition(ref result1, side, this.Size, out result1);
        MyCubeBlockDefinition.TransformMountPointPosition(ref result2, side, this.Size, out result2);
        Vector3I.TransformNormal(ref result3, ref MyCubeBlockDefinition.m_mountPointTransforms[side], out result3);
        mountPoints[index].Start = result1;
        mountPoints[index].End = result2;
        mountPoints[index].Normal = result3;
        mountPoints[index].ExclusionMask = mpBuilder.ExclusionMask;
        mountPoints[index].PropertiesMask = mpBuilder.PropertiesMask;
        mountPoints[index].Enabled = mpBuilder.Enabled;
        mountPoints[index].PressurizedWhenOpen = mpBuilder.PressurizedWhenOpen;
        mountPoints[index].Default = mpBuilder.Default;
      }
    }

    public MyCubeBlockDefinition.MountPoint[] GetBuildProgressModelMountPoints(
      float currentIntegrityRatio)
    {
      if (this.BuildProgressModels == null || this.BuildProgressModels.Length == 0 || (double) currentIntegrityRatio >= (double) this.BuildProgressModels[this.BuildProgressModels.Length - 1].BuildRatioUpperBound)
        return this.MountPoints;
      int index;
      for (index = 0; index < this.BuildProgressModels.Length - 1; ++index)
      {
        MyCubeBlockDefinition.BuildProgressModel buildProgressModel = this.BuildProgressModels[index];
        if ((double) currentIntegrityRatio <= (double) buildProgressModel.BuildRatioUpperBound)
          break;
      }
      return this.BuildProgressModels[index].MountPoints ?? this.MountPoints;
    }

    public void InitPressurization()
    {
      this.IsCubePressurized = new Dictionary<Vector3I, Dictionary<Vector3I, MyCubeBlockDefinition.MyCubePressurizationMark>>();
      for (int x = 0; x < this.Size.X; ++x)
      {
        for (int y = 0; y < this.Size.Y; ++y)
        {
          for (int z = 0; z < this.Size.Z; ++z)
          {
            Vector3 position1 = new Vector3((float) x, (float) y, (float) z);
            Vector3 position2 = new Vector3((float) x, (float) y, (float) z) + Vector3.One;
            Vector3I key = new Vector3I(x, y, z);
            this.IsCubePressurized[key] = new Dictionary<Vector3I, MyCubeBlockDefinition.MyCubePressurizationMark>();
            foreach (Vector3I intDirection in Base6Directions.IntDirections)
            {
              this.IsCubePressurized[key][intDirection] = MyCubeBlockDefinition.MyCubePressurizationMark.NotPressurized;
              if ((intDirection.X != 1 || x == this.Size.X - 1) && (intDirection.X != -1 || x == 0) && ((intDirection.Y != 1 || y == this.Size.Y - 1) && (intDirection.Y != -1 || y == 0)) && ((intDirection.Z != 1 || z == this.Size.Z - 1) && (intDirection.Z != -1 || z == 0)))
              {
                foreach (MyCubeBlockDefinition.MountPoint mountPoint in this.MountPoints)
                {
                  if (intDirection == mountPoint.Normal)
                  {
                    int mountPointWallIndex = MyCubeBlockDefinition.GetMountPointWallIndex(Base6Directions.GetDirection(ref intDirection));
                    Vector3I size = this.Size;
                    Vector3 start = mountPoint.Start;
                    Vector3 end = mountPoint.End;
                    Vector3 result1;
                    MyCubeBlockDefinition.UntransformMountPointPosition(ref start, mountPointWallIndex, size, out result1);
                    Vector3 result2;
                    MyCubeBlockDefinition.UntransformMountPointPosition(ref end, mountPointWallIndex, size, out result2);
                    Vector3 result3;
                    MyCubeBlockDefinition.UntransformMountPointPosition(ref position1, mountPointWallIndex, size, out result3);
                    Vector3 result4;
                    MyCubeBlockDefinition.UntransformMountPointPosition(ref position2, mountPointWallIndex, size, out result4);
                    Vector3 vector3_1 = new Vector3(Math.Max(result3.X, result4.X), Math.Max(result3.Y, result4.Y), Math.Max(result3.Z, result4.Z));
                    Vector3 vector3_2 = new Vector3(Math.Min(result3.X, result4.X), Math.Min(result3.Y, result4.Y), Math.Min(result3.Z, result4.Z));
                    if ((double) result1.X - 0.05 <= (double) vector3_2.X && (double) result2.X + 0.05 > (double) vector3_1.X && ((double) result1.Y - 0.05 <= (double) vector3_2.Y && (double) result2.Y + 0.05 > (double) vector3_1.Y))
                    {
                      this.IsCubePressurized[key][intDirection] = !mountPoint.PressurizedWhenOpen ? MyCubeBlockDefinition.MyCubePressurizationMark.PressurizedClosed : MyCubeBlockDefinition.MyCubePressurizationMark.PressurizedAlways;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    public void InitNavigationInfo(
      MyObjectBuilder_CubeBlockDefinition blockDef,
      string infoSubtypeId)
    {
      if (!MyPerGameSettings.EnableAi)
        return;
      if (infoSubtypeId == "Default")
        MyDefinitionManager.Static.SetDefaultNavDef(this);
      else
        MyDefinitionManager.Static.TryGetDefinition<MyBlockNavigationDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlockNavigationDefinition), infoSubtypeId), out this.NavigationDefinition);
      if (this.NavigationDefinition == null || this.NavigationDefinition.Mesh == null)
        return;
      this.NavigationDefinition.Mesh.MakeStatic();
    }

    private void InitEntityComponents(
      MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] entityComponentDefinitions)
    {
      if (entityComponentDefinitions == null)
        return;
      this.EntityComponents = new Dictionary<string, MyObjectBuilder_ComponentBase>(entityComponentDefinitions.Length);
      for (int index = 0; index < entityComponentDefinitions.Length; ++index)
      {
        MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition componentDefinition = entityComponentDefinitions[index];
        MyObjectBuilderType type = MyObjectBuilderType.Parse(componentDefinition.BuilderType);
        if (!type.IsNull && MyObjectBuilderSerializer.CreateNewObject(type) is MyObjectBuilder_ComponentBase newObject)
          this.EntityComponents.Add(componentDefinition.ComponentType, newObject);
      }
    }

    public bool ContainsComputer() => ((IEnumerable<MyCubeBlockDefinition.Component>) this.Components).Count<MyCubeBlockDefinition.Component>((Func<MyCubeBlockDefinition.Component, bool>) (x => x.Definition.Id.TypeId == typeof (MyObjectBuilder_Component) && x.Definition.Id.SubtypeName == "Computer")) > 0;

    public MyCubeBlockDefinition GetGeneratedBlockDefinition(
      MyStringId additionalModelType)
    {
      if (this.GeneratedBlockDefinitions == null)
        return (MyCubeBlockDefinition) null;
      foreach (MyDefinitionId generatedBlockDefinition in this.GeneratedBlockDefinitions)
      {
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(generatedBlockDefinition, out blockDefinition);
        if (blockDefinition != null && blockDefinition.IsGeneratedBlock && blockDefinition.GeneratedBlockType == additionalModelType)
          return blockDefinition;
      }
      return (MyCubeBlockDefinition) null;
    }

    public static void PreloadConstructionModels(MyCubeBlockDefinition block)
    {
      if (block == null || MyCubeBlockDefinition.m_preloadedDefinitions.Contains(block))
        return;
      MyCubeBlockDefinition.m_stringList.AssertEmpty<string>();
      for (int index = 0; index < block.BuildProgressModels.Length; ++index)
      {
        MyCubeBlockDefinition.BuildProgressModel buildProgressModel = block.BuildProgressModels[index];
        if (buildProgressModel != null && !string.IsNullOrEmpty(buildProgressModel.File))
          MyCubeBlockDefinition.m_stringList.Add(buildProgressModel.File);
      }
      MyRenderProxy.PreloadModels(MyCubeBlockDefinition.m_stringList, true);
      MyCubeBlockDefinition.m_stringList.Clear();
      MyCubeBlockDefinition.m_preloadedDefinitions.Add(block);
    }

    public static void ClearPreloadedConstructionModels() => MyCubeBlockDefinition.m_preloadedDefinitions.Clear();

    public class Component
    {
      public MyComponentDefinition Definition;
      public int Count;
      public MyPhysicalItemDefinition DeconstructItem;
    }

    public class BuildProgressModel
    {
      public float BuildRatioUpperBound;
      public string File;
      public bool RandomOrientation;
      public MyCubeBlockDefinition.MountPoint[] MountPoints;
      public bool Visible;
    }

    public struct MountPoint
    {
      public Vector3I Normal;
      public Vector3 Start;
      public Vector3 End;
      public byte ExclusionMask;
      public byte PropertiesMask;
      public bool Enabled;
      public bool PressurizedWhenOpen;
      public bool Default;

      public MyObjectBuilder_CubeBlockDefinition.MountPoint GetObjectBuilder(
        Vector3I cubeSize)
      {
        MyObjectBuilder_CubeBlockDefinition.MountPoint mountPoint = new MyObjectBuilder_CubeBlockDefinition.MountPoint();
        mountPoint.Side = MyCubeBlockDefinition.NormalToBlockSide(this.Normal);
        Vector3 result1;
        MyCubeBlockDefinition.UntransformMountPointPosition(ref this.Start, (int) mountPoint.Side, cubeSize, out result1);
        Vector3 result2;
        MyCubeBlockDefinition.UntransformMountPointPosition(ref this.End, (int) mountPoint.Side, cubeSize, out result2);
        mountPoint.Start = new SerializableVector2(result1.X, result1.Y);
        mountPoint.End = new SerializableVector2(result2.X, result2.Y);
        mountPoint.ExclusionMask = this.ExclusionMask;
        mountPoint.PropertiesMask = this.PropertiesMask;
        mountPoint.Enabled = this.Enabled;
        mountPoint.Default = this.Default;
        mountPoint.PressurizedWhenOpen = this.PressurizedWhenOpen;
        return mountPoint;
      }
    }

    public enum MyCubePressurizationMark
    {
      NotPressurized,
      PressurizedAlways,
      PressurizedClosed,
    }

    private class Sandbox_Definitions_MyCubeBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyCubeBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCubeBlockDefinition();

      MyCubeBlockDefinition IActivator<MyCubeBlockDefinition>.CreateInstance() => new MyCubeBlockDefinition();
    }
  }
}
