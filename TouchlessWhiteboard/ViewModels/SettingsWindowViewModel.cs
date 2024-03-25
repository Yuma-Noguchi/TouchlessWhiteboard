using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TouchlessWhiteboard.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WinUIEx;
using System.IO;

namespace TouchlessWhiteboard.ViewModel;

public partial class SettingsWindowViewModel : ObservableObject, INotifyPropertyChanged
{
    [ObservableProperty]
    private string name;
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
    private ObservableCollection<Profile> profiles;

    [ObservableProperty]
    private bool isSelected;

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
            name = ActiveProfile.Name;
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
            ActiveProfile.IsSelected = true;
            isSelected = true;
        }
        PropertyChanged += (s, e) => UpdateProfile(s, e);
    }

    private async void InitializeAsync()
    {
        Profiles = new ObservableCollection<Profile>(await _profileService.LoadProfilesFromJson("Resources/settings.json"));

        webcams = await _webcamService.GetAvailableWebcams();
        if (webcams.Count > 0)
        {
            SelectedWebcam = webcams[0];
        }
    }

    private void UpdateProfile(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Name):
                ActiveProfile.Name = Name;
                break;
            case nameof(IsTouchlessArtsEnabled):
                ActiveProfile.IsTouchlessArtsEnabled = IsTouchlessArtsEnabled;
                break;
            case nameof(IsEraserEnabled):
                ActiveProfile.IsEraserEnabled = IsEraserEnabled;
                break;
            case nameof(IsShapesEnabled):
                ActiveProfile.IsShapesEnabled = IsShapesEnabled;
                break;
            case nameof(IsSelectionEnabled):
                ActiveProfile.IsSelectionEnabled = IsSelectionEnabled;
                break;
            case nameof(IsStickyNotesEnabled):
                ActiveProfile.IsStickyNotesEnabled = IsStickyNotesEnabled;
                break;
            case nameof(IsCameraEnabled):
                ActiveProfile.IsCameraEnabled = IsCameraEnabled;
                break;
            case nameof(IsSearchEnabled):
                ActiveProfile.IsSearchEnabled = IsSearchEnabled;
                break;
            case nameof(IsCopilotEnabled):
                ActiveProfile.IsCopilotEnabled = IsCopilotEnabled;
                break;
            case nameof(IsToolsEnabled):
                ActiveProfile.IsToolsEnabled = IsToolsEnabled;
                break;
            case nameof(IsInAir3DMouseEnabled):
                ActiveProfile.IsInAir3DMouseEnabled = IsInAir3DMouseEnabled;
                break;
            case nameof(IsLeftHanded):
                ActiveProfile.IsLeftHanded = IsLeftHanded;
                break;
            case nameof(IsRightHanded):
                ActiveProfile.IsRightHanded = IsRightHanded;
                break;
            case nameof(PinchSensitivity):
                ActiveProfile.PinchSensitivity = PinchSensitivity;
                break;
            case nameof(IsCalculatorEnabled):
                ActiveProfile.IsCalculatorEnabled = IsCalculatorEnabled;
                break;
            case nameof(IsRulerEnabled):
                ActiveProfile.IsRulerEnabled = IsRulerEnabled;
                break;
            case nameof(IsTimerEnabled):
                ActiveProfile.IsTimerEnabled = IsTimerEnabled;
                break;
            case nameof(IsAlarmEnabled):
                ActiveProfile.IsAlarmEnabled = IsAlarmEnabled;
                break;
            case nameof(IsQuickFileAccessEnabled):
                ActiveProfile.IsQuickFileAccessEnabled = IsQuickFileAccessEnabled;
                break;
            case nameof(SelectedWebcam):
                ActiveProfile.SelectedWebcam = SelectedWebcam;
                break;
        }
    }

    public async void ChangeProfile(Profile profile)
    {
        // change active profile to this one
        ActiveProfile = profile;

        // set the UI to reflect the active profile
        Name = ActiveProfile.Name;
        IsTouchlessArtsEnabled = ActiveProfile.IsTouchlessArtsEnabled;
        IsEraserEnabled = ActiveProfile.IsEraserEnabled;
        IsShapesEnabled = ActiveProfile.IsShapesEnabled;
        IsSelectionEnabled = ActiveProfile.IsSelectionEnabled;
        IsStickyNotesEnabled = ActiveProfile.IsStickyNotesEnabled;
        IsCameraEnabled = ActiveProfile.IsCameraEnabled;
        IsSearchEnabled = ActiveProfile.IsSearchEnabled;
        IsCopilotEnabled = ActiveProfile.IsCopilotEnabled;
        IsToolsEnabled = ActiveProfile.IsToolsEnabled;
        IsInAir3DMouseEnabled = ActiveProfile.IsInAir3DMouseEnabled;
        IsLeftHanded = ActiveProfile.IsLeftHanded;
        IsRightHanded = ActiveProfile.IsRightHanded;
        PinchSensitivity = ActiveProfile.PinchSensitivity;
        IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
        IsRulerEnabled = ActiveProfile.IsRulerEnabled;
        IsTimerEnabled = ActiveProfile.IsTimerEnabled;
        IsAlarmEnabled = ActiveProfile.IsAlarmEnabled;
        IsQuickFileAccessEnabled = ActiveProfile.IsQuickFileAccessEnabled;
        if (webcams.Contains(ActiveProfile.SelectedWebcam))
        {
            SelectedWebcam = ActiveProfile.SelectedWebcam;
        }
        ActiveProfile.IsSelected = true;
    }

    public void AddProfile()
    {
        // count the number of New Profile profiles
        int count = 1;
        foreach (Profile p in profiles)
        {
            if (p.Name.Contains("New Profile"))
            {
                count++;
            }
        }
        // create name by adding New Profile and the count
        string newName = "New Profile " + count;

        // create a new profile
        Profile newProfile = new Profile()
        {
            Name = newName,
            IsTouchlessArtsEnabled = false,
            IsEraserEnabled = false,
            IsShapesEnabled = false,
            IsSelectionEnabled = false,
            IsStickyNotesEnabled = false,
            IsCameraEnabled = false,
            IsSearchEnabled = false,
            IsCopilotEnabled = false,
            IsToolsEnabled = false,
            IsInAir3DMouseEnabled = false,
            IsLeftHanded = true,
            IsRightHanded = false,
            PinchSensitivity = 0.5,
            IsCalculatorEnabled = false,
            IsRulerEnabled = false,
            IsTimerEnabled = false,
            IsAlarmEnabled = false,
            IsQuickFileAccessEnabled = false
        };

        // add the new profile to the list of profiles
        Profiles.Add(newProfile);
        ActiveProfile = newProfile;
        ChangeProfile(newProfile);
    }

    public void DeleteProfile()
    {
        Profiles.Remove(ActiveProfile);
        if (Profiles.Count > 0)
        {
            ActiveProfile = Profiles.Last();
            ChangeProfile(ActiveProfile);
        }
    }

    public void SaveProfiles()
    {
        _profileService.SaveProfilesToJson("Resources/settings.json", profiles.ToList());
    }

    public void SetMotionInputConfig()
    {

    }

    public void Launch()
    {

        var mainWindowViewModel = new MainWindowViewModel();

        mainWindowViewModel.Name = Name;

        mainWindowViewModel.IsTouchlessArtsEnabled = IsTouchlessArtsEnabled;
        mainWindowViewModel.IsEraserEnabled = IsEraserEnabled;
        mainWindowViewModel.IsShapesEnabled = IsShapesEnabled;
        mainWindowViewModel.IsSelectionEnabled = IsSelectionEnabled;
        mainWindowViewModel.IsStickyNotesEnabled = IsStickyNotesEnabled;
        mainWindowViewModel.IsCameraEnabled = IsCameraEnabled;
        mainWindowViewModel.IsSearchEnabled = IsSearchEnabled;
        mainWindowViewModel.IsCopilotEnabled = IsCopilotEnabled;
        mainWindowViewModel.IsToolsEnabled = IsToolsEnabled;
        mainWindowViewModel.IsInAir3DMouseEnabled = IsInAir3DMouseEnabled;

        mainWindowViewModel.IsCalculatorEnabled = IsCalculatorEnabled;
        mainWindowViewModel.IsRulerEnabled = IsRulerEnabled;
        mainWindowViewModel.IsTimerEnabled = IsTimerEnabled;
        mainWindowViewModel.IsAlarmEnabled = IsAlarmEnabled;
        mainWindowViewModel.IsQuickFileAccessEnabled = IsQuickFileAccessEnabled;


        // overwrite json file with new profile
        //SaveProfiles();

        //// activate motioninput
        //// 1. kill motioninput
        //Process.Start("taskkill", "/F /IM MotionInput.exe");
        //// 2. start motioninput
        //SetMotionInputConfig();
        //// check MottionInput/motioninput.dist exist

        //// if not, copy from MotionInput/motioninput.dist

        //string FilePath = "motioninput.exe";
        //ProcessStartInfo startInfo = new ProcessStartInfo
        //{
        //    FileName = "@" + FilePath,
        //    UseShellExecute = true,
        //    Verb = "runas"
        //};

        //try
        //{
        //    Process.Start(startInfo);
        //}
        //catch (Exception ex)
        //{
        //    // Handle the error. For example, you can display it in a message box.
        //    Console.WriteLine(ex.Message);
        //}

        //// 4. Minimize this window

        //// Launch the main window
        var mainWindow = new MainWindow();
        mainWindow.Activate();
    }
}
