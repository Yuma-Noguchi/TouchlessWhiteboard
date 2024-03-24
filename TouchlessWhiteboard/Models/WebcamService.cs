using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace TouchlessWhiteboard.Models;

public class WebcamService
{
    public async Task<List<string>> GetAvailableWebcams()
    {
        try
        {
            string deviceSelector = DeviceInformation.GetAqsFilterFromDeviceClass(DeviceClass.VideoCapture);

            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(deviceSelector);

            if (devices.Count == 0)
            {
                throw new Exception("No webcams found.");
            }

            return devices.Select(device => device.Name).ToList();
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during device enumeration
            // This could be logging the error, showing a message to the user, etc.
            // For now, we'll just rethrow the exception
            throw;
        }
    }
}

