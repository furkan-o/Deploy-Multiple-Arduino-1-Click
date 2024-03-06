using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;

class Program
{
    static void Main()
    {
        string avrdudePath = "avrdude.exe"; // Path to avrdude (change path if you didn't copy it in the same folder)
        string avrdudeConfPath = "avrdude.conf"; // Path to avrdude.conf (change path if you didn't copy it in the same folder)

        string[] hexFiles = Directory.GetFiles(".", "*.hex");

        foreach (string hexFile in hexFiles)
        {
            Console.WriteLine($"Deploying {hexFile}");

            foreach (string portName in SerialPort.GetPortNames())
            {
                Console.WriteLine($"  Uploading to {portName}");
                UploadFirmware(hexFile, portName, avrdudePath, avrdudeConfPath);
            }
        }

        Console.WriteLine("Deployment complete.");
    }

    static void UploadFirmware(string hexFile, string portName, string avrdudePath, string avrdudeConfPath)
    {
        Process avrdudeProcess = new Process();
        avrdudeProcess.StartInfo.FileName = avrdudePath;
        avrdudeProcess.StartInfo.Arguments = $"-C{avrdudeConfPath} -v -patmega2560 -carduino -P{portName} -b115200 -D -Uflash:w:\"{hexFile}\":i";
        avrdudeProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(avrdudePath);
        avrdudeProcess.StartInfo.UseShellExecute = false;
        avrdudeProcess.StartInfo.RedirectStandardOutput = true;
        avrdudeProcess.Start();
        avrdudeProcess.WaitForExit();

        string output = avrdudeProcess.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
    }
}
