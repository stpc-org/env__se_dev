// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyParticleEffect
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.ComponentModel;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Render.Particles;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game
{
  [GenerateActivator]
  public class MyParticleEffect
  {
    private MyParticleEffectData m_data;
    private MyParticleEffectState m_state;
    private float m_elapsedTime;
    private MyTimeSpan m_lastTime;

    public event Action<MyParticleEffect> OnDelete;

    public float UserScale
    {
      get => this.m_state.UserScale;
      set
      {
        if ((double) this.m_state.UserScale == (double) value)
          return;
        this.m_state.UserScale = value;
        this.m_state.TransformDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public bool Autodelete
    {
      get => this.m_state.Autodelete;
      set
      {
        if (this.m_state.Autodelete == value)
          return;
        this.m_state.Autodelete = value;
        this.m_state.Dirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public void SetDirty() => this.m_state.Dirty = true;

    [Obsolete("Use UserScale instead.")]
    public float UserEmitterScale
    {
      get => this.UserScale;
      set => this.UserScale = value;
    }

    public float DistanceMax => this.m_data.DistanceMax;

    public float DurationMax => this.m_data.DurationMax;

    public bool Loop => this.m_data.Loop;

    public float UserBirthMultiplier
    {
      get => this.m_state.UserBirthMultiplier;
      set
      {
        if ((double) this.m_state.UserBirthMultiplier == (double) value)
          return;
        this.m_state.UserBirthMultiplier = value;
        this.m_state.UserDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float UserVelocityMultiplier
    {
      get => this.m_state.UserVelocityMultiplier;
      set
      {
        if ((double) this.m_state.UserVelocityMultiplier == (double) value)
          return;
        this.m_state.UserVelocityMultiplier = value;
        this.m_state.UserDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float UserColorIntensityMultiplier
    {
      get => this.m_state.UserColorIntensityMultiplier;
      set
      {
        if ((double) this.m_state.UserColorIntensityMultiplier == (double) value)
          return;
        this.m_state.UserColorIntensityMultiplier = value;
        this.m_state.UserDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float UserLifeMultiplier
    {
      get => this.m_state.UserLifeMultiplier;
      set
      {
        if ((double) this.m_state.UserLifeMultiplier == (double) value)
          return;
        this.m_state.UserLifeMultiplier = value;
        this.m_state.UserDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float CameraSoftRadiusMultiplier
    {
      get => this.m_state.CameraSoftRadiusMultiplier;
      set
      {
        if ((double) this.m_state.CameraSoftRadiusMultiplier == (double) value)
          return;
        this.m_state.CameraSoftRadiusMultiplier = value;
        this.m_state.Dirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float SoftParticleDistanceScaleMultiplier
    {
      get => this.m_state.SoftParticleDistanceScaleMultiplier;
      set
      {
        if ((double) this.m_state.SoftParticleDistanceScaleMultiplier == (double) value)
          return;
        this.m_state.SoftParticleDistanceScaleMultiplier = value;
        this.m_state.Dirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    public float UserRadiusMultiplier
    {
      get => this.m_state.UserRadiusMultiplier;
      set
      {
        if ((double) this.m_state.UserRadiusMultiplier == (double) value)
          return;
        this.m_state.UserDirty = true;
        this.m_state.UserRadiusMultiplier = value;
        this.ScheduleUpdateInRender();
      }
    }

    public float UserFadeMultiplier
    {
      get => this.m_state.UserFadeMultiplier;
      set
      {
        if ((double) this.m_state.UserFadeMultiplier == (double) value)
          return;
        this.m_state.UserDirty = true;
        this.m_state.UserFadeMultiplier = value;
        this.ScheduleUpdateInRender();
      }
    }

    public Vector4 UserColorMultiplier
    {
      get => this.m_state.UserColorMultiplier;
      set
      {
        if (!(this.m_state.UserColorMultiplier != value))
          return;
        this.m_state.UserDirty = true;
        this.m_state.UserColorMultiplier = value;
        this.ScheduleUpdateInRender();
      }
    }

    public Vector3 Velocity
    {
      set
      {
        if (this.m_state.Velocity.HasValue)
        {
          Vector3? velocity = this.m_state.Velocity;
          Vector3 vector3 = value;
          if ((velocity.HasValue ? (velocity.HasValue ? (velocity.GetValueOrDefault() != vector3 ? 1 : 0) : 0) : 1) == 0)
            return;
        }
        this.m_state.Velocity = new Vector3?(value);
        this.ScheduleUpdateInRender();
      }
    }

    public float GetElapsedTime() => this.m_elapsedTime;

    [Browsable(false)]
    public MatrixD WorldMatrix
    {
      get => this.m_state.WorldMatrix;
      set
      {
        if (value.EqualsFast(ref this.m_state.WorldMatrix, 0.001))
          return;
        this.m_state.WorldMatrix = value;
        this.m_state.TransformDirty = true;
        this.ScheduleUpdateInRender();
      }
    }

    [Browsable(false)]
    public bool IsStopped => this.m_state.IsStopped;

    public bool IsEmittingStopped => this.m_state.IsEmittingStopped;

    public uint Id => this.m_state.ID;

    public MyParticleEffectData Data => this.m_data;

    public void Init(MyParticleEffectData data, ref MatrixD effectMatrix, uint parentId)
    {
      this.m_lastTime = MyParticlesManager.CurrentTime;
      this.m_data = data;
      this.m_state = new MyParticleEffectState()
      {
        ID = MyRenderProxy.CreateParticleEffect(this.m_data, data.Name),
        Dirty = true,
        AnimDirty = true,
        ParentID = parentId,
        UserBirthMultiplier = 1f,
        UserRadiusMultiplier = 1f,
        UserVelocityMultiplier = 1f,
        UserColorIntensityMultiplier = 1f,
        UserLifeMultiplier = 1f,
        CameraSoftRadiusMultiplier = 1f,
        SoftParticleDistanceScaleMultiplier = 1f,
        UserColorMultiplier = Vector4.One,
        UserScale = 1f,
        WorldMatrix = effectMatrix,
        IsStopped = false,
        IsSimulationPaused = false,
        IsEmittingStopped = false,
        InstantStop = false,
        Timer = 0.0f,
        Autodelete = true,
        UserFadeMultiplier = 1f
      };
      this.m_elapsedTime = 0.0f;
      this.Update();
    }

    public void Stop(bool instant = true)
    {
      this.m_state.IsStopped = true;
      this.m_state.IsEmittingStopped = true;
      this.m_state.IsSimulationPaused = false;
      this.m_state.InstantStop = instant;
      this.m_state.Dirty = true;
      this.m_elapsedTime = 0.0f;
      this.ScheduleUpdateInRender();
    }

    public void Play()
    {
      this.m_lastTime = MyParticlesManager.CurrentTime;
      this.m_state.IsStopped = false;
      this.m_state.IsSimulationPaused = false;
      this.m_state.IsEmittingStopped = false;
      this.m_state.Dirty = true;
      this.m_state.Timer = 0.0f;
      this.ScheduleUpdateInRender();
    }

    public void Pause()
    {
      this.m_state.IsSimulationPaused = true;
      this.m_state.IsEmittingStopped = true;
      this.m_state.Dirty = true;
      this.ScheduleUpdateInRender();
    }

    public void StopEmitting(float timeout = 0.0f)
    {
      this.m_state.IsEmittingStopped = true;
      this.m_state.Timer = timeout;
      this.m_state.Dirty = true;
      this.ScheduleUpdateInRender();
    }

    public void StopLights()
    {
      this.m_state.StopLights = true;
      this.ScheduleUpdateInRender();
    }

    [Obsolete("SetTranslation(Vector3D) is deprecated, please use SetTranslation(ref Vector3D) instead.")]
    public void SetTranslation(Vector3D trans) => this.SetTranslation(ref trans);

    public void SetTranslation(ref Vector3D trans)
    {
      if (trans.Equals(this.m_state.WorldMatrix.Translation, 0.001))
        return;
      this.m_state.WorldMatrix.Translation = trans;
      this.m_state.TransformDirty = true;
      this.ScheduleUpdateInRender();
    }

    public void Close()
    {
      this.OnRemoved();
      MyRenderProxy.RemoveRenderObject(this.m_state.ID, MyRenderProxy.ObjectType.ParticleEffect);
    }

    public void OnRemoved()
    {
      this.OnDelete.InvokeIfNotNull<MyParticleEffect>(this);
      this.OnDelete = (Action<MyParticleEffect>) null;
    }

    private void ScheduleUpdateInRender() => MyParticlesManager.ScheduleUpdate(this);

    public void Update()
    {
      if (!this.m_state.IsStopped && !this.m_state.IsSimulationPaused)
      {
        float seconds = (float) (MyParticlesManager.CurrentTime - this.m_lastTime).Seconds;
        this.m_lastTime = MyParticlesManager.CurrentTime;
        this.m_elapsedTime += seconds;
      }
      MyParticleEffectState state = this.m_state;
      int num1 = state.IsSimulationPaused ? 1 : 0;
      bool isEmittingStopped = state.IsEmittingStopped;
      int num2 = state.IsSimulationPaused ? 1 : 0;
      if ((num1 != num2 || isEmittingStopped != state.IsEmittingStopped || (this.m_state.Dirty || this.m_state.AnimDirty) || this.m_state.TransformDirty ? 1 : (this.m_state.UserDirty ? 1 : 0)) != 0)
        MyRenderProxy.UpdateParticleEffect(ref state);
      this.m_state.Dirty = this.m_state.AnimDirty = this.m_state.TransformDirty = this.m_state.UserDirty = false;
    }

    public override string ToString() => this.m_data.Name;

    public string GetName() => this.m_data.Name;

    public void AssertUnload()
    {
    }

    public void Clear()
    {
      this.OnRemoved();
      this.m_data = (MyParticleEffectData) null;
    }

    private class VRage_Game_MyParticleEffect\u003C\u003EActor : IActivator, IActivator<MyParticleEffect>
    {
      object IActivator.CreateInstance() => (object) new MyParticleEffect();

      MyParticleEffect IActivator<MyParticleEffect>.CreateInstance() => new MyParticleEffect();
    }
  }
}
