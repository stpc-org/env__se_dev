// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_SharedStorageComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SharedStorageComponent : MyObjectBuilder_SessionComponent
  {
    public SerializableDictionary<string, bool> BoolStorage = new SerializableDictionary<string, bool>();
    public SerializableDictionary<string, int> IntStorage = new SerializableDictionary<string, int>();
    public SerializableDictionary<string, long> LongStorage = new SerializableDictionary<string, long>();
    public SerializableDictionary<string, ulong> ULongStorage = new SerializableDictionary<string, ulong>();
    public SerializableDictionary<string, string> StringStorage = new SerializableDictionary<string, string>();
    public SerializableDictionary<string, float> FloatStorage = new SerializableDictionary<string, float>();
    public SerializableDictionary<string, SerializableVector3D> Vector3DStorage = new SerializableDictionary<string, SerializableVector3D>();
    public SerializableDictionary<string, MySerializableList<bool>> BoolListStorage = new SerializableDictionary<string, MySerializableList<bool>>();
    public SerializableDictionary<string, MySerializableList<int>> IntListStorage = new SerializableDictionary<string, MySerializableList<int>>();
    public SerializableDictionary<string, MySerializableList<long>> LongListStorage = new SerializableDictionary<string, MySerializableList<long>>();
    public SerializableDictionary<string, MySerializableList<ulong>> ULongListStorage = new SerializableDictionary<string, MySerializableList<ulong>>();
    public SerializableDictionary<string, MySerializableList<string>> StringListStorage = new SerializableDictionary<string, MySerializableList<string>>();
    public SerializableDictionary<string, MySerializableList<float>> FloatListStorage = new SerializableDictionary<string, MySerializableList<float>>();
    public SerializableDictionary<string, MySerializableList<SerializableVector3D>> Vector3DListStorage = new SerializableDictionary<string, MySerializableList<SerializableVector3D>>();
    public SerializableDictionary<string, SerializableDictionary<string, bool>> BoolStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, bool>>();
    public SerializableDictionary<string, SerializableDictionary<string, int>> IntStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, int>>();
    public SerializableDictionary<string, SerializableDictionary<string, long>> LongStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, long>>();
    public SerializableDictionary<string, SerializableDictionary<string, ulong>> ULongStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, ulong>>();
    public SerializableDictionary<string, SerializableDictionary<string, string>> StringStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, string>>();
    public SerializableDictionary<string, SerializableDictionary<string, float>> FloatStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, float>>();
    public SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> Vector3DStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> BoolListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> IntListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> LongListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> ULongListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> StringListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> FloatListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>>();
    public SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> Vector3DListStorageSecondary = new SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>();

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EBoolStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, bool>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, bool> value)
      {
        owner.BoolStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, bool> value)
      {
        value = owner.BoolStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EIntStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, int> value)
      {
        owner.IntStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, int> value)
      {
        value = owner.IntStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003ELongStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, long> value)
      {
        owner.LongStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, long> value)
      {
        value = owner.LongStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EULongStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, ulong>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, ulong> value)
      {
        owner.ULongStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, ulong> value)
      {
        value = owner.ULongStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EStringStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, string> value)
      {
        owner.StringStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, string> value)
      {
        value = owner.StringStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EFloatStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, float>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, float> value)
      {
        owner.FloatStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, float> value)
      {
        value = owner.FloatStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EVector3DStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableVector3D>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableVector3D> value)
      {
        owner.Vector3DStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableVector3D> value)
      {
        value = owner.Vector3DStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EBoolListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<bool>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<bool>> value)
      {
        owner.BoolListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<bool>> value)
      {
        value = owner.BoolListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EIntListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<int>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<int>> value)
      {
        owner.IntListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<int>> value)
      {
        value = owner.IntListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003ELongListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<long>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<long>> value)
      {
        owner.LongListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<long>> value)
      {
        value = owner.LongListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EULongListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<ulong>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<ulong>> value)
      {
        owner.ULongListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<ulong>> value)
      {
        value = owner.ULongListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EStringListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<string>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<string>> value)
      {
        owner.StringListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<string>> value)
      {
        value = owner.StringListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EFloatListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<float>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<float>> value)
      {
        owner.FloatListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<float>> value)
      {
        value = owner.FloatListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EVector3DListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
      {
        owner.Vector3DListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
      {
        value = owner.Vector3DListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EBoolStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, bool>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, bool>> value)
      {
        owner.BoolStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, bool>> value)
      {
        value = owner.BoolStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EIntStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, int>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, int>> value)
      {
        owner.IntStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, int>> value)
      {
        value = owner.IntStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003ELongStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, long>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, long>> value)
      {
        owner.LongStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, long>> value)
      {
        value = owner.LongStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EULongStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, ulong>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, ulong>> value)
      {
        owner.ULongStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, ulong>> value)
      {
        value = owner.ULongStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EStringStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, string>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, string>> value)
      {
        owner.StringStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, string>> value)
      {
        value = owner.StringStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EFloatStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, float>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, float>> value)
      {
        owner.FloatStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, float>> value)
      {
        value = owner.FloatStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EVector3DStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> value)
      {
        owner.Vector3DStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, SerializableVector3D>> value)
      {
        value = owner.Vector3DStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EBoolListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> value)
      {
        owner.BoolListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<bool>>> value)
      {
        value = owner.BoolListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EIntListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> value)
      {
        owner.IntListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<int>>> value)
      {
        value = owner.IntListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003ELongListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> value)
      {
        owner.LongListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<long>>> value)
      {
        value = owner.LongListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EULongListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> value)
      {
        owner.ULongListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<ulong>>> value)
      {
        value = owner.ULongListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EStringListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> value)
      {
        owner.StringListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<string>>> value)
      {
        value = owner.StringListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EFloatListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> value)
      {
        owner.FloatListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<float>>> value)
      {
        value = owner.FloatListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EVector3DListStorageSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> value)
      {
        owner.Vector3DListStorageSecondary = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDictionary<string, SerializableDictionary<string, MySerializableList<SerializableVector3D>>> value)
      {
        value = owner.Vector3DListStorageSecondary;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SharedStorageComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SharedStorageComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SharedStorageComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SharedStorageComponent();

      MyObjectBuilder_SharedStorageComponent IActivator<MyObjectBuilder_SharedStorageComponent>.CreateInstance() => new MyObjectBuilder_SharedStorageComponent();
    }
  }
}
