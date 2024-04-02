﻿using System;
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
using Windows.Graphics.DirectX.Direct3D11;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using static System.Net.WebRequestMethods;

namespace TouchlessWhiteboard.ViewModel;

public partial class SettingsWindowViewModel : ObservableObject, INotifyPropertyChanged
{
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private bool isTouchlessArtsEnabled;
    [ObservableProperty]
    private bool isStickyNotesEnabled;
    [ObservableProperty]
    private bool isCameraEnabled;
    [ObservableProperty]
    private bool isSearchEnabled;
    [ObservableProperty]
    private bool isCopilotEnabled;
    [ObservableProperty]
    private bool isCalculatorEnabled;
    [ObservableProperty]
    private bool isClockEnabled;
    [ObservableProperty]
    private bool isQuickWebSiteAccess1Enabled;
    [ObservableProperty]
    private string quickWebSiteAccess1URL;
    [ObservableProperty]
    private bool isQuickWebSiteAccess2Enabled;
    [ObservableProperty]
    private string quickWebSiteAccess2URL;
    [ObservableProperty]
    private bool isQuickWebSiteAccess3Enabled;
    [ObservableProperty]
    private string quickWebSiteAccess3URL;
    [ObservableProperty]
    private bool isInAir3DMouseEnabled;
    [ObservableProperty]
    private bool isNotepadEnabled;
    [ObservableProperty]
    private bool isQuickFileAccess1Enabled;
    [ObservableProperty]
    private StorageFile quickFileAccess1File;
    [ObservableProperty]
    private bool isQuickFileAccess2Enabled;
    [ObservableProperty]
    private StorageFile quickFileAccess2File;
    [ObservableProperty]
    private bool isQuickFileAccess3Enabled;
    [ObservableProperty]
    private StorageFile quickFileAccess3File;

    [ObservableProperty]
    private bool isLeftHanded;
    [ObservableProperty]
    private bool isRightHanded;

    [ObservableProperty]
    private double pinchSensitivity;

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
        PropertyChanged += (s, e) => UpdateProfile(s, e);
    }

    private async void InitializeAsync()
    {
        await GetFilesandWebcamsAsync();
        if (profiles != null && profiles.Any())
        {
            ActiveProfile = profiles.First();

            // set all the properties to the values of the active profile
            Name = ActiveProfile.Name;
            IsTouchlessArtsEnabled = ActiveProfile.IsTouchlessArtsEnabled;
            IsStickyNotesEnabled = ActiveProfile.IsStickyNotesEnabled;
            IsCameraEnabled = ActiveProfile.IsCameraEnabled;
            IsSearchEnabled = ActiveProfile.IsSearchEnabled;
            IsCopilotEnabled = ActiveProfile.IsCopilotEnabled;
            IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
            IsClockEnabled = ActiveProfile.IsClockEnabled;
            IsQuickWebSiteAccess1Enabled = ActiveProfile.IsQuickWebSiteAccess1Enabled;
            QuickWebSiteAccess1URL = ActiveProfile.QuickWebSiteAccess1URL;
            IsQuickWebSiteAccess2Enabled = ActiveProfile.IsQuickWebSiteAccess2Enabled;
            QuickWebSiteAccess2URL = ActiveProfile.QuickWebSiteAccess2URL;
            IsQuickWebSiteAccess3Enabled = ActiveProfile.IsQuickWebSiteAccess3Enabled;
            QuickWebSiteAccess3URL = ActiveProfile.QuickWebSiteAccess3URL;
            IsInAir3DMouseEnabled = ActiveProfile.IsInAir3DMouseEnabled;
            IsNotepadEnabled = ActiveProfile.IsNotepadEnabled;
            IsQuickFileAccess1Enabled = ActiveProfile.IsQuickFileAccess1Enabled;
            QuickFileAccess1File = ActiveProfile.QuickFileAccess1File;
            IsQuickFileAccess2Enabled = ActiveProfile.IsQuickFileAccess2Enabled;
            QuickFileAccess2File = ActiveProfile.QuickFileAccess2File;
            IsQuickFileAccess3Enabled = ActiveProfile.IsQuickFileAccess3Enabled;
            QuickFileAccess3File = ActiveProfile.QuickFileAccess3File;
            IsLeftHanded = ActiveProfile.IsLeftHanded;
            IsRightHanded = ActiveProfile.IsRightHanded;
            PinchSensitivity = ActiveProfile.PinchSensitivity;
            IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;

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
            // convert file path to storage file
            ActiveProfile.IsSelected = true;
            IsSelected = true;
        }
    }

    private async Task GetFilesandWebcamsAsync()
    {
        Profiles = new ObservableCollection<Profile>(await _profileService.LoadProfilesFromJson("Resources/settings.json"));

        webcams = await _webcamService.GetAvailableWebcams();
        if (webcams.Count > 0)
        {
            SelectedWebcam = webcams[0];
        }

        // for all profiles, convert file paths to storage files
        foreach (Profile p in Profiles)
        {
            if (p.QuickFileAccess1Path != null)
            {
                try
                {
                    p.QuickFileAccess1File = await StorageFile.GetFileFromPathAsync(p.QuickFileAccess1Path);
                }
                catch (Exception ex)

                {
                    p.QuickFileAccess1Path = null;
                }
            }

            if (p.QuickFileAccess2Path != null)
            {
                try
                {
                    p.QuickFileAccess2File = await StorageFile.GetFileFromPathAsync(p.QuickFileAccess2Path);
                }
                catch (Exception ex)
                {
                    p.QuickFileAccess2Path = null;
                }
            }

            if (p.QuickFileAccess3Path != null)
            {
                try
                {
                    p.QuickFileAccess3File = await StorageFile.GetFileFromPathAsync(p.QuickFileAccess3Path);
                }
                catch (Exception ex)
                {
                    p.QuickFileAccess3Path = null;
                }
            }
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
            case nameof(IsCalculatorEnabled):
                ActiveProfile.IsCalculatorEnabled = IsCalculatorEnabled;
                break;
            case nameof(IsClockEnabled):
                ActiveProfile.IsClockEnabled = IsClockEnabled;
                break;
            case nameof(IsQuickWebSiteAccess1Enabled):
                ActiveProfile.IsQuickWebSiteAccess1Enabled = IsQuickWebSiteAccess1Enabled;
                break;
            case nameof(QuickWebSiteAccess1URL):
                ActiveProfile.QuickWebSiteAccess1URL = QuickWebSiteAccess1URL;
                break;
            case nameof(IsQuickWebSiteAccess2Enabled):
                ActiveProfile.IsQuickWebSiteAccess2Enabled = IsQuickWebSiteAccess2Enabled;
                break;
            case nameof(QuickWebSiteAccess2URL):
                ActiveProfile.QuickWebSiteAccess2URL = QuickWebSiteAccess2URL;
                break;
            case nameof(IsQuickWebSiteAccess3Enabled):
                ActiveProfile.IsQuickWebSiteAccess3Enabled = IsQuickWebSiteAccess3Enabled;
                break;
            case nameof(QuickWebSiteAccess3URL):
                ActiveProfile.QuickWebSiteAccess3URL = QuickWebSiteAccess3URL;
                break;
            case nameof(IsInAir3DMouseEnabled):
                ActiveProfile.IsInAir3DMouseEnabled = IsInAir3DMouseEnabled;
                break;
            case nameof(IsNotepadEnabled):
                ActiveProfile.IsNotepadEnabled = IsNotepadEnabled;
                break;
            case nameof(IsQuickFileAccess1Enabled):
                ActiveProfile.IsQuickFileAccess1Enabled = IsQuickFileAccess1Enabled;
                break;
            case nameof(QuickFileAccess1File):
                ActiveProfile.QuickFileAccess1File = QuickFileAccess1File;
                break;
            case nameof(IsQuickFileAccess2Enabled):
                ActiveProfile.IsQuickFileAccess2Enabled = IsQuickFileAccess2Enabled;
                break;
            case nameof(QuickFileAccess2File):
                ActiveProfile.QuickFileAccess2File = QuickFileAccess2File;
                break;
            case nameof(IsQuickFileAccess3Enabled):
                ActiveProfile.IsQuickFileAccess3Enabled = IsQuickFileAccess3Enabled;
                break;
            case nameof(QuickFileAccess3File):
                ActiveProfile.QuickFileAccess3File = QuickFileAccess3File;
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
        IsStickyNotesEnabled = ActiveProfile.IsStickyNotesEnabled;
        IsCameraEnabled = ActiveProfile.IsCameraEnabled;
        IsSearchEnabled = ActiveProfile.IsSearchEnabled;
        IsCopilotEnabled = ActiveProfile.IsCopilotEnabled;
        IsCalculatorEnabled = ActiveProfile.IsCalculatorEnabled;
        IsClockEnabled = ActiveProfile.IsClockEnabled;
        IsQuickWebSiteAccess1Enabled = ActiveProfile.IsQuickWebSiteAccess1Enabled;
        QuickWebSiteAccess1URL = ActiveProfile.QuickWebSiteAccess1URL;
        IsQuickWebSiteAccess2Enabled = ActiveProfile.IsQuickWebSiteAccess2Enabled;
        QuickWebSiteAccess2URL = ActiveProfile.QuickWebSiteAccess2URL;
        IsQuickWebSiteAccess3Enabled = ActiveProfile.IsQuickWebSiteAccess3Enabled;
        QuickWebSiteAccess3URL = ActiveProfile.QuickWebSiteAccess3URL;
        IsInAir3DMouseEnabled = ActiveProfile.IsInAir3DMouseEnabled;
        IsNotepadEnabled = ActiveProfile.IsNotepadEnabled;
        IsQuickFileAccess1Enabled = ActiveProfile.IsQuickFileAccess1Enabled;
        QuickFileAccess1File = ActiveProfile.QuickFileAccess1File;
        IsQuickFileAccess2Enabled = ActiveProfile.IsQuickFileAccess2Enabled;
        QuickFileAccess2File = ActiveProfile.QuickFileAccess2File;
        IsQuickFileAccess3Enabled = ActiveProfile.IsQuickFileAccess3Enabled;
        QuickFileAccess3File = ActiveProfile.QuickFileAccess3File;
        IsLeftHanded = ActiveProfile.IsLeftHanded;
        IsRightHanded = ActiveProfile.IsRightHanded;
        PinchSensitivity = ActiveProfile.PinchSensitivity;
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
            IsStickyNotesEnabled = false,
            IsCameraEnabled = false,
            IsSearchEnabled = false,
            IsCopilotEnabled = false,
            IsCalculatorEnabled = false,
            IsClockEnabled = false,
            IsQuickWebSiteAccess1Enabled = false,
            QuickWebSiteAccess1URL = "",
            IsQuickWebSiteAccess2Enabled = false,
            QuickWebSiteAccess2URL = "",
            IsQuickWebSiteAccess3Enabled = false,
            QuickWebSiteAccess3URL = "",
            IsInAir3DMouseEnabled = false,
            IsNotepadEnabled = false,
            IsQuickFileAccess1Enabled = false,
            QuickFileAccess1File = null,
            IsQuickFileAccess2Enabled = false,
            QuickFileAccess2File = null,
            IsQuickFileAccess3Enabled = false,
            QuickFileAccess3File = null,
            IsLeftHanded = true,
            IsRightHanded = false,
            PinchSensitivity = 0.5
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

    public async void Launch()
    {
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
        //StorageFile storageFile = await StorageFile.GetFileFromPathAsync("C:\\Users\\student\\Downloads\\MI3.4_Education_Executable_Final\\motioninput.exe");
        //Process P = new();
        //P.StartInfo.UseShellExecute = true;
        //P.StartInfo.Verb = "runas";
        //P.StartInfo.FileName = storageFile.Path;
        //P.StartInfo.Arguments = "";
        //P.Start();
    }


}
