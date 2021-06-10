// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGps
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGps : IMyGps
  {
    internal static readonly int DROP_NONFINAL_AFTER_SEC = 180;
    private IMyEntity m_entity;
    private bool m_unupdatedEntityLocation;
    private long m_entityId;
    private long m_contractId;
    private string m_displayName = string.Empty;
    private Vector3D m_coords;
    private Color m_color = new Color(117, 201, 241);

    public string Name { get; set; }

    public bool IsObjective { get; set; }

    public string DisplayName
    {
      get => this.m_displayName;
      set => this.m_displayName = value;
    }

    public string Description { get; set; }

    public Vector3D Coords
    {
      get
      {
        if (this.CoordsFunc != null)
          return this.CoordsFunc();
        if (this.m_entityId != 0L && this.m_entity == null)
        {
          IMyEntity entityById = (IMyEntity) MyEntities.GetEntityById(this.m_entityId);
          if (entityById != null)
          {
            this.m_unupdatedEntityLocation = false;
            this.SetEntity(entityById);
          }
          else
            this.m_unupdatedEntityLocation = true;
        }
        return this.m_coords;
      }
      set => this.m_coords = value;
    }

    public Color GPSColor
    {
      get => this.m_color;
      set => this.m_color = value;
    }

    public bool ShowOnHud { get; set; }

    public bool AlwaysVisible { get; set; }

    public TimeSpan? DiscardAt { get; set; }

    public bool IsLocal { get; set; }

    public Func<Vector3D> CoordsFunc { get; set; }

    public long EntityId => this.m_entityId;

    public long ContractId
    {
      set => this.m_contractId = value;
      get => this.m_contractId;
    }

    public bool IsContainerGPS { get; set; }

    public string ContainerRemainingTime { get; set; }

    public int Hash { get; private set; }

    public MyGps(MyObjectBuilder_Gps.Entry builder)
    {
      this.Name = builder.name;
      this.DisplayName = builder.DisplayName;
      this.Description = builder.description;
      this.Coords = builder.coords;
      this.ShowOnHud = builder.showOnHud;
      this.AlwaysVisible = builder.alwaysVisible;
      this.IsObjective = builder.isObjective;
      this.ContractId = builder.contractId;
      this.GPSColor = !(builder.color != Color.Transparent) || !(builder.color != Color.Black) ? new Color(117, 201, 241) : builder.color;
      if (!builder.isFinal)
        this.SetDiscardAt();
      this.SetEntityId(builder.entityId);
      this.UpdateHash();
    }

    public MyGps()
    {
      this.GPSColor = new Color(117, 201, 241);
      this.SetDiscardAt();
    }

    public void SetDiscardAt() => this.DiscardAt = new TimeSpan?(TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double) MyGps.DROP_NONFINAL_AFTER_SEC));

    public void SetEntity(IMyEntity entity)
    {
      if (entity == null)
        return;
      this.m_entity = entity;
      this.m_entityId = entity.EntityId;
      this.m_entity.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.m_entity.NeedsWorldMatrix = true;
      this.m_entity.OnClose += new Action<IMyEntity>(this.m_entity_OnClose);
      this.Coords = this.m_entity.PositionComp.GetPosition();
    }

    public void SetEntityId(long entityId)
    {
      if (entityId == 0L)
        return;
      this.m_entityId = entityId;
    }

    private void m_entity_OnClose(IMyEntity obj)
    {
      if (this.m_entity == null)
        return;
      this.m_entity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.m_entity.OnClose -= new Action<IMyEntity>(this.m_entity_OnClose);
      this.m_entity = (IMyEntity) null;
    }

    private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
    {
      if (this.m_entity == null)
        return;
      this.Coords = this.m_entity.PositionComp.GetPosition();
    }

    public void Close()
    {
      if (this.m_entity == null)
        return;
      this.m_entity.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.m_entity.OnClose -= new Action<IMyEntity>(this.m_entity_OnClose);
    }

    public int CalculateHash()
    {
      int hash = MyUtils.GetHash(this.Name);
      return this.m_entityId != 0L ? hash * this.m_entityId.GetHashCode() : MyUtils.GetHash(this.Coords.Z, MyUtils.GetHash(this.Coords.Y, MyUtils.GetHash(this.Coords.X, hash)));
    }

    public void UpdateHash() => this.Hash = this.CalculateHash();

    public override int GetHashCode() => this.Hash;

    public override string ToString() => MyGps.ConvertToString(this);

    internal static string ConvertToString(MyGps gps) => MyGps.ConvertToString(gps.Name, gps.Coords, new Color?(gps.GPSColor));

    internal static string ConvertToString(string name, Vector3D coords, Color? color = null)
    {
      if (!color.HasValue)
        color = new Color?(new Color(117, 201, 241));
      ColorDefinitionRGBA colorDefinitionRgba;
      ref ColorDefinitionRGBA local = ref colorDefinitionRgba;
      Color color1 = color.Value;
      int r = (int) color1.R;
      color1 = color.Value;
      int g = (int) color1.G;
      color1 = color.Value;
      int b = (int) color1.B;
      color1 = color.Value;
      int a = (int) color1.A;
      local = new ColorDefinitionRGBA((byte) r, (byte) g, (byte) b, (byte) a);
      StringBuilder stringBuilder = new StringBuilder("GPS:", 256);
      stringBuilder.Append(name);
      stringBuilder.Append(":");
      stringBuilder.Append(coords.X.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(":");
      stringBuilder.Append(coords.Y.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(":");
      stringBuilder.Append(coords.Z.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(":");
      stringBuilder.Append(colorDefinitionRgba.Hex);
      stringBuilder.Append(":");
      return stringBuilder.ToString();
    }

    public void ToClipboard() => MyVRage.Platform.System.Clipboard = this.ToString();

    string IMyGps.Name
    {
      get => this.Name;
      set => this.Name = value != null ? value : throw new ArgumentNullException("Value must not be null!");
    }

    string IMyGps.Description
    {
      get => this.Description;
      set => this.Description = value != null ? value : throw new ArgumentNullException("Value must not be null!");
    }

    Vector3D IMyGps.Coords
    {
      get => this.Coords;
      set => this.Coords = value;
    }

    Color IMyGps.GPSColor
    {
      get => this.GPSColor;
      set => this.GPSColor = value;
    }

    bool IMyGps.ShowOnHud
    {
      get => this.ShowOnHud;
      set => this.ShowOnHud = value;
    }

    TimeSpan? IMyGps.DiscardAt
    {
      get => this.DiscardAt;
      set => this.DiscardAt = value;
    }
  }
}
