using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlightDataLibrary
{
    /// <summary>
    ///     Header of <see cref="DataPacket"></see>
    /// </summary>
    [Serializable()]
    public struct Header
    {
        public string TailNumber;
        public uint SequenceNumber;
    }

    /// <summary>
    ///     Body of <see cref="DataPacket"></see>
    /// </summary>
    [Serializable()]
    public struct Body
    {
        public string Timestamp;
        public string AccelX;       // according to flight data management system V2.pdf, these should be all string
        public string AccelY;
        public string AccelZ;
        public string Weight;
        public string Altitude;
        public string Pitch;
        public string Bank;
    }

    /// <summary>
    ///     Trailer of <see cref="DataPacket"></see>
    /// </summary>
    [Serializable()]
    public struct Trailer
    {
        public int Checksum;
    }

    /// <summary>
    /// This class is the telemetry packet format.
    /// </summary>
    [Serializable()]
    public class DataPacket
    {
        public Header Header;
        public Body Body;
        public Trailer Trailer;
    }

    public class Serialization
    {
        /// <summary>
        /// This method converts the object passed into a byte array.
        /// </summary>
        /// <param name="obj">Object to convert to byte array</param>
        /// <returns>byte[]: converted object in byte array</returns>
        public byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
