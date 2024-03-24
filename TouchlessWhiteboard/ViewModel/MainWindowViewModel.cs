﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouchlessWhiteboard.ViewModel;

public partial class MainWindowViewModel
{
    public bool IsTouchlessArtsEnabled { get; set; }
    public bool IsEraserEnabled { get; set; }
    public bool IsShapesEnabled { get; set; }
    public bool IsSelectionEnabled { get; set; }
    public bool IsStickyNotesEnabled { get; set; }
    public bool IsCameraEnabled { get; set; }
    public bool IsSearchEnabled { get; set; }
    public bool IsCopilotEnabled { get; set; }
    public bool IsToolsEnabled { get; set; }
    public bool IsInAir3DMouseEnabled { get; set; }
    public string DominantHand { get; set; }
    public double PinchSensitivity { get; set; }
    public bool IsCalculatorEnabled { get; set; }
    public bool IsRulerEnabled { get; set; }
    public bool IsTimerEnabled { get; set; }
    public bool IsAlarmEnabled { get; set; }
    public bool IsQuickFileAccessEnabled { get; set; }


    public MainWindowViewModel()
    {
    }



}

