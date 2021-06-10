// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyModel
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using BulletXNA.BulletCollision;
using Havok;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VRage.FileSystem;
using VRage.Game.ModAPI;
using VRage.Import;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Animations;
using VRageRender.Fractures;
using VRageRender.Import;
using VRageRender.Models;
using VRageRender.Utils;

namespace VRage.Game.Models
{
  public class MyModel : IDisposable, IPrimitiveManagerBase, IMyModel
  {
    private static readonly bool ENABLE_LOD1_GEOM_DATA = true;
    private static int m_nextUniqueId = 0;
    private static Dictionary<int, string> m_uniqueModelNames = new Dictionary<int, string>();
    private static Dictionary<string, int> m_uniqueModelIds = new Dictionary<string, int>();
    [ThreadStatic]
    private static MyModelImporter m_perThreadImporter;
    public readonly int UniqueId;
    private string m_geometryDataAssetName;
    public Dictionary<string, MyModelDummy> Dummies;
    public MyModelInfo ModelInfo;
    public ModelAnimations Animations;
    public MyModelBone[] Bones;
    public byte[] HavokData;
    public MyModelFractures ModelFractures;
    public bool ExportedWrong;
    public HkShape[] HavokCollisionShapes;
    public HkdBreakableShape[] HavokBreakableShapes;
    public byte[] HavokDestructionData;
    private HalfVector2[] m_texCoords;
    private Byte4[] m_tangents;
    private BoundingSphere m_boundingSphere;
    private BoundingBox m_boundingBox;
    private Vector3 m_boundingBoxSize;
    private Vector3 m_boundingBoxSizeHalf;
    public Vector3I[] BoneMapping;
    public float PatternScale = 1f;
    private MyLODDescriptor[] m_lods;
    private readonly string m_assetName;
    private volatile bool m_loadedData;
    private Dictionary<string, MyMeshSection> m_meshSections = new Dictionary<string, MyMeshSection>();
    private bool m_loadingErrorProcessed;
    private float m_scaleFactor = 1f;
    private int m_verticesCount;
    private int m_trianglesCount;
    private bool m_hasUV;
    public bool m_loadUV;
    private MyCompressedVertexNormal[] m_vertices;
    private MyCompressedBoneIndicesWeights[] m_bonesIndicesWeights;
    private int[] m_Indices;
    private ushort[] m_Indices_16bit;
    public MyTriangleVertexIndices[] Triangles;
    private IMyTriangePruningStructure m_bvh;
    private List<MyMesh> m_meshContainer = new List<MyMesh>();

    public bool KeepInMemory { get; private set; }

    public int DataVersion { get; private set; }

    private static MyModelImporter m_importer
    {
      get
      {
        if (MyModel.m_perThreadImporter == null)
          MyModel.m_perThreadImporter = new MyModelImporter();
        return MyModel.m_perThreadImporter;
      }
    }

    public bool LoadedData => this.m_loadedData;

    public MyLODDescriptor[] LODs
    {
      get => this.m_lods;
      private set => this.m_lods = value;
    }

    public float ScaleFactor => this.m_scaleFactor;

    public string AssetName => this.m_assetName;

    public BoundingSphere BoundingSphere => this.m_boundingSphere;

    public BoundingBox BoundingBox => this.m_boundingBox;

    public Vector3 BoundingBoxSize => this.m_boundingBoxSize;

    public Vector3 BoundingBoxSizeHalf => this.m_boundingBoxSizeHalf;

    public static string GetById(int id) => MyModel.m_uniqueModelNames[id];

    public static int GetId(string assetName)
    {
      int key;
      lock (MyModel.m_uniqueModelIds)
      {
        if (!MyModel.m_uniqueModelIds.TryGetValue(assetName, out key))
        {
          key = MyModel.m_nextUniqueId++;
          MyModel.m_uniqueModelIds.Add(assetName, key);
          MyModel.m_uniqueModelNames.Add(key, assetName);
        }
      }
      return key;
    }

    public MyModel(string assetName)
      : this(assetName, false)
      => this.UniqueId = MyModel.GetId(assetName);

    public MyModel(string assetName, bool keepInMemory)
    {
      this.m_assetName = assetName;
      this.m_loadedData = false;
      this.KeepInMemory = keepInMemory;
    }

    public MyMeshSection GetMeshSection(string name) => this.m_meshSections[name];

    public bool TryGetMeshSection(string name, out MyMeshSection section) => this.m_meshSections.TryGetValue(name, out section);

    public void LoadData(bool forceFullDetail = false)
    {
      lock (this)
      {
        if (this.m_loadedData)
          return;
        MyLog.Default.WriteLine("MyModel.LoadData -> START", LoggingOptions.LOADING_MODELS);
        MyLog.Default.IncreaseIndent(LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("m_assetName: " + this.m_assetName, LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine(string.Format("Importing asset {0}, path: {1}", (object) this.m_assetName, (object) this.AssetName), LoggingOptions.LOADING_MODELS);
        string assetFileName = this.AssetName;
        string str = Path.IsPathRooted(this.AssetName) ? this.AssetName : Path.Combine(MyFileSystem.ContentPath, this.AssetName);
        if (!MyFileSystem.FileExists(str))
          assetFileName = "Models\\Debug\\Error.mwm";
        try
        {
          MyModel.m_importer.ImportData(assetFileName);
        }
        catch
        {
          MyLog.Default.WriteLine(string.Format("Importing asset failed {0}", (object) this.m_assetName));
          throw;
        }
        this.DataVersion = MyModel.m_importer.DataVersion;
        Dictionary<string, object> tagData = MyModel.m_importer.GetTagData();
        if (tagData.Count == 0)
          throw new Exception(string.Format("Uncompleted tagData for asset: {0}, path: {1}", (object) this.m_assetName, (object) this.AssetName));
        this.m_meshSections.Clear();
        if (tagData.ContainsKey("Sections"))
        {
          List<MyMeshSectionInfo> myMeshSectionInfoList = tagData["Sections"] as List<MyMeshSectionInfo>;
          int num = 0;
          foreach (MyMeshSectionInfo myMeshSectionInfo in myMeshSectionInfoList)
          {
            MyMeshSection myMeshSection = new MyMeshSection()
            {
              Name = myMeshSectionInfo.Name,
              Index = num
            };
            this.m_meshSections[myMeshSection.Name] = myMeshSection;
            ++num;
          }
        }
        if (tagData.ContainsKey("LODs"))
          this.m_lods = tagData["LODs"] as MyLODDescriptor[];
        this.Animations = (ModelAnimations) tagData["Animations"];
        this.Bones = (MyModelBone[]) tagData["Bones"];
        this.m_boundingBox = (BoundingBox) tagData["BoundingBox"];
        this.m_boundingSphere = (BoundingSphere) tagData["BoundingSphere"];
        this.m_boundingBoxSize = this.BoundingBox.Max - this.BoundingBox.Min;
        this.m_boundingBoxSizeHalf = this.BoundingBoxSize / 2f;
        this.Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
        this.BoneMapping = tagData["BoneMapping"] as Vector3I[];
        if (this.BoneMapping.Length == 0)
          this.BoneMapping = (Vector3I[]) null;
        if (tagData.ContainsKey("ModelFractures"))
          this.ModelFractures = (MyModelFractures) tagData["ModelFractures"];
        object obj;
        if (tagData.TryGetValue("PatternScale", out obj))
          this.PatternScale = (float) obj;
        if (tagData.ContainsKey("HavokCollisionGeometry"))
        {
          this.HavokData = (byte[]) tagData["HavokCollisionGeometry"];
          byte[] memoryBuffer = (byte[]) tagData["HavokCollisionGeometry"];
          if (memoryBuffer.Length != 0 && HkBaseSystem.IsThreadInitialized)
          {
            List<HkShape> shapes = new List<HkShape>();
            bool containsScene;
            bool containsDestructionData;
            if (!HkShapeLoader.LoadShapesListFromBuffer(memoryBuffer, shapes, out containsScene, out containsDestructionData))
              MyLog.Default.WriteLine(string.Format("Model {0} - Unable to load collision geometry", (object) this.AssetName), LoggingOptions.LOADING_MODELS);
            if (shapes.Count > 10)
              MyLog.Default.WriteLine(string.Format("Model {0} - Found too many collision shapes, only the first 10 will be used", (object) this.AssetName), LoggingOptions.LOADING_MODELS);
            HkShape[] havokCollisionShapes = this.HavokCollisionShapes;
            if (shapes.Count > 0)
            {
              this.HavokCollisionShapes = shapes.ToArray();
              foreach (HkShape havokCollisionShape in this.HavokCollisionShapes)
                HkShape.FlagShapeAsReadOnly(havokCollisionShape);
            }
            else
              MyLog.Default.WriteLine(string.Format("Model {0} - Unable to load collision geometry from file, default collision will be used !", (object) this.AssetName));
            if (containsDestructionData)
              this.HavokDestructionData = memoryBuffer;
            this.ExportedWrong = !containsScene;
          }
        }
        if (tagData.ContainsKey("HavokDestruction") && ((byte[]) tagData["HavokDestruction"]).Length != 0)
          this.HavokDestructionData = (byte[]) tagData["HavokDestruction"];
        this.m_geometryDataAssetName = this.AssetName;
        if (!forceFullDetail && this.LODs != null && (this.LODs.Length != 0 && MyModel.ENABLE_LOD1_GEOM_DATA))
        {
          string absoluteFilePath = this.LODs[0].GetModelAbsoluteFilePath(str);
          if (absoluteFilePath != null)
            this.m_geometryDataAssetName = absoluteFilePath;
        }
        if (this.m_geometryDataAssetName == this.AssetName && tagData.ContainsKey("GeometryDataAsset"))
          this.m_geometryDataAssetName = new MyLODDescriptor()
          {
            Model = ((string) tagData["GeometryDataAsset"])
          }.GetModelAbsoluteFilePath(str);
        try
        {
          if (this.m_geometryDataAssetName != this.AssetName)
            MyModel.m_importer.ImportData(this.m_geometryDataAssetName);
          this.LoadGeometryData(MyModel.m_importer);
        }
        catch
        {
          throw new Exception(string.Format("Can't load geometry data for asset: {0}, geometry data path: {1}", (object) this.AssetName, (object) this.m_geometryDataAssetName));
        }
        MyLog.Default.WriteLine("Triangles.Length: " + (object) this.Triangles.Length, LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("Vertexes.Length: " + (object) this.GetVerticesCount(), LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("UseChannelTextures: " + ((bool) tagData["UseChannelTextures"]).ToString(), LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("BoundingBox: " + (object) this.BoundingBox, LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("BoundingSphere: " + (object) this.BoundingSphere, LoggingOptions.LOADING_MODELS);
        ++Stats.PerAppLifetime.MyModelsCount;
        Stats.PerAppLifetime.MyModelsMeshesCount += this.m_meshContainer.Count;
        Stats.PerAppLifetime.MyModelsVertexesCount += this.GetVerticesCount();
        Stats.PerAppLifetime.MyModelsTrianglesCount += this.Triangles.Length;
        this.ModelInfo = new MyModelInfo(this.GetTrianglesCount(), this.GetVerticesCount(), this.BoundingBoxSize);
        this.m_loadedData = true;
        this.m_loadingErrorProcessed = false;
        MyLog.Default.DecreaseIndent(LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("MyModel.LoadData -> END", LoggingOptions.LOADING_MODELS);
      }
    }

    public bool LoadTexCoordData()
    {
      if (!this.m_hasUV)
      {
        lock (this)
        {
          if (!this.m_loadedData)
          {
            this.m_loadUV = true;
            this.LoadData();
          }
          else
          {
            try
            {
              MyModel.m_importer.ImportData(this.m_geometryDataAssetName, new string[1]
              {
                "TexCoords0"
              });
            }
            catch
            {
              MyLog.Default.WriteLine(string.Format("Importing asset failed {0}", (object) this.m_assetName));
              return false;
            }
            this.m_texCoords = (HalfVector2[]) MyModel.m_importer.GetTagData()["TexCoords0"];
          }
          this.m_hasUV = true;
          this.m_loadUV = false;
        }
      }
      return this.m_hasUV;
    }

    public void LoadAnimationData(bool forceReload = false)
    {
      if (this.m_loadedData && !forceReload)
        return;
      lock (this)
      {
        MyLog.Default.WriteLine("MyModel.LoadData -> START", LoggingOptions.LOADING_MODELS);
        MyLog.Default.IncreaseIndent(LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("m_assetName: " + this.m_assetName, LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine(string.Format("Importing asset {0}, path: {1}", (object) this.m_assetName, (object) this.AssetName), LoggingOptions.LOADING_MODELS);
        try
        {
          MyModel.m_importer.ImportData(this.AssetName);
        }
        catch
        {
          MyLog.Default.WriteLine(string.Format("Importing asset failed {0}", (object) this.m_assetName));
          throw;
        }
        Dictionary<string, object> tagData = MyModel.m_importer.GetTagData();
        if (tagData.Count != 0)
        {
          this.DataVersion = MyModel.m_importer.DataVersion;
          this.Animations = (ModelAnimations) tagData["Animations"];
          this.Bones = (MyModelBone[]) tagData["Bones"];
          this.m_boundingBox = (BoundingBox) tagData["BoundingBox"];
          this.m_boundingSphere = (BoundingSphere) tagData["BoundingSphere"];
          this.m_boundingBoxSize = this.BoundingBox.Max - this.BoundingBox.Min;
          this.m_boundingBoxSizeHalf = this.BoundingBoxSize / 2f;
          this.Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
          this.BoneMapping = tagData["BoneMapping"] as Vector3I[];
          if (this.BoneMapping.Length == 0)
            this.BoneMapping = (Vector3I[]) null;
        }
        else
        {
          this.DataVersion = 0;
          this.Animations = (ModelAnimations) null;
          this.Bones = (MyModelBone[]) null;
          this.m_boundingBox = new BoundingBox();
          this.m_boundingSphere = new BoundingSphere();
          this.m_boundingBoxSize = new Vector3();
          this.m_boundingBoxSizeHalf = new Vector3();
          this.Dummies = (Dictionary<string, MyModelDummy>) null;
          this.BoneMapping = (Vector3I[]) null;
        }
        this.ModelInfo = new MyModelInfo(this.GetTrianglesCount(), this.GetVerticesCount(), this.BoundingBoxSize);
        if (tagData.Count != 0)
          this.m_loadedData = true;
        MyLog.Default.DecreaseIndent(LoggingOptions.LOADING_MODELS);
        MyLog.Default.WriteLine("MyModel.LoadAnimationData -> END", LoggingOptions.LOADING_MODELS);
      }
    }

    public bool UnloadData()
    {
      bool loadedData = this.m_loadedData;
      this.m_loadedData = false;
      if (this.m_bvh != null)
      {
        this.m_bvh.Close();
        this.m_bvh = (IMyTriangePruningStructure) null;
      }
      Stats.PerAppLifetime.MyModelsMeshesCount -= this.m_meshContainer.Count;
      if (this.m_vertices != null)
        Stats.PerAppLifetime.MyModelsVertexesCount -= this.GetVerticesCount();
      if (this.Triangles != null)
        Stats.PerAppLifetime.MyModelsTrianglesCount -= this.Triangles.Length;
      if (loadedData)
        --Stats.PerAppLifetime.MyModelsCount;
      if (this.HavokCollisionShapes != null)
      {
        for (int index = 0; index < this.HavokCollisionShapes.Length; ++index)
          this.HavokCollisionShapes[index].RemoveReference();
        this.HavokCollisionShapes = (HkShape[]) null;
      }
      this.HavokBreakableShapes = (HkdBreakableShape[]) null;
      this.Triangles = (MyTriangleVertexIndices[]) null;
      this.m_vertices = (MyCompressedVertexNormal[]) null;
      this.m_bonesIndicesWeights = (MyCompressedBoneIndicesWeights[]) null;
      this.m_meshContainer.Clear();
      this.m_meshSections.Clear();
      this.m_Indices_16bit = (ushort[]) null;
      this.m_Indices = (int[]) null;
      this.ModelInfo = (MyModelInfo) null;
      this.Bones = (MyModelBone[]) null;
      this.Dummies = (Dictionary<string, MyModelDummy>) null;
      this.HavokData = (byte[]) null;
      this.HavokDestructionData = (byte[]) null;
      this.ModelFractures = (MyModelFractures) null;
      this.m_texCoords = (HalfVector2[]) null;
      this.m_tangents = (Byte4[]) null;
      this.BoneMapping = (Vector3I[]) null;
      this.m_lods = (MyLODDescriptor[]) null;
      this.m_scaleFactor = 1f;
      this.Animations = (ModelAnimations) null;
      return loadedData;
    }

    public void Dispose() => this.m_meshContainer.Clear();

    public void LoadOnlyDummies()
    {
      if (this.m_loadedData)
        return;
      lock (this)
      {
        MyLog.Default.WriteLine("MyModel.LoadSnapPoints -> START", LoggingOptions.LOADING_MODELS);
        using (MyLog.Default.IndentUsing(LoggingOptions.LOADING_MODELS))
        {
          MyLog.Default.WriteLine("m_assetName: " + this.m_assetName, LoggingOptions.LOADING_MODELS);
          MyModelImporter myModelImporter = new MyModelImporter();
          MyLog.Default.WriteLine(string.Format("Importing asset {0}, path: {1}", (object) this.m_assetName, (object) this.AssetName), LoggingOptions.LOADING_MODELS);
          try
          {
            myModelImporter.ImportData(this.AssetName, new string[1]
            {
              "Dummies"
            });
          }
          catch (Exception ex)
          {
            MyLog.Default.WriteLine(string.Format("Importing asset failed {0}, message: {1}, stack:{2}", (object) this.m_assetName, (object) ex.Message, (object) ex.StackTrace));
          }
          Dictionary<string, object> tagData = myModelImporter.GetTagData();
          if (tagData.Count > 0)
            this.Dummies = tagData["Dummies"] as Dictionary<string, MyModelDummy>;
          else
            this.Dummies = new Dictionary<string, MyModelDummy>();
        }
      }
    }

    public void LoadOnlyModelInfo()
    {
      if (this.m_loadedData)
        return;
      lock (this)
      {
        MyLog.Default.WriteLine("MyModel.LoadModelData -> START", LoggingOptions.LOADING_MODELS);
        using (MyLog.Default.IndentUsing(LoggingOptions.LOADING_MODELS))
        {
          MyLog.Default.WriteLine("m_assetName: " + this.m_assetName, LoggingOptions.LOADING_MODELS);
          MyModelImporter myModelImporter = new MyModelImporter();
          MyLog.Default.WriteLine(string.Format("Importing asset {0}, path: {1}", (object) this.m_assetName, (object) this.AssetName), LoggingOptions.LOADING_MODELS);
          try
          {
            myModelImporter.ImportData(this.AssetName, new string[1]
            {
              "ModelInfo"
            });
          }
          catch (Exception ex)
          {
            MyLog.Default.WriteLine(string.Format("Importing asset failed {0}, message: {1}, stack:{2}", (object) this.m_assetName, (object) ex.Message, (object) ex.StackTrace));
          }
          Dictionary<string, object> tagData = myModelImporter.GetTagData();
          if (tagData.Count > 0)
            this.ModelInfo = tagData["ModelInfo"] as MyModelInfo;
          else
            this.ModelInfo = new MyModelInfo(0, 0, Vector3.Zero);
        }
      }
    }

    void IPrimitiveManagerBase.Cleanup()
    {
    }

    bool IPrimitiveManagerBase.IsTrimesh() => true;

    int IPrimitiveManagerBase.GetPrimitiveCount() => this.m_trianglesCount;

    void IPrimitiveManagerBase.GetPrimitiveBox(
      int prim_index,
      out AABB primbox)
    {
      BoundingBox invalid = BoundingBox.CreateInvalid();
      Vector3 vertex1 = this.GetVertex(this.Triangles[prim_index].I0);
      Vector3 vertex2 = this.GetVertex(this.Triangles[prim_index].I1);
      Vector3 vertex3 = this.GetVertex(this.Triangles[prim_index].I2);
      invalid.Include(ref vertex1, ref vertex2, ref vertex3);
      primbox = new AABB()
      {
        m_min = invalid.Min.ToBullet(),
        m_max = invalid.Max.ToBullet()
      };
    }

    void IPrimitiveManagerBase.GetPrimitiveTriangle(
      int prim_index,
      PrimitiveTriangle triangle)
    {
      triangle.m_vertices[0] = this.GetVertex(this.Triangles[prim_index].I0).ToBullet();
      triangle.m_vertices[1] = this.GetVertex(this.Triangles[prim_index].I1).ToBullet();
      triangle.m_vertices[2] = this.GetVertex(this.Triangles[prim_index].I2).ToBullet();
    }

    public void CheckLoadingErrors(MyModContext context, out bool errorFound)
    {
      if (this.ExportedWrong && !this.m_loadingErrorProcessed)
      {
        errorFound = true;
        this.m_loadingErrorProcessed = true;
      }
      else
        errorFound = false;
    }

    public void Rescale(float scaleFactor)
    {
      if ((double) this.m_scaleFactor == (double) scaleFactor)
        return;
      float num = scaleFactor / this.m_scaleFactor;
      this.m_scaleFactor = scaleFactor;
      for (int vertexIndex = 0; vertexIndex < this.m_verticesCount; ++vertexIndex)
      {
        Vector3 newPosition = this.GetVertex(vertexIndex) * num;
        this.SetVertexPosition(vertexIndex, ref newPosition);
      }
      if (this.Dummies != null)
      {
        foreach (KeyValuePair<string, MyModelDummy> dummy in this.Dummies)
        {
          Matrix matrix = dummy.Value.Matrix;
          matrix.Translation *= num;
          dummy.Value.Matrix = matrix;
        }
      }
      this.m_boundingBox.Min *= num;
      this.m_boundingBox.Max *= num;
      this.m_boundingBoxSize = this.BoundingBox.Max - this.BoundingBox.Min;
      this.m_boundingBoxSizeHalf = this.BoundingBoxSize / 2f;
      this.m_boundingSphere.Radius *= num;
    }

    int IMyModel.GetDummies(IDictionary<string, IMyModelDummy> dummies)
    {
      if (this.Dummies == null)
        return 0;
      if (dummies != null)
      {
        foreach (KeyValuePair<string, MyModelDummy> dummy in this.Dummies)
          dummies.Add(dummy.Key, (IMyModelDummy) dummy.Value);
      }
      return this.Dummies.Count;
    }

    int IMyModel.UniqueId => this.UniqueId;

    int IMyModel.DataVersion => this.DataVersion;

    Vector3I[] IMyModel.BoneMapping => this.BoneMapping.Clone() as Vector3I[];

    float IMyModel.PatternScale => this.PatternScale;

    public MyCompressedVertexNormal[] Vertices => this.m_vertices;

    public int[] Indices => this.m_Indices;

    public ushort[] Indices16 => this.m_Indices_16bit;

    public bool HasUV => this.m_hasUV;

    public bool LoadUV
    {
      set => this.m_loadUV = value;
    }

    public HalfVector2[] TexCoords => this.m_texCoords;

    public Vector3 GetVertexInt(int vertexIndex) => VF_Packer.UnpackPosition(ref this.m_vertices[vertexIndex].Position);

    public MyTriangleVertexIndices GetTriangle(int triangleIndex) => this.Triangles[triangleIndex];

    public Vector3 GetVertex(int vertexIndex) => this.GetVertexInt(vertexIndex);

    public void SetVertexPosition(int vertexIndex, ref Vector3 newPosition) => this.m_vertices[vertexIndex].Position = VF_Packer.PackPosition(newPosition);

    public void GetVertex(
      int vertexIndex1,
      int vertexIndex2,
      int vertexIndex3,
      out Vector3 v1,
      out Vector3 v2,
      out Vector3 v3)
    {
      v1 = this.GetVertex(vertexIndex1);
      v2 = this.GetVertex(vertexIndex2);
      v3 = this.GetVertex(vertexIndex3);
    }

    public MyTriangle_BoneIndicesWeigths? GetBoneIndicesWeights(
      int triangleIndex)
    {
      if (this.m_bonesIndicesWeights == null)
        return new MyTriangle_BoneIndicesWeigths?();
      MyTriangleVertexIndices triangle = this.Triangles[triangleIndex];
      MyCompressedBoneIndicesWeights bonesIndicesWeight1 = this.m_bonesIndicesWeights[triangle.I0];
      MyCompressedBoneIndicesWeights bonesIndicesWeight2 = this.m_bonesIndicesWeights[triangle.I1];
      MyCompressedBoneIndicesWeights bonesIndicesWeight3 = this.m_bonesIndicesWeights[triangle.I2];
      Vector4UByte vector4Ubyte1 = bonesIndicesWeight1.Indices.ToVector4UByte();
      Vector4 vector4_1 = bonesIndicesWeight1.Weights.ToVector4();
      Vector4UByte vector4Ubyte2 = bonesIndicesWeight2.Indices.ToVector4UByte();
      Vector4 vector4_2 = bonesIndicesWeight2.Weights.ToVector4();
      Vector4UByte vector4Ubyte3 = bonesIndicesWeight3.Indices.ToVector4UByte();
      Vector4 vector4_3 = bonesIndicesWeight3.Weights.ToVector4();
      MyTriangle_BoneIndicesWeigths boneIndicesWeigths = new MyTriangle_BoneIndicesWeigths();
      ref MyTriangle_BoneIndicesWeigths local1 = ref boneIndicesWeigths;
      MyVertex_BoneIndicesWeights boneIndicesWeights1 = new MyVertex_BoneIndicesWeights();
      boneIndicesWeights1.Indices = vector4Ubyte1;
      boneIndicesWeights1.Weights = vector4_1;
      MyVertex_BoneIndicesWeights boneIndicesWeights2 = boneIndicesWeights1;
      local1.Vertex0 = boneIndicesWeights2;
      ref MyTriangle_BoneIndicesWeigths local2 = ref boneIndicesWeigths;
      boneIndicesWeights1 = new MyVertex_BoneIndicesWeights();
      boneIndicesWeights1.Indices = vector4Ubyte2;
      boneIndicesWeights1.Weights = vector4_2;
      MyVertex_BoneIndicesWeights boneIndicesWeights3 = boneIndicesWeights1;
      local2.Vertex1 = boneIndicesWeights3;
      ref MyTriangle_BoneIndicesWeigths local3 = ref boneIndicesWeigths;
      boneIndicesWeights1 = new MyVertex_BoneIndicesWeights();
      boneIndicesWeights1.Indices = vector4Ubyte3;
      boneIndicesWeights1.Weights = vector4_3;
      MyVertex_BoneIndicesWeights boneIndicesWeights4 = boneIndicesWeights1;
      local3.Vertex2 = boneIndicesWeights4;
      return new MyTriangle_BoneIndicesWeigths?(boneIndicesWeigths);
    }

    public Vector3 GetVertexNormal(int vertexIndex) => VF_Packer.UnpackNormal(ref this.m_vertices[vertexIndex].Normal);

    public Vector3 GetVertexTangent(int vertexIndex)
    {
      if (this.m_tangents == null)
      {
        MyModel.m_importer.ImportData(this.AssetName, new string[1]
        {
          "Tangents"
        });
        Dictionary<string, object> tagData = MyModel.m_importer.GetTagData();
        if (tagData.ContainsKey("Tangents"))
          this.m_tangents = (Byte4[]) tagData["Tangents"];
      }
      return this.m_tangents != null ? VF_Packer.UnpackNormal(this.m_tangents[vertexIndex]) : Vector3.Zero;
    }

    public List<MyMesh> GetMeshList() => this.m_meshContainer;

    private int GetNumberOfTrianglesForColDet()
    {
      int num = 0;
      foreach (MyMesh myMesh in this.m_meshContainer)
        num += myMesh.TriCount;
      return num;
    }

    private void CopyTriangleIndices()
    {
      this.Triangles = new MyTriangleVertexIndices[this.GetNumberOfTrianglesForColDet()];
      int index1 = 0;
      foreach (MyMesh myMesh in this.m_meshContainer)
      {
        myMesh.TriStart = index1;
        if (this.m_Indices != null)
        {
          for (int index2 = 0; index2 < myMesh.TriCount; ++index2)
          {
            this.Triangles[index1] = new MyTriangleVertexIndices(this.m_Indices[myMesh.IndexStart + index2 * 3], this.m_Indices[myMesh.IndexStart + index2 * 3 + 2], this.m_Indices[myMesh.IndexStart + index2 * 3 + 1]);
            ++index1;
          }
        }
        else
        {
          if (this.m_Indices_16bit == null)
            throw new InvalidBranchException();
          for (int index2 = 0; index2 < myMesh.TriCount; ++index2)
          {
            this.Triangles[index1] = new MyTriangleVertexIndices((int) this.m_Indices_16bit[myMesh.IndexStart + index2 * 3], (int) this.m_Indices_16bit[myMesh.IndexStart + index2 * 3 + 2], (int) this.m_Indices_16bit[myMesh.IndexStart + index2 * 3 + 1]);
            ++index1;
          }
        }
      }
    }

    [Conditional("DEBUG")]
    private void CheckTriangles(int triangleCount)
    {
      bool flag = true;
      foreach (MyTriangleVertexIndices triangle in this.Triangles)
        flag = flag & triangle.I0 != triangle.I1 & triangle.I1 != triangle.I2 & triangle.I2 != triangle.I0 & (triangle.I0 >= 0 & triangle.I0 < this.m_verticesCount) & (triangle.I1 >= 0 & triangle.I1 < this.m_verticesCount) & (triangle.I2 >= 0 & triangle.I2 < this.m_verticesCount);
    }

    public IMyTriangePruningStructure GetTrianglePruningStructure() => this.m_bvh;

    public void GetTriangleBoundingBox(int triangleIndex, ref BoundingBox boundingBox)
    {
      boundingBox = BoundingBox.CreateInvalid();
      Vector3 v1;
      Vector3 v2;
      Vector3 v3;
      this.GetVertex(this.Triangles[triangleIndex].I0, this.Triangles[triangleIndex].I1, this.Triangles[triangleIndex].I2, out v1, out v2, out v3);
      boundingBox.Include(v1, v2, v3);
    }

    public int GetTrianglesCount() => this.m_trianglesCount;

    public int GetVerticesCount() => this.m_verticesCount;

    public int GetBVHSize() => this.m_bvh == null ? 0 : this.m_bvh.Size;

    public MyMeshDrawTechnique GetDrawTechnique(int triangleIndex)
    {
      MyMeshDrawTechnique meshDrawTechnique = MyMeshDrawTechnique.MESH;
      for (int index = 0; index < this.m_meshContainer.Count; ++index)
      {
        if (triangleIndex >= this.m_meshContainer[index].TriStart && triangleIndex < this.m_meshContainer[index].TriStart + this.m_meshContainer[index].TriCount)
          meshDrawTechnique = this.m_meshContainer[index].Material.DrawTechnique;
      }
      return meshDrawTechnique;
    }

    private void LoadGeometryData(MyModelImporter importer)
    {
      Dictionary<string, object> tagData = importer.GetTagData();
      HalfVector4[] halfVector4Array = (HalfVector4[]) tagData["Vertices"];
      Byte4[] byte4Array = (Byte4[]) tagData["Normals"];
      this.m_vertices = new MyCompressedVertexNormal[halfVector4Array.Length];
      if (byte4Array.Length != 0)
      {
        for (int index = 0; index < halfVector4Array.Length; ++index)
          this.m_vertices[index] = new MyCompressedVertexNormal()
          {
            Position = halfVector4Array[index],
            Normal = byte4Array[index]
          };
      }
      else
      {
        for (int index = 0; index < halfVector4Array.Length; ++index)
          this.m_vertices[index] = new MyCompressedVertexNormal()
          {
            Position = halfVector4Array[index]
          };
      }
      this.m_verticesCount = halfVector4Array.Length;
      if (this.m_loadUV)
      {
        this.m_texCoords = (HalfVector2[]) tagData["TexCoords0"];
        this.m_hasUV = true;
        this.m_loadUV = false;
      }
      this.m_meshContainer.Clear();
      if (tagData.ContainsKey("MeshParts"))
      {
        List<int> intList = new List<int>(this.GetVerticesCount());
        int num = 0;
        foreach (MyMeshPartInfo meshInfo in tagData["MeshParts"] as List<MyMeshPartInfo>)
        {
          MyMesh myMesh = new MyMesh(meshInfo, this.m_assetName);
          myMesh.IndexStart = intList.Count;
          myMesh.TriCount = meshInfo.m_indices.Count / 3;
          if (myMesh.TriCount == 0)
            return;
          foreach (int index in meshInfo.m_indices)
          {
            intList.Add(index);
            if (index > num)
              num = index;
          }
          this.m_meshContainer.Add(myMesh);
        }
        if (num <= (int) ushort.MaxValue)
        {
          this.m_Indices_16bit = new ushort[intList.Count];
          for (int index = 0; index < intList.Count; ++index)
            this.m_Indices_16bit[index] = (ushort) intList[index];
        }
        else
          this.m_Indices = intList.ToArray();
      }
      if (tagData.ContainsKey("ModelBvh"))
        this.m_bvh = (IMyTriangePruningStructure) new MyQuantizedBvhAdapter(tagData["ModelBvh"] as GImpactQuantizedBvh, this);
      Vector4I[] vector4IArray = (Vector4I[]) tagData["BlendIndices"];
      Vector4[] vector4Array = (Vector4[]) tagData["BlendWeights"];
      if (vector4IArray != null && vector4IArray.Length != 0 && (vector4Array != null && vector4IArray.Length == vector4Array.Length) && vector4IArray.Length == this.m_vertices.Length)
      {
        this.m_bonesIndicesWeights = new MyCompressedBoneIndicesWeights[vector4IArray.Length];
        for (int index = 0; index < vector4IArray.Length; ++index)
        {
          this.m_bonesIndicesWeights[index].Indices = new Byte4((float) vector4IArray[index].X, (float) vector4IArray[index].Y, (float) vector4IArray[index].Z, (float) vector4IArray[index].W);
          this.m_bonesIndicesWeights[index].Weights = new HalfVector4(vector4Array[index]);
        }
      }
      this.CopyTriangleIndices();
      this.m_trianglesCount = this.Triangles.Length;
    }
  }
}
