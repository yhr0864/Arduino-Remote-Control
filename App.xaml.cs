using System.Configuration;
using System.Data;
using System.Windows;

namespace MyArduinoApp;

public partial class App : Application
{   
    public ArduinoBoard arduino { get; private set; } = null!;
    private void Application_Startup(object sender, StartupEventArgs e)
        {
            arduino = new ArduinoBoard();
            arduino.Initialize();

            Console.WriteLine("Arduino initialized: " + (arduino != null));

            // Console.WriteLine(arduino);
            // Launch the main window
            var mainWindow = new MainWindow();
            mainWindow.DataContext = arduino; // Pass arduino to the UI
            mainWindow.Show();
        }

    private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Perform cleanup if needed
            MessageBox.Show("Application is shutting down.");
        }
}

