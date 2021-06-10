// Decompiled with JetBrains decompiler
// Type: System.Reflection.PropertyAccess
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Linq.Expressions;

namespace System.Reflection
{
  public static class PropertyAccess
  {
    public static Func<T, TProperty> CreateGetter<T, TProperty>(this PropertyInfo propertyInfo)
    {
      Type type1 = typeof (T);
      Type type2 = typeof (TProperty);
      ParameterExpression parameterExpression = Expression.Parameter(type1, "value");
      Expression expression = (Expression) Expression.Property(propertyInfo.DeclaringType == type1 ? (Expression) parameterExpression : (Expression) Expression.Convert((Expression) parameterExpression, propertyInfo.DeclaringType), propertyInfo);
      if (type2 != propertyInfo.PropertyType)
        expression = (Expression) Expression.Convert(expression, type2);
      return Expression.Lambda<Func<T, TProperty>>(expression, parameterExpression).Compile();
    }

    public static Action<T, TProperty> CreateSetter<T, TProperty>(
      this PropertyInfo propertyInfo)
    {
      Type type1 = typeof (T);
      Type type2 = typeof (TProperty);
      ParameterExpression parameterExpression3 = Expression.Parameter(type1);
      ParameterExpression parameterExpression4 = Expression.Parameter(type2);
      Expression expression = propertyInfo.DeclaringType == type1 ? (Expression) parameterExpression3 : (Expression) Expression.Convert((Expression) parameterExpression3, propertyInfo.DeclaringType);
      Expression right = propertyInfo.PropertyType == type2 ? (Expression) parameterExpression4 : (Expression) Expression.Convert((Expression) parameterExpression4, propertyInfo.PropertyType);
      return ((Expression<Action<T, TProperty>>) ((parameterExpression1, parameterExpression2) => Expression.Assign((Expression) Expression.Property(expression, propertyInfo), right))).Compile();
    }

    public static Getter<T, TProperty> CreateGetterRef<T, TProperty>(
      this PropertyInfo propertyInfo)
    {
      Type type1 = typeof (T);
      Type type2 = typeof (TProperty);
      ParameterExpression parameterExpression1 = Expression.Parameter(type1.MakeByRefType(), "value");
      Expression expression = (Expression) Expression.Property(propertyInfo.DeclaringType == type1 ? (Expression) parameterExpression1 : (Expression) Expression.Convert((Expression) parameterExpression1, propertyInfo.DeclaringType), propertyInfo);
      if (type2 != propertyInfo.PropertyType)
        expression = (Expression) Expression.Convert(expression, type2);
      ParameterExpression parameterExpression2 = Expression.Parameter(type2.MakeByRefType(), "out");
      return Expression.Lambda<Getter<T, TProperty>>((Expression) Expression.Assign((Expression) parameterExpression2, expression), parameterExpression1, parameterExpression2).Compile();
    }

    public static Setter<T, TProperty> CreateSetterRef<T, TProperty>(
      this PropertyInfo propertyInfo)
    {
      Type type1 = typeof (T);
      Type type2 = typeof (TProperty);
      ParameterExpression parameterExpression3 = Expression.Parameter(type1.MakeByRefType());
      ParameterExpression parameterExpression4 = Expression.Parameter(type2.MakeByRefType());
      Expression expression = propertyInfo.DeclaringType == type1 ? (Expression) parameterExpression3 : (Expression) Expression.Convert((Expression) parameterExpression3, propertyInfo.DeclaringType);
      Expression right = propertyInfo.PropertyType == type2 ? (Expression) parameterExpression4 : (Expression) Expression.Convert((Expression) parameterExpression4, propertyInfo.PropertyType);
      return ((Expression<Setter<T, TProperty>>) ((parameterExpression1, parameterExpression2) => Expression.Assign((Expression) Expression.Property(expression, propertyInfo), right))).Compile();
    }
  }
}
