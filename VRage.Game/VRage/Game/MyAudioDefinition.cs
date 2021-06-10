// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyAudioDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Data.Audio;
using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_AudioDefinition), null)]
  public class MyAudioDefinition : MyDefinitionBase
  {
    public MySoundData SoundData;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.SoundData = (builder as MyObjectBuilder_AudioDefinition).SoundData;
      this.SoundData.SubtypeId = this.Id.SubtypeId;
      if (!this.SoundData.Loopable)
        return;
      bool flag = true;
      for (int index = 0; index < this.SoundData.Waves.Count; ++index)
        flag &= this.SoundData.Waves[index].Loop != null;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_AudioDefinition objectBuilder = (MyObjectBuilder_AudioDefinition) base.GetObjectBuilder();
      objectBuilder.SoundData = this.SoundData;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class VRage_Game_MyAudioDefinition\u003C\u003EActor : IActivator, IActivator<MyAudioDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAudioDefinition();

      MyAudioDefinition IActivator<MyAudioDefinition>.CreateInstance() => new MyAudioDefinition();
    }
  }
}
