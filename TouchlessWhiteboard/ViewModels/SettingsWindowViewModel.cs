using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TouchlessWhiteboard.Models;

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
    private double pinchSensitivity;

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

    private readonly WebcamService _webcamService;
    [ObservableProperty]
    private List<string> webcams;
    [ObservableProperty]
    private string selectedWebcam;

    private readonly ProfileService _profileService;
    [ObservableProperty]
    private List<Profile> profiles;

    public Profile ActiveProfile { get; set; }

    public SettingsWindowViewModel(WebcamService webcamService, ProfileService profileService)
    {
        _webcamService = webcamService;
        _profileService = profileService;
        InitializeAsync();
        if (profiles != null && profiles.Any())
        {
            ActiveProfile = profiles.First();

            // set all the properties to the values of the active profile
            isTouchlessArtsEnabled = ActiveProfile.IsTouchlessArtsEnabled;
            isEraserEnabled = ActiveProfile.IsEraserEnabled;
            isShapesEnabled = ActiveProfile.IsShapesEnabled;
            isSelectionEnabled = ActiveProfile.IsSelectionEnabled;
            isStickyNotesEnabled = ActiveProfile.IsStickyNotesEnabled;
            isCameraEnabled = ActiveProfile.IsCameraEnabled;
            isSearchEnabled = ActiveProfile.IsSearchEnabled;
            isCopilotEnabled = ActiveProfile.IsCopilotEnabled;
            isToolsEnabled = ActiveProfile.IsToolsEnabled;
            isInAir3DMouseEnabled = ActiveProfile.IsInAir3DMouseEnabled;
            isLeftHanded = ActiveProfile.IsLeftHanded;
            isRightHanded = ActiveProfile.IsRightHanded;
            pinchSensitivity = ActiveProfile.PinchSensitivity;
            isCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
            isRulerEnabled = ActiveProfile.IsRulerEnabled;
            isTimerEnabled = ActiveProfile.IsTimerEnabled;
            isAlarmEnabled = ActiveProfile.IsAlarmEnabled;
            isQuickFileAccessEnabled = ActiveProfile.IsQuickFileAccessEnabled;
            // if selected webcam exists in the list of available webcams, set it as the selected webcam
            if (webcams.Contains(ActiveProfile.SelectedWebcam))
            {
                SelectedWebcam = ActiveProfile.SelectedWebcam;
            }
            else
            {
                if (webcams.Count > 0)
                {
                    SelectedWebcam = webcams[0];
                }
            }
        }
        PropertyChanged += (s, e) => UpdateProfile();
    }

    private async void InitializeAsync()
    {
        Profiles = await _profileService.LoadProfilesFromJson("Resources/settings.json");

        webcams = await _webcamService.GetAvailableWebcams();
        if (webcams.Count > 0)
        {
            SelectedWebcam = webcams[0];
        }
    }

    private void UpdateProfile()
    {
        // Assuming 'profiles' is not null and has at least one element

        ActiveProfile.IsTouchlessArtsEnabled = isTouchlessArtsEnabled;
        ActiveProfile.IsEraserEnabled = isEraserEnabled;
        ActiveProfile.IsShapesEnabled = isShapesEnabled;
        ActiveProfile.IsSelectionEnabled = isSelectionEnabled;
        ActiveProfile.IsStickyNotesEnabled = isStickyNotesEnabled;
        ActiveProfile.IsCameraEnabled = isCameraEnabled;
        ActiveProfile.IsSearchEnabled = isSearchEnabled;
        ActiveProfile.IsCopilotEnabled = isCopilotEnabled;
        ActiveProfile.IsToolsEnabled = isToolsEnabled;
        ActiveProfile.IsInAir3DMouseEnabled = isInAir3DMouseEnabled;
        ActiveProfile.IsLeftHanded = isLeftHanded;
        ActiveProfile.IsRightHanded = isRightHanded;
        ActiveProfile.PinchSensitivity = pinchSensitivity;
        ActiveProfile.IsCalculatorEnabled = isCalculatorEnabled;
        ActiveProfile.IsRulerEnabled = isRulerEnabled;
        ActiveProfile.IsTimerEnabled = isTimerEnabled;
        ActiveProfile.IsAlarmEnabled = isAlarmEnabled;
        ActiveProfile.IsQuickFileAccessEnabled = isQuickFileAccessEnabled;
        ActiveProfile.SelectedWebcam = SelectedWebcam;
    }
}
