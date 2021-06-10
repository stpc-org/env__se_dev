// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeShapes
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Noise;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal struct MyCompositeShapes
  {
    private List<MyVoxelMaterialDefinition> m_coreMaterials;
    private List<MyVoxelMaterialDefinition> m_surfaceMaterials;
    private List<MyVoxelMaterialDefinition> m_depositMaterials;
    public static readonly MyCompositeShapeGeneratorDelegate[] AsteroidGenerators;
    private List<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>> m_primarySelections;
    private List<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>> m_secondarySelections;

    static MyCompositeShapes()
    {
      int[] numArray1 = new int[3]{ 0, 1, 2 };
      int[] numArray2 = new int[3]{ 3, 4, 5 };
      MyCompositeShapes.AsteroidGenerators = ((IEnumerable<int>) numArray1).Select<int, MyTuple<int, bool>>((Func<int, MyTuple<int, bool>>) (x => MyTuple.Create<int, bool>(x, false))).Concat<MyTuple<int, bool>>(((IEnumerable<int>) numArray2).Select<int, MyTuple<int, bool>>((Func<int, MyTuple<int, bool>>) (x => MyTuple.Create<int, bool>(x, true)))).Select<MyTuple<int, bool>, MyCompositeShapeGeneratorDelegate>((Func<MyTuple<int, bool>, MyCompositeShapeGeneratorDelegate>) (info =>
      {
        int version = info.Item1;
        bool combined = info.Item2;
        return (MyCompositeShapeGeneratorDelegate) ((generatorSeed, seed, size) =>
        {
          if ((double) size == 0.0)
            size = MyUtils.GetRandomFloat(128f, 512f);
          MyCompositeShapes myCompositeShapes = new MyCompositeShapes(generatorSeed, seed, version);
          using (MyRandom.Instance.PushSeed(seed))
            return combined ? myCompositeShapes.CombinedGenerator(version, seed, size) : myCompositeShapes.ProceduralGenerator(version, seed, size);
        });
      })).ToArray<MyCompositeShapeGeneratorDelegate>();
    }

    private MyCompositeShapes(int generatorSeed, int asteroidSeed, int version)
      : this()
    {
      if (version <= 2)
        return;
      this.m_coreMaterials = new List<MyVoxelMaterialDefinition>();
      this.m_depositMaterials = new List<MyVoxelMaterialDefinition>();
      this.m_surfaceMaterials = new List<MyVoxelMaterialDefinition>();
      using (MyRandom.Instance.PushSeed(generatorSeed))
      {
        MyRandom instance = MyRandom.Instance;
        this.FillMaterials(version);
        MyCompositeShapes.FilterKindDuplicates(this.m_coreMaterials, instance);
        MyCompositeShapes.FilterKindDuplicates(this.m_depositMaterials, instance);
        MyCompositeShapes.FilterKindDuplicates(this.m_surfaceMaterials, instance);
        MyCompositeShapes.ProcessMaterialSpawnProbabilities(this.m_coreMaterials);
        MyCompositeShapes.ProcessMaterialSpawnProbabilities(this.m_depositMaterials);
        MyCompositeShapes.ProcessMaterialSpawnProbabilities(this.m_surfaceMaterials);
        if (instance.Next(100) < 1)
        {
          this.MakeIceAsteroid(version, instance);
        }
        else
        {
          if (version < 4)
            return;
          int maxCount1 = instance.NextDouble() > 0.800000011920929 ? 4 : 2;
          int maxCount2 = instance.NextDouble() > 0.400000005960464 ? 2 : 1;
          MyCompositeShapes.LimitMaterials(this.m_coreMaterials, maxCount1, instance);
          MyCompositeShapes.LimitMaterials(this.m_depositMaterials, maxCount1, instance);
          using (MyRandom.Instance.PushSeed(asteroidSeed))
          {
            MyCompositeShapes.LimitMaterials(this.m_coreMaterials, maxCount2, instance);
            MyCompositeShapes.LimitMaterials(this.m_depositMaterials, maxCount2, instance);
          }
        }
      }
    }

    private IMyCompositionInfoProvider CombinedGenerator(
      int version,
      int seed,
      float size)
    {
      int length1 = 0;
      MyRandom instance = MyRandom.Instance;
      MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ConstructionData data;
      data.DefaultMaterial = (MyVoxelMaterialDefinition) null;
      data.Deposits = Array.Empty<MyCompositeShapeOreDeposit>();
      data.FilledShapes = new MyCsgShapeBase[length1];
      IMyCompositeShape[] myCompositeShapeArray = new IMyCompositeShape[6];
      this.FillSpan(instance, version, size, new Span<IMyCompositeShape>(myCompositeShapeArray, 0, 1), MyDefinitionManager.Static.GetVoxelMapStorageDefinitionsForProceduralPrimaryAdditions(), true);
      size = (float) ((MyStorageBase) myCompositeShapeArray[0]).Size.AbsMax();
      float idealSize1 = size / 2f;
      float idealSize2 = size / 2f;
      int length2 = 5 / ((double) size > 200.0 ? 1 : 2);
      int length3 = 1;
      if ((double) size <= 64.0)
      {
        length2 = 0;
        length3 = 0;
      }
      IMyCompositeShape[] removedShapes = new IMyCompositeShape[length2];
      data.RemovedShapes = new MyCsgShapeBase[length3];
      this.FillSpan(instance, version, idealSize2, (Span<IMyCompositeShape>) myCompositeShapeArray, MyDefinitionManager.Static.GetVoxelMapStorageDefinitionsForProceduralAdditions());
      this.FillSpan(instance, version, idealSize1, (Span<IMyCompositeShape>) removedShapes, MyDefinitionManager.Static.GetVoxelMapStorageDefinitionsForProceduralRemovals());
      this.TranslateShapes((Span<IMyCompositeShape>) removedShapes, size, instance);
      this.TranslateShapes(new Span<IMyCompositeShape>(myCompositeShapeArray, 1, myCompositeShapeArray.Length - 1), size, instance);
      if ((double) size > 512.0)
        size /= 2f;
      float num = size * 0.5f;
      float storageOffset = (float) MathHelper.GetNearestBiggerPowerOfTwo(size) * 0.5f - num;
      MyCompositeShapes.GetProceduralModules(seed, size, instance, out data.MacroModule, out data.DetailModule);
      MyCompositeShapes.GenerateProceduralAdditions(version, size, data.FilledShapes, instance, storageOffset);
      MyCompositeShapes.GenerateProceduralRemovals(version, size, data.RemovedShapes, instance, storageOffset);
      MyCompositeShapeProvider.MyCombinedCompositeInfoProvider shapeInfo = new MyCompositeShapeProvider.MyCombinedCompositeInfoProvider(ref data, myCompositeShapeArray, removedShapes);
      MyVoxelMaterialDefinition defaultMaterial;
      MyCompositeShapeOreDeposit[] deposits;
      this.GenerateMaterials(version, size, instance, data.FilledShapes, storageOffset, out defaultMaterial, out deposits, shapeInfo);
      shapeInfo.UpdateMaterials(defaultMaterial, deposits);
      return (IMyCompositionInfoProvider) shapeInfo;
    }

    private void TranslateShapes(Span<IMyCompositeShape> array, float size, MyRandom random)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        int num = 0;
        if (array[index] is MyStorageBase myStorageBase)
          num = myStorageBase.Size.AbsMax();
        array[index] = (IMyCompositeShape) new MyCompositeTranslateShape(array[index], MyCompositeShapes.CreateRandomPointInBox(random, size - (float) num));
      }
    }

    private void FillSpan(
      MyRandom random,
      int version,
      float idealSize,
      Span<IMyCompositeShape> shapes,
      ListReader<MyVoxelMapStorageDefinition> voxelMaps,
      bool prefferOnlyBestFittingSize = false)
    {
      bool flag = false;
      for (int index = 0; index < shapes.Length; ++index)
      {
        if (shapes[index] == null)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      using (MyUtils.ReuseCollection<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>(ref this.m_primarySelections))
      {
        using (MyUtils.ReuseCollection<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>(ref this.m_secondarySelections))
        {
          this.m_primarySelections.EnsureCapacity<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>(voxelMaps.Count);
          this.m_secondarySelections.EnsureCapacity<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>(voxelMaps.Count);
          int num1 = int.MinValue;
          int num2 = int.MaxValue;
          foreach (MyVoxelMapStorageDefinition voxelMap in voxelMaps)
          {
            if (voxelMap == null)
            {
              MyLog.Default.Error("MyCompositeShape - Voxelmaps contain null!");
            }
            else
            {
              HashSet<int> generatorVersions = voxelMap.GeneratorVersions;
              // ISSUE: explicit non-virtual call
              if ((generatorVersions != null ? (!__nonvirtual (generatorVersions.Contains(version)) ? 1 : 0) : 0) == 0)
              {
                MyOctreeStorage asteroidStorage = MyCompositeShapes.CreateAsteroidStorage(voxelMap);
                int num3 = asteroidStorage.Size.AbsMax();
                if ((double) num3 > (double) idealSize)
                {
                  if (num3 <= num2)
                  {
                    if (num3 < num2)
                    {
                      num2 = num3;
                      this.m_secondarySelections.Clear();
                    }
                    this.m_secondarySelections.Add(MyTuple.Create<MyVoxelMapStorageDefinition, MyOctreeStorage>(voxelMap, asteroidStorage));
                  }
                }
                else
                {
                  if (prefferOnlyBestFittingSize)
                  {
                    if (num3 >= num1)
                    {
                      if (num3 > num1)
                      {
                        num1 = num3;
                        this.m_primarySelections.Clear();
                      }
                    }
                    else
                      continue;
                  }
                  this.m_primarySelections.Add(MyTuple.Create<MyVoxelMapStorageDefinition, MyOctreeStorage>(voxelMap, asteroidStorage));
                }
              }
            }
          }
          List<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>> source = this.m_primarySelections.Count > 0 ? this.m_primarySelections : this.m_secondarySelections;
          float num4 = source.Sum<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>((Func<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>, float>) (x => x.Item1.SpawnProbability));
label_37:
          for (int index = 0; index < shapes.Length; ++index)
          {
            if (shapes[index] == null)
            {
              float num3 = num4 * random.NextFloat();
              foreach (MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage> myTuple in source)
              {
                float spawnProbability = myTuple.Item1.SpawnProbability;
                if ((double) num3 < (double) spawnProbability)
                {
                  shapes[index] = (IMyCompositeShape) myTuple.Item2;
                  goto label_37;
                }
                else
                  num3 -= spawnProbability;
              }
              shapes[index] = (IMyCompositeShape) source.MaxBy<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>>((Func<MyTuple<MyVoxelMapStorageDefinition, MyOctreeStorage>, float>) (x => x.Item1.SpawnProbability)).Item2;
            }
          }
        }
      }
    }

    public static MyOctreeStorage CreateAsteroidStorage(
      MyVoxelMapStorageDefinition definition)
    {
      return (MyOctreeStorage) MyStorageBase.LoadFromFile(Path.Combine(definition.Context.IsBaseGame ? MyFileSystem.ContentPath : definition.Context.ModPath, definition.StorageFile));
    }

    private IMyCompositionInfoProvider ProceduralGenerator(
      int version,
      int seed,
      float size)
    {
      MyRandom instance = MyRandom.Instance;
      MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ConstructionData data;
      data.FilledShapes = new MyCsgShapeBase[2];
      data.RemovedShapes = new MyCsgShapeBase[2];
      MyCompositeShapes.GetProceduralModules(seed, size, instance, out data.MacroModule, out data.DetailModule);
      float num1 = size * 0.5f;
      float num2 = (float) MathHelper.GetNearestBiggerPowerOfTwo(size) * 0.5f;
      float storageOffset = num2 - num1;
      MyCsgShapeBase myCsgShapeBase;
      switch (instance.Next() % 3)
      {
        case 0:
          float secondaryRadius = (float) ((double) instance.NextFloat() * 0.0500000007450581 + 0.100000001490116) * size;
          myCsgShapeBase = (MyCsgShapeBase) new MyCsgTorus(new Vector3(num2), MyCompositeShapes.CreateRandomRotation(instance), (float) ((double) instance.NextFloat() * 0.100000001490116 + 0.200000002980232) * size, secondaryRadius, (float) ((double) instance.NextFloat() * 0.400000005960464 + 0.400000005960464) * secondaryRadius, (float) ((double) instance.NextFloat() * 0.800000011920929 + 0.200000002980232), (float) ((double) instance.NextFloat() * 0.600000023841858 + 0.400000005960464));
          break;
        default:
          myCsgShapeBase = (MyCsgShapeBase) new MyCsgSphere(new Vector3(num2), (float) (((double) instance.NextFloat() * 0.100000001490116 + 0.349999994039536) * (double) size * (version > 2 ? 0.800000011920929 : 1.0)), (float) (((double) instance.NextFloat() * 0.0500000007450581 + 0.0500000007450581) * (double) size + 1.0), (float) ((double) instance.NextFloat() * 0.800000011920929 + 0.200000002980232), (float) ((double) instance.NextFloat() * 0.600000023841858 + 0.400000005960464));
          break;
      }
      data.FilledShapes[0] = myCsgShapeBase;
      MyCompositeShapes.GenerateProceduralAdditions(version, size, data.FilledShapes, instance, storageOffset);
      MyCompositeShapes.GenerateProceduralRemovals(version, size, data.RemovedShapes, instance, storageOffset);
      this.GenerateMaterials(version, size, instance, data.FilledShapes, storageOffset, out data.DefaultMaterial, out data.Deposits);
      return (IMyCompositionInfoProvider) new MyCompositeShapeProvider.MyProceduralCompositeInfoProvider(ref data);
    }

    private static void GetProceduralModules(
      int seed,
      float size,
      MyRandom random,
      out IMyModule macroModule,
      out IMyModule detailModule)
    {
      macroModule = (IMyModule) new MySimplexFast(seed, 7.0 / (double) size);
      switch (random.Next() & 1)
      {
        case 0:
          detailModule = (IMyModule) new MyRidgedMultifractalFast(MyNoiseQuality.Low, 1, seed, (double) random.NextFloat() * 0.0900000035762787 + 0.109999999403954);
          break;
        default:
          detailModule = (IMyModule) new MyBillowFast(MyNoiseQuality.Low, 1, seed, (double) random.NextFloat() * 0.0700000002980232 + 0.129999995231628);
          break;
      }
    }

    private static void GenerateProceduralAdditions(
      int version,
      float size,
      MyCsgShapeBase[] filledShapes,
      MyRandom random,
      float storageOffset)
    {
      bool flag = version > 2;
      for (int index = 0; index < filledShapes.Length; ++index)
      {
        if (filledShapes[index] == null)
        {
          float num1 = (float) ((double) size * ((double) random.NextFloat() * 0.200000002980232 + 0.100000001490116) + 2.0);
          float num2 = 2f * num1;
          float boxSize = size - num2;
          switch (random.Next() % (flag ? 2 : 3))
          {
            case 0:
              Vector3 vector3_1 = MyCompositeShapes.CreateRandomPointOnBox(random, boxSize, version) + num1;
              float radius1 = (float) ((double) num1 * ((double) random.NextFloat() * 0.400000005960464 + 0.349999994039536) * (flag ? 0.800000011920929 : 1.0));
              double num3 = (double) storageOffset;
              MyCsgSphere myCsgSphere = new MyCsgSphere(vector3_1 + (float) num3, radius1, radius1 * (float) ((double) random.NextFloat() * 0.100000001490116 + 0.100000001490116), (float) ((double) random.NextFloat() * 0.800000011920929 + 0.200000002980232), (float) ((double) random.NextFloat() * 0.600000023841858 + 0.400000005960464));
              filledShapes[index] = (MyCsgShapeBase) myCsgSphere;
              continue;
            case 1:
              Vector3 vector3_2 = MyCompositeShapes.CreateRandomPointOnBox(random, boxSize, version) + num1;
              Vector3 vector3_3 = new Vector3(size) - vector3_2;
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_2.X, ref vector3_3.X);
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_2.Y, ref vector3_3.Y);
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_2.Z, ref vector3_3.Z);
              float radius2 = (float) (((double) random.NextFloat() * 0.25 + 0.5) * (double) num1 * (flag ? 0.5 : 1.0));
              MyCsgCapsule myCsgCapsule = new MyCsgCapsule(vector3_2 + storageOffset, vector3_3 + storageOffset, radius2, (float) (((double) random.NextFloat() * 0.25 + 0.5) * (flag ? 1.0 : (double) radius2)), (float) ((double) random.NextFloat() * 0.400000005960464 + 0.400000005960464), (float) ((double) random.NextFloat() * 0.600000023841858 + 0.400000005960464));
              filledShapes[index] = (MyCsgShapeBase) myCsgCapsule;
              continue;
            case 2:
              Vector3 point = MyCompositeShapes.CreateRandomPointInBox(random, boxSize) + num1;
              Quaternion randomRotation = MyCompositeShapes.CreateRandomRotation(random);
              float boxSideDistance = MyCompositeShapes.ComputeBoxSideDistance(point, size);
              float secondaryRadius = (float) ((double) random.NextFloat() * 0.150000005960464 + 0.100000001490116) * boxSideDistance;
              MyCsgTorus myCsgTorus = new MyCsgTorus(point + storageOffset, randomRotation, (float) ((double) random.NextFloat() * 0.200000002980232 + 0.5) * boxSideDistance, secondaryRadius, (float) ((double) random.NextFloat() * 0.25 + 0.200000002980232) * secondaryRadius, (float) ((double) random.NextFloat() * 0.800000011920929 + 0.200000002980232), (float) ((double) random.NextFloat() * 0.600000023841858 + 0.400000005960464));
              filledShapes[index] = (MyCsgShapeBase) myCsgTorus;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private static void GenerateProceduralRemovals(
      int version,
      float size,
      MyCsgShapeBase[] removedShapes,
      MyRandom random,
      float storageOffset)
    {
      bool flag = version > 2;
      for (int index = 0; index < removedShapes.Length; ++index)
      {
        if (removedShapes[index] == null)
        {
          float num1 = (float) ((double) size * ((double) random.NextFloat() * 0.200000002980232 + 0.100000001490116) + 2.0);
          float num2 = 2f * num1;
          float boxSize = size - num2;
          switch (random.Next() % 7)
          {
            case 0:
              Vector3 point1 = MyCompositeShapes.CreateRandomPointInBox(random, boxSize) + num1;
              float boxSideDistance1 = MyCompositeShapes.ComputeBoxSideDistance(point1, size);
              float radius1 = (float) ((double) random.NextFloat() * (flag ? 0.300000011920929 : 0.400000005960464) + (flag ? 0.100000001490116 : 0.300000011920929)) * boxSideDistance1;
              MyCsgSphere myCsgSphere = new MyCsgSphere(point1 + storageOffset, radius1, (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.300000011920929) + (flag ? 0.449999988079071 : 0.349999994039536)) * radius1, (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.800000011920929) + (flag ? 1.0 : 0.200000002980232)), (float) ((double) random.NextFloat() * (flag ? 0.100000001490116 : 0.600000023841858) + 0.400000005960464));
              removedShapes[index] = (MyCsgShapeBase) myCsgSphere;
              continue;
            case 1:
            case 2:
            case 3:
              Vector3 point2 = MyCompositeShapes.CreateRandomPointInBox(random, boxSize) + num1;
              Quaternion randomRotation = MyCompositeShapes.CreateRandomRotation(random);
              float boxSideDistance2 = MyCompositeShapes.ComputeBoxSideDistance(point2, size);
              float secondaryRadius = (float) ((double) random.NextFloat() * (flag ? 0.100000001490116 : 0.150000005960464) + (flag ? 0.200000002980232 : 0.100000001490116)) * boxSideDistance2;
              MyCsgTorus myCsgTorus = new MyCsgTorus(point2 + storageOffset, randomRotation, (float) ((double) random.NextFloat() * 0.200000002980232 + (flag ? 0.300000011920929 : 0.5)) * boxSideDistance2, secondaryRadius, (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.25) + (flag ? 1.0 : 0.200000002980232)) * secondaryRadius, (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.800000011920929) + (flag ? 1.0 : 0.200000002980232)), (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.600000023841858) + 0.400000005960464));
              removedShapes[index] = (MyCsgShapeBase) myCsgTorus;
              continue;
            default:
              Vector3 vector3_1 = MyCompositeShapes.CreateRandomPointOnBox(random, boxSize, version) + num1;
              Vector3 vector3_2 = new Vector3(size) - vector3_1;
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_1.X, ref vector3_2.X);
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_1.Y, ref vector3_2.Y);
              if (random.Next() % 2 == 0)
                MyUtils.Swap<float>(ref vector3_1.Z, ref vector3_2.Z);
              float radius2 = (float) ((double) random.NextFloat() * (flag ? 0.300000011920929 : 0.25) + (flag ? 0.100000001490116 : 0.5)) * num1;
              MyCsgCapsule myCsgCapsule = new MyCsgCapsule(vector3_1 + storageOffset, vector3_2 + storageOffset, radius2, (float) (((double) random.NextFloat() * (flag ? 0.5 : 0.25) + (flag ? 1.0 : 0.5)) * (flag ? 1.0 : (double) radius2)), (float) ((double) random.NextFloat() * (flag ? 0.5 : 0.400000005960464) + (flag ? 1.0 : 0.400000005960464)), (float) ((double) random.NextFloat() * (flag ? 0.200000002980232 : 0.600000023841858) + 0.400000005960464));
              removedShapes[index] = (MyCsgShapeBase) myCsgCapsule;
              continue;
          }
        }
      }
    }

    private void GenerateMaterials(
      int version,
      float size,
      MyRandom random,
      MyCsgShapeBase[] filledShapes,
      float storageOffset,
      out MyVoxelMaterialDefinition defaultMaterial,
      out MyCompositeShapeOreDeposit[] deposits,
      MyCompositeShapeProvider.MyCombinedCompositeInfoProvider shapeInfo = null)
    {
      bool flag = version > 2;
      int num1 = flag ? 1 : 0;
      if (this.m_coreMaterials == null)
      {
        this.m_coreMaterials = new List<MyVoxelMaterialDefinition>();
        this.m_depositMaterials = new List<MyVoxelMaterialDefinition>();
        this.m_surfaceMaterials = new List<MyVoxelMaterialDefinition>();
        this.FillMaterials(version);
      }
      Action<List<MyVoxelMaterialDefinition>> action = (Action<List<MyVoxelMaterialDefinition>>) (list =>
      {
        int count = list.Count;
        while (count > 1)
        {
          int index = random.Next() % count;
          --count;
          MyVoxelMaterialDefinition materialDefinition = list[index];
          list[index] = list[count];
          list[count] = materialDefinition;
        }
      });
      action(this.m_depositMaterials);
      defaultMaterial = this.m_surfaceMaterials.Count != 0 ? this.m_surfaceMaterials[random.Next() % this.m_surfaceMaterials.Count] : (this.m_depositMaterials.Count != 0 ? this.m_depositMaterials[random.Next() % this.m_depositMaterials.Count] : this.m_coreMaterials[random.Next() % this.m_coreMaterials.Count]);
      int val1;
      if (flag)
      {
        val1 = (int) ((double) MySession.Static.Settings.DepositsCountCoefficient * ((double) size > 64.0 ? ((double) size > 128.0 ? ((double) size > 256.0 ? ((double) size > 512.0 ? 5.0 : 4.0) : 3.0) : 2.0) : 1.0));
        if (this.m_depositMaterials.Count == 0)
          val1 = 0;
      }
      else
        val1 = (int) Math.Log((double) size);
      int length1 = Math.Max(val1, filledShapes.Length);
      deposits = new MyCompositeShapeOreDeposit[length1];
      float depositSizeDenominator = MySession.Static.Settings.DepositSizeDenominator;
      float num2 = !flag || (double) depositSizeDenominator <= 0.0 ? size / 10f : (float) ((double) size / (double) depositSizeDenominator + 8.0);
      MyVoxelMaterialDefinition material1 = defaultMaterial;
      int num3 = 0;
      for (int index = 0; index < filledShapes.Length; ++index)
      {
        if (index == 0)
        {
          if (this.m_coreMaterials.Count == 0)
          {
            if (this.m_depositMaterials.Count == 0)
            {
              if (this.m_surfaceMaterials.Count != 0)
                material1 = this.m_surfaceMaterials[random.Next() % this.m_surfaceMaterials.Count];
            }
            else
              material1 = this.m_depositMaterials[num3++];
          }
          else
            material1 = this.m_coreMaterials[random.Next() % this.m_coreMaterials.Count];
        }
        else if (this.m_depositMaterials.Count == 0)
        {
          if (this.m_surfaceMaterials.Count != 0)
            material1 = this.m_surfaceMaterials[random.Next() % this.m_surfaceMaterials.Count];
        }
        else
          material1 = this.m_depositMaterials[num3++];
        deposits[index] = new MyCompositeShapeOreDeposit(filledShapes[index].DeepCopy(), material1);
        deposits[index].Shape.ShrinkTo((float) ((double) random.NextFloat() * (flag ? 0.600000023841858 : 0.150000005960464) + (flag ? 0.100000001490116 : 0.600000023841858)));
        if (num3 == this.m_depositMaterials.Count)
        {
          num3 = 0;
          action(this.m_depositMaterials);
        }
      }
      for (int length2 = filledShapes.Length; length2 < length1; ++length2)
      {
        float radius = 0.0f;
        Vector3 translation = Vector3.Zero;
        for (int index = 0; index < 10; ++index)
        {
          translation = MyCompositeShapes.CreateRandomPointInBox(random, size * (flag ? 0.6f : 0.7f)) + storageOffset + size * 0.15f;
          radius = (float) ((double) random.NextFloat() * (double) num2 + (flag ? 5.0 : 8.0));
          if (shapeInfo != null)
          {
            Vector3I vector3I = new Vector3I((int) (Math.Sqrt((double) radius * (double) radius / 2.0) * 0.5));
            BoundingBoxI box = new BoundingBoxI((Vector3I) translation - vector3I, (Vector3I) translation + vector3I);
            if (MyCompositeShapeProviderBase.Intersect((IMyCompositionInfoProvider) shapeInfo, box, 0) != ContainmentType.Disjoint)
              break;
          }
          else
            break;
        }
        double num4 = (double) random.NextFloat();
        double num5 = (double) random.NextFloat();
        MyCsgShapeBase shape = (MyCsgShapeBase) new MyCsgSphere(translation, radius);
        MyVoxelMaterialDefinition material2 = this.m_depositMaterials.Count != 0 ? this.m_depositMaterials[num3++] : this.m_surfaceMaterials[num3++];
        deposits[length2] = new MyCompositeShapeOreDeposit(shape, material2);
        if (this.m_depositMaterials.Count == 0)
        {
          if (num3 == this.m_surfaceMaterials.Count)
          {
            num3 = 0;
            action(this.m_surfaceMaterials);
          }
        }
        else if (num3 == this.m_depositMaterials.Count)
        {
          num3 = 0;
          action(this.m_depositMaterials);
        }
      }
    }

    private void FillMaterials(int version)
    {
      foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
      {
        if (MyCompositeShapes.IsAcceptedAsteroidMaterial(materialDefinition, version))
        {
          if (version > 2)
          {
            if (materialDefinition.MinedOre == "Stone")
              this.m_surfaceMaterials.Add(materialDefinition);
            else
              this.m_depositMaterials.Add(materialDefinition);
          }
          else if (materialDefinition.MinedOre == "Stone")
            this.m_surfaceMaterials.Add(materialDefinition);
          else if (materialDefinition.MinedOre == "Iron")
            this.m_coreMaterials.Add(materialDefinition);
          else if (materialDefinition.MinedOre == "Uranium")
          {
            this.m_depositMaterials.Add(materialDefinition);
            this.m_depositMaterials.Add(materialDefinition);
          }
          else if (materialDefinition.MinedOre == "Ice")
          {
            this.m_depositMaterials.Add(materialDefinition);
            this.m_depositMaterials.Add(materialDefinition);
          }
          else
            this.m_depositMaterials.Add(materialDefinition);
        }
      }
      if (this.m_surfaceMaterials.Count == 0 && this.m_depositMaterials.Count == 0)
        throw new Exception("There are no voxel materials allowed to spawn in asteroids!");
    }

    private static Vector3 CreateRandomPointInBox(MyRandom random, float boxSize) => new Vector3(random.NextFloat() * boxSize, random.NextFloat() * boxSize, random.NextFloat() * boxSize);

    private static Vector3 CreateRandomPointOnBox(
      MyRandom random,
      float boxSize,
      int version)
    {
      Vector3 vector3 = Vector3.Zero;
      if (version <= 2)
      {
        switch (random.Next() & 6)
        {
          case 0:
            return new Vector3(0.0f, random.NextFloat(), random.NextFloat());
          case 1:
            return new Vector3(1f, random.NextFloat(), random.NextFloat());
          case 2:
            return new Vector3(random.NextFloat(), 0.0f, random.NextFloat());
          case 3:
            return new Vector3(random.NextFloat(), 1f, random.NextFloat());
          case 4:
            return new Vector3(random.NextFloat(), random.NextFloat(), 0.0f);
          case 5:
            return new Vector3(random.NextFloat(), random.NextFloat(), 1f);
        }
      }
      else
      {
        float num1 = random.NextFloat();
        float num2 = random.NextFloat();
        switch (random.Next() % 6)
        {
          case 0:
            vector3 = new Vector3(0.0f, num1, num2);
            break;
          case 1:
            vector3 = new Vector3(1f, num1, num2);
            break;
          case 2:
            vector3 = new Vector3(num1, 0.0f, num2);
            break;
          case 3:
            vector3 = new Vector3(num1, 1f, num2);
            break;
          case 4:
            vector3 = new Vector3(num1, num2, 0.0f);
            break;
          case 5:
            vector3 = new Vector3(num1, num2, 1f);
            break;
        }
      }
      return vector3 * boxSize;
    }

    private static Quaternion CreateRandomRotation(MyRandom self)
    {
      Quaternion quaternion = new Quaternion((float) ((double) self.NextFloat() * 2.0 - 1.0), (float) ((double) self.NextFloat() * 2.0 - 1.0), (float) ((double) self.NextFloat() * 2.0 - 1.0), (float) ((double) self.NextFloat() * 2.0 - 1.0));
      quaternion.Normalize();
      return quaternion;
    }

    private static float ComputeBoxSideDistance(Vector3 point, float boxSize) => Vector3.Min(point, new Vector3(boxSize) - point).Min();

    private static void FilterKindDuplicates(
      List<MyVoxelMaterialDefinition> materials,
      MyRandom random)
    {
      materials.SortNoAlloc<MyVoxelMaterialDefinition>((Comparison<MyVoxelMaterialDefinition>) ((x, y) => string.Compare(x.MinedOre, y.MinedOre, StringComparison.InvariantCultureIgnoreCase)));
      int num1 = 0;
      for (int index1 = 1; index1 <= materials.Count; ++index1)
      {
        if (index1 == materials.Count || materials[index1].MinedOre != materials[num1].MinedOre)
        {
          int num2 = random.Next(num1, index1);
          for (int index2 = index1 - 1; index2 >= num1; --index2)
          {
            if (index2 != num2)
              materials.RemoveAt(index2);
          }
          ++num1;
          index1 = num1;
        }
      }
    }

    private static void LimitMaterials(
      List<MyVoxelMaterialDefinition> materials,
      int maxCount,
      MyRandom random)
    {
      while (materials.Count > maxCount)
        materials.RemoveAt(random.Next(materials.Count));
    }

    private static void ProcessMaterialSpawnProbabilities(List<MyVoxelMaterialDefinition> materials)
    {
      int count = materials.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        MyVoxelMaterialDefinition material = materials[index1];
        int num = material.AsteroidGeneratorSpawnProbabilityMultiplier - 1;
        for (int index2 = 0; index2 < num; ++index2)
          materials.Add(material);
      }
    }

    private void MakeIceAsteroid(int version, MyRandom random)
    {
      List<MyVoxelMaterialDefinition> materialDefinitionList = new List<MyVoxelMaterialDefinition>();
      foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
      {
        if (MyCompositeShapes.IsAcceptedAsteroidMaterial(materialDefinition, version) && materialDefinition.MinedOre == "Ice")
          materialDefinitionList.Add(materialDefinition);
      }
      if (materialDefinitionList.Count == 0)
      {
        MyLog.Default.Log(MyLogSeverity.Error, "No ice material suitable for ice cluster. Ice cluster will not be generated!");
      }
      else
      {
        this.m_coreMaterials.Clear();
        this.m_depositMaterials.Clear();
        this.m_surfaceMaterials = materialDefinitionList;
        MyCompositeShapes.FilterKindDuplicates(this.m_surfaceMaterials, random);
      }
    }

    private static bool IsAcceptedAsteroidMaterial(MyVoxelMaterialDefinition material, int version) => material.SpawnsInAsteroids && material.MinVersion <= version && material.MaxVersion >= version;
  }
}
