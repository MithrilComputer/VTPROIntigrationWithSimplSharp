using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTProIntigrationTestSimpleSharp
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
            // Check if the device is null to avoid null reference exceptions.
            if (device == null)
            {
                CrestronConsole.PrintLine("Device is null, cannot process signal change.", nameof(device));
                return;
            }

            // Check what type of signal has changed and process accordingly.
            switch (args.Sig.Type)
            {
                case eSigType.NA: // Unrecognized Input
                    CrestronConsole.PrintLine("Device is null, cannot process signal change.", nameof(device));
                    return;

                case eSigType.Bool: // Digital Input

                    if (args.Sig.Number == 20)
                    {
                        if (args.Sig.BoolValue == true)
                        {
                            CrestronConsole.PrintLine("Time Button Pressed");
                            device.BooleanInput[24].BoolValue = false; // Enable Digital Join For Time Button

                            device.BooleanInput[25].BoolValue = true; // Enable Digital Join For Weather Button

                            device.BooleanInput[23].BoolValue = false; // Visibility Digital Join for The Weather Widget

                            device.BooleanInput[22].BoolValue = true; // Visibility Digital Join for The Time Widget
                        }
                    }

                    if (args.Sig.Number == 21)
                    {
                        if (args.Sig.BoolValue == true)
                        {
                            CrestronConsole.PrintLine("Weather Button Pressed");
                            device.BooleanInput[24].BoolValue = true; // Enable Digital Join For Time Button

                            device.BooleanInput[25].BoolValue = false; // Enable Digital Join For Weather Button

                            device.BooleanInput[23].BoolValue = true; // Visibility Digital Join for The Weather Widget

                            device.BooleanInput[22].BoolValue = false; // Visibility Digital Join for The Time Widget
                        }
                    }

                    break;

                case eSigType.UShort: // Analog Input

                    if(args.Sig.Number == 30) // If analog join slider for blue value is changed
                    {
                        device.UShortInput[33].UShortValue = args.Sig.UShortValue; // Red value for Color Chip
                        CrestronConsole.PrintLine($"Color Red Input Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    if (args.Sig.Number == 31) // If analog join slider for blue value is changed
                    {
                        device.UShortInput[34].UShortValue = args.Sig.UShortValue; // Green value for Color Chip
                        CrestronConsole.PrintLine($"Color Green Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    if (args.Sig.Number == 32) // If analog join slider for blue value is changed
                    {
                        device.UShortInput[35].UShortValue = args.Sig.UShortValue; // Blue value for Color Chip
                        CrestronConsole.PrintLine($"Color Blue Changed: {args.Sig.UShortValue}", nameof(args));
                    }

                    break;

                case eSigType.String: // Serial Input

                    if (args.Sig.Number == 10) // If serial join for signal 10 has changed
                    {
                        device.StringInput[11].StringValue = args.Sig.StringValue; // Set the output serial to the input

                        CrestronConsole.PrintLine($"String Input Changed: {args.Sig.StringValue}", nameof(args));
                    }

                    break;  

                default:
                    CrestronConsole.PrintLine("Device is null, cannot process signal change.", nameof(device));
                    return;
            }
        }

        /// <summary>
        /// 
        ///</summary>
        /// <param name="device">The Touchpanel logic to Initialize </param>
        public static void Initialize(BasicTriList device)
        {
            device.BooleanInput[24].BoolValue = false; // Enable Digital Join For Time Button

            device.BooleanInput[25].BoolValue = true; // Enable Digital Join For Weather Button

            device.BooleanInput[23].BoolValue = false; // Visibility Digital Join for The Weather Widget

            device.BooleanInput[22].BoolValue = true; // Visibility Digital Join for The Time Widget

            device.UShortInput[33].UShortValue = 255;
            device.UShortInput[34].UShortValue = 255;
            device.UShortInput[35].UShortValue = 255;

            device.StringInput[11].StringValue = "Hello World!"; // Set the output serial to the input
        }

    }
}
