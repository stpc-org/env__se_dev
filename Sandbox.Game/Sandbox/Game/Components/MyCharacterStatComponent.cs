// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyCharacterStatComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using VRage;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_CharacterStatComponent), true)]
  public class MyCharacterStatComponent : MyEntityStatComponent
  {
    public static MyStringHash HealthId = MyStringHash.GetOrCompute(nameof (Health));
    public MyDamageInformation LastDamage;
    public static readonly float HEALTH_RATIO_CRITICAL = 0.2f;
    public static readonly float HEALTH_RATIO_LOW = 0.4f;
    private MyCharacter m_character;

    public MyEntityStat Health
    {
      get
      {
        MyEntityStat result;
        return this.Stats.TryGetValue(MyCharacterStatComponent.HealthId, out result) ? result : (MyEntityStat) null;
      }
    }

    public float HealthRatio
    {
      get
      {
        float num = 1f;
        MyEntityStat health = this.Health;
        if (health != null)
          num = health.Value / health.MaxValue;
        return num;
      }
    }

    public override void Update()
    {
      if (this.m_character != null && this.m_character.IsDead)
      {
        foreach (MyEntityStat stat in this.Stats)
          stat.ClearEffects();
        this.m_scripts.Clear();
      }
      base.Update();
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_character = this.Container.Entity as MyCharacter;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      this.m_character = (MyCharacter) null;
      base.OnBeforeRemovedFromContainer();
    }

    public void OnHealthChanged(float newHealth, float oldHealth, object statChangeData)
    {
      if (this.m_character == null || !this.m_character.CharacterCanDie)
        return;
      this.m_character.ForceUpdateBreath();
      if ((double) newHealth >= (double) oldHealth)
        return;
      this.OnDamage(newHealth, oldHealth);
    }

    private void OnDamage(float newHealth, float oldHealth)
    {
      if (this.m_character == null || this.m_character.IsDead)
        return;
      this.m_character.SoundComp.PlayDamageSound(oldHealth);
      this.m_character.Render.Damage();
    }

    public void DoDamage(float damage, object statChangeData = null)
    {
      MyEntityStat health = this.Health;
      if (health == null)
        return;
      if (this.m_character != null)
        this.m_character.CharacterAccumulatedDamage += damage;
      if (statChangeData is MyDamageInformation damageInformation)
        this.LastDamage = damageInformation;
      health.Decrease(damage, statChangeData);
    }

    public void Consume(MyFixedPoint amount, MyConsumableItemDefinition definition)
    {
      if (definition == null)
        return;
      MyObjectBuilder_EntityStatRegenEffect objectBuilder = new MyObjectBuilder_EntityStatRegenEffect();
      objectBuilder.Interval = 1f;
      objectBuilder.MaxRegenRatio = 1f;
      objectBuilder.MinRegenRatio = 0.0f;
      foreach (MyConsumableItemDefinition.StatValue stat in definition.Stats)
      {
        MyEntityStat result;
        if (this.Stats.TryGetValue(MyStringHash.GetOrCompute(stat.Name), out result))
        {
          objectBuilder.TickAmount = stat.Value * (float) amount;
          objectBuilder.Duration = stat.Time;
          result.AddEffect(objectBuilder);
        }
      }
    }

    private class Sandbox_Game_Components_MyCharacterStatComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterStatComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterStatComponent();

      MyCharacterStatComponent IActivator<MyCharacterStatComponent>.CreateInstance() => new MyCharacterStatComponent();
    }
  }
}
