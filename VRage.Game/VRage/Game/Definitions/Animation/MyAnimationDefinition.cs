// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.Animation.MyAnimationDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Network;

namespace VRage.Game.Definitions.Animation
{
  [MyDefinitionType(typeof (MyObjectBuilder_AnimationDefinition), null)]
  public class MyAnimationDefinition : MyDefinitionBase
  {
    public string AnimationModel;
    public string AnimationModelFPS;
    public int ClipIndex;
    public string InfluenceArea;
    public string[] InfluenceAreas;
    public bool AllowInCockpit;
    public bool AllowWithWeapon;
    public bool Loop;
    public string[] SupportedSkeletons;
    public MyAnimationDefinition.AnimationStatus Status;
    public MyDefinitionId LeftHandItem;
    public AnimationSet[] AnimationSets;
    public string ChatCommand;
    public string ChatCommandName;
    public string ChatCommandDescription;
    public int Priority;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AnimationDefinition animationDefinition = builder as MyObjectBuilder_AnimationDefinition;
      this.AnimationModel = animationDefinition.AnimationModel;
      this.AnimationModelFPS = animationDefinition.AnimationModelFPS;
      this.ClipIndex = animationDefinition.ClipIndex;
      this.InfluenceArea = animationDefinition.InfluenceArea;
      if (!string.IsNullOrEmpty(animationDefinition.InfluenceArea))
        this.InfluenceAreas = animationDefinition.InfluenceArea.Split(' ');
      this.AllowInCockpit = animationDefinition.AllowInCockpit;
      this.AllowWithWeapon = animationDefinition.AllowWithWeapon;
      if (!string.IsNullOrEmpty(animationDefinition.SupportedSkeletons))
        this.SupportedSkeletons = animationDefinition.SupportedSkeletons.Split(' ');
      this.Loop = animationDefinition.Loop;
      if (!animationDefinition.LeftHandItem.TypeId.IsNull)
        this.LeftHandItem = (MyDefinitionId) animationDefinition.LeftHandItem;
      this.AnimationSets = animationDefinition.AnimationSets;
      this.ChatCommand = animationDefinition.ChatCommand;
      this.ChatCommandName = animationDefinition.ChatCommandName;
      this.ChatCommandDescription = animationDefinition.ChatCommandDescription;
      this.Priority = animationDefinition.Priority;
    }

    public enum AnimationStatus
    {
      Unchecked,
      OK,
      Failed,
    }

    private class VRage_Game_Definitions_Animation_MyAnimationDefinition\u003C\u003EActor : IActivator, IActivator<MyAnimationDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAnimationDefinition();

      MyAnimationDefinition IActivator<MyAnimationDefinition>.CreateInstance() => new MyAnimationDefinition();
    }
  }
}
