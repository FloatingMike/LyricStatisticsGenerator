using System;

namespace AireLogicTest.LyricStatistics
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}