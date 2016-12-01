﻿using System.Reflection;

namespace Echo.Core
{
    internal interface IInvocationReader
    {
        object[] FindEntryArguments();

        InvocationResult FindInvocationResult<TTarget>(MethodInfo methodInfo, object[] arguments)
            where TTarget : class;
    }
}
