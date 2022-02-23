using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AircraftTransmission
{
    class NetworkConfigManager
    {
        internal Socket sender;

        /// <summary>
        /// This method sends data packet byte array through the connected socket.
        /// </summary>
        /// <param name="bytePacket">Converted object in byte array</param>
        internal void SendPacket(byte[] bytePacket)
        {
            try
            {
                // send the data through the socket
                if (CheckConnection())
                {
                    Console.WriteLine("Sent: " + sender.Send(bytePacket) + " bytes . . ." );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Failed to send packet");
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// This method checks the network connection.
        /// </summary>
        /// <returns>bool : true if connected</returns>

        internal bool CheckConnection()
        {
            // determine whether a socket is still connected
            if (sender == null)
            {
                ConfigureSocket();
            }
            bool blockingState = sender.Blocking;
            try
            {
                byte[] tmp = new byte[1];

                sender.Blocking = false;
                sender.Send(tmp, 0, 0);
            }
            catch (SocketException e)
            {
                sender.Blocking = blockingState;
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Socket Connected *BUT* the Send would block");
                }
                else
                {
                    Console.WriteLine("Socket Disconnected: error code {0}!", e.NativeErrorCode);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method configures network connection.
        /// </summary>
        internal void ConfigureSocket()
        {
            try
            {
                // establish the remote endpoint for the socket 
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];    // uses host ip: local computer
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // create a TCP/IP  socket 
                sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: failed to configure socket");
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// This method wraps up the network connection.
        /// </summary>
        internal void EndFlight()
        {
            try
            {
                //close connection
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                AtsManager.Boot();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: failed to close socket");
                Console.WriteLine(e);
            }
        }
    }
}
