using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using CommunityToolkit.Mvvm.DependencyInjection;
using TouchlessWhiteboard.ViewModel;
using TouchlessWhiteboard.ViewModels;
using System.Threading.Tasks;
using Windows.System;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;
using TouchlessWhiteboard.Models;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Devices.SmartCards;
using Windows.Storage.Pickers;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using WinRT.Interop;
using WinUIEx;
using Microsoft.UI.Windowing;
using Microsoft.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TouchlessWhiteboard;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TeachingMaterials : Window
{
    private int screenWidth = 50;
    private int screenHeight;
    private int MiddleY;
    private int buttonNum = 1;
    public TeachingMaterials(StorageFile file)
    {
        ViewModel = Ioc.Default.GetService<TeachingMaterialsViewModel>();
        Title = "Teaching Materials";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        this.InitializeComponent();
        InitializeAsync(file);
        this.SetIsAlwaysOnTop(true);
    }
    public TeachingMaterialsViewModel? ViewModel { get; }

    private async void InitializeAsync(StorageFile file)
    {
        bool success = await Addbuttons(file);
        if (!success)
        {
            // If the file is not a valid file, show an error message
        }
    }
    private void calculateCoordinates(Window window)
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

        if (Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId) is Microsoft.UI.Windowing.AppWindow appWindow &&
            DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest) is DisplayArea displayArea)
        {
            MiddleY = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;
        }
    }

    private async Task<bool> Addbuttons(StorageFile file)
    {
        try
        {
            using (StreamReader filereader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                string line;
                while ((line = await filereader.ReadLineAsync()) != null)
                {
                    // Remove quotes at the start and end of the line if they exist
                    if (line.StartsWith("\"") && line.EndsWith("\""))
                    {
                        line = line.Substring(1, line.Length - 2);
                    }

                    // Check if line is a valid file path
                    try
                    {
                        StorageFile storageFile = await StorageFile.GetFileFromPathAsync(line);

                        Button button = new Button();
                        Style toolBarButtonStyle = (Style)Application.Current.Resources["TeachingMaterialsButtonStyle"];
                        button.Style = toolBarButtonStyle;

                        ToolTipService.SetToolTip(button, storageFile.Name);

                        button.Content = buttonNum;
                        button.Click += Button_Click;
                        TeachingMaterialsPanel.Children.Add(button);
                        buttonNum++;
                    }
                    catch (Exception)
                    {
                        // If GetFileFromPathAsync throws an exception, the file does not exist
                        // Check if line is a valid URL
                        if (Uri.IsWellFormedUriString(line, UriKind.Absolute))
                        {
                            Button button = new Button();
                            Style toolBarButtonStyle = (Style)Application.Current.Resources["TeachingMaterialsButtonStyle"];
                            button.Style = toolBarButtonStyle;

                            ToolTipService.SetToolTip(button, line);

                            // Create an Image control
                            button.Content = buttonNum;
                            button.Click += Button_Click;

                            TeachingMaterialsPanel.Children.Add(button);
                            buttonNum++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        // If all lines are valid file paths or URLs, return true
        this.SetWindowSize(screenWidth, (buttonNum - 1) * 60);
        calculateCoordinates(this);
        this.Move(0, MiddleY);
        return true;

    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string pathOrUrl = (string)button.Content;

        // Check if it's a URL or a file path
        if (Uri.IsWellFormedUriString(pathOrUrl, UriKind.Absolute))
        {
            // Open the URL
            await Launcher.LaunchUriAsync(new Uri(pathOrUrl));
        }
        else
        {
            // Open the file
            StorageFile file = await StorageFile.GetFileFromPathAsync(pathOrUrl);
            await Launcher.LaunchFileAsync(file);
        }
    }

}
