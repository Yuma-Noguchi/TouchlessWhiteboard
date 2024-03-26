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
using Windows.Foundation;
using Microsoft.UI.Input.DragDrop;
using Windows.Security.Authentication.OnlineId;
using Windows.ApplicationModel.Core;

namespace TouchlessWhiteboard;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private bool IsToolBarOpen = true;
    private int screenHeight;
    private int screenWidth;


    private Point MouseDownLocation;
    public MainWindow()
    {
        ViewModel = Ioc.Default.GetService<MainWindowViewModel>();
        SettingsWindowViewModel settingsWindowViewModel = Ioc.Default.GetService<SettingsWindowViewModel>();

        ViewModel.IsTouchlessArtsEnabled = settingsWindowViewModel.IsTouchlessArtsEnabled;
        ViewModel.IsEraserEnabled = settingsWindowViewModel.IsEraserEnabled;
        ViewModel.IsShapesEnabled = settingsWindowViewModel.IsShapesEnabled;
        ViewModel.IsSelectionEnabled = settingsWindowViewModel.IsSelectionEnabled;
        ViewModel.IsStickyNotesEnabled = settingsWindowViewModel.IsStickyNotesEnabled;
        ViewModel.IsCameraEnabled = settingsWindowViewModel.IsCameraEnabled;
        ViewModel.IsSearchEnabled = settingsWindowViewModel.IsSearchEnabled;
        ViewModel.IsCopilotEnabled = settingsWindowViewModel.IsCopilotEnabled;
        ViewModel.IsToolsEnabled = settingsWindowViewModel.IsToolsEnabled;
        ViewModel.IsInAir3DMouseEnabled = settingsWindowViewModel.IsInAir3DMouseEnabled;
        ViewModel.IsCalculatorEnabled = settingsWindowViewModel.IsCalculatorEnabled;
        ViewModel.IsRulerEnabled = settingsWindowViewModel.IsRulerEnabled;
        ViewModel.IsTimerEnabled = settingsWindowViewModel.IsTimerEnabled;
        ViewModel.IsAlarmEnabled = settingsWindowViewModel.IsAlarmEnabled;
        ViewModel.IsQuickFileAccessEnabled = settingsWindowViewModel.IsQuickFileAccessEnabled;

        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Visible;
        ViewModel.IsIconShown = Visibility.Collapsed;
        this.InitializeComponent();
        this.SetIsAlwaysOnTop(true);
        this.Maximize();
        CreateButtons(ToolBarPanel);
        //remove title bar
        var coreTitleBar = this.GetAppWindow().TitleBar;
        coreTitleBar.ExtendsContentIntoTitleBar = true;

        screenHeight = this.AppWindow.Size.Height;
        screenWidth = this.AppWindow.Size.Width;
    }
    public MainWindowViewModel? ViewModel { get; }
    public SettingsWindowViewModel? SettingsWindowViewModel { get; }

    private TranslateTransform dragtranslation;

    private void objectManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        var stackDragged = e.OriginalSource as StackPanel;
        (stackDragged.RenderTransform as TranslateTransform).X += e.Delta.Translation.X;
        (stackDragged.RenderTransform as TranslateTransform).Y += e.Delta.Translation.Y;

    }

    public void CreateButtons(StackPanel panel)
    {
        panel.Children.Clear();
        if (IsToolBarOpen)
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
                button.Click += Camera_Clicked;
                break;
            case "Search":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Search-icon.png"));
                button.Click += Search_Clicked;
                break;
            case "Copilot":
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Copilot-icon.png"));
                button.Click += Copilot_Clicked;
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
    private void MinimizeTouchlessWhiteboard()
    {
        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Collapsed;
        ViewModel.IsIconShown = Visibility.Visible;
        this.MoveAndResize(10,10,150,150);
        var coreTitleBar = this.GetAppWindow().TitleBar;
        coreTitleBar.ExtendsContentIntoTitleBar = true;
        this.SetIsMaximizable(false);
        this.Bindings.Update();
    }
    private void Camera_Clicked(object sender, RoutedEventArgs e)
    {
        // open snipping tool
        Windows.System.Launcher.LaunchUriAsync(new Uri("ms-screenclip:"));
    }

    private void Search_Clicked(object sender, RoutedEventArgs e)
    {
        // open bing search
        Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.bing.com/"));
        MinimizeTouchlessWhiteboard();
    }

    private void Copilot_Clicked(object sender, RoutedEventArgs e)
    {
        Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.bing.com/chat"));
        MinimizeTouchlessWhiteboard();
    }

    private void CloseToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsToolBarOpen = false;
        CreateButtons(ToolBarPanel);
    }

    private void OpenToolBar_Clicked(object sender, RoutedEventArgs e)
    {
        IsToolBarOpen = true;
        CreateButtons(ToolBarPanel);
    }

    private void TouchlessWhiteboard_MouseDown(object sender, PointerRoutedEventArgs e)
    {
        ViewModel.IsTouchlessWhiteboardOpen = Visibility.Visible;
        ViewModel.IsIconShown = Visibility.Collapsed;
        // remove title bar
        var coreTitleBar = this.GetAppWindow().TitleBar;
        coreTitleBar.ExtendsContentIntoTitleBar = true;
        this.Bindings.Update();
        this.SetWindowSize(screenWidth, screenHeight);
        this.SetIsMaximizable(true);
    }
}
