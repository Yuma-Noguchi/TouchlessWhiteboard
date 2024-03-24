using CommunityToolkit.Mvvm.DependencyInjection;
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
using System.Runtime.InteropServices.WindowsRuntime;
using TouchlessWhiteboard.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TouchlessWhiteboard;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        this.InitializeComponent();
        
        Title = "Settings";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        ViewModel = Ioc.Default.GetService<SettingsWindowViewModel>();
    }

    public SettingsWindowViewModel? ViewModel { get; }

    private void LaunchButton_Click(object sender, RoutedEventArgs e)
    {

        var mainWindowViewModel = new MainWindowViewModel();

        mainWindowViewModel.IsTouchlessArtsEnabled = ViewModel.IsTouchlessArtsEnabled;
        mainWindowViewModel.IsEraserEnabled = ViewModel.IsEraserEnabled;
        mainWindowViewModel.IsShapesEnabled = ViewModel.IsShapesEnabled;
        mainWindowViewModel.IsSelectionEnabled = ViewModel.IsSelectionEnabled;
        mainWindowViewModel.IsStickyNotesEnabled = ViewModel.IsStickyNotesEnabled;
        mainWindowViewModel.IsCameraEnabled = ViewModel.IsCameraEnabled;
        mainWindowViewModel.IsSearchEnabled = ViewModel.IsSearchEnabled;
        mainWindowViewModel.IsCopilotEnabled = ViewModel.IsCopilotEnabled;
        mainWindowViewModel.IsToolsEnabled = ViewModel.IsToolsEnabled;
        mainWindowViewModel.IsInAir3DMouseEnabled = ViewModel.IsInAir3DMouseEnabled;

        mainWindowViewModel.DominantHand = ViewModel.IsLeftHanded ? "Left" : "Right";

        mainWindowViewModel.PinchSensitivity = ViewModel.PinchSensitivity / 100.0;

        //// Launch the main window
        var mainWindow = new MainWindow();
        mainWindow.Activate();
    }

    private async void OnClickTouchlessArtsHelpBtn(object sender, RoutedEventArgs e)
    {
        // if wifi connection exists, open the help page
        await Windows.System.Launcher.LaunchUriAsync(new Uri("http://touchlesswhiteboard.com/"));
    }

    private void OpenEraserPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!EraserHelpPopup.IsOpen) { EraserHelpPopup.IsOpen = true; }
    }

    private void CloseEraserPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (EraserHelpPopup.IsOpen) { EraserHelpPopup.IsOpen = false; }
    }

    private void OpenShapesPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!ShapesHelpPopup.IsOpen) { ShapesHelpPopup.IsOpen = true; }
    }

    private void CloseShapesPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (ShapesHelpPopup.IsOpen) { ShapesHelpPopup.IsOpen = false; }
    }

    private void OpenSelectionPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!SelectionHelpPopup.IsOpen) { SelectionHelpPopup.IsOpen = true; }
    }

    private void CloseSelectionPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (SelectionHelpPopup.IsOpen) { SelectionHelpPopup.IsOpen = false; }
    }

    private void OpenStickyNotesPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!StickyNotesHelpPopup.IsOpen) { StickyNotesHelpPopup.IsOpen = true; }
    }

    private void CloseStickyNotesPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (StickyNotesHelpPopup.IsOpen) { StickyNotesHelpPopup.IsOpen = false; }
    }

    private void OpenCameraPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!CameraHelpPopup.IsOpen) { CameraHelpPopup.IsOpen = true; }
    }

    private void CloseCameraPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (CameraHelpPopup.IsOpen) { CameraHelpPopup.IsOpen = false; }
    }

    private void OpenSearchPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!SearchHelpPopup.IsOpen) { SearchHelpPopup.IsOpen = true; }
    }

    private void CloseSearchPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (SearchHelpPopup.IsOpen) { SearchHelpPopup.IsOpen = false; }
    }

    private void OpenCopilotPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!CopilotHelpPopup.IsOpen) { CopilotHelpPopup.IsOpen = true; }
    }

    private void CloseCopilotPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (CopilotHelpPopup.IsOpen) { CopilotHelpPopup.IsOpen = false; }
    }

    private void OpenToolsPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!ToolsHelpPopup.IsOpen) { ToolsHelpPopup.IsOpen = true; }
    }

    private void CloseToolsPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (ToolsHelpPopup.IsOpen) { ToolsHelpPopup.IsOpen = false; }
    }

    private void OpenInAir3DMousePopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!InAir3DMouseHelpPopup.IsOpen) { InAir3DMouseHelpPopup.IsOpen = true; }
    }

    private void CloseInAir3DMousePopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (InAir3DMouseHelpPopup.IsOpen) { InAir3DMouseHelpPopup.IsOpen = false; }
    }
}
