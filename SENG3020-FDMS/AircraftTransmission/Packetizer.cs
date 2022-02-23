using FlightDataLibrary;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AircraftTransmission
{
    /// <summary>
    /// This class packetizes the parsed aircraft data.
    /// </summary>
    class Packetizer
    {
        internal ChecksumCalculator checksumCalculator;
        internal TransmissionManager transmissionManager;
        private Serialization serialization;

        private uint sequenceNum;    // packet sequence number

        /// <summary>
        /// This method packetizes the data passed in from <see cref="Parser"></see>
        /// and pass it to <see cref="TransmissionManager"></see>.
        /// </summary>
        /// <param name="parsed">parsed aircraft data passed in from <see cref="Parser"></see></param>
        internal void Packetize(string[] parsed)
        {
            try
            {
                if ((checksumCalculator == null) || (transmissionManager == null))
                {
                    Setup();
                }
                // make as DataPacket format
                DataPacket packet = new DataPacket();
                AttachHead(ref packet);
                AttachBody(ref packet, parsed);
                AttachTrailer(ref packet);
                sequenceNum++;

                // convert DataPacket into a byte array and pass it to transmission manager
                transmissionManager.SendPacket(serialization.ObjectToByteArray(packet));
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: failed to create packet - " + e);
                SimpleLogger.Log.Except(e);
                throw;
            }
        }

        /// <summary>
        /// This method attaches head to <see cref="DataPacket"></see>.
        /// </summary>
        /// <param name="packet">reference to the data packet to fill in</param>
        private void AttachHead(ref DataPacket packet)
        {
            packet.Header.SequenceNumber = sequenceNum;
            packet.Header.TailNumber = AtsManager.TailNumber;
        }

        /// <summary>
        /// This method attaches body to <see cref="DataPacket"></see>.
        /// </summary>
        /// <param name="packet">reference to the data packet to fill in</param>
        /// <param name="parsed">parsed aircraft data passed in from <see cref="Parser"></see></param>
        private void AttachBody(ref DataPacket packet, string[] parsed)
        {
            packet.Body.Timestamp = parsed[0];
            packet.Body.AccelX = parsed[1];
            packet.Body.AccelY = parsed[2];
            packet.Body.AccelZ = parsed[3];
            packet.Body.Weight = parsed[4];
            packet.Body.Altitude = parsed[5];
            packet.Body.Pitch = parsed[6];
            packet.Body.Bank = parsed[7];
        }

        /// <summary>
        /// This method attaches trailer to <see cref="DataPacket"></see>.
        /// </summary>
        /// <param name="packet">reference to the data packet to fill in</param>
        private void AttachTrailer(ref DataPacket packet)
        {
            // set any invalid data as 0 to minimize potential disaster
            checksumCalculator.CalculateChecksum(ref packet.Trailer.Checksum, ref packet.Body.Altitude, ref packet.Body.Pitch, ref packet.Body.Bank);
        }

        /// <summary>
        /// This method wraps up the flight data in this class.
        /// </summary>
        internal void EndFlight()
        {
            sequenceNum = 0;
            transmissionManager.EndFlight();
        }

        /// <summary>
        /// This method sets up the connected classes.
        /// </summary>
        private void Setup()
        {
            checksumCalculator = new ChecksumCalculator();
            transmissionManager = new TransmissionManager();
            serialization = new Serialization();
        }
    }
}
