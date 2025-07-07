﻿using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTProIntegrationsTestSimpleSharp.Src.Collections;

namespace VTProIntegrationsTestSimpleSharp
{
    internal static class SignalProcessor
    {
        /// <summary>
        /// Processes any signal changes that occur from touchPanels or other interactive devices.
        /// </summary>
        /// <param name="device">The Device that called the signal change.</param>
        /// <param name="args">The signal that changed.</param> 
        public static void ProcessSignalChange(BasicTriList device, SigEventArgs args)
        {
            // Check if the device and signal is null to avoid null reference exceptions.
            if (device == null || args == null)
            {
                CrestronConsole.PrintLine("Device or signal is null, cannot process signal change.");
                return;
            }

            // Check what type of signal has changed and process accordingly.
            switch (args.Sig.Type)
            {
                case eSigType.NA: // Unrecognized Input
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal:  {args.Sig.Name}", device?.Name ?? "Unknown", args);
                    return;

                case eSigType.Bool: // Digital Input

                    if (args.Sig.Number == (uint)MainPage.DigitalJoins.TimeButtonPress)
                    {
                        if (args.Sig.BoolValue == true)
                        {
                            CrestronConsole.PrintLine("Time Button Pressed");

                            SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, false); // Enable Digital Join For Time Button
                            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, true); // Enable Digital Join For Weather Button
                            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, false); // Visibility Digital Join for The Weather Widget
                            SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, true); // Visibility Digital Join for The Time Widget
                        }
                    }

                    if (args.Sig.Number == (uint)MainPage.DigitalJoins.WeatherButtonPress)
                    {
                        if (args.Sig.BoolValue == true)
                        {
                            CrestronConsole.PrintLine("Weather Button Pressed");

                            SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, true); // Enable Digital Join For Time Button
                            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, false); // Enable Digital Join For Weather Button
                            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, true); // Visibility Digital Join for The Weather Widget
                            SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, false); // Visibility Digital Join for The Time Widget
                        }
                    }

                    break;

                case eSigType.UShort: // Analog Input

                    if (args.Sig.Number == (uint)MainPage.AnalogJoins.RedSliderTouchFeedback)
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipRed, args.Sig.UShortValue); // Red value for Color Chip
                        CrestronConsole.PrintLine($"Color Red Input Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    if (args.Sig.Number == (uint)MainPage.AnalogJoins.BlueSliderTouchFeedback)
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipBlue, args.Sig.UShortValue); // Blue value for Color Chip
                        CrestronConsole.PrintLine($"Color Blue Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    if (args.Sig.Number == (uint)MainPage.AnalogJoins.GreenSliderTouchFeedback)
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipGreen, args.Sig.UShortValue); // Green value for Color Chip
                        CrestronConsole.PrintLine($"Color Green Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    break;

                case eSigType.String: // Serial Input

                    if (args.Sig.Number == (uint)MainPage.SerialJoins.TextEntryOutput)
                    {
                        SetSerialJoin(device, MainPage.SerialJoins.FormattedTextInput, args.Sig.StringValue); // Set the output serial to the input

                        CrestronConsole.PrintLine($"String Input Changed: {args.Sig.StringValue}", args);
                    }

                    break;

                default:
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal:  {args.Sig.Name}", device?.Name ?? "Unknown", args);
                    return;
            }
        }

        /// <summary>
        /// Initializes the Touchpanel logic for the program.
        ///</summary>
        /// <param name="device">The Touchpanel logic to Initialize </param>
        public static void Initialize(BasicTriList device)
        {
            SetDigitalJoin(device, MainPage.DigitalJoins.TimeButtonEnable, false); // Enable Digital Join For Time Button
            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherButtonEnable, true); // Enable Digital Join For Weather Button
            SetDigitalJoin(device, MainPage.DigitalJoins.WeatherWidgetVisibility, false); // Visibility Digital Join for The Weather Widget
            SetDigitalJoin(device, MainPage.DigitalJoins.DateAndTimeWidgetVisibility, true); // Visibility Digital Join for The Time Widget

            SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipRed, 255);
            SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipGreen, 255);
            SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipBlue, 255);

            SetSerialJoin(device, MainPage.SerialJoins.FormattedTextInput, "Hello World!"); // Set the output serial to the input
        }

        /// <summary>
        /// Sets a digital join on the device.
        /// </summary>
        /// <param name="device"> The device to set the digital join on.</param>
        /// <param name="digitalJoin"> The digital join to set.</param>
        /// <param name="value"> The value to set the digital join to.</param>
        private static void SetDigitalJoin(BasicTriList device, MainPage.DigitalJoins digitalJoin, bool value)
        {
            device.BooleanInput[(uint)digitalJoin].BoolValue = value;
        }

        /// <summary>
        /// Sets a analog join on the device.
        /// </summary>
        /// <param name="device"> The device to set the analogJoin join on.</param>
        /// <param name="analogJoin"> The analogJoin join to set.</param>
        /// <param name="value"> The value to set the analogJoin join to.</param>
        private static void SetAnalogJoin(BasicTriList device, MainPage.AnalogJoins analogJoin, ushort value)
        {
            device.UShortInput[(uint)analogJoin].UShortValue = value;
        }

        /// <summary>
        /// Sets a serial join on the device.
        /// </summary>
        /// <param name="device"> The device to set the serial join on.</param>
        /// <param name="serialJoin"> The serial join to set.</param>
        /// <param name="value"> The value to set the serial join to.</param>
        private static void SetSerialJoin(BasicTriList device, MainPage.SerialJoins serialJoin, string value)
        {
            device.StringInput[(uint)serialJoin].StringValue = value;
        }
    }
}
