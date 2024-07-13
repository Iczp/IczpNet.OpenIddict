using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict;

public static class Helper
{
    public static List<string> ParseToList(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }
        using var document = JsonDocument.Parse(json);

        var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

        foreach (var element in document.RootElement.EnumerateArray())
        {
            var value = element.GetString();
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            builder.Add(value);
        }

        return [.. builder];
    }


    public static Dictionary<string, object> GetConstantsTreeDictionary(Type constantsType)
    {
        var result = new Dictionary<string, object>();
        PopulateTreeDictionary(constantsType, result);
        return result;
    }

    public static Dictionary<string, object> GetConstantsTreeDictionary<T>()
    {
        return GetConstantsTreeDictionary(typeof(T));
    }

    private static void PopulateTreeDictionary(Type type, Dictionary<string, object> result)
    {
        foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
        {
            var nestedDictionary = new Dictionary<string, object>();

            foreach (var field in nestedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
                {
                    nestedDictionary.Add(field.Name, (string)field.GetValue(null));
                }
            }

            // 递归调用以处理嵌套类型
            PopulateTreeDictionary(nestedType, nestedDictionary);

            result.Add(nestedType.Name, nestedDictionary);
        }
    }

    public static Dictionary<string, string> GetConstantsFlatDictionary(Type constantsType)
    {
        var result = new Dictionary<string, string>();
        PopulateFlatDictionary(constantsType, result, [constantsType.Name]);
        return result;
    }

    public static Dictionary<string, string> GetConstantsFlatDictionary<T>()
    {
        return GetConstantsFlatDictionary(typeof(T));
    }

    private static void PopulateFlatDictionary(Type type, Dictionary<string, string> result, List<string> parentPath)
    {
        foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
        {
            var currentPaths = new List<string>(parentPath) { nestedType.Name };

            foreach (var field in nestedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
                {
                    var fullPath = currentPaths.Concat([field.Name]).JoinAsString(".");
                    result[fullPath] = (string)field.GetValue(null);
                }
            }

            // 递归调用以处理嵌套类型
            PopulateFlatDictionary(nestedType, result, currentPaths);
        }
    }






}
