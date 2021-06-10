// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubePart
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyCubePart
  {
    public MyCubeInstanceData InstanceData;
    public MyModel Model;
    public MyStringHash SkinSubtypeId;

    public void Init(MyModel model, MyStringHash skinSubtypeId, Matrix matrix, float rescaleModel = 1f)
    {
      this.Model = model;
      model.Rescale(rescaleModel);
      model.LoadData();
      this.SkinSubtypeId = skinSubtypeId;
      this.InstanceData.LocalMatrix = matrix;
    }
  }
}
