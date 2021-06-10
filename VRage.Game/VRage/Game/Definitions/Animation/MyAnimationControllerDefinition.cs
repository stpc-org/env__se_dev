// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.Animation.MyAnimationControllerDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Definitions.Animation
{
  [MyDefinitionType(typeof (MyObjectBuilder_AnimationControllerDefinition), typeof (MyAnimationControllerDefinitionPostprocess))]
  public class MyAnimationControllerDefinition : MyDefinitionBase
  {
    public List<MyObjectBuilder_AnimationLayer> Layers = new List<MyObjectBuilder_AnimationLayer>();
    public List<MyObjectBuilder_AnimationSM> StateMachines = new List<MyObjectBuilder_AnimationSM>();
    public List<MyObjectBuilder_AnimationFootIkChain> FootIkChains = new List<MyObjectBuilder_AnimationFootIkChain>();
    public List<string> IkIgnoredBones = new List<string>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AnimationControllerDefinition controllerDefinition = builder as MyObjectBuilder_AnimationControllerDefinition;
      if (controllerDefinition.Layers != null)
        this.Layers.AddRange((IEnumerable<MyObjectBuilder_AnimationLayer>) controllerDefinition.Layers);
      if (controllerDefinition.StateMachines != null)
        this.StateMachines.AddRange((IEnumerable<MyObjectBuilder_AnimationSM>) controllerDefinition.StateMachines);
      if (controllerDefinition.FootIkChains != null)
        this.FootIkChains.AddRange((IEnumerable<MyObjectBuilder_AnimationFootIkChain>) controllerDefinition.FootIkChains);
      if (controllerDefinition.IkIgnoredBones == null)
        return;
      this.IkIgnoredBones.AddRange((IEnumerable<string>) controllerDefinition.IkIgnoredBones);
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_AnimationControllerDefinition objectBuilder = MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_AnimationControllerDefinition>((MyDefinitionBase) this);
      objectBuilder.Id = (SerializableDefinitionId) this.Id;
      MyStringId myStringId;
      string descriptionString;
      if (!this.DescriptionEnum.HasValue)
      {
        descriptionString = this.DescriptionString;
      }
      else
      {
        myStringId = this.DescriptionEnum.Value;
        descriptionString = myStringId.ToString();
      }
      objectBuilder.Description = descriptionString;
      string displayNameString;
      if (!this.DisplayNameEnum.HasValue)
      {
        displayNameString = this.DisplayNameString;
      }
      else
      {
        myStringId = this.DisplayNameEnum.Value;
        displayNameString = myStringId.ToString();
      }
      objectBuilder.DisplayName = displayNameString;
      objectBuilder.Icons = this.Icons;
      objectBuilder.Public = this.Public;
      objectBuilder.Enabled = this.Enabled;
      objectBuilder.AvailableInSurvival = this.AvailableInSurvival;
      objectBuilder.StateMachines = this.StateMachines.ToArray();
      objectBuilder.Layers = this.Layers.ToArray();
      objectBuilder.FootIkChains = this.FootIkChains.ToArray();
      objectBuilder.IkIgnoredBones = this.IkIgnoredBones.ToArray();
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    public void Clear()
    {
      this.Layers.Clear();
      this.StateMachines.Clear();
      this.FootIkChains.Clear();
      this.IkIgnoredBones.Clear();
    }

    private class VRage_Game_Definitions_Animation_MyAnimationControllerDefinition\u003C\u003EActor : IActivator, IActivator<MyAnimationControllerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAnimationControllerDefinition();

      MyAnimationControllerDefinition IActivator<MyAnimationControllerDefinition>.CreateInstance() => new MyAnimationControllerDefinition();
    }
  }
}
