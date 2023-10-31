namespace HelmerDemo.BlazorServer.Shared.Tools.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Set the DateTime to the Utc kind
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? SetKindUtc(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.SetKindUtc();
            }

            return null;
        }

        /// <summary>
        /// Set the DateTime to the Utc kind
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime SetKindUtc(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
