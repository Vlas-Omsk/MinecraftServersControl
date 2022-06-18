using MinecraftServersControl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.API.Vk
{
    internal static class ServiceHelper
    {
        public static object[] MapParameters(MethodInfo method, string[] segments)
        {
            var parameters = method.GetParameters();
            var parameterObjects = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (i == parameters.Length - 1 && parameter.IsParamArray())
                {
                    segments = segments.Skip(i).ToArray();

                    var elementType = parameter.ParameterType.GetElementType();
                    var array = Array.CreateInstance(elementType, segments.Length);

                    for (var j = 0; j < array.Length; j++)
                    {
                        try
                        {
                            array.SetValue(ChangeType(segments[j], elementType), j);
                        }
                        catch (Exception ex)
                        {
                            throw new ParameterException(parameter, ex);
                        }
                    }

                    parameterObjects[i] = array;
                }
                else
                {
                    try
                    {
                        parameterObjects[i] = ChangeType(segments[i], parameter.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new ParameterException(parameter, ex);
                    }
                }
            }

            return parameterObjects;
        }

        private static object ChangeType(string value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        public static CommandFromStringResult GetCommandFromString(Type[] serviceTypes, string[] segments)
        {
            var matchesList = new List<MethodInfo>();

            foreach (var serviceType in serviceTypes)
            {
                var serviceAttribute = serviceType.GetCustomAttribute<ServiceAttribute>();
                string[] methodSegments;

                if (serviceAttribute != null)
                {
                    if (segments.Length < serviceAttribute.Segments.Length)
                        continue;

                    if (!CompareSegments(segments, serviceAttribute.Segments))
                        continue;

                    methodSegments = segments.Skip(serviceAttribute.Segments.Length).ToArray();
                }
                else
                {
                    methodSegments = segments;
                }

                var methodAttributePairs = GetCommands(serviceType);

                if (methodSegments.Length == 0 && serviceAttribute != null)
                {
                    matchesList.AddRange(methodAttributePairs.Select(x => x.Method));
                    continue;
                }

                foreach (var methodAttributePair in methodAttributePairs)
                {
                    var parameters = methodAttributePair.Method.GetParameters();
                    var hasParamArray = parameters.LastOrDefault()?.IsParamArray() ?? false;

                    if (methodSegments.Length < methodAttributePair.Attribute.Segments.Length && !hasParamArray)
                        continue;

                    if (!CompareSegments(methodSegments, methodAttributePair.Attribute.Segments))
                        continue;

                    var parametersSegments = methodSegments.Skip(methodAttributePair.Attribute.Segments.Length).ToArray();

                    if ((hasParamArray && parameters.Length > parametersSegments.Length) || 
                        (!hasParamArray && parameters.Length != parametersSegments.Length))
                    {
                        matchesList.Add(methodAttributePair.Method);
                        continue;
                    }

                    return new CommandFromStringResult(methodAttributePair.Method, parametersSegments, null);
                }
            }

            return new CommandFromStringResult(null, null, matchesList.ToArray());
        }

        private static IEnumerable<MethodAttributePair<CommandAttribute>> GetCommands(Type serviceType)
        {
            return serviceType.GetMethodsWithAttribute<CommandAttribute>();
        }

        private static bool CompareSegments(string[] currentSerments, string[] targetSegments)
        {
            for (var i = 0; i < targetSegments.Length; i++)
                if (!currentSerments[i].Equals(targetSegments[i], StringComparison.OrdinalIgnoreCase))
                    return false;

            return true;
        }

        public static string[] ParseSegments(string commandString)
        {
            return commandString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
