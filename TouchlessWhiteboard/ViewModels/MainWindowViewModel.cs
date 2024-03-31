using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Drawing;
using Windows.Storage;


namespace TouchlessWhiteboard.ViewModel;

public partial class MainWindowViewModel : ObservableObject, INotifyPropertyChanged
{
    //IsTouchlessArtsEnabled = ActiveProfile.IsTouchlessArtsEnabled;
    //IsStickyNotesEnabled = ActiveProfile.IsStickyNotesEnabled;
    //IsCameraEnabled = ActiveProfile.IsCameraEnabled;
    //IsSearchEnabled = ActiveProfile.IsSearchEnabled;
    //IsCopilotEnabled = ActiveProfile.IsCopilotEnabled;
    //IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
    //IsClockEnabled = ActiveProfile.IsClockEnabled;
    //IsQuickWebSiteAccess1Enabled = ActiveProfile.IsQuickWebSiteAccess1Enabled;
    //QuickWebSiteAccess1URL = ActiveProfile.QuickWebSiteAccess1URL;
    //IsQuickWebSiteAccess2Enabled = ActiveProfile.IsQuickWebSiteAccess2Enabled;
    //QuickWebSiteAccess2URL = ActiveProfile.QuickWebSiteAccess2URL;
    //IsQuickWebSiteAccess3Enabled = ActiveProfile.IsQuickWebSiteAccess3Enabled;
    //QuickWebSiteAccess3URL = ActiveProfile.QuickWebSiteAccess3URL;
    //IsInAir3DMouseEnabled = ActiveProfile.IsInAir3DMouseEnabled;
    //IsNotepadEnabled = ActiveProfile.IsNotepadEnabled;
    //IsQuickFileAccess1Enabled = ActiveProfile.IsQuickFileAccess1Enabled;
    //QuickFileAccess1File = ActiveProfile.QuickFileAccess1File;
    //IsQuickFileAccess2Enabled = ActiveProfile.IsQuickFileAccess2Enabled;
    //QuickFileAccess2File = ActiveProfile.QuickFileAccess2File;
    //IsQuickFileAccess3Enabled = ActiveProfile.IsQuickFileAccess3Enabled;
    //QuickFileAccess3File = ActiveProfile.QuickFileAccess3File;
    //IsLeftHanded = ActiveProfile.IsLeftHanded;
    //IsRightHanded = ActiveProfile.IsRightHanded;
    //PinchSensitivity = ActiveProfile.PinchSensitivity;
    //IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
    public string Name { get; set; }
    public bool IsTouchlessArtsEnabled { get; set; }
    public bool IsStickyNotesEnabled { get; set; }
    public bool IsCameraEnabled { get; set; }
    public bool IsSearchEnabled { get; set; }
    public bool IsCopilotEnabled { get; set; }
    public bool IsCalculatorEnabled { get; set; }
    public bool IsClockEnabled { get; set; }
    public bool IsQuickWebSiteAccess1Enabled { get; set; }
    public string QuickWebSiteAccess1URL { get; set; }
    public bool IsQuickWebSiteAccess2Enabled { get; set; }
    public string QuickWebSiteAccess2URL { get; set; }
    public bool IsQuickWebSiteAccess3Enabled { get; set; }
    public string QuickWebSiteAccess3URL { get; set; }
    public bool IsInAir3DMouseEnabled { get; set; }
    public bool IsNotepadEnabled { get; set; }
    public bool IsQuickFileAccess1Enabled { get; set; }
    public StorageFile QuickFileAccess1File { get; set; }
    public bool IsQuickFileAccess2Enabled { get; set; }
    public StorageFile QuickFileAccess2File { get; set; }
    public bool IsQuickFileAccess3Enabled { get; set; }
    public StorageFile QuickFileAccess3File { get; set; }
        
    [ObservableProperty]
    private Visibility isTouchlessWhiteboardOpen;

    [ObservableProperty]
    private Visibility isIconShown;

    [ObservableProperty]
    private Visibility isTouchlessArtsOpen;


    public MainWindowViewModel()
    {
        //Name = "Touchless Whiteboard";
        //IsTouchlessArtsEnabled = true;
        //IsEraserEnabled = true;
        //IsShapesEnabled = true;
        //IsSelectionEnabled = true;
        //IsStickyNotesEnabled = true;
        //IsCameraEnabled = true;
        //IsSearchEnabled = true;
        //IsCopilotEnabled = true;
        //IsToolsEnabled = true;
        //IsInAir3DMouseEnabled = true;
        //IsCalculatorEnabled = true;
        //IsRulerEnabled = true;
        //IsTimerEnabled = true;
        //IsAlarmEnabled = true;
        //IsQuickFileAccessEnabled = true;

    }

}

