using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using TouchlessWhiteboard.Models;
using TouchlessWhiteboard.ViewModel;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Devices.SmartCards;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Popups;
using Windows.UI.WindowManagement;
using WinRT.Interop;
using WinUIEx;
using System.Threading.Tasks;

namespace TouchlessWhiteboard;

public sealed partial class SettingsWindow : Window
{
    private int _counter = 0;
    private List<CheckBox> ToolsCheckBoxList = new List<CheckBox>();
    private StorageFile selectedFile;

    private string QuickFileAccess1Name;
    private string QuickFileAccess2Name;
    private string QuickFileAccess3Name;

    private string TeachingMaterialsErrorMessage = "The file you selected is not a valid teaching materials file. Please select a valid file.";

    public SettingsWindow()
    {
        this.InitializeComponent();
        
        Title = "Settings";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        ViewModel = Ioc.Default.GetService<SettingsWindowViewModel>();
        
        this.CenterOnScreen();
        this.SetWindowSize(1200, 840);
        this.CenterOnScreen();

        // add checkboxes to list
        ToolsCheckBoxList.Add(TouchlessArtsCheckBox);
        ToolsCheckBoxList.Add(StickyNotesCheckBox);
        ToolsCheckBoxList.Add(CameraCheckBox);
        ToolsCheckBoxList.Add(SearchCheckBox);
        ToolsCheckBoxList.Add(CopilotCheckBox);
        ToolsCheckBoxList.Add(CalculatorCheckBox);
        ToolsCheckBoxList.Add(ClockCheckBox);
        ToolsCheckBoxList.Add(QuickWebSiteAccess1CheckBox);
        ToolsCheckBoxList.Add(QuickWebSiteAccess2CheckBox);
        ToolsCheckBoxList.Add(QuickWebSiteAccess3CheckBox);
        ToolsCheckBoxList.Add(InAir3DMouseCheckBox);
        ToolsCheckBoxList.Add(NotepadCheckBox);
        ToolsCheckBoxList.Add(QuickFileAccess1CheckBox);
        ToolsCheckBoxList.Add(QuickFileAccess2CheckBox);
        ToolsCheckBoxList.Add(QuickFileAccess3CheckBox);


    }

    public SettingsWindowViewModel? ViewModel { get; }

    private void SettingsNameChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        ViewModel.Name = textBox.Text;
        this.Bindings.Update();
    }

    private void LaunchButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Launch();
        this.Minimize();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ProfileChange_Clicked(object sender, RoutedEventArgs e)
    {
        Button clickedButton = (Button)sender;
        Profile profile = (Profile)clickedButton.DataContext;

        // call changeprofile function in viewmodel
        ViewModel.ChangeProfile(profile);
        if (ViewModel.QuickFileAccess1File != null)
        {
            QuickFileAccess1TextBox.Text = ViewModel.QuickFileAccess1File.DisplayName;
        }
        else
        {
            QuickFileAccess1TextBox.Text = "";
        }
        if (ViewModel.QuickFileAccess2File != null)
        {
            QuickFileAccess2TextBox.Text = ViewModel.QuickFileAccess2File.DisplayName;
        }
        else
        {
            QuickFileAccess2TextBox.Text = "";
        }
        if (ViewModel.QuickFileAccess3File != null)
        {
            QuickFileAccess3TextBox.Text = ViewModel.QuickFileAccess3File.DisplayName;
        }
        else
        {
            QuickFileAccess3TextBox.Text = "";
        }
        this.Bindings.Update();
    }

    private void AddProfile_Clicked(object sender, RoutedEventArgs e)
    {
        ViewModel.AddProfile();
    }

    private void DeleteProfile_Clicked(object sender, RoutedEventArgs e)
    {
        // call deleteprofile function in viewmodel
        ViewModel.DeleteProfile();
    }

    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenHelp();
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        _counter++;
        if (_counter >= 10)
        {
            foreach (var checkbox in ToolsCheckBoxList)
            {
                if (checkbox.IsChecked == false)
                {
                    checkbox.IsEnabled = false;
                }
            }
        }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        _counter--;
        if (_counter < 10)
        {
            foreach (var checkbox in ToolsCheckBoxList)
            {
                checkbox.IsEnabled = true;
            }
        }
    }

    private async Task<StorageFile> ChooseFile(string type)
    {
        var filePicker = new FileOpenPicker();
        if (type != "")
        {
            filePicker.FileTypeFilter.Add(type);
        }
        else
        {
            filePicker.FileTypeFilter.Add("*");
        }
        nint windowHandle = this.GetWindowHandle();
        InitializeWithWindow.Initialize(filePicker, windowHandle);

        StorageFile file = await filePicker.PickSingleFileAsync();

        return file;
    }

    private async void QuickFileAccess1_Clicked(object sender, RoutedEventArgs e)
    {
        StorageFile file = await ChooseFile("");
        ViewModel.QuickFileAccess1File = file;
    }

    private async void QuickFileAccess2_Clicked(object sender, RoutedEventArgs e)
    {
        StorageFile file = await ChooseFile("");
        ViewModel.QuickFileAccess2File = file;
    }

    private async void QuickFileAccess3_Clicked(object sender, RoutedEventArgs e)
    {
        StorageFile file = await ChooseFile("");
        ViewModel.QuickFileAccess3File = file;
    }

    private async void TeachingMaterials_Clicked(object sender, RoutedEventArgs e)
    {
        StorageFile file = await ChooseFile(".txt");
        bool Isvalid = await CheckTeachingMaterials(file);
        if (Isvalid)
        {
            ViewModel.TeachingMaterials = file;
        }
        else
        {
            if (!TeachingMaterialsErrorPopup.IsOpen) { TeachingMaterialsErrorPopup.IsOpen = true; }
        }
    }

    private void TeachingMaterialsClear_Clicked(object sender, RoutedEventArgs e)
    {
        ViewModel.TeachingMaterials = null;
        TeachingMaterialsTextBox.Text = "";
        this.Bindings.Update();
    }

    private void CloseTeachingMaterialsErrorPopupClicked(object sender, RoutedEventArgs e)
    {
        if (TeachingMaterialsErrorPopup.IsOpen) { TeachingMaterialsErrorPopup.IsOpen = false; }
    }

    private async Task<bool> CheckTeachingMaterials(StorageFile file)
    {
        bool IsValid = await ViewModel.CheckTeachingMaterials(file);
        return IsValid;
    }
    private async void OnClickTouchlessArtsHelpBtn(object sender, RoutedEventArgs e)
    {
        // if wifi connection exists, open the help page
        await Windows.System.Launcher.LaunchUriAsync(new Uri("http://touchlesswhiteboard.com/"));
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
    private void OpenCalculatorPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!CalculatorHelpPopup.IsOpen) { CalculatorHelpPopup.IsOpen = true; }
    }

    private void CloseCalculatorPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (CalculatorHelpPopup.IsOpen) { CalculatorHelpPopup.IsOpen = false; }
    }
    
    private void OpenClockPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!ClockHelpPopup.IsOpen) { ClockHelpPopup.IsOpen = true; }
    }

    private void CloseClockPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (ClockHelpPopup.IsOpen) { ClockHelpPopup.IsOpen = false; }
    }

    private void OpenQuickWebSiteAccessPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!QuickWebSiteAccessHelpPopup.IsOpen) { QuickWebSiteAccessHelpPopup.IsOpen = true; }
    }

    private void CloseQuickWebSiteAccessPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (QuickWebSiteAccessHelpPopup.IsOpen) { QuickWebSiteAccessHelpPopup.IsOpen = false; }
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

    private void OpenNotepadPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!NotepadHelpPopup.IsOpen) { NotepadHelpPopup.IsOpen = true; }
    }

    private void CloseNotepadPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (NotepadHelpPopup.IsOpen) { NotepadHelpPopup.IsOpen = false; }
    }

    private void OpenQuickFileAccessPopupClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!QuickFileAccessHelpPopup.IsOpen) { QuickFileAccessHelpPopup.IsOpen = true; }
    }

    private void CloseQuickFileAccessPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (QuickFileAccessHelpPopup.IsOpen) { QuickFileAccessHelpPopup.IsOpen = false; }
    }
}