// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System.Collections.ObjectModel;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Models
{
  public abstract class MyContractModel : BindableBase
  {
    private string m_nameWithId;
    private string m_name;
    private BitmapImage m_icon;
    private BitmapImage m_header;
    private bool m_isFactionIconPrepared;
    private bool m_shouldFactionIconBeVisible;
    private BitmapImage m_factionIcon;
    private ColorW m_factionIconBackgroundColor;
    private ColorW m_factionIconColor;
    private string m_factionIconTooltip;
    private long m_id;
    private long m_rewardMoney;
    private int m_rewardReputation;
    private long m_startingDeposit;
    private int m_failReputationPrice;
    private double m_timeRemaining;
    private ObservableCollection<MyContractConditionModel> m_conditions;
    private BitmapImage m_currencyIcon;
    private MyContractStateEnum m_state;
    protected MyContractTypeDefinition ContractTypeDefinition;

    public MyDefinitionId? DefinitionId => this.ContractTypeDefinition == null ? new MyDefinitionId?() : new MyDefinitionId?(this.ContractTypeDefinition.Id);

    public string NameWithId
    {
      get => this.m_nameWithId;
      set => this.SetProperty<string>(ref this.m_nameWithId, value, nameof (NameWithId));
    }

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public bool IsFactionIconPrepared
    {
      get => this.m_isFactionIconPrepared;
      set
      {
        this.SetProperty<bool>(ref this.m_isFactionIconPrepared, value, nameof (IsFactionIconPrepared));
        this.RaisePropertyChanged("IsFactionIconVisible");
      }
    }

    public bool ShouldFactionIconBeVisible
    {
      get => this.m_shouldFactionIconBeVisible;
      set
      {
        this.SetProperty<bool>(ref this.m_shouldFactionIconBeVisible, value, nameof (ShouldFactionIconBeVisible));
        this.RaisePropertyChanged("IsFactionIconVisible");
      }
    }

    public bool IsFactionIconVisible => this.IsFactionIconPrepared && this.ShouldFactionIconBeVisible;

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public BitmapImage FactionIcon
    {
      get => this.m_factionIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_factionIcon, value, nameof (FactionIcon));
    }

    public ColorW FactionIconBackgroundColor
    {
      get => this.m_factionIconBackgroundColor;
      set => this.SetProperty<ColorW>(ref this.m_factionIconBackgroundColor, value, nameof (FactionIconBackgroundColor));
    }

    public ColorW FactionIconColor
    {
      get => this.m_factionIconColor;
      set => this.SetProperty<ColorW>(ref this.m_factionIconColor, value, nameof (FactionIconColor));
    }

    public string FactionIconTooltip
    {
      get => this.m_factionIconTooltip;
      set => this.SetProperty<string>(ref this.m_factionIconTooltip, value, nameof (FactionIconTooltip));
    }

    public BitmapImage Header
    {
      get => this.m_header;
      set => this.SetProperty<BitmapImage>(ref this.m_header, value, nameof (Header));
    }

    public long Id
    {
      get => this.m_id;
      set => this.SetProperty<long>(ref this.m_id, value, nameof (Id));
    }

    public long RewardMoney
    {
      get => this.m_rewardMoney;
      set => this.SetProperty<long>(ref this.m_rewardMoney, value, nameof (RewardMoney));
    }

    public string RewardMoney_Formatted => MyBankingSystem.GetFormatedValue(this.RewardMoney);

    public int RewardReputation
    {
      get => this.m_rewardReputation;
      set => this.SetProperty<int>(ref this.m_rewardReputation, value, nameof (RewardReputation));
    }

    public string RewardReputation_Formatted => this.RewardReputation.ToString();

    public long StartingDeposit
    {
      get => this.m_startingDeposit;
      set => this.SetProperty<long>(ref this.m_startingDeposit, value, nameof (StartingDeposit));
    }

    public int FailReputationPrice
    {
      get => this.m_failReputationPrice;
      set => this.SetProperty<int>(ref this.m_failReputationPrice, value, nameof (FailReputationPrice));
    }

    public double RemainingTime
    {
      get => this.m_timeRemaining;
      set
      {
        this.SetProperty<double>(ref this.m_timeRemaining, value, nameof (RemainingTime));
        this.RaisePropertyChanged("TimeLimit_Formated");
      }
    }

    public bool CanBeFinishedInTerminal => this.Conditions.Count > 0;

    public ObservableCollection<MyContractConditionModel> Conditions
    {
      get => this.m_conditions;
      set
      {
        this.SetProperty<ObservableCollection<MyContractConditionModel>>(ref this.m_conditions, value, nameof (Conditions));
        this.RaisePropertyChanged("CanBeFinishedInTerminal");
      }
    }

    public string TimeLimit_Formated
    {
      get
      {
        if (this.RemainingTime <= 0.0)
          return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_No);
        MyTimeSpan myTimeSpan = MyTimeSpan.FromSeconds(this.RemainingTime);
        int minutes = (int) myTimeSpan.Minutes;
        int num1 = minutes / 60;
        int num2 = num1 / 24;
        int num3 = minutes - num1 * 60;
        int num4 = num1 - num2 * 24;
        if (num2 > 0)
          return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Days), (object) num2, (object) num4, (object) num3);
        if (num4 > 0)
          return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Hours), (object) num4, (object) num3);
        return num3 > 0 ? string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Mins), (object) num3) : string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Secs), (object) myTimeSpan.Seconds);
      }
    }

    public string TimeRemaining_Formated
    {
      get
      {
        if (this.m_state != MyContractStateEnum.Active)
          return string.Empty;
        if (this.RemainingTime <= 0.0)
          return MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_No);
        MyTimeSpan myTimeSpan = MyTimeSpan.FromSeconds(this.RemainingTime);
        int minutes = (int) myTimeSpan.Minutes;
        int num1 = minutes / 60;
        int num2 = num1 / 24;
        int num3 = minutes - num1 * 60;
        int num4 = num1 - num2 * 24;
        if (num2 > 0)
          return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Days), (object) num2, (object) num4, (object) num3);
        if (num4 > 0)
          return string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Hours), (object) num4, (object) num3);
        return num3 > 0 ? string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Mins), (object) num3) : string.Format(MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_TimeLimit_Secs), (object) myTimeSpan.Seconds);
      }
    }

    public string TimeLeft => this.m_state == MyContractStateEnum.Active ? this.TimeRemaining_Formated : this.TimeLimit_Formated;

    public string InitialDeposit_Formated => this.StartingDeposit <= 0L ? MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_Deposit_None) : MyBankingSystem.GetFormatedValue(this.StartingDeposit);

    public string FailReputationPenalty_Formated => this.FailReputationPrice <= 0 ? MyTexts.GetString(MySpaceTexts.ContractScreen_Formating_RepPenalty_None) : this.FailReputationPrice.ToString();

    public abstract string Description { get; }

    protected virtual string BuildNameWithId(long id) => string.Format("DefaultContract {0}", (object) id);

    protected virtual string BuildName() => string.Format("DefaultContract");

    protected virtual BitmapImage CreateIcon()
    {
      BitmapImage bitmapImage = new BitmapImage();
      MyContractTypeDefinition contractTypeDefinition = this.ContractTypeDefinition;
      int num1;
      if (contractTypeDefinition == null)
      {
        num1 = 0;
      }
      else
      {
        int? length = contractTypeDefinition.Icons?.Length;
        int num2 = 0;
        num1 = length.GetValueOrDefault() > num2 & length.HasValue ? 1 : 0;
      }
      if (num1 != 0)
        bitmapImage.TextureAsset = this.ContractTypeDefinition.Icons[0];
      return bitmapImage;
    }

    protected virtual BitmapImage CreateHeader()
    {
      BitmapImage bitmapImage = new BitmapImage();
      MyContractTypeDefinition contractTypeDefinition = this.ContractTypeDefinition;
      int num1;
      if (contractTypeDefinition == null)
      {
        num1 = 0;
      }
      else
      {
        int? length = contractTypeDefinition.Icons?.Length;
        int num2 = 1;
        num1 = length.GetValueOrDefault() > num2 & length.HasValue ? 1 : 0;
      }
      if (num1 != 0)
        bitmapImage.TextureAsset = this.ContractTypeDefinition.Icons[1];
      return bitmapImage;
    }

    public static ColorW ConvertVector3ToColorW(Vector3 color) => new ColorW(color.X, color.Y, color.Z);

    protected virtual void PrepareFactionIcon(long factionId, bool showFactionIcons)
    {
      this.ShouldFactionIconBeVisible = showFactionIcons;
      if (!this.ShouldFactionIconBeVisible)
        return;
      if (factionId == 0L)
      {
        this.IsFactionIconPrepared = false;
      }
      else
      {
        IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(factionId);
        if (factionById == null)
        {
          this.IsFactionIconPrepared = false;
        }
        else
        {
          BitmapImage bitmapImage1 = new BitmapImage();
          BitmapImage bitmapImage2 = bitmapImage1;
          MyStringId? factionIcon = factionById.FactionIcon;
          string str;
          if (!factionIcon.HasValue)
          {
            str = "";
          }
          else
          {
            factionIcon = factionById.FactionIcon;
            str = factionIcon.Value.ToString();
          }
          bitmapImage2.TextureAsset = str;
          this.FactionIcon = bitmapImage1;
          this.FactionIconColor = MyContractModel.ConvertVector3ToColorW(MyColorPickerConstants.HSVOffsetToHSV(factionById.IconColor).HsvToRgb());
          this.FactionIconBackgroundColor = MyContractModel.ConvertVector3ToColorW(MyColorPickerConstants.HSVOffsetToHSV(factionById.CustomColor).HsvToRgb());
          this.FactionIconTooltip = string.Format("[{0}] {1}", (object) factionById.Tag, (object) factionById.Name);
          this.IsFactionIconPrepared = true;
        }
      }
    }

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    public virtual void Init(MyObjectBuilder_Contract ob, bool showFactionIcons = true)
    {
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
      this.NameWithId = this.BuildNameWithId(ob.Id);
      this.Name = this.BuildName();
      this.Icon = this.CreateIcon();
      this.Header = this.CreateHeader();
      this.Id = ob.Id;
      this.RewardMoney = ob.RewardMoney;
      this.RewardReputation = ob.RewardReputation;
      this.StartingDeposit = ob.StartingDeposit;
      this.FailReputationPrice = ob.FailReputationPrice;
      this.RemainingTime = ob.RemainingTimeInS.HasValue ? ob.RemainingTimeInS.Value : 0.0;
      this.m_state = ob.State;
      this.PrepareFactionIcon(ob.StartFaction, showFactionIcons);
      ObservableCollection<MyContractConditionModel> observableCollection = new ObservableCollection<MyContractConditionModel>();
      if (ob.ContractCondition != null)
      {
        MyContractConditionModel instance = MyContractConditionModelFactory.CreateInstance(ob.ContractCondition);
        observableCollection.Add(instance);
      }
      this.Conditions = observableCollection;
    }
  }
}
