using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

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
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace TouchlessWhiteboard.Models;

public class MotionInputService
{
    public async Task<bool> SetConfig(bool IsLeftHanded, bool IsRightHanded, double PinchSensitivity, int CameraIndex)
    {
        try
        {
            // Path to your JSON file
            string configFilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "MotionInput\\data\\config.json");

            // Read the JSON file
            string configJson = File.ReadAllText(configFilePath);

            // Parse the JSON file
            JObject configJsonObj = JObject.Parse(configJson);

            // Modify the specific key-value pairs
            configJsonObj["camera"]["source"] = CameraIndex;
            configJsonObj["hands"]["pinch_sensitivity"] = PinchSensitivity;

            // Write the modified JSON back to the file
            File.WriteAllText(configFilePath, configJsonObj.ToString());

            string modeFilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "MotionInput\\data\\modes\\inking.json");

            // Read the JSON file
            string modeJson = File.ReadAllText(modeFilePath);

            // Parse the JSON file
            JObject modeJsonObj = JObject.Parse(modeJson);

            // Modify hand key-value pair (note the value is flipped because the transcription is enabled)
            modeJsonObj["config"]["hand"] = IsLeftHanded ? "right" : "left";

            // Write the modified JSON back to the file
            File.WriteAllText(modeFilePath, modeJsonObj.ToString());
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

}