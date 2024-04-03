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
using System.Net.Http.Json;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace TouchlessWhiteboard.Models;

public class MotionInputService
{

    private Process MotionInput = null;
    private string configFilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "MotionInput\\data\\config.json");
    public async Task<bool> SetConfig(bool IsLeftHanded, bool IsRightHanded, double PinchSensitivity, int CameraIndex)
    {
        try
        {
            // Read the JSON file
            string configJson = File.ReadAllText(configFilePath);

            // Parse the JSON file
            JObject configJsonObj = JObject.Parse(configJson);

            // Modify the specific key-value pairs
            configJsonObj["mode"] = "inking";
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

    public async Task<bool> Start(bool IsLeftHanded, bool IsRightHanded, double PinchSensitivity, int CameraIndex)
    {
        bool success = await SetConfig(IsLeftHanded, IsRightHanded, PinchSensitivity, CameraIndex);
        if (!success)
        {
            return false;
        }
        return await Launch();
    }

    public async Task<bool> ChangeMode(string mode)
    {
        try
        {
            // Read the JSON file
            string configJson = File.ReadAllText(configFilePath);

            // Parse the JSON file
            JObject configJsonObj = JObject.Parse(configJson);

            // Modify the specific key-value pairs
            configJsonObj["mode"] = mode;

            // Write the modified JSON back to the file
            File.WriteAllText(configFilePath, configJsonObj.ToString());

            return await Launch();
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> Launch()
    {
        try
        {
            // Path to the executable
            string FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "MotionInput\\MotionInput.exe");

            if (MotionInput != null)
            {
                MotionInput.Kill();
            }
            MotionInput = new();
            MotionInput.StartInfo.UseShellExecute = true;
            MotionInput.StartInfo.FileName = FilePath;
            MotionInput.StartInfo.WorkingDirectory = Path.GetDirectoryName(FilePath);
            MotionInput.Start();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
}