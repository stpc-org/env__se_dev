// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyToolBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public class MyToolBase : MyDeviceBase
  {
    protected Vector3 m_positionMuzzleLocal;
    protected Vector3D m_positionMuzzleWorld;

    public MyToolBase()
      : this(Vector3.Zero, MatrixD.Identity)
    {
    }

    public MyToolBase(Vector3 localMuzzlePosition, MatrixD matrix)
    {
      this.m_positionMuzzleLocal = localMuzzlePosition;
      this.OnWorldPositionChanged(matrix);
    }

    public void OnWorldPositionChanged(MatrixD matrix) => this.m_positionMuzzleWorld = Vector3D.Transform(this.m_positionMuzzleLocal, matrix);

    public override bool CanSwitchAmmoMagazine() => false;

    public override bool SwitchToNextAmmoMagazine() => false;

    public override bool SwitchAmmoMagazineToNextAvailable() => false;

    public override Vector3D GetMuzzleLocalPosition() => (Vector3D) this.m_positionMuzzleLocal;

    public override Vector3D GetMuzzleWorldPosition() => this.m_positionMuzzleWorld;

    public MyObjectBuilder_ToolBase GetObjectBuilder()
    {
      MyObjectBuilder_ToolBase newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolBase>();
      newObject.InventoryItemId = this.InventoryItemId;
      return newObject;
    }

    public void Init(MyObjectBuilder_ToolBase objectBuilder) => this.Init((MyObjectBuilder_DeviceBase) objectBuilder);
  }
}
