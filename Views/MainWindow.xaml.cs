using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyArduinoApp;

public partial class MainWindow : Window
{   
    private ArduinoBoard _appLogic = null!;

    // Constructor
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Ensure DataContext is correctly set
        if (Application.Current is App app)
        {
            _appLogic = app.arduino;
            Console.WriteLine("Arduino successfully retrieved: " + (_appLogic != null));
        }
        else
        {
            Console.WriteLine("Application.Current is not of type App.");
            MessageBox.Show("Failed to access App instance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SendCommand_Click(object sender, RoutedEventArgs e)
    {   
        if (_appLogic == null)
        {
            MessageBox.Show("Failed to initialize Arduino. Please check your configuration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Send a command to Arduino and display the response
        string command = CommandInput.Text;

        Thread.Sleep(1000);

        string feedback = _appLogic.SendCommand(command);

        ResponseOutput.Text = feedback;
        
    }
}