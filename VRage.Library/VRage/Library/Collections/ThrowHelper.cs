﻿// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ThrowHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

namespace VRage.Library.Collections
{
  internal static class ThrowHelper
  {
    internal static void ThrowArgumentOutOfRangeException() => ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);

    private static string GetResourceString(string name) => name;

    private static string GetResourceString(string name, params object[] args) => name;

    internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType) => throw new ArgumentException(ThrowHelper.GetResourceString("Arg_WrongType", value, (object) targetType), nameof (value));

    internal static void ThrowKeyNotFoundException() => throw new KeyNotFoundException();

    internal static void ThrowArgumentException(ExceptionResource resource) => throw new ArgumentException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowArgumentException(
      ExceptionResource resource,
      ExceptionArgument argument)
    {
      throw new ArgumentException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)), ThrowHelper.GetArgumentName(argument));
    }

    internal static void ThrowArgumentNullException(ExceptionArgument argument) => throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));

    internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument) => throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));

    internal static void ThrowArgumentOutOfRangeException(
      ExceptionArgument argument,
      ExceptionResource resource)
    {
      throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));
    }

    internal static void ThrowInvalidOperationException(ExceptionResource resource) => throw new InvalidOperationException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowSerializationException(ExceptionResource resource) => throw new SerializationException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowSecurityException(ExceptionResource resource) => throw new SecurityException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowNotSupportedException(ExceptionResource resource) => throw new NotSupportedException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowUnauthorizedAccessException(ExceptionResource resource) => throw new UnauthorizedAccessException(ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource) => throw new ObjectDisposedException(objectName, ThrowHelper.GetResourceString(ThrowHelper.GetResourceName(resource)));

    internal static void IfNullAndNullsAreIllegalThenThrow<T>(
      object value,
      ExceptionArgument argName)
    {
      if (value != null || (object) default (T) == null)
        return;
      ThrowHelper.ThrowArgumentNullException(argName);
    }

    internal static string GetArgumentName(ExceptionArgument argument)
    {
      switch (argument)
      {
        case ExceptionArgument.obj:
          return "obj";
        case ExceptionArgument.dictionary:
          return "dictionary";
        case ExceptionArgument.dictionaryCreationThreshold:
          return "dictionaryCreationThreshold";
        case ExceptionArgument.array:
          return "array";
        case ExceptionArgument.info:
          return "info";
        case ExceptionArgument.key:
          return "key";
        case ExceptionArgument.collection:
          return "collection";
        case ExceptionArgument.list:
          return "list";
        case ExceptionArgument.match:
          return "match";
        case ExceptionArgument.converter:
          return "converter";
        case ExceptionArgument.queue:
          return "queue";
        case ExceptionArgument.stack:
          return "stack";
        case ExceptionArgument.capacity:
          return "capacity";
        case ExceptionArgument.index:
          return "index";
        case ExceptionArgument.startIndex:
          return "startIndex";
        case ExceptionArgument.value:
          return "value";
        case ExceptionArgument.count:
          return "count";
        case ExceptionArgument.arrayIndex:
          return "arrayIndex";
        case ExceptionArgument.name:
          return "name";
        case ExceptionArgument.mode:
          return "mode";
        case ExceptionArgument.item:
          return "item";
        case ExceptionArgument.options:
          return "options";
        case ExceptionArgument.view:
          return "view";
        case ExceptionArgument.sourceBytesToCopy:
          return "sourceBytesToCopy";
        default:
          return string.Empty;
      }
    }

    internal static string GetResourceName(ExceptionResource resource)
    {
      switch (resource)
      {
        case ExceptionResource.Argument_ImplementIComparable:
          return "Argument_ImplementIComparable";
        case ExceptionResource.Argument_InvalidType:
          return "Argument_InvalidType";
        case ExceptionResource.Argument_InvalidArgumentForComparison:
          return "Argument_InvalidArgumentForComparison";
        case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
          return "Argument_InvalidRegistryKeyPermissionCheck";
        case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
          return "ArgumentOutOfRange_NeedNonNegNum";
        case ExceptionResource.Arg_ArrayPlusOffTooSmall:
          return "Arg_ArrayPlusOffTooSmall";
        case ExceptionResource.Arg_NonZeroLowerBound:
          return "Arg_NonZeroLowerBound";
        case ExceptionResource.Arg_RankMultiDimNotSupported:
          return "Arg_RankMultiDimNotSupported";
        case ExceptionResource.Arg_RegKeyDelHive:
          return "Arg_RegKeyDelHive";
        case ExceptionResource.Arg_RegKeyStrLenBug:
          return "Arg_RegKeyStrLenBug";
        case ExceptionResource.Arg_RegSetStrArrNull:
          return "Arg_RegSetStrArrNull";
        case ExceptionResource.Arg_RegSetMismatchedKind:
          return "Arg_RegSetMismatchedKind";
        case ExceptionResource.Arg_RegSubKeyAbsent:
          return "Arg_RegSubKeyAbsent";
        case ExceptionResource.Arg_RegSubKeyValueAbsent:
          return "Arg_RegSubKeyValueAbsent";
        case ExceptionResource.Argument_AddingDuplicate:
          return "Argument_AddingDuplicate";
        case ExceptionResource.Serialization_InvalidOnDeser:
          return "Serialization_InvalidOnDeser";
        case ExceptionResource.Serialization_MissingKeys:
          return "Serialization_MissingKeys";
        case ExceptionResource.Serialization_NullKey:
          return "Serialization_NullKey";
        case ExceptionResource.Argument_InvalidArrayType:
          return "Argument_InvalidArrayType";
        case ExceptionResource.NotSupported_KeyCollectionSet:
          return "NotSupported_KeyCollectionSet";
        case ExceptionResource.NotSupported_ValueCollectionSet:
          return "NotSupported_ValueCollectionSet";
        case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
          return "ArgumentOutOfRange_SmallCapacity";
        case ExceptionResource.ArgumentOutOfRange_Index:
          return "ArgumentOutOfRange_Index";
        case ExceptionResource.Argument_InvalidOffLen:
          return "Argument_InvalidOffLen";
        case ExceptionResource.Argument_ItemNotExist:
          return "Argument_ItemNotExist";
        case ExceptionResource.ArgumentOutOfRange_Count:
          return "ArgumentOutOfRange_Count";
        case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
          return "ArgumentOutOfRange_InvalidThreshold";
        case ExceptionResource.ArgumentOutOfRange_ListInsert:
          return "ArgumentOutOfRange_ListInsert";
        case ExceptionResource.NotSupported_ReadOnlyCollection:
          return "NotSupported_ReadOnlyCollection";
        case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
          return "InvalidOperation_CannotRemoveFromStackOrQueue";
        case ExceptionResource.InvalidOperation_EmptyQueue:
          return "InvalidOperation_EmptyQueue";
        case ExceptionResource.InvalidOperation_EnumOpCantHappen:
          return "InvalidOperation_EnumOpCantHappen";
        case ExceptionResource.InvalidOperation_EnumFailedVersion:
          return "InvalidOperation_EnumFailedVersion";
        case ExceptionResource.InvalidOperation_EmptyStack:
          return "InvalidOperation_EmptyStack";
        case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
          return "ArgumentOutOfRange_BiggerThanCollection";
        case ExceptionResource.InvalidOperation_EnumNotStarted:
          return "InvalidOperation_EnumNotStarted";
        case ExceptionResource.InvalidOperation_EnumEnded:
          return "InvalidOperation_EnumEnded";
        case ExceptionResource.NotSupported_SortedListNestedWrite:
          return "NotSupported_SortedListNestedWrite";
        case ExceptionResource.InvalidOperation_NoValue:
          return "InvalidOperation_NoValue";
        case ExceptionResource.InvalidOperation_RegRemoveSubKey:
          return "InvalidOperation_RegRemoveSubKey";
        case ExceptionResource.Security_RegistryPermission:
          return "Security_RegistryPermission";
        case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
          return "UnauthorizedAccess_RegistryNoWrite";
        case ExceptionResource.ObjectDisposed_RegKeyClosed:
          return "ObjectDisposed_RegKeyClosed";
        case ExceptionResource.NotSupported_InComparableType:
          return "NotSupported_InComparableType";
        case ExceptionResource.Argument_InvalidRegistryOptionsCheck:
          return "Argument_InvalidRegistryOptionsCheck";
        case ExceptionResource.Argument_InvalidRegistryViewCheck:
          return "Argument_InvalidRegistryViewCheck";
        default:
          return string.Empty;
      }
    }

    public static void ThrowWrongKeyTypeArgumentException(object key, Type type) => throw new ArgumentException(string.Format("Key {0} is not of the correct type.", key), nameof (key));
  }
}