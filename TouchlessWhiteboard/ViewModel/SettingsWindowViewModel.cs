using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouchlessWhiteboard.ViewModel;

public partial class SettingsWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isTouchlessArtsEnabled;
    [ObservableProperty]
    private bool isEraserEnabled;
    [ObservableProperty]
    private bool isShapesEnabled;
    [ObservableProperty]
    private bool isSelectionEnabled;
    [ObservableProperty]
    private bool isStickyNotesEnabled;
    [ObservableProperty]
    private bool isCameraEnabled;
    [ObservableProperty]
    private bool isSearchEnabled;
    [ObservableProperty]
    private bool isCopilotEnabled;
    [ObservableProperty]
    private bool isToolsEnabled;
    [ObservableProperty]
    private bool isInAir3DMouseEnabled;

    [ObservableProperty]
    private bool isLeftHanded;
    [ObservableProperty]
    private bool isRightHanded;

    [ObservableProperty]
    private int pinchSensitivity;

    [ObservableProperty]
    private bool isCalculatorEnabled;
    [ObservableProperty]
    private bool isRulerEnabled;
    [ObservableProperty]
    private bool isTimerEnabled;
    [ObservableProperty]
    private bool isAlarmEnabled;
    [ObservableProperty]
    private bool isQuickFileAccessEnabled;

    public SettingsWindowViewModel()
    {
    }
    
}
