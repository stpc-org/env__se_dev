// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyAdminSelectionItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.Models
{
  public class MyAdminSelectionItemModel : BindableBase
  {
    public static readonly int NAME_MAX_ALLOWED_LENGTH = 25;
    public static readonly int NAME_MAX_CUT_OFF_LENGTH = 11;
    public static readonly int FORMATER_ADDED_LENGTH = 3;
    private string m_namePrefix;
    private string m_nameSuffix;
    private long m_id;

    public string NamePrefix
    {
      get => this.m_namePrefix;
      set
      {
        this.SetProperty<string>(ref this.m_namePrefix, value, nameof (NamePrefix));
        this.RaisePropertyChanged("NameCombined");
      }
    }

    public string NameSuffix
    {
      get => this.m_nameSuffix;
      set
      {
        this.SetProperty<string>(ref this.m_nameSuffix, value, nameof (NameSuffix));
        this.RaisePropertyChanged("NameCombined");
        this.RaisePropertyChanged("NameCombinedShort");
      }
    }

    public long Id
    {
      get => this.m_id;
      set => this.SetProperty<long>(ref this.m_id, value, nameof (Id));
    }

    public string NameCombined => string.IsNullOrEmpty(this.NamePrefix) ? (string.IsNullOrEmpty(this.NameSuffix) ? string.Empty : this.NameSuffix) : (string.IsNullOrEmpty(this.NameSuffix) ? this.NamePrefix : string.Format("{0} - {1}", (object) this.NamePrefix, (object) this.NameSuffix));

    public string NameCombinedShort
    {
      get
      {
        if (string.IsNullOrEmpty(this.NamePrefix))
          return string.IsNullOrEmpty(this.NameSuffix) ? string.Empty : this.NameSuffix;
        if (string.IsNullOrEmpty(this.NameSuffix))
          return this.NamePrefix;
        if (this.NamePrefix.Length + this.NameSuffix.Length + MyAdminSelectionItemModel.FORMATER_ADDED_LENGTH <= MyAdminSelectionItemModel.NAME_MAX_ALLOWED_LENGTH)
          return string.Format("{0} - {1}", (object) this.NamePrefix, (object) this.NameSuffix);
        if (this.NamePrefix.Length < MyAdminSelectionItemModel.NAME_MAX_CUT_OFF_LENGTH)
          return string.Format("{0} - {1}", (object) this.NamePrefix, (object) this.NameSuffix.Substring(0, MyAdminSelectionItemModel.NAME_MAX_ALLOWED_LENGTH - MyAdminSelectionItemModel.FORMATER_ADDED_LENGTH - this.NamePrefix.Length));
        if (this.NameSuffix.Length < MyAdminSelectionItemModel.NAME_MAX_CUT_OFF_LENGTH)
          return string.Format("{0} - {1}", (object) this.NamePrefix.Substring(0, MyAdminSelectionItemModel.NAME_MAX_ALLOWED_LENGTH - MyAdminSelectionItemModel.FORMATER_ADDED_LENGTH - this.NameSuffix.Length), (object) this.NameSuffix);
        int maxAllowedLength = MyAdminSelectionItemModel.NAME_MAX_ALLOWED_LENGTH;
        int formaterAddedLength = MyAdminSelectionItemModel.FORMATER_ADDED_LENGTH;
        int length = this.NameSuffix.Length;
        return string.Format("{0} - {1}", (object) this.NamePrefix.Substring(0, MyAdminSelectionItemModel.NAME_MAX_CUT_OFF_LENGTH), (object) this.NameSuffix.Substring(0, MyAdminSelectionItemModel.NAME_MAX_CUT_OFF_LENGTH));
      }
    }

    public MyAdminSelectionItemModel(string prefix, string suffix, long id)
    {
      this.NamePrefix = prefix;
      this.NameSuffix = suffix;
      this.Id = id;
    }
  }
}
