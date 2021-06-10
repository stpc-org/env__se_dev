// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberAccess
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System.Reflection
{
  public static class MemberAccess
  {
    public static bool IsMemberPublic(this MemberInfo memberInfo)
    {
      switch (memberInfo.MemberType)
      {
        case MemberTypes.Field:
          return (((FieldInfo) memberInfo).Attributes & FieldAttributes.Public) == FieldAttributes.Public;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
          MethodInfo getMethod = propertyInfo.GetGetMethod();
          MethodInfo setMethod = propertyInfo.GetSetMethod();
          return getMethod != (MethodInfo) null && setMethod != (MethodInfo) null && (getMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public && (setMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public;
        default:
          throw new NotImplementedException();
      }
    }

    public static Type GetMemberType(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as PropertyInfo) != null)
        return ((PropertyInfo) memberInfo).PropertyType;
      if ((object) (memberInfo as FieldInfo) != null)
        return ((FieldInfo) memberInfo).FieldType;
      return (object) (memberInfo as MethodInfo) != null ? ((MethodInfo) memberInfo).ReturnType : throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo or MethodInfo");
    }

    public static object GetValue(this MemberInfo memberInfo, object forObject)
    {
      switch (memberInfo.MemberType)
      {
        case MemberTypes.Field:
          return ((FieldInfo) memberInfo).GetValue(forObject);
        case MemberTypes.Property:
          return ((PropertyInfo) memberInfo).GetValue(forObject);
        default:
          throw new NotImplementedException();
      }
    }

    public static void SetValue(this MemberInfo memberInfo, object forObject, object value)
    {
      switch (memberInfo.MemberType)
      {
        case MemberTypes.Field:
          ((FieldInfo) memberInfo).SetValue(forObject, value);
          break;
        case MemberTypes.Property:
          ((PropertyInfo) memberInfo).SetValue(forObject, value);
          break;
        default:
          throw new NotImplementedException();
      }
    }

    public static Func<T, TMember> CreateGetter<T, TMember>(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as PropertyInfo) != null)
        return PropertyAccess.CreateGetter<T, TMember>((PropertyInfo) memberInfo);
      return (object) (memberInfo as FieldInfo) != null ? FieldAccess.CreateGetter<T, TMember>((FieldInfo) memberInfo) : throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }

    public static Action<T, TMember> CreateSetter<T, TMember>(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as PropertyInfo) != null)
        return PropertyAccess.CreateSetter<T, TMember>((PropertyInfo) memberInfo);
      return (object) (memberInfo as FieldInfo) != null ? FieldAccess.CreateSetter<T, TMember>((FieldInfo) memberInfo) : throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }

    public static Getter<T, TMember> CreateGetterRef<T, TMember>(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as PropertyInfo) != null)
        return PropertyAccess.CreateGetterRef<T, TMember>((PropertyInfo) memberInfo);
      return (object) (memberInfo as FieldInfo) != null ? FieldAccess.CreateGetterRef<T, TMember>((FieldInfo) memberInfo) : throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }

    public static Setter<T, TMember> CreateSetterRef<T, TMember>(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as PropertyInfo) != null)
        return PropertyAccess.CreateSetterRef<T, TMember>((PropertyInfo) memberInfo);
      return (object) (memberInfo as FieldInfo) != null ? FieldAccess.CreateSetterRef<T, TMember>((FieldInfo) memberInfo) : throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }

    public static bool CheckGetterSignature<T, TMember>(this MemberInfo memberInfo)
    {
      if (!typeof (T).IsAssignableFrom(memberInfo.DeclaringType))
        return false;
      if ((object) (memberInfo as PropertyInfo) != null)
        return typeof (TMember).IsAssignableFrom(((PropertyInfo) memberInfo).PropertyType);
      if ((object) (memberInfo as FieldInfo) != null)
        return typeof (TMember).IsAssignableFrom(((FieldInfo) memberInfo).FieldType);
      throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }

    public static bool CheckSetterSignature<T, TMember>(this MemberInfo memberInfo)
    {
      if (!typeof (T).IsAssignableFrom(memberInfo.DeclaringType))
        return false;
      if ((object) (memberInfo as PropertyInfo) != null)
        return typeof (TMember).IsAssignableFrom(((PropertyInfo) memberInfo).PropertyType);
      if ((object) (memberInfo as FieldInfo) != null)
        return typeof (TMember).IsAssignableFrom(((FieldInfo) memberInfo).FieldType);
      throw new InvalidOperationException("Member info must be PropertyInfo, FieldInfo");
    }
  }
}
