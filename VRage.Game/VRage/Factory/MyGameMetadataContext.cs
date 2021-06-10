// Decompiled with JetBrains decompiler
// Type: VRage.Factory.MyGameMetadataContext
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Reflection;
using VRage.Meta;
using VRage.Serialization;

namespace VRage.Factory
{
  internal class MyGameMetadataContext : MyMetadataContext
  {
    public void RegisterAttributeObserver(Type attributeType, AttributeObserver observer) => this.AttributeIndexers.Add(attributeType, (IMyAttributeIndexer) new MyGameMetadataContext.Crawler(observer));

    protected override void Index(Assembly assembly, bool batch = false)
    {
      MyFactory.RegisterFromAssembly(assembly);
      base.Index(assembly, batch);
    }

    internal class Crawler : IMyAttributeIndexer, IMyMetadataIndexer
    {
      public AttributeObserver Observer;

      public Crawler(AttributeObserver observer) => this.Observer = observer;

      public void SetParent(IMyMetadataIndexer indexer)
      {
      }

      public void Activate()
      {
      }

      public void Observe(Attribute attribute, Type type) => this.Observer(type, attribute);

      public void Close()
      {
      }

      public void Process()
      {
      }
    }
  }
}
