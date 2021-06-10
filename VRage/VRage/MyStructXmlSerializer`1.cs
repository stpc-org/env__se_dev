// Decompiled with JetBrains decompiler
// Type: VRage.MyStructXmlSerializer`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace VRage
{
  public class MyStructXmlSerializer<TStruct> : MyXmlSerializerBase<TStruct> where TStruct : struct
  {
    public static FieldInfo m_defaultValueField;
    private static Dictionary<string, MyStructXmlSerializer<TStruct>.Accessor> m_accessorMap;

    public MyStructXmlSerializer()
    {
    }

    public MyStructXmlSerializer(ref TStruct data) => this.m_data = data;

    public override void ReadXml(XmlReader reader)
    {
      MyStructXmlSerializer<TStruct>.BuildAccessorsInfo();
      object obj1 = (object) (TStruct) MyStructXmlSerializer<TStruct>.m_defaultValueField.GetValue((object) null);
      reader.MoveToElement();
      if (reader.IsEmptyElement)
      {
        reader.Skip();
      }
      else
      {
        reader.ReadStartElement();
        int content1 = (int) reader.MoveToContent();
        while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            MyStructXmlSerializer<TStruct>.Accessor accessor;
            if (MyStructXmlSerializer<TStruct>.m_accessorMap.TryGetValue(reader.LocalName, out accessor))
            {
              object obj2;
              if (accessor.IsPrimitiveType)
              {
                string str = reader.ReadElementString();
                obj2 = TypeDescriptor.GetConverter(accessor.Type).ConvertFrom((ITypeDescriptorContext) null, CultureInfo.InvariantCulture, (object) str);
              }
              else if (accessor.SerializerType != (Type) null)
              {
                IMyXmlSerializable instance = Activator.CreateInstance(accessor.SerializerType) as IMyXmlSerializable;
                instance.ReadXml(reader.ReadSubtree());
                obj2 = instance.Data;
                reader.ReadEndElement();
              }
              else
              {
                XmlSerializer serializer = MyXmlSerializerManager.GetOrCreateSerializer(accessor.Type);
                string serializedName = MyXmlSerializerManager.GetSerializedName(accessor.Type);
                obj2 = this.Deserialize(reader, serializer, serializedName);
              }
              accessor.SetValue(obj1, obj2);
            }
            else
              reader.Skip();
          }
          int content2 = (int) reader.MoveToContent();
        }
        reader.ReadEndElement();
        this.m_data = (TStruct) obj1;
      }
    }

    private static void BuildAccessorsInfo()
    {
      if (MyStructXmlSerializer<TStruct>.m_defaultValueField != (FieldInfo) null)
        return;
      lock (typeof (TStruct))
      {
        if (MyStructXmlSerializer<TStruct>.m_defaultValueField != (FieldInfo) null)
          return;
        MyStructXmlSerializer<TStruct>.m_defaultValueField = MyStructDefault.GetDefaultFieldInfo(typeof (TStruct));
        if (MyStructXmlSerializer<TStruct>.m_defaultValueField == (FieldInfo) null)
          throw new Exception("Missing default value for struct " + typeof (TStruct).FullName + ". Decorate one static read-only field with StructDefault attribute");
        MyStructXmlSerializer<TStruct>.m_accessorMap = new Dictionary<string, MyStructXmlSerializer<TStruct>.Accessor>();
        foreach (FieldInfo field in typeof (TStruct).GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
          if (field.GetCustomAttribute(typeof (XmlIgnoreAttribute)) == null)
            MyStructXmlSerializer<TStruct>.m_accessorMap.Add(field.Name, (MyStructXmlSerializer<TStruct>.Accessor) new MyStructXmlSerializer<TStruct>.FieldAccessor(field));
        }
        foreach (PropertyInfo property in typeof (TStruct).GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
          if (property.GetCustomAttribute(typeof (XmlIgnoreAttribute)) == null && property.GetIndexParameters().Length == 0)
            MyStructXmlSerializer<TStruct>.m_accessorMap.Add(property.Name, (MyStructXmlSerializer<TStruct>.Accessor) new MyStructXmlSerializer<TStruct>.PropertyAccessor(property));
        }
      }
    }

    public static implicit operator MyStructXmlSerializer<TStruct>(TStruct data) => new MyStructXmlSerializer<TStruct>(ref data);

    private abstract class Accessor
    {
      public abstract object GetValue(object obj);

      public abstract void SetValue(object obj, object value);

      public abstract Type Type { get; }

      public Type SerializerType { get; private set; }

      public bool IsPrimitiveType
      {
        get
        {
          Type type = this.Type;
          return type.IsPrimitive || type == typeof (string);
        }
      }

      protected void CheckXmlElement(MemberInfo info)
      {
        if (!(info.GetCustomAttribute(typeof (XmlElementAttribute), false) is XmlElementAttribute customAttribute) || !(customAttribute.Type != (Type) null) || !typeof (IMyXmlSerializable).IsAssignableFrom(customAttribute.Type))
          return;
        this.SerializerType = customAttribute.Type;
      }
    }

    private class FieldAccessor : MyStructXmlSerializer<TStruct>.Accessor
    {
      public FieldInfo Field { get; private set; }

      public FieldAccessor(FieldInfo field)
      {
        this.Field = field;
        this.CheckXmlElement((MemberInfo) field);
      }

      public override object GetValue(object obj) => this.Field.GetValue(obj);

      public override void SetValue(object obj, object value) => this.Field.SetValue(obj, value);

      public override Type Type => this.Field.FieldType;
    }

    private class PropertyAccessor : MyStructXmlSerializer<TStruct>.Accessor
    {
      public PropertyInfo Property { get; private set; }

      public PropertyAccessor(PropertyInfo property)
      {
        this.Property = property;
        this.CheckXmlElement((MemberInfo) property);
      }

      public override object GetValue(object obj) => this.Property.GetValue(obj);

      public override void SetValue(object obj, object value) => this.Property.SetValue(obj, value);

      public override Type Type => this.Property.PropertyType;
    }
  }
}
