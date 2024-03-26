using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition;
using Microsoft.UI.Text;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TouchlessWhiteboard.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using WinUIEx;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace TouchlessWhiteboard;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{

    const int GWL_EXSTYLE = -20;
    const int WS_EX_TRANSPARENT = 0x20;
    const int WS_EX_LAYERED = 0x80000;

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    public static extern IntPtr GetWindowLong64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLong64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // Use this method to call the appropriate function based on the platform
    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong)
    {
        if (IntPtr.Size == 8)
            return SetWindowLong64(hWnd, nIndex, new IntPtr(dwNewLong)); // 64-bit
        else
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong)); // 32-bit
    }

    // Use this method to call the appropriate function based on the platform
    public static int GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 8)
            return (int)GetWindowLong64(hWnd, nIndex).ToInt64(); // 64-bit
        else
            return GetWindowLong32(hWnd, nIndex); // 32-bit
    }

    private bool IsOpen = true;
    public MainWindow()
    {
        this.InitializeComponent();
        //this.SetIsAlwaysOnTop(true);
        this.Maximize();

        //var windowHandle = new IntPtr((long)WindowNative.GetWindowHandle(this));
        //int extendedStyle = GetWindowLongPtr(windowHandle, GWL_EXSTYLE);
        //SetWindowLongPtr(windowHandle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

        ViewModel = Ioc.Default.GetService<MainWindowViewModel>();
        CreateButtons(ToolBarPanel);
    }
    public MainWindowViewModel? ViewModel { get; }

    public void CreateButtons(StackPanel panel)
    {
        panel.Children.Clear();
        if (IsOpen)
        {
            if (ViewModel.IsTouchlessArtsEnabled)
                panel.Children.Add(CreateButton("Touchless Arts"));
            if (ViewModel.IsEraserEnabled)
                panel.Children.Add(CreateButton("Eraser"));
            if (ViewModel.IsShapesEnabled)
                panel.Children.Add(CreateButton("Shapes"));
            if (ViewModel.IsSelectionEnabled)
                panel.Children.Add(CreateButton("Selection"));
            if (ViewModel.IsStickyNotesEnabled)
                panel.Children.Add(CreateButton("Sticky Notes"));
            if (ViewModel.IsCameraEnabled)
                panel.Children.Add(CreateButton("Camera"));
            if (ViewModel.IsSearchEnabled)
                panel.Children.Add(CreateButton("Search"));
            if (ViewModel.IsCopilotEnabled)
                panel.Children.Add(CreateButton("Copilot"));
            if (ViewModel.IsToolsEnabled)
                panel.Children.Add(CreateButton("Tools"));
            if (ViewModel.IsInAir3DMouseEnabled)
                panel.Children.Add(CreateButton("In Air 3D Mouse"));
            panel.Children.Add(CreateButton("Close"));
        }
        else
        {
            panel.Children.Add(CreateButton("Open"));
        }
    }


    private Button CreateButton(string buttonType)
    {

        Button button = new Button();
        Style toolBarButtonStyle = (Style)Application.Current.Resources["ToolBarButtonStyle"];
        button.Style = toolBarButtonStyle;

        // Create an Image control
        Image image = new Image();
        Style toolBarButtonImageStyle = (Style)Application.Current.Resources["ToolBarButtonImageStyle"];
        image.Style = toolBarButtonImageStyle;

        // Set the source of the Image control based on the buttonType
        switch (buttonType)
        {
            case "Touchless Arts":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Touchless-Arts-icon.png"));
                break;
            case "Eraser":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Eraser-icon.png"));
                break;
            case "Shapes":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Shapes-icon.png"));
                break;
            case "Selection":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Selection-icon.png"));
                break;
            case "Sticky Notes":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Sticky-Notes-icon.png"));
                break;
            case "Camera":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Camera-icon.png"));
                break;
            case "Search":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Search-icon.png"));
                break;
            case "Copilot":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Copilot-icon.png"));
                break;
            case "Tools":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Tools-icon.png"));
                break;
            case "In Air 3D Mouse":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/In-Air-3D-Mouse-icon.png"));
                break;
            case "Close":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Close-icon.png"));
                button.Click += CloseToolBar_Clicked;
                break;
            case "Open":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Open-icon.png"));
                button.Click += OpenToolBar_Clicked;
                break;
        }

        // Set the Image control as the content of the button
        button.Content = image;

        // Set other properties and event handlers as needed
        return button;

    }

    private void CloseToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsOpen = false;
        CreateButtons(ToolBarPanel);
    }

    private void OpenToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsOpen = true;
        CreateButtons(ToolBarPanel);
    }



}
