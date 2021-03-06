﻿using System.Reflection;

namespace Echo.Utilities
{
    public static class InvocationUtility
    {
        public static bool IsArgumentListMatch(object[] arguments, object[] otherArguments)
        {
            if (arguments == null || otherArguments == null)
            {
                return arguments == otherArguments;
            }
            if (arguments.Length == otherArguments.Length)
            {
                for (var argumentIndex = 0; argumentIndex < arguments.Length; argumentIndex++)
                {
                    if (!IsArgumentMatch(arguments[argumentIndex], otherArguments[argumentIndex]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsArgumentMatch(object argument, object otherArgument)
        {
            if (argument == null || otherArgument == null)
            {
                return argument == otherArgument;
            }
            return argument.Equals(otherArgument);
        }

        public static bool IsMethodMatch(MethodInfo method, MethodInfo anotherMethod)
        {
            if (method == anotherMethod)
            {
                return method == anotherMethod;
            }
            return method.Equals(anotherMethod);
        }
    }
}
