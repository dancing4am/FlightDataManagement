using System;

namespace FlightDataLibrary
{
    /// <summary>
    ///     Provides conversion between the <see cref="DateTime"/> format provided
    ///     by the Aircraft Transmission system and the format expected by a database
    ///     management system.
    /// </summary>
    public static class TimeConverter
    {
        /// <summary>
        ///     Datetime format used by the Aircraft Transmission system
        /// </summary>
        public const string DATETIME_ATS_FORMAT = "M_d_yyyy H:m:s";

        /// <summary>
        ///     Datetime format for use with databases using standard 24-hour datetime
        /// </summary>
        public const string DATETIME_DB_FORMAT = "yyyy-MM-dd HH:mm:ss";


        /// <summary>
        ///     Converts the given datetime as a string to the format expected for
        ///     the DATETIME datatype in an SQL database
        /// </summary>
        /// <param name="datetime">
        ///     A string representing a datetime, which must follow the format
        ///     indicated in <paramref name="timeFormat"/>
        /// </param>
        /// <param name="timeFormat">
        ///     Time format used to parse the provided input
        /// </param>
        /// <returns>
        ///     Datetime string in the format <see cref="DATETIME_DB_FORMAT"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="FormatException" />
        public static string ConvertToDatabaseFormat(string datetime, string timeFormat = DATETIME_ATS_FORMAT)
        {
            return DateTime.ParseExact(datetime.Trim(), timeFormat,
                System.Globalization.CultureInfo.InvariantCulture).ToString(DATETIME_DB_FORMAT);
        }
    }
}
