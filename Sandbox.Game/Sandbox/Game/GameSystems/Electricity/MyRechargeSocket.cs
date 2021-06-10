// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Electricity.MyRechargeSocket
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;

namespace Sandbox.Game.GameSystems.Electricity
{
  public class MyRechargeSocket
  {
    private MyResourceDistributorComponent m_resourceDistributor;
    private MyResourceSinkComponent m_pluggedInConsumer;

    public MyResourceDistributorComponent ResourceDistributor
    {
      get => this.m_resourceDistributor;
      set
      {
        if (this.m_resourceDistributor == value)
          return;
        if (this.m_pluggedInConsumer != null && this.m_resourceDistributor != null)
          this.m_resourceDistributor.RemoveSink(this.m_pluggedInConsumer);
        this.m_resourceDistributor = value;
        if (this.m_pluggedInConsumer == null || this.m_resourceDistributor == null)
          return;
        this.m_resourceDistributor.AddSink(this.m_pluggedInConsumer);
      }
    }

    public void PlugIn(MyResourceSinkComponent consumer)
    {
      if (this.m_pluggedInConsumer == consumer)
        return;
      this.m_pluggedInConsumer = consumer;
      if (this.m_resourceDistributor == null)
        return;
      this.m_resourceDistributor.AddSink(consumer);
      consumer.Update();
    }

    public void Unplug()
    {
      if (this.m_pluggedInConsumer == null)
        return;
      if (this.m_resourceDistributor != null)
        this.m_resourceDistributor.RemoveSink(this.m_pluggedInConsumer);
      this.m_pluggedInConsumer = (MyResourceSinkComponent) null;
    }
  }
}
