using System;

namespace AireLogicTest.LyricStatistics
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}