﻿using Castle.DynamicProxy;
using Echo.Core;
using System;

namespace Echo
{
    public class Recorder
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();
        private readonly IInvocationWritter _invocationWritter;

        public Recorder(IInvocationLogger invocationLogger)
            : this(new InvocationWriter(invocationLogger))
        {
        }

        public Recorder(IInvocationWritter invocationWritter)
        {
            _invocationWritter = invocationWritter;
        }

        public TTarget GetRecordingTarget<TTarget>(TTarget target)
            where TTarget : class
        {
            // only public interface recording is supported
            var targetType = typeof(TTarget);
            if (!targetType.IsInterface || !targetType.IsPublic)
            {
                throw new NotSupportedException();
            }

            var recordingInterceptor = new RecordingInterceptor(_invocationWritter);
            return _generator.CreateInterfaceProxyWithTarget<TTarget>(target,
#if DEBUG
                new RecordingInterceptor(new ConsoleWritter()),
#endif
                recordingInterceptor);
        }
    }
}
