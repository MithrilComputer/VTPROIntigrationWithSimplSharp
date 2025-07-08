using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTProIntegrationsTestSimpleSharp.Src.Collections;
using VTProIntigrationTestSimpleSharp.Src.Utility.Helpers;

namespace VTProIntigrationTestSimpleSharp.Src.Utility
{
    /// <summary>
    /// A store of all the actions that can be performed by buttons in the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    internal static class ButtonLogic
    {

        /// <summary>
        /// Action to perform when the time button is pressed.
        /// </summary>
        /// <param name="device">Device that signaled the event</param>
        /// <param name="args">The signal that changed</param>
        public static void OnTimeButtonPress(BasicTriList device, SigEventArgs args)
        {
            if (IsRisingEdge(args))
            {
                CrestronConsole.PrintLine("Time Button Pressed");

                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, false);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, true);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, false);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, true);
            }
        }

        /// <summary>
        /// Action to perform when the weather button is pressed.
        /// </summary>
        /// <param name="device">Device that signaled the event</param>
        /// <param name="args">The signal that changed</param>
        public static void OnWeatherButtonPress(BasicTriList device, SigEventArgs args)
        {
            if (IsRisingEdge(args))
            {
                CrestronConsole.PrintLine("Weather Button Pressed");

                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, true);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, false);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, true);
                GHelpers.SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, false);
            }
        }

        /// <summary>
        /// Sees if a button is pressed on a rising edge. (Not Smart Object)
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        private static bool IsRisingEdge(SigEventArgs args)
        {
            return args.Sig.BoolValue;

        }
    }
}
