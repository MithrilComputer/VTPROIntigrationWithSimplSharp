using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTProIntegrationsTestSimpleSharp.Src.Collections;
using VTProIntigrationTestSimpleSharp.Src.Utility.Helpers;

namespace VTProIntigrationTestSimpleSharp.Src.Service
{
    internal class LogicController
    {
        /// <summary>
        /// Initializes the Touchpanel logic for the program.
        ///</summary>
        /// <param name="device">The Touchpanel logic to Initialize </param>
        public void InitializePanelLogic(BasicTriList device)
        {
            GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, false); // Enable Digital Join For Time Button
            GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, true); // Enable Digital Join For Weather Button
            GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, false); // Visibility Digital Join for The Weather Widget
            GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, true); // Visibility Digital Join for The Time Widget

            // Set the Analog Joins for the Color Chip
            GHelpers.SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipRed, 255);
            GHelpers.SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipGreen, 255);
            GHelpers.SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipBlue, 255);

            GHelpers.SetSerialJoin(device, MainPage.SerialJoins.FormattedTextInput, "Hello World!"); // Set the output serial to the input
        }
    }
}
