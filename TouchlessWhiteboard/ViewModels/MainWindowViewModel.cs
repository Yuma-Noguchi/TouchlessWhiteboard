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
using TouchlessWhiteboard;


namespace TouchlessWhiteboard.ViewModel;

public partial class MainWindowViewModel : ObservableObject, INotifyPropertyChanged
{
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
    public StorageFile TeachingMaterials { get; set; }
        
    [ObservableProperty]
    private Visibility isTouchlessWhiteboardOpen;

    [ObservableProperty]
    private Visibility isIconShown;

    [ObservableProperty]
    private Visibility isTouchlessArtsOpen;


    public MainWindowViewModel()
    {
        //Name = "Touchless Whiteboard";
        //IsTouchlessArtsEnabled = false;
        //IsStickyNotesEnabled = true;
        //IsCameraEnabled = false;
        //IsSearchEnabled = false;
        //IsCopilotEnabled = false;
        //IsCalculatorEnabled = true;
        //IsClockEnabled = true;
        //IsQuickWebSiteAccess1Enabled = true;
        //QuickWebSiteAccess1URL = "https://www.google.com";
        //IsQuickWebSiteAccess2Enabled = true;
        //QuickWebSiteAccess2URL = "https://www.bing.com";
        //IsQuickWebSiteAccess3Enabled = true;
        //QuickWebSiteAccess3URL = "https://www.yahoo.com";
        //IsInAir3DMouseEnabled = false;
        //IsNotepadEnabled = true;
        //IsQuickFileAccess1Enabled = true;
        //IsQuickFileAccess2Enabled = true;
        //IsQuickFileAccess3Enabled = true;
    }

}

