using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace MyArduinoApp;

public class ArduinoBoard
{
    string port;
    int baudrate;
    int timeout;
    string feedback = string.Empty;
    SerialPort arduino = null;

    public ArduinoBoard(string port = "COM14", int baudrate = 9600, int timeout = 100)
    {
        this.port = port;
        this.baudrate = baudrate;
        this.timeout = timeout;
    }

    public void Initialize()
    {
        arduino = new SerialPort(port, baudrate)
        {
            ReadTimeout = timeout, // Timeout in milliseconds
            WriteTimeout = timeout
        };
        arduino.Open();
    }

    public string SendCommand(string cmd, int timeout = 10000)
    {
        if (arduino == null || !arduino.IsOpen)
        {
            throw new InvalidOperationException("Arduino is not initialized or the connection is not open.");
        }

        // Send command to Arduino
        arduino.Write(cmd);

        DateTime startTime = DateTime.Now;

        // Wait for a response
        while ((DateTime.Now - startTime).TotalMilliseconds < timeout)
        {
            // Check if there is data available
            if (arduino.BytesToRead > 0)
            {
                feedback = arduino.ReadLine().Trim();
                if (!string.IsNullOrEmpty(feedback))
                {
                    return feedback;
                }
            }
            Thread.Sleep(10); // Small delay to prevent CPU overuse
        }

        throw new TimeoutException("No response from Arduino within the specified timeout.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            ArduinoBoard arduino = new ArduinoBoard();
            arduino.Initialize();

            Thread.Sleep(1000);

            string feedback = arduino.SendCommand("motor2 rotate");

            Console.WriteLine(feedback);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
