// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterPickupComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities.Character.Components
{
  [MyComponentType(typeof (MyCharacterPickupComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_CharacterPickupComponent), true)]
  public class MyCharacterPickupComponent : MyCharacterComponent
  {
    public virtual void PickUp()
    {
      MyCharacterDetectorComponent detectorComponent = this.Character.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent == null || detectorComponent.UseObject == null || !detectorComponent.UseObject.IsActionSupported(UseActionEnum.PickUp))
        return;
      if (detectorComponent.UseObject.PlayIndicatorSound)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
        this.Character.SoundComp.StopStateSound();
      }
      detectorComponent.UseObject.Use(UseActionEnum.PickUp, (IMyEntity) this.Character);
    }

    public virtual void PickUpContinues()
    {
      MyCharacterDetectorComponent detectorComponent = this.Character.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent == null || detectorComponent.UseObject == null || (!detectorComponent.UseObject.IsActionSupported(UseActionEnum.PickUp) || !detectorComponent.UseObject.ContinuousUsage))
        return;
      detectorComponent.UseObject.Use(UseActionEnum.PickUp, (IMyEntity) this.Character);
    }

    public virtual void PickUpFinished()
    {
      MyCharacterDetectorComponent detectorComponent = this.Character.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent.UseObject == null || !detectorComponent.UseObject.IsActionSupported(UseActionEnum.UseFinished))
        return;
      detectorComponent.UseObject.Use(UseActionEnum.UseFinished, (IMyEntity) this.Character);
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterPickupComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterPickupComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterPickupComponent();

      MyCharacterPickupComponent IActivator<MyCharacterPickupComponent>.CreateInstance() => new MyCharacterPickupComponent();
    }
  }
}
