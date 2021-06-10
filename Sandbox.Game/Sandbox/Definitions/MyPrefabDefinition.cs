// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPrefabDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PrefabDefinition), null)]
  public class MyPrefabDefinition : MyDefinitionBase
  {
    private MyObjectBuilder_CubeGrid[] m_cubeGrids;
    private BoundingSphere m_boundingSphere;
    private BoundingBox m_boundingBox;
    public string PrefabPath;
    public bool Initialized;

    public MyObjectBuilder_CubeGrid[] CubeGrids
    {
      get
      {
        if (!this.Initialized)
          MyDefinitionManager.Static.ReloadPrefabsFromFile(this.PrefabPath);
        return this.m_cubeGrids;
      }
    }

    public BoundingSphere BoundingSphere
    {
      get
      {
        if (!this.Initialized)
          MyDefinitionManager.Static.ReloadPrefabsFromFile(this.PrefabPath);
        return this.m_boundingSphere;
      }
    }

    public BoundingBox BoundingBox
    {
      get
      {
        if (!this.Initialized)
          MyDefinitionManager.Static.ReloadPrefabsFromFile(this.PrefabPath);
        return this.m_boundingBox;
      }
    }

    public MyEnvironmentTypes EnvironmentType { get; set; }

    public string TooltipImage { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase baseBuilder)
    {
      base.Init(baseBuilder);
      this.PrefabPath = (baseBuilder as MyObjectBuilder_PrefabDefinition).PrefabPath;
      this.Initialized = false;
    }

    public void InitLazy(MyObjectBuilder_DefinitionBase baseBuilder)
    {
      MyObjectBuilder_PrefabDefinition prefabDefinition = baseBuilder as MyObjectBuilder_PrefabDefinition;
      this.Icons = prefabDefinition.Icons;
      this.DisplayNameString = baseBuilder.DisplayName;
      this.DescriptionString = baseBuilder.Description;
      this.EnvironmentType = prefabDefinition.EnvironmentType;
      this.TooltipImage = prefabDefinition.TooltipImage;
      if (prefabDefinition.CubeGrid == null && prefabDefinition.CubeGrids == null)
        return;
      if (prefabDefinition.CubeGrid != null)
        this.m_cubeGrids = new MyObjectBuilder_CubeGrid[1]
        {
          prefabDefinition.CubeGrid
        };
      else
        this.m_cubeGrids = prefabDefinition.CubeGrids;
      this.m_boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
      this.m_boundingBox = BoundingBox.CreateInvalid();
      foreach (MyObjectBuilder_CubeGrid cubeGrid in this.m_cubeGrids)
      {
        BoundingBox boundingBox = cubeGrid.CalculateBoundingBox();
        Matrix matrix1;
        if (!cubeGrid.PositionAndOrientation.HasValue)
        {
          matrix1 = Matrix.Identity;
        }
        else
        {
          MatrixD matrix2 = cubeGrid.PositionAndOrientation.Value.GetMatrix();
          matrix1 = (Matrix) ref matrix2;
        }
        Matrix worldMatrix = matrix1;
        this.m_boundingBox.Include(boundingBox.Transform(worldMatrix));
      }
      this.m_boundingSphere = BoundingSphere.CreateFromBoundingBox(this.m_boundingBox);
      foreach (MyObjectBuilder_CubeGrid cubeGrid in this.m_cubeGrids)
      {
        cubeGrid.CreatePhysics = true;
        cubeGrid.XMirroxPlane = new SerializableVector3I?();
        cubeGrid.YMirroxPlane = new SerializableVector3I?();
        cubeGrid.ZMirroxPlane = new SerializableVector3I?();
      }
      this.Initialized = true;
    }

    private class Sandbox_Definitions_MyPrefabDefinition\u003C\u003EActor : IActivator, IActivator<MyPrefabDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPrefabDefinition();

      MyPrefabDefinition IActivator<MyPrefabDefinition>.CreateInstance() => new MyPrefabDefinition();
    }
  }
}
