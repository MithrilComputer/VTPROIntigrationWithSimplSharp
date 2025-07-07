using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using VTProIntegrationsTestSimpleSharp.Src.Collections;
using VTProIntegrationsTestSimpleSharp.Src.Collections.SubPages;
using VTProIntigrationTestSimpleSharp.Src.Collections;

namespace VTProIntegrationsTestSimpleSharp
{
    /// <summary>
    /// A class that processes signal changes from touch panels or other interactive devices.
    /// </summary>
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
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal:  {args.Sig.Name}");
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
                        CrestronConsole.PrintLine($"Color Red Input Changed: {args.Sig.UShortValue}");
                    }

                    if (args.Sig.Number == (uint)MainPage.AnalogJoins.BlueSliderTouchFeedback)
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipBlue, args.Sig.UShortValue); // Blue value for Color Chip
                        CrestronConsole.PrintLine($"Color Blue Changed: {args.Sig.UShortValue}");
                    }

                    if (args.Sig.Number == (uint)MainPage.AnalogJoins.GreenSliderTouchFeedback)
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipGreen, args.Sig.UShortValue); // Green value for Color Chip
                        CrestronConsole.PrintLine($"Color Green Changed: {args.Sig.UShortValue}");
                    }

                    break;

                case eSigType.String: // Serial Input

                    if (args.Sig.Number == (uint)MainPage.SerialJoins.TextEntryOutput)
                    {
                        SetSerialJoin(device, MainPage.SerialJoins.FormattedTextInput, args.Sig.StringValue); // Set the output serial to the input

                        CrestronConsole.PrintLine($"String Input Changed: {args.Sig.StringValue}");
                    }

                    break;

                default:
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal:  {args.Sig.Name}");
                    return;
            }
        }

        /// <summary>
        /// Processes Smart Graphics signal changes from touch panels or other interactive Crestron devices.
        /// </summary>
        /// <param name="device">The Crestron device (e.g., XPanel, touch panel) that generated the Smart Graphics signal event.</param>
        /// <param name="args">The <see cref="SmartObjectEventArgs"/> containing details about the Smart Graphics event, including the SmartObject ID and the signal name.</param>
        /// <remarks>
        /// This method handles different Smart Object IDs (e.g., SmartDPad, SmartKeypad) and parses the associated signal names
        /// into strongly typed enums to determine which control was activated, allowing clear and scalable Smart Graphics signal management.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when an unsupported or unknown Smart Object ID is received, indicating that the signal could not be processed.
        /// </exception>
        public static void ProcessSmartGraphicsChange(GenericBase device, SmartObjectEventArgs args)
        {
            // Check if the device and signal is null to avoid null reference exceptions.
            if (device == null || args == null)
            {
                CrestronConsole.PrintLine("Device or signal is null, cannot process signal change.");
                return;
            }

            // Check witch smart object was signaled
            switch((TVManagementSubPage.SmartJoins)args.SmartObjectArgs.ID)
            {
                // Smart DPad Joins
                case TVManagementSubPage.SmartJoins.SmartDPad: // Smart DPad Joins

                    // Parse the Smart DPad Signal
                    switch (ParseSmartSig<SmartObjectDefinitions.DPad>(args.Sig.Name))
                    {
                        case SmartObjectDefinitions.DPad.Up:
                            CrestronConsole.PrintLine("Smart DPad Up Pressed");
                            break;

                        case SmartObjectDefinitions.DPad.Down:
                            CrestronConsole.PrintLine("Smart DPad Down Pressed");
                            break;  

                        case SmartObjectDefinitions.DPad.Left:
                            CrestronConsole.PrintLine("Smart DPad Left Pressed");
                            break;

                        case SmartObjectDefinitions.DPad.Right:
                            CrestronConsole.PrintLine("Smart DPad Right Pressed");
                            break;
                    }
                    break;

                // Smart Keypad Joins
                case TVManagementSubPage.SmartJoins.SmartKeypad: // Smart Keypad Joins
                    
                    // Parse the Smart Keypad Signal
                    switch (ParseSmartSig<SmartObjectDefinitions.KeyPad>(args.Sig.Name))
                    {
                        case SmartObjectDefinitions.KeyPad.One:
                            CrestronConsole.PrintLine("Smart Keypad One Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Two:
                            CrestronConsole.PrintLine("Smart Keypad Two Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Three:
                            CrestronConsole.PrintLine("Smart Keypad Three Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Four:
                            CrestronConsole.PrintLine("Smart Keypad Four Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Five:
                            CrestronConsole.PrintLine("Smart Keypad Five Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Six:
                            CrestronConsole.PrintLine("Smart Keypad Six Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Seven:
                            CrestronConsole.PrintLine("Smart Keypad Seven Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Eight:
                            CrestronConsole.PrintLine("Smart Keypad Eight Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Nine:
                            CrestronConsole.PrintLine("Smart Keypad Nine Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Zero:
                            CrestronConsole.PrintLine("Smart Keypad Zero Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Misc_1:
                            CrestronConsole.PrintLine("Smart Keypad Misc_1 Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Misc_2:
                            CrestronConsole.PrintLine("Smart Keypad Misc_2 Pressed");
                            break;
                    }
                    break;
                
                default:
                    throw new ArgumentException($"Invalid Smart Object ID: {args.SmartObjectArgs.ID} for device: {device.Name}");
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


        /// <summary>
        /// Parses a string input to a Smart Object Signal type.
        /// </summary>
        /// <typeparam name="T">The Smart Object Enum Type</typeparam>
        /// <param name="input">Signal Input</param>
        /// <returns>The parsed enum value of type T corresponding to the input string.</returns>
        /// <exception cref="ArgumentException">Invalid input string</exception>
        private static T ParseSmartSig<T>(string input) where T : struct, Enum
        {
            // Check if the type is KeyPad, and handle it separately
            if (typeof(T) == typeof(SmartObjectDefinitions.KeyPad))
            {
                switch(input)
                {
                    case "1":
                        return (T)(object)SmartObjectDefinitions.KeyPad.One; // Cast to object to avoid type mismatch

                    case "2":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Two; // Cast to object to avoid type mismatch

                    case "3":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Three; // Cast to object to avoid type mismatch

                    case "4":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Four; // Cast to object to avoid type mismatch

                    case "5":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Five; // Cast to object to avoid type mismatch

                    case "6":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Six; // Cast to object to avoid type mismatch

                    case "7":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Seven; // Cast to object to avoid type mismatch

                    case "8":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Eight; // Cast to object to avoid type mismatch

                    case "9":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Nine; // Cast to object to avoid type mismatch

                    case "0":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Zero; // Cast to object to avoid type mismatch

                    case "Misc_1":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Misc_1; // Cast to object to avoid type mismatch

                    case "Misc_2":
                        return (T)(object)SmartObjectDefinitions.KeyPad.Misc_2; // Cast to object to avoid type mismatch

                    default:

                        throw new ArgumentException($"Invalid input string '{input}' for type {typeof(T).Name}");
                }
            }

            // For other types, use Enum.TryParse to parse the input string
            if (Enum.TryParse<T>(input, true, out T parsedSig))
            {
                return parsedSig;
            }

            throw new ArgumentException($"Invalid input string: {input}"); 
        }
    }
}
