// Decompiled with JetBrains decompiler
// Type: Medieval.ObjectBuilders.Definitions.MyObjectBuilder_Dx11VoxelMaterialDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Medieval.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Dx11VoxelMaterialDefinition : MyObjectBuilder_VoxelMaterialDefinition
  {
    [ProtoMember(1)]
    [ModdableContentFile("dds")]
    public string ColorMetalXZnY;
    [ProtoMember(4)]
    [ModdableContentFile("dds")]
    public string ColorMetalY;
    [ProtoMember(7)]
    [ModdableContentFile("dds")]
    public string NormalGlossXZnY;
    [ProtoMember(10)]
    [ModdableContentFile("dds")]
    public string NormalGlossY;
    [ProtoMember(13)]
    [ModdableContentFile("dds")]
    public string ExtXZnY;
    [ProtoMember(16)]
    [ModdableContentFile("dds")]
    public string ExtY;
    [ProtoMember(19)]
    [ModdableContentFile("dds")]
    public string ColorMetalXZnYFar1;
    [ProtoMember(22)]
    [ModdableContentFile("dds")]
    public string ColorMetalYFar1;
    [ProtoMember(25)]
    [ModdableContentFile("dds")]
    public string NormalGlossXZnYFar1;
    [ProtoMember(28)]
    [ModdableContentFile("dds")]
    public string NormalGlossYFar1;
    [ProtoMember(31)]
    public float Scale = 8f;
    [ProtoMember(34)]
    public float ScaleFar1 = 8f;
    [ProtoMember(37)]
    [ModdableContentFile("dds")]
    public string ExtXZnYFar1;
    [ProtoMember(40)]
    [ModdableContentFile("dds")]
    public string ExtYFar1;
    [ProtoMember(43)]
    [ModdableContentFile("dds")]
    public string FoliageTextureArray1;
    [ProtoMember(46)]
    [ModdableContentFile("dds")]
    public string FoliageTextureArray2;
    [ProtoMember(49)]
    [ModdableContentFile("dds")]
    [XmlArrayItem("Color")]
    public string[] FoliageColorTextureArray;
    [ProtoMember(52)]
    [ModdableContentFile("dds")]
    [XmlArrayItem("Normal")]
    public string[] FoliageNormalTextureArray;
    [ProtoMember(55)]
    public float FoliageDensity;
    [ProtoMember(58)]
    public Vector2 FoliageScale = Vector2.One;
    [ProtoMember(61)]
    public float FoliageRandomRescaleMult;
    [ProtoMember(64)]
    public int FoliageType;
    [ProtoMember(67)]
    public byte BiomeValueMin;
    [ProtoMember(70)]
    public byte BiomeValueMax;
    [ProtoMember(73)]
    [ModdableContentFile("dds")]
    public string ColorMetalXZnYFar2;
    [ProtoMember(76)]
    [ModdableContentFile("dds")]
    public string ColorMetalYFar2;
    [ProtoMember(79)]
    [ModdableContentFile("dds")]
    public string NormalGlossXZnYFar2;
    [ProtoMember(82)]
    [ModdableContentFile("dds")]
    public string NormalGlossYFar2;
    [ProtoMember(85)]
    [ModdableContentFile("dds")]
    public string ExtXZnYFar2;
    [ProtoMember(88)]
    [ModdableContentFile("dds")]
    public string ExtYFar2;
    [ProtoMember(91)]
    public float InitialScale = 2f;
    [ProtoMember(94)]
    public float ScaleMultiplier = 4f;
    [ProtoMember(97)]
    public float InitialDistance = 5f;
    [ProtoMember(100)]
    public float DistanceMultiplier = 4f;
    [ProtoMember(103)]
    public float TilingScale = 32f;
    [ProtoMember(106)]
    public float Far1Distance;
    [ProtoMember(109)]
    public float Far2Distance;
    [ProtoMember(112)]
    public float Far3Distance;
    [ProtoMember(115)]
    public float Far1Scale = 400f;
    [ProtoMember(118)]
    public float Far2Scale = 2000f;
    [ProtoMember(121)]
    public float Far3Scale = 7000f;
    [ProtoMember(124)]
    public Vector4 Far3Color = (Vector4) Color.Black;
    [ProtoMember(127)]
    public float ExtDetailScale;
    [ProtoMember(130, IsRequired = false)]
    public TilingSetup SimpleTilingSetup;

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalXZnY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalXZnY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalXZnY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossXZnY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossXZnY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossXZnY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtXZnY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtXZnY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtXZnY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtY;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalXZnYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalXZnYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalXZnYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossXZnYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossXZnYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossXZnYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Scale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Scale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EScaleFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.ScaleFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.ScaleFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtXZnYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtXZnYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtXZnYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtYFar1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtYFar1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtYFar1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageTextureArray1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.FoliageTextureArray1 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.FoliageTextureArray1;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageTextureArray2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.FoliageTextureArray2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.FoliageTextureArray2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageColorTextureArray\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string[] value)
      {
        owner.FoliageColorTextureArray = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string[] value)
      {
        value = owner.FoliageColorTextureArray;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageNormalTextureArray\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string[] value)
      {
        owner.FoliageNormalTextureArray = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string[] value)
      {
        value = owner.FoliageNormalTextureArray;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.FoliageDensity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.FoliageDensity;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in Vector2 value)
      {
        owner.FoliageScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out Vector2 value)
      {
        value = owner.FoliageScale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageRandomRescaleMult\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.FoliageRandomRescaleMult = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.FoliageRandomRescaleMult;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFoliageType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in int value)
      {
        owner.FoliageType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out int value)
      {
        value = owner.FoliageType;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EBiomeValueMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in byte value)
      {
        owner.BiomeValueMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out byte value)
      {
        value = owner.BiomeValueMin;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EBiomeValueMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in byte value)
      {
        owner.BiomeValueMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out byte value)
      {
        value = owner.BiomeValueMax;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalXZnYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalXZnYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalXZnYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorMetalYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ColorMetalYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ColorMetalYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossXZnYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossXZnYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossXZnYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ENormalGlossYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.NormalGlossYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.NormalGlossYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtXZnYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtXZnYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtXZnYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtYFar2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        owner.ExtYFar2 = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        value = owner.ExtYFar2;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EInitialScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.InitialScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.InitialScale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EScaleMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.ScaleMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.ScaleMultiplier;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EInitialDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.InitialDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.InitialDistance;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDistanceMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.DistanceMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.DistanceMultiplier;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ETilingScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.TilingScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.TilingScale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar1Distance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far1Distance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far1Distance;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar2Distance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far2Distance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far2Distance;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar3Distance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far3Distance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far3Distance;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar1Scale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far1Scale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far1Scale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar2Scale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far2Scale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far2Scale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar3Scale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.Far3Scale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.Far3Scale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFar3Color\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in Vector4 value)
      {
        owner.Far3Color = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out Vector4 value)
      {
        value = owner.Far3Color;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EExtDetailScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        owner.ExtDetailScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        value = owner.ExtDetailScale;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ESimpleTilingSetup\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, TilingSetup>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in TilingSetup value)
      {
        owner.SimpleTilingSetup = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out TilingSetup value)
      {
        value = owner.SimpleTilingSetup;
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EMaterialTypeName\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMaterialTypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EMinedOre\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinedOre\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EMinedOreRatio\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinedOreRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ECanBeHarvested\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ECanBeHarvested\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EIsRare\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EIsRare\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EUseTwoTextures\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EUseTwoTextures\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EVoxelHandPreview\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EVoxelHandPreview\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EMinVersion\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinVersion\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EMaxVersion\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMaxVersion\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ESpawnsInAsteroids\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ESpawnsInAsteroids\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ESpawnsFromMeteorites\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ESpawnsFromMeteorites\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDamagedMaterial\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDamagedMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EFriction\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EFriction\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ERestitution\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ERestitution\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EColorKey\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EColorKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, ColorDefinitionRGBA?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in ColorDefinitionRGBA? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out ColorDefinitionRGBA? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ELandingEffect\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ELandingEffect\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EAsteroidGeneratorSpawnProbabilityMultiplier\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EAsteroidGeneratorSpawnProbabilityMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EBareVariant\u003C\u003EAccessor : MyObjectBuilder_VoxelMaterialDefinition.VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EBareVariant\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelMaterialDefinition&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Dx11VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Dx11VoxelMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Medieval_ObjectBuilders_Definitions_MyObjectBuilder_Dx11VoxelMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Dx11VoxelMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Dx11VoxelMaterialDefinition();

      MyObjectBuilder_Dx11VoxelMaterialDefinition IActivator<MyObjectBuilder_Dx11VoxelMaterialDefinition>.CreateInstance() => new MyObjectBuilder_Dx11VoxelMaterialDefinition();
    }
  }
}
