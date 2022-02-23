namespace FlightDataLibrary
{
    /// <summary>
    ///     Data structure for a single telemetry record
    /// </summary>
    public class TelemetryRecord
    {
        /// <summary>
        ///     Identifier of the aircraft
        /// </summary>
        public string TailNumber;

        /// <summary>
        ///     Time at which the record was initially recorded by
        ///     the Aircraft Transmission System
        /// </summary>
        public string Timestamp;

        /// <summary>
        ///     Time at which the record was stored in the database.
        /// </summary>
        /// <value>
        ///     <see langword="null"/> unless the object has been retrieved from a database.
        /// </value>
        public string StoredTimestamp;

        /// <summary>
        ///     G-Force parameter which tracks acceleration in the X axis
        /// </summary>
        public string AccelX;

        /// <summary>
        ///     G-Force parameter which tracks acceleration in the Y axis
        /// </summary>
        public string AccelY;

        /// <summary>
        ///     G-Force parameter which tracks acceleration in the Z axis
        /// </summary>
        public string AccelZ;

        /// <summary>
        ///     G-Force parameter which tracks the weight of the aircraft
        /// </summary>
        public string Weight;

        /// <summary>
        ///     Attitude parameter which tracks the altitude of the aircraft
        /// </summary>
        public string Altitude;

        /// <summary>
        ///     Attitude parameter which tracks the pitch of the aircraft
        /// </summary>
        public string Pitch;

        /// <summary>
        ///     Attitude parameter which bank the altitude of the aircraft
        /// </summary>
        public string Bank;
    }
}
