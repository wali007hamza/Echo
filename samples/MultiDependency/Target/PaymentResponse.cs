﻿namespace Samples.MultiDependency.Target
{
    public class PaymentResponse
    {
        public string Message { get; set; }

        public PaymentCode Result { get; set; }
    }

    public enum PaymentCode
    {
        Success,
        Declined,
    }
}
