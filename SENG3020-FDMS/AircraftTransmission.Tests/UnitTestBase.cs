using NUnit.Framework;
using System;
using System.IO;

namespace AircraftTransmission.Tests
{
    public class Tests
    {
        FileManager fileManager;
        StringWriter sw;

        [OneTimeSetUp]
        public void Setup()
        {
            sw = new StringWriter();
            Console.SetOut(sw);

            fileManager = new FileManager();
            fileManager.parser = new Parser();
            fileManager.parser.ParserCalledHandler += CountFrame;
            fileManager.parser.packetizer = new Packetizer();
            fileManager.parser.packetizer.checksumCalculator = new FlightDataLibrary.ChecksumCalculator();
            fileManager.parser.packetizer.transmissionManager = new TransmissionManager();
            fileManager.parser.packetizer.transmissionManager.configManager = new NetworkConfigManager();
        }

        [Test]
        public void DefaultSanityTest()
        {
            Assert.Pass();
        }

        [Test]
        public void LoadingATelemetryFile_4111_1()
        {
            // 4111-1 Initialize File Manager
            Assert.IsNotNull(fileManager);
        }

        [Test]
        public void LoadingATelemetryFile_4111_2()
        {
            // 4111-2 Call ReadData() method with 3 original telemetry data file names
            // inputs: C-FGAX, C-GEFC, C-QWWT
            string[] originalNames = { "C-FGAX", "C-GEFC", "C-QWWT" };
            for (int i = 0; i < originalNames.Length; i++)
            {
                // 4111-3 ***MANUAL*** Step into the Parser instance receiving the ‘line’ data               
                Assert.DoesNotThrow(() => fileManager.ReadData(originalNames[i]));
            }
        }

        [Test]
        public void LoadingATelemetryFile_4111_5()
        {
            // 4111-5 Call ReadData() method with 3 extension-different lure file names 
            // inputs: C-Fgax.txt, C-Fgax.bin, C-Fgax.asm
            string[] extenstionTests = { "C-FGAX.txt", "C-FGAX.bin", "C-FGAX.asm" };
            string expected = string.Format("ERROR: ");

            for (int i = 0; i < extenstionTests.Length; i++)
            {
                fileManager.ReadData(extenstionTests[i]);
                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [Test]
        public void LoadingATelemetryFile_4111_6()
        {
            // 4111-6 Call ReadData() method with 3 non-English named data files
            // inputs: 43 2D 46 47 41 58, 0043 002D 0046 0047 0041 0058, 0043 002D 0046 0047 0041 0058
            string[] unicodeTests = { "43 2D 46 47 41 58", "0043 002D 0046 0047 0041 0058", "0043 002D 0046 0047 0041 0058" };
            string expected = string.Format("ERROR: ");

            for (int i = 0; i < unicodeTests.Length; i++)
            {
                fileManager.ReadData(unicodeTests[i]);
                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }


        long lastCalledTime;
        long timeGap;

        [Test]
        public void ReadingALineOfDataPerSecond_4112_3()
        {
            lastCalledTime = 0;
            timeGap = 0;
            fileManager.parser.ParserCalledHandler += CountFrame;

            Assert.LessOrEqual(timeGap, 5000);
        }

        void CountFrame(object sender, EventArgs e)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (lastCalledTime == 0)
            {
                lastCalledTime = milliseconds;
            }
            else
            {
                long timeGap = Math.Abs(milliseconds-lastCalledTime);
            }
        }

        [Test]
        public void ParsingLinesOfTelemetryData_4121_1()
        {
            Assert.IsNotNull(fileManager.parser);
        }

        [Test]
        public void ParsingLinesOfTelemetryData_4121_2()
        {
            string originalSample = " 7_8_2018 19:34:3,-0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622, ";
            Assert.DoesNotThrow(() => fileManager.parser.Parse(originalSample));
        }

        [Test]
        public void ParsingLinesOfTelemetryData_4121_3()
        {
            string fakeSample = " 7_8_2018, this, is, a, fake, lure, string, !,";
            Assert.Catch(() => fileManager.parser.Parse(fakeSample));
        }

        [Test]
        public void GeneratingChecksum_4122_1()
        {
            Assert.IsNotNull(fileManager.parser.packetizer.checksumCalculator);
        }

        [Test]
        public void GeneratingChecksum_4122_2()
        {
            int result = 0;
            string[] validSample = { "1124.106079", "0.022695", "0.001006" };
            Assert.DoesNotThrow(() => fileManager.parser.packetizer.checksumCalculator
            .CalculateChecksum(ref result, ref validSample[0], ref validSample[1], ref validSample[2]));
        }

        [Test]
        public void GeneratingChecksum_4112_3()
        {
            int result = 0;
            string[] validSample = { "1124.106079", "0.022695", "0.001006" };
            fileManager.parser.packetizer.checksumCalculator
            .CalculateChecksum(ref result, ref validSample[0], ref validSample[1], ref validSample[2]);
            Assert.AreEqual(Convert.ToInt32(Math.Round((double.Parse(validSample[0]) + double.Parse(validSample[1]) + double.Parse(validSample[2])))/3), result);
        }

        [Test]
        public void GeneratingChecksum_4122_4()
        {
            int result = 0;
            string[] validSample = { "1124.106079", "0.022695", "0.001006" };
            string[] dataTypeTesters = { "abc", "!@#", "123 123", "0031 0032 0033" };
            string[] overflowTesters = { "1.7976931348623157E+309", "-1.7976931348623157E+309" };

            for (int i = 0; i < dataTypeTesters.Length; i++)
            {
                Assert.Catch(() => fileManager.parser.packetizer.checksumCalculator
                .CalculateChecksum(ref result, ref dataTypeTesters[i], ref validSample[1], ref validSample[2]));
            }

            for (int i = 0; i < overflowTesters.Length; i++)
            {
                Assert.Catch(() => fileManager.parser.packetizer.checksumCalculator
                .CalculateChecksum(ref result, ref overflowTesters[i], ref validSample[1], ref validSample[2]));
            }
        }

        [Test]
        public void PacketizingTelemetryData_4123_1()
        {
            Assert.IsNotNull(fileManager.parser.packetizer);
        }

        [Test]
        public void PacketizingTelemetryData_4123_2()
        {
            string[] validParsedData = { " 7_8_2018 19:34:3", "-0.319754", "-0.716176", "1.797150", "2154.670410", "1643.844116", "0.022278", "0.033622" };
            Assert.DoesNotThrow(() => fileManager.parser.packetizer.Packetize(validParsedData));
        }
        
        [Test]
        public void PacketizingTelemetryData_4123_4()
        {
            string[] validParsedData = { " 7_8_2018 19:34:3", "this", "is", "a", "fake", "telemetry", "data", "!" };
            Assert.Catch(() => fileManager.parser.packetizer.Packetize(validParsedData));
        }

        [Test]
        public void ConfiguringNetwork_4131_1()
        {
            Assert.IsNotNull(fileManager.parser.packetizer.transmissionManager);
        }

        [Test]
        public void ConfiguringNetwork_4131_2()
        {
            fileManager.parser.packetizer.transmissionManager.configManager.ConfigureSocket();
            Assert.IsNotNull(fileManager.parser.packetizer.transmissionManager.configManager.sender);
        }

        [Test]
        public void ConnectingToAServer_4132_1()
        {
            Assert.IsNotNull(fileManager.parser.packetizer.transmissionManager.configManager);
        }

        [Test]
        public void ConnectingToAServer_4132_4()
        {
            string expected = "ERROR: failed to configure socket";
            fileManager.parser.packetizer.transmissionManager.configManager.ConfigureSocket();

            Assert.IsTrue(sw.ToString().Contains(expected));
        }
    }
}