// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyDescriptor
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System.Text;
using VRage.Utils;

namespace VRage.Input
{
  public abstract class MyDescriptor
  {
    private bool m_isDirty = true;
    protected StringBuilder m_name;
    protected StringBuilder m_description;
    private MyStringId? m_descriptionEnum;
    private MyStringId m_nameEnum;

    public MyStringId? DescriptionEnum
    {
      get => this.m_descriptionEnum;
      set
      {
        MyStringId? nullable = value;
        MyStringId? descriptionEnum = this.m_descriptionEnum;
        if ((nullable.HasValue == descriptionEnum.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != descriptionEnum.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.m_descriptionEnum = value;
        this.m_isDirty = true;
      }
    }

    public MyStringId NameEnum
    {
      get => this.m_nameEnum;
      set
      {
        if (!(value != this.m_nameEnum))
          return;
        this.m_nameEnum = value;
        this.m_isDirty = true;
      }
    }

    public StringBuilder Name
    {
      get
      {
        if (this.m_isDirty)
        {
          this.m_isDirty = false;
          this.UpdateDirty();
        }
        return this.m_name;
      }
    }

    public StringBuilder Description
    {
      get
      {
        if (this.m_isDirty)
        {
          this.m_isDirty = false;
          this.UpdateDirty();
        }
        return this.m_description;
      }
    }

    public MyDescriptor(MyStringId name, MyStringId? description = null)
    {
      this.m_nameEnum = name;
      this.DescriptionEnum = description;
    }

    protected abstract void UpdateDirty();
  }
}
