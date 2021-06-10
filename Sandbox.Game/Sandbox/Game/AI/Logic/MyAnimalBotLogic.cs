// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Logic.MyAnimalBotLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.Navigation;
using VRageMath;

namespace Sandbox.Game.AI.Logic
{
  public class MyAnimalBotLogic : MyAgentLogic
  {
    private readonly MyCharacterAvoidanceSteering m_characterAvoidanceSteering;

    public MyAnimalBot AnimalBot => this.m_bot as MyAnimalBot;

    public MyAnimalBotLogic(MyAnimalBot bot)
      : base((IMyBot) bot)
    {
      MyBotNavigation navigation = this.AnimalBot.Navigation;
      navigation.AddSteering((MySteeringBase) new MyTreeAvoidance(navigation, 0.1f));
      this.m_characterAvoidanceSteering = new MyCharacterAvoidanceSteering(navigation, 1f);
      navigation.AddSteering((MySteeringBase) this.m_characterAvoidanceSteering);
      navigation.MaximumRotationAngle = new float?(MathHelper.ToRadians(23f));
    }

    public void EnableCharacterAvoidance(bool isTrue)
    {
      MyBotNavigation navigation = this.AnimalBot.Navigation;
      bool flag = navigation.HasSteeringOfType(this.m_characterAvoidanceSteering.GetType());
      if (isTrue && !flag)
      {
        navigation.AddSteering((MySteeringBase) this.m_characterAvoidanceSteering);
      }
      else
      {
        if (!(!isTrue & flag))
          return;
        navigation.RemoveSteering((MySteeringBase) this.m_characterAvoidanceSteering);
      }
    }

    public override BotType BotType => BotType.ANIMAL;
  }
}
