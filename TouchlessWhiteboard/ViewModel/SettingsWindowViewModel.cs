using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouchlessWhiteboard.ViewModel;

public partial class SettingsWindowViewModel : INotifyPropertyChanged
{
    private bool _isTouchlessArtsEnabled;
    private bool _isEraserEnabled;
    private bool _isShapesEnabled;
    private bool _isSelectionEnabled;
    private bool _isStickyNotesEnabled;
    private bool _isCameraEnabled;
    private bool _isSearchEnabled;
    private bool _isCopilotEnabled;
    private bool _isToolsEnabled;
    private bool _isInAir3DMouseEnabled;

    private bool _isLeftHanded;
    private bool _isRightHanded;

    private int _pinchSensitivity;

    private bool _isCalculatorEnabled;
    private bool _isRulerEnabled;
    private bool _isTimerEnabled;
    private bool _isAlarmEnabled;
    private bool _isQuickFileAccessEnabled;

    public SettingsWindowViewModel()
    {
    }
    public bool IsTouchlessArtsEnabled
    {
        get { return _isTouchlessArtsEnabled; }
        set
        {
            _isTouchlessArtsEnabled = value;
            OnPropertyChanged("IsTouchlessArtsEnabled");
        }
    }

    public bool IsEraserEnabled
    {
        get { return _isEraserEnabled; }
        set
        {
            _isEraserEnabled = value;
            OnPropertyChanged("IsEraserEnabled");
        }
    }

    public bool IsShapesEnabled
    {
        get { return _isShapesEnabled; }
        set
        {
            _isShapesEnabled = value;
            OnPropertyChanged("IsShapesEnabled");
        }
    }

    public bool IsSelectionEnabled
    {
        get { return _isSelectionEnabled; }
        set
        {
            _isSelectionEnabled = value;
            OnPropertyChanged("IsSelectionEnabled");
        }
    }

    public bool IsStickyNotesEnabled
    {
        get { return _isStickyNotesEnabled; }
        set
        {
            _isStickyNotesEnabled = value;
            OnPropertyChanged("IsStickyNotesEnabled");
        }
    }

    public bool IsCameraEnabled
    {
        get { return _isCameraEnabled; }
        set
        {
            _isCameraEnabled = value;
            OnPropertyChanged("IsCameraEnabled");
        }
    }

    public bool IsSearchEnabled
    {
        get { return _isSearchEnabled; }
        set
        {
            _isSearchEnabled = value;
            OnPropertyChanged("IsSearchEnabled");
        }
    }

    public bool IsCopilotEnabled
    {
        get { return _isCopilotEnabled; }
        set
        {
            _isCopilotEnabled = value;
            OnPropertyChanged("IsCopilotEnabled");
        }
    }

    public bool IsToolsEnabled
    {
        get { return _isToolsEnabled; }
        set
        {
            _isToolsEnabled = value;
            OnPropertyChanged("IsToolsEnabled");
        }
    }

    public bool IsInAir3DMouseEnabled
    {
        get { return _isInAir3DMouseEnabled; }
        set
        {
            _isInAir3DMouseEnabled = value;
            OnPropertyChanged("IsInAir3DMouseEnabled");
        }
    }

    public bool IsLeftHanded
    {
        get { return _isLeftHanded; }
        set
        {
            _isLeftHanded = value;
            OnPropertyChanged("IsLeftHanded");
        }
    }

    public bool IsRightHanded
    {
        get { return _isRightHanded; }
        set
        {
            _isRightHanded = value;
            OnPropertyChanged("IsRightHanded");
        }
    }

    public int PinchSensitivity
    {
        get { return _pinchSensitivity; }
        set
        {
            _pinchSensitivity = value;
            OnPropertyChanged("PinchSensitivity");
        }
    }

    public bool IsCalculatorEnabled
    {
        get { return _isCalculatorEnabled; }
        set
        {
            _isCalculatorEnabled = value;
            OnPropertyChanged("IsCalculatorEnabled");
        }
    }

    public bool IsRulerEnabled
    {
        get { return _isRulerEnabled; }
        set
        {
            _isRulerEnabled = value;
            OnPropertyChanged("IsRulerEnabled");
        }
    }

    public bool IsTimerEnabled
    {
        get { return _isTimerEnabled; }
        set
        {
            _isTimerEnabled = value;
            OnPropertyChanged("IsTimerEnabled");
        }
    }

    public bool IsAlarmEnabled
    {
        get { return _isAlarmEnabled; }
        set
        {
            _isAlarmEnabled = value;
            OnPropertyChanged("IsAlarmEnabled");
        }
    }

    public bool IsQuickFileAccessEnabled
    {
        get { return _isQuickFileAccessEnabled; }
        set
        {
            _isQuickFileAccessEnabled = value;
            OnPropertyChanged("IsQuickFileAccessEnabled");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
