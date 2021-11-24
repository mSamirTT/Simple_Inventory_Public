using StarterApp.Core.Common.Interfaces;
using System;

namespace StarterApp.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
