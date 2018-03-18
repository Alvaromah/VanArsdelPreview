using System;

namespace VanArsdel.Inventory
{
    static public class DateTimeExtensions
    {
        static public DateTimeOffset? AsDateTimeOffset(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime);
        }
        static public DateTimeOffset? AsNullableDateTimeOffset(this DateTime? dateTime)
        {
            if (dateTime != null)
            {
                return new DateTimeOffset(dateTime.Value);
            }
            return null;
        }

        static public DateTime AsDateTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }
        static public DateTime? AsNullableDateTime(this DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset != null)
            {
                return dateTimeOffset.Value.DateTime;
            }
            return null;
        }
    }
}
