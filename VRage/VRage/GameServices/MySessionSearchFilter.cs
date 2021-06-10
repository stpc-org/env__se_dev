// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MySessionSearchFilter
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MySessionSearchFilter
  {
    public const string Names = "SERVER_PROP_NAMES";
    public const string Data = "SERVER_PROP_DATA";
    public const string PlayerCount = "SERVER_PROP_PLAYER_COUNT";
    public const string Ping = "SERVER_PROP_PING";
    public const string Tags = "SERVER_PROP_TAGS";
    public const string CustomPropertyPrefix = "SERVER_CPROP_";
    public readonly List<MySessionSearchFilter.Query> Queries = new List<MySessionSearchFilter.Query>();

    public void AddQuery(string property, MySearchCondition condition, string value) => this.Queries.Add(new MySessionSearchFilter.Query(property, condition, value));

    public void AddQueryCustom(string customProperty, MySearchCondition condition, string value) => this.Queries.Add(new MySessionSearchFilter.Query("SERVER_CPROP_" + customProperty, condition, value));

    public override string ToString() => "[" + string.Join<MySessionSearchFilter.Query>("", (IEnumerable<MySessionSearchFilter.Query>) this.Queries) + "]";

    public struct Query
    {
      public string Property;
      public string Value;
      public MySearchCondition Condition;

      public Query(string property, MySearchCondition condition, string value)
      {
        this.Property = property;
        this.Condition = condition;
        this.Value = value;
      }

      public override string ToString() => string.Format("{0}::{1}::{2}", (object) this.Property, (object) this.Condition, (object) this.Value);
    }
  }
}
