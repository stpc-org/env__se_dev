// Decompiled with JetBrains decompiler
// Type: VRage.Entities.Components.MyVoxelPostprocessing
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Voxels;

namespace VRage.Entities.Components
{
  public abstract class MyVoxelPostprocessing
  {
    private static MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing> m_objectFactory = new MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing>();

    static MyVoxelPostprocessing() => MyVoxelPostprocessing.m_objectFactory.RegisterFromCreatedObjectAssembly();

    public static MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing> Factory => MyVoxelPostprocessing.m_objectFactory;

    public bool UseForPhysics { get; set; }

    public abstract bool Get(int lod, out VrPostprocessing postprocess);

    protected internal virtual void Init(MyObjectBuilder_VoxelPostprocessing builder) => this.UseForPhysics = builder.ForPhysics;
  }
}
