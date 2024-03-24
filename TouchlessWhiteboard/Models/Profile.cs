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

namespace TouchlessWhiteboard.Models;

public class Profile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("isTouchlessArtsEnabled")]
    public bool IsTouchlessArtsEnabled { get; set; }

    [JsonPropertyName("isEraserEnabled")]
    public bool IsEraserEnabled { get; set; }

    [JsonPropertyName("isShapesEnabled")]
    public bool IsShapesEnabled { get; set; }

    [JsonPropertyName("isSelectionEnabled")]
    public bool IsSelectionEnabled { get; set; }

    [JsonPropertyName("isStickyNotesEnabled")]
    public bool IsStickyNotesEnabled { get; set; }

    [JsonPropertyName("isCameraEnabled")]
    public bool IsCameraEnabled { get; set; }

    [JsonPropertyName("isSearchEnabled")]
    public bool IsSearchEnabled { get; set; }

    [JsonPropertyName("isCopilotEnabled")]
    public bool IsCopilotEnabled { get; set; }

    [JsonPropertyName("isToolsEnabled")]
    public bool IsToolsEnabled { get; set; }

    [JsonPropertyName("isInAir3DMouseEnabled")]
    public bool IsInAir3DMouseEnabled { get; set; }

    [JsonPropertyName("isLeftHanded")]
    public bool IsLeftHanded { get; set; }

    [JsonPropertyName("isRightHanded")]
    public bool IsRightHanded { get; set; }

    [JsonPropertyName("pinchSensitivity")]
    public double PinchSensitivity { get; set; }

    [JsonPropertyName("isCalculatorEnabled")]
    public bool IsCalculatorEnabled { get; set; }

    [JsonPropertyName("isRulerEnabled")]
    public bool IsRulerEnabled { get; set; }

    [JsonPropertyName("isTimerEnabled")]
    public bool IsTimerEnabled { get; set; }

    [JsonPropertyName("isAlarmEnabled")]
    public bool IsAlarmEnabled { get; set; }

    [JsonPropertyName("isQuickFileAccessEnabled")]
    public bool IsQuickFileAccessEnabled { get; set; }

    [JsonPropertyName("selectedWebcam")]
    public string SelectedWebcam { get; set; }
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
        string FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, filePath);
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
}

