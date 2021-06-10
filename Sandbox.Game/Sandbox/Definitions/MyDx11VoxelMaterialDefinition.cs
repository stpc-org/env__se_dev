// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDx11VoxelMaterialDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Medieval.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_Dx11VoxelMaterialDefinition), null)]
  public class MyDx11VoxelMaterialDefinition : MyVoxelMaterialDefinition
  {
    public new string VoxelHandPreview;

    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      this.VoxelHandPreview = ((MyObjectBuilder_VoxelMaterialDefinition) ob).VoxelHandPreview;
    }

    private class Sandbox_Definitions_MyDx11VoxelMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyDx11VoxelMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDx11VoxelMaterialDefinition();

      MyDx11VoxelMaterialDefinition IActivator<MyDx11VoxelMaterialDefinition>.CreateInstance() => new MyDx11VoxelMaterialDefinition();
    }
  }
}
