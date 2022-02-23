using System;
using System.Collections.Generic;
using System.Text;

namespace AircraftTransmission
{
    class TransmissionManager
    {
        internal NetworkConfigManager configManager;

        /// <summary>
        /// This method sends packet through the socket.
        /// </summary>
        /// <param name="bytePacket"></param>
        internal void SendPacket(byte[] bytePacket)
        {
            try
            {
                if (configManager == null)
                {
                    Setup();
                }

                //send packet
                configManager.SendPacket(bytePacket);
                Console.WriteLine(bytePacket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// This method wraps up the network connection.
        /// </summary>
        internal void EndFlight()
        {
            configManager.EndFlight();
        }

        /// <summary>
        /// This method sets up the connected classes.
        /// </summary>
        private void Setup()
        {
            configManager = new NetworkConfigManager();
        }
    }
}
