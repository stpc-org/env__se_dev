// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySoundCategoryDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SoundCategoryDefinition), null)]
  public class MySoundCategoryDefinition : MyDefinitionBase
  {
    public List<MySoundCategoryDefinition.SoundDescription> Sounds;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SoundCategoryDefinition categoryDefinition = builder as MyObjectBuilder_SoundCategoryDefinition;
      this.Sounds = new List<MySoundCategoryDefinition.SoundDescription>();
      if (categoryDefinition.Sounds == null)
        return;
      foreach (MyObjectBuilder_SoundCategoryDefinition.SoundDesc sound in categoryDefinition.Sounds)
      {
        MyStringId orCompute = MyStringId.GetOrCompute(sound.SoundName);
        if (MyTexts.Exists(orCompute))
          this.Sounds.Add(new MySoundCategoryDefinition.SoundDescription(sound.Id, sound.SoundName, new MyStringId?(orCompute)));
        else
          this.Sounds.Add(new MySoundCategoryDefinition.SoundDescription(sound.Id, sound.SoundName, new MyStringId?()));
      }
    }

    public class SoundDescription
    {
      public string SoundId;
      public string SoundName;
      public MyStringId? SoundNameEnum;

      public SoundDescription(string soundId, string soundName, MyStringId? soundNameEnum)
      {
        this.SoundId = soundId;
        this.SoundName = soundName;
        this.SoundNameEnum = soundNameEnum;
      }

      public string SoundText => !this.SoundNameEnum.HasValue ? this.SoundName : MyTexts.GetString(this.SoundNameEnum.Value);
    }

    private class Sandbox_Definitions_MySoundCategoryDefinition\u003C\u003EActor : IActivator, IActivator<MySoundCategoryDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySoundCategoryDefinition();

      MySoundCategoryDefinition IActivator<MySoundCategoryDefinition>.CreateInstance() => new MySoundCategoryDefinition();
    }
  }
}
