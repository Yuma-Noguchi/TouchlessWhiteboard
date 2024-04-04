using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Windows.Storage;

namespace TouchlessWhiteboard.Models;

public class Profile : ObservableObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("isTouchlessArtsEnabled")]
    public bool IsTouchlessArtsEnabled { get; set; }
    [JsonPropertyName("isStickyNotesEnabled")]
    public bool IsStickyNotesEnabled { get; set; }
    [JsonPropertyName("isCameraEnabled")]
    public bool IsCameraEnabled { get; set; }
    [JsonPropertyName("isSearchEnabled")]
    public bool IsSearchEnabled { get; set; }
    [JsonPropertyName("isCopilotEnabled")]
    public bool IsCopilotEnabled { get; set; }
    [JsonPropertyName("isCalculatorEnabled")]
    public bool IsCalculatorEnabled { get; set; }
    [JsonPropertyName("isClockEnabled")]
    public bool IsClockEnabled { get; set; }
    [JsonPropertyName("isQuickWebSiteAccess1Enabled")]
    public bool IsQuickWebSiteAccess1Enabled { get; set; }
    [JsonPropertyName("quickWebSiteAccess1URL")]
    public string QuickWebSiteAccess1URL { get; set; }
    [JsonPropertyName("isQuickWebSiteAccess2Enabled")]
    public bool IsQuickWebSiteAccess2Enabled { get; set; }
    [JsonPropertyName("quickWebSiteAccess2URL")]
    public string QuickWebSiteAccess2URL { get; set; }
    [JsonPropertyName("isQuickWebSiteAccess3Enabled")]
    public bool IsQuickWebSiteAccess3Enabled { get; set; }
    [JsonPropertyName("quickWebSiteAccess3URL")]
    public string QuickWebSiteAccess3URL { get; set; }
    [JsonPropertyName("isInAir3DMouseEnabled")]
    public bool IsInAir3DMouseEnabled { get; set; }
    [JsonPropertyName("isNotepadEnabled")]
    public bool IsNotepadEnabled { get; set; }
    [JsonPropertyName("isQuickFileAccess1Enabled")]
    public bool IsQuickFileAccess1Enabled { get; set; }
    [JsonPropertyName("quickFileAccess1Path")]
    public string QuickFileAccess1Path { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public StorageFile QuickFileAccess1File { get; set; }
    [JsonPropertyName("isQuickFileAccess2Enabled")]
    public bool IsQuickFileAccess2Enabled { get; set; }
    [JsonPropertyName("quickFileAccess2Path")]
    public string QuickFileAccess2Path { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public StorageFile QuickFileAccess2File { get; set; }
    [JsonPropertyName("isQuickFileAccess3Enabled")]
    public bool IsQuickFileAccess3Enabled { get; set; }
    [JsonPropertyName("quickFileAccess3Path")]
    public string QuickFileAccess3Path { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public StorageFile QuickFileAccess3File { get; set; }
    [JsonPropertyName("isLeftHanded")]
    public bool IsLeftHanded { get; set; }

    [JsonPropertyName("isRightHanded")]
    public bool IsRightHanded { get; set; }

    [JsonPropertyName("pinchSensitivity")]
    public double PinchSensitivity { get; set; }

    [JsonPropertyName("selectedWebcam")]
    public string SelectedWebcam { get; set; }
    [JsonPropertyName("teachingMaterialsPath")]
    public string TeachingMaterialsPath { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public StorageFile TeachingMaterials { get; set; }
    public bool IsSelected { get; set; }

    public Profile() {
        IsSelected = false;
    }
}

public class ProfileList
{
    [JsonPropertyName("profiles")]
    public List<Profile> Profiles { get; set; }
}

public class ProfileService
{
    public async Task<List<Profile>> LoadProfilesFromJson(string filePath)
    {
        //string FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, filePath);


        string FilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        // Read json from file
        using (var sr = new StreamReader(FilePath))
        {
            var json = sr.ReadToEnd();
            // Deserialize json into model
            var profileList = JsonConvert.DeserializeObject<ProfileList>(json);
            var profiles = profileList.Profiles;
            return profiles.ToList();
        }
    }

    public async Task SaveProfilesToJson(string filePath, List<Profile> profiles)
    {
        //string FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, filePath);
        string FilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        // Serialize model into json
        var profileList = new ProfileList { Profiles = profiles };
        var json = JsonConvert.SerializeObject(profileList);
        // Write json to file
        using (var sw = new StreamWriter(FilePath))
        {
            sw.Write(json);
        }
    }
}

