// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [GenerateActivator]
  [MyDefinitionType(typeof (MyObjectBuilder_DefinitionBase), null)]
  public class MyDefinitionBase
  {
    public MyDefinitionId Id;
    public MyStringId? DisplayNameEnum;
    public MyStringId? DescriptionEnum;
    public string DisplayNameString;
    public string DescriptionString;
    public string DescriptionArgs;
    public string[] Icons;
    public bool Enabled = true;
    public bool Public = true;
    public bool AvailableInSurvival;
    public MyModContext Context;

    public string[] DLCs { get; private set; }

    public virtual string DisplayNameText => !this.DisplayNameEnum.HasValue ? this.DisplayNameString : MyTexts.GetString(this.DisplayNameEnum.Value);

    public virtual string DescriptionText => !this.DescriptionEnum.HasValue ? this.DescriptionString : MyTexts.GetString(this.DescriptionEnum.Value);

    public void Init(MyObjectBuilder_DefinitionBase builder, MyModContext modContext)
    {
      this.Context = modContext;
      this.Init(builder);
    }

    protected virtual void Init(MyObjectBuilder_DefinitionBase builder)
    {
      this.Id = (MyDefinitionId) builder.Id;
      this.Public = builder.Public;
      this.Enabled = builder.Enabled;
      this.AvailableInSurvival = builder.AvailableInSurvival;
      this.Icons = builder.Icons;
      this.DescriptionArgs = builder.DescriptionArgs;
      this.DLCs = builder.DLCs;
      string displayName = builder.DisplayName;
      if (IsTextEnumKey(displayName, "DisplayName_"))
        this.DisplayNameEnum = new MyStringId?(MyStringId.GetOrCompute(displayName));
      else
        this.DisplayNameString = displayName;
      string description = builder.Description;
      if (IsTextEnumKey(description, "Description_"))
        this.DescriptionEnum = new MyStringId?(MyStringId.GetOrCompute(description));
      else
        this.DescriptionString = description;

      bool IsTextEnumKey(string str, string pattern)
      {
        if (string.IsNullOrEmpty(str))
          return false;
        if (str.StartsWith(pattern))
          return true;
        return str.Contains(pattern) && MyTexts.MatchesReplaceFormat(str);
      }
    }

    [Obsolete("Prefer to use MyDefinitionPostprocessor instead.")]
    public virtual void Postprocess()
    {
    }

    public void Save(string filepath) => this.GetObjectBuilder().Save(filepath);

    public virtual MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_DefinitionBase objectBuilder = MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_DefinitionBase>(this);
      objectBuilder.Id = (SerializableDefinitionId) this.Id;
      MyStringId myStringId;
      string str1;
      if (!this.DescriptionEnum.HasValue)
      {
        str1 = this.DescriptionString != null ? this.DescriptionString.ToString() : (string) null;
      }
      else
      {
        myStringId = this.DescriptionEnum.Value;
        str1 = myStringId.ToString();
      }
      objectBuilder.Description = str1;
      string str2;
      if (!this.DisplayNameEnum.HasValue)
      {
        str2 = this.DisplayNameString != null ? this.DisplayNameString.ToString() : (string) null;
      }
      else
      {
        myStringId = this.DisplayNameEnum.Value;
        str2 = myStringId.ToString();
      }
      objectBuilder.DisplayName = str2;
      objectBuilder.Icons = this.Icons;
      objectBuilder.Public = this.Public;
      objectBuilder.Enabled = this.Enabled;
      objectBuilder.DescriptionArgs = this.DescriptionArgs;
      objectBuilder.AvailableInSurvival = this.AvailableInSurvival;
      return objectBuilder;
    }

    public override string ToString() => this.Id.ToString();

    private class VRage_Game_MyDefinitionBase\u003C\u003EActor : IActivator, IActivator<MyDefinitionBase>
    {
      object IActivator.CreateInstance() => (object) new MyDefinitionBase();

      MyDefinitionBase IActivator<MyDefinitionBase>.CreateInstance() => new MyDefinitionBase();
    }
  }
}
