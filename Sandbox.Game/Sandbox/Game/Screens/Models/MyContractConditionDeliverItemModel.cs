// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractConditionDeliverItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Definitions;
using Sandbox.Game.Localization;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Utils;

namespace Sandbox.Game.Screens.Models
{
  [MyContractConditionModelDescriptor(typeof (MyObjectBuilder_ContractConditionDeliverItems))]
  public class MyContractConditionDeliverItemModel : MyContractConditionModel
  {
    public MyDefinitionId m_itemType;
    public int m_itemAmount;
    public float m_itemVolume;

    public MyDefinitionId ItemType
    {
      get => this.m_itemType;
      set
      {
        this.SetProperty<MyDefinitionId>(ref this.m_itemType, value, nameof (ItemType));
        this.RaisePropertyChanged("ItemType_Formated");
      }
    }

    public int ItemAmount
    {
      get => this.m_itemAmount;
      set => this.SetProperty<int>(ref this.m_itemAmount, value, nameof (ItemAmount));
    }

    public float ItemVolume
    {
      get => this.m_itemVolume;
      set
      {
        this.SetProperty<float>(ref this.m_itemVolume, value, nameof (ItemVolume));
        this.RaisePropertyChanged("ItemVolume_Formated");
      }
    }

    public override void Init(MyObjectBuilder_ContractCondition ob)
    {
      if (ob is MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems)
      {
        this.ItemType = (MyDefinitionId) conditionDeliverItems.ItemType;
        this.ItemAmount = conditionDeliverItems.ItemAmount;
        this.ItemVolume = conditionDeliverItems.ItemVolume;
      }
      base.Init(ob);
    }

    public string ItemType_Formated => this.ItemType.TypeId.ToString() + "/" + this.ItemType.SubtypeName;

    public string ItemVolume_Formated => string.Format(MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue), (object) MyValueFormatter.GetFormatedFloat(1000f * this.ItemVolume, 2, ","));

    protected override string BuildName(long id)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(this.ItemType);
      return physicalItemDefinition == null ? string.Empty : physicalItemDefinition.DisplayNameText;
    }

    protected override BitmapImage BuildImage()
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(this.ItemType);
      if (physicalItemDefinition == null)
        return (BitmapImage) null;
      return new BitmapImage()
      {
        TextureAsset = physicalItemDefinition.Icons[0]
      };
    }
  }
}
