// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyStorageBaseCompatibility
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  internal class MyStorageBaseCompatibility
  {
    private const int MAX_ENCODED_NAME_LENGTH = 256;
    private readonly byte[] m_encodedNameBuffer = new byte[256];

    public MyStorageBase Compatibility_LoadCellStorage(int fileVersion, Stream stream)
    {
      Vector3I size;
      size.X = stream.ReadInt32();
      size.Y = stream.ReadInt32();
      size.Z = stream.ReadInt32();
      MyOctreeStorage myOctreeStorage = new MyOctreeStorage((IMyStorageDataProvider) null, size);
      Vector3I vector3I1;
      vector3I1.X = stream.ReadInt32();
      vector3I1.Y = stream.ReadInt32();
      vector3I1.Z = stream.ReadInt32();
      Vector3I vector3I2 = size / vector3I1;
      Dictionary<byte, MyVoxelMaterialDefinition> mapping = (Dictionary<byte, MyVoxelMaterialDefinition>) null;
      if (fileVersion == 2)
        mapping = this.Compatibility_LoadMaterialIndexMapping(stream);
      Vector3I zero = Vector3I.Zero;
      Vector3I end = new Vector3I(7);
      MyStorageData source = new MyStorageData();
      source.Resize(Vector3I.Zero, end);
      Vector3I vector3I3;
      for (vector3I3.X = 0; vector3I3.X < vector3I2.X; ++vector3I3.X)
      {
        for (vector3I3.Y = 0; vector3I3.Y < vector3I2.Y; ++vector3I3.Y)
        {
          for (vector3I3.Z = 0; vector3I3.Z < vector3I2.Z; ++vector3I3.Z)
          {
            switch (stream.ReadByteNoAlloc())
            {
              case 0:
                source.ClearContent((byte) 0);
                break;
              case 1:
                source.ClearContent(byte.MaxValue);
                break;
              case 2:
                Vector3I p;
                for (p.X = 0; p.X < 8; ++p.X)
                {
                  for (p.Y = 0; p.Y < 8; ++p.Y)
                  {
                    for (p.Z = 0; p.Z < 8; ++p.Z)
                      source.Content(ref p, stream.ReadByteNoAlloc());
                  }
                }
                break;
            }
            Vector3I voxelRangeMin = vector3I3 * 8;
            Vector3I voxelRangeMax = voxelRangeMin + 7;
            myOctreeStorage.WriteRange(source, MyStorageDataTypeFlags.Content, voxelRangeMin, voxelRangeMax, true, false);
          }
        }
      }
      try
      {
        for (vector3I3.X = 0; vector3I3.X < vector3I2.X; ++vector3I3.X)
        {
          for (vector3I3.Y = 0; vector3I3.Y < vector3I2.Y; ++vector3I3.Y)
          {
            for (vector3I3.Z = 0; vector3I3.Z < vector3I2.Z; ++vector3I3.Z)
            {
              if ((stream.ReadByteNoAlloc() == (byte) 1 ? 1 : 0) != 0)
              {
                MyVoxelMaterialDefinition materialDefinition = this.Compatibility_LoadCellVoxelMaterial(stream, mapping);
                source.ClearMaterials(materialDefinition.Index);
              }
              else
              {
                Vector3I p;
                for (p.X = 0; p.X < 8; ++p.X)
                {
                  for (p.Y = 0; p.Y < 8; ++p.Y)
                  {
                    for (p.Z = 0; p.Z < 8; ++p.Z)
                    {
                      MyVoxelMaterialDefinition materialDefinition = this.Compatibility_LoadCellVoxelMaterial(stream, mapping);
                      int num = (int) stream.ReadByteNoAlloc();
                      source.Material(ref p, materialDefinition.Index);
                    }
                  }
                }
              }
              Vector3I voxelRangeMin = vector3I3 * 8;
              Vector3I voxelRangeMax = voxelRangeMin + 7;
              myOctreeStorage.WriteRange(source, MyStorageDataTypeFlags.Material, voxelRangeMin, voxelRangeMax, true, false);
            }
          }
        }
      }
      catch (EndOfStreamException ex)
      {
        MySandboxGame.Log.WriteLine((Exception) ex);
      }
      return (MyStorageBase) myOctreeStorage;
    }

    private MyVoxelMaterialDefinition Compatibility_LoadCellVoxelMaterial(
      Stream stream,
      Dictionary<byte, MyVoxelMaterialDefinition> mapping)
    {
      byte num1 = stream.ReadByteNoAlloc();
      MyVoxelMaterialDefinition materialDefinition;
      if (num1 != byte.MaxValue)
      {
        materialDefinition = mapping == null ? MyDefinitionManager.Static.GetVoxelMaterialDefinition(num1) : mapping[num1];
      }
      else
      {
        Encoding utF8 = Encoding.UTF8;
        byte num2 = stream.ReadByteNoAlloc();
        stream.Read(this.m_encodedNameBuffer, 0, (int) num2);
        byte[] encodedNameBuffer = this.m_encodedNameBuffer;
        int count = (int) num2;
        materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(utF8.GetString(encodedNameBuffer, 0, count));
      }
      if (materialDefinition == null)
        materialDefinition = MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition();
      return materialDefinition;
    }

    private Dictionary<byte, MyVoxelMaterialDefinition> Compatibility_LoadMaterialIndexMapping(
      Stream stream)
    {
      int capacity = stream.Read7BitEncodedInt();
      Dictionary<byte, MyVoxelMaterialDefinition> dictionary = new Dictionary<byte, MyVoxelMaterialDefinition>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        byte key = stream.ReadByteNoAlloc();
        MyVoxelMaterialDefinition definition;
        if (!MyDefinitionManager.Static.TryGetVoxelMaterialDefinition(stream.ReadString(), out definition))
          definition = MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition();
        dictionary.Add(key, definition);
      }
      return dictionary;
    }
  }
}
