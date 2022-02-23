using System.Threading;

namespace AircraftTransmission
{
    /// <summary>
    /// This class managers time control functionalities.
    /// </summary>
    class TimeManager
    {
        /// <summary>
        /// This methods waits for the requested integer amount of seconds.
        /// </summary>
        /// <param name="sec">represents the seconds to wait</param>
        public void WaitForSeconds(int sec)
        {
            Thread.Sleep(sec * 1000);
        }
    }
}
