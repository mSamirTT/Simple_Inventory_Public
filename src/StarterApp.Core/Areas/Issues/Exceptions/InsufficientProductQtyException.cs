using System;

namespace StarterApp.Core.Common.Exceptions
{
    public class InsufficientProductQtyException : Exception
    {
        public InsufficientProductQtyException(string productName, int availableQty, int netQty)
            : base($"Available quantity for product [{productName}] is [{availableQty}] which is insufficient for required quantity [{netQty * -1}].")
        {
        }
    }
}
