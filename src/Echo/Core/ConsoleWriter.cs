﻿using System;
using System.Reflection;

namespace Echo.Core
{
    internal class ConsoleWriter : IInvocationWriter
    {
        public void WriteInvocation<TTarget>(MethodInfo methodInfo, InvocationResult invocationResult, object[] arguments)
            where TTarget : class
        {
            Console.WriteLine($"Intercepting {typeof(TTarget).Name}.{methodInfo.Name} method:");
            foreach (var argument in arguments)
            {
                Console.WriteLine($"\tArgument {argument?.GetType()}: {argument}");
            }
            Console.WriteLine($"\tReturn {invocationResult?.GetResultType()}: {invocationResult}");
        }
    }
}
