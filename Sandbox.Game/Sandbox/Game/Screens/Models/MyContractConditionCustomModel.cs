// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractConditionCustomModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.Models
{
  [MyContractConditionModelDescriptor(typeof (MyObjectBuilder_ContractConditionCustom))]
  public class MyContractConditionCustomModel : MyContractConditionModel
  {
    public override void Init(MyObjectBuilder_ContractCondition ob)
    {
      base.Init(ob);
      MyObjectBuilder_ContractConditionCustom contractConditionCustom = ob as MyObjectBuilder_ContractConditionCustom;
    }

    protected override string BuildName(long id) => string.Empty;

    protected override BitmapImage BuildImage() => new BitmapImage()
    {
      TextureAsset = "Textures\\GUI\\Icons\\WeaponWelder_1.dds"
    };
  }
}
