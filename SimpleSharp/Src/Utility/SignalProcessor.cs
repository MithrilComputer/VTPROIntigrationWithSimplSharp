using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Security.Cryptography;
using System.Security.Policy;
using VTProIntegrationsTestSimpleSharp.Src.Collections;
using VTProIntegrationsTestSimpleSharp.Src.Collections.SubPages;
using VTProIntigrationTestSimpleSharp.Src.Collections;
using VTProIntigrationTestSimpleSharp.Src.Utility;
using VTProIntigrationTestSimpleSharp.Src.Utility.Dispatchers;

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
                case eSigType.NA: // Unrecognized Change
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal: {args.Sig.Name}");
                    return;

                case eSigType.Bool: // Digital Change
                    if(DigitalDispatcher._buttonDispatch.TryGetValue
                        (args.Sig.Number, out Action<BasicTriList, SigEventArgs> action))
                    {
                        action(device, args);
                    }

                    break;

                case eSigType.UShort: // Analog Change
                    /*
                     
                    if (args.IsSignalSource(MainPage.AnalogJoins.RedSliderTouchFeedback))
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipRed, args.Sig.UShortValue);
                        CrestronConsole.PrintLine($"Color Red Input Changed: {args.Sig.UShortValue}");
                    }

                    if (args.IsSignalSource(MainPage.AnalogJoins.BlueSliderTouchFeedback))
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipBlue, args.Sig.UShortValue);
                        CrestronConsole.PrintLine($"Color Blue Changed: {args.Sig.UShortValue}");
                    }

                    if (args.IsSignalSource(MainPage.AnalogJoins.GreenSliderTouchFeedback))
                    {
                        SetAnalogJoin(device, MainPage.AnalogJoins.ColorChipGreen, args.Sig.UShortValue);
                        CrestronConsole.PrintLine($"Color Green Changed: {args.Sig.UShortValue}");
                    }
                    */
                    break;

                case eSigType.String: // Serial Change
                    /*
                    if (args.IsSignalSource(MainPage.SerialJoins.TextEntryOutput))
                    {
                        SetSerialJoin(device, MainPage.SerialJoins.FormattedTextInput, args.Sig.StringValue);
                        CrestronConsole.PrintLine($"String Input Changed: {args.Sig.StringValue}");
                    }
                    */
                    break;

                default:
                    CrestronConsole.PrintLine($"Cannot process signal change. Device: {device.Name}; Signal: {args.Sig.Name}");
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
            switch ((TVManagementSubPage.SmartJoins)args.SmartObjectArgs.ID)
            {
                // Smart DPad Joins
                case TVManagementSubPage.SmartJoins.SmartDPad: // Smart DPad Joins

                    // Parse the Smart DPad Signal
                    switch (ParseSmartSig<SmartObjectDefinitions.DPad>(args.Sig.Name))
                    {
                        case SmartObjectDefinitions.DPad.Up:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart DPad Up Pressed");
                            break;

                        case SmartObjectDefinitions.DPad.Down:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart DPad Down Pressed");
                            break;  

                        case SmartObjectDefinitions.DPad.Left:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart DPad Left Pressed");
                            break;

                        case SmartObjectDefinitions.DPad.Right:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
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
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad One Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Two:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Two Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Three:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Three Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Four:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Four Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Five:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Five Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Six:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Six Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Seven:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Seven Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Eight:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Eight Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Nine:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Nine Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Zero:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Zero Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Misc_1:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Misc_1 Pressed");
                            break;

                        case SmartObjectDefinitions.KeyPad.Misc_2:
                            if (IsSmartRisingEdge(args)) // Check for rising edge
                                CrestronConsole.PrintLine("Smart Keypad Misc_2 Pressed");
                            break;
                    }
                    break;
                
                default:
                    throw new ArgumentException($"Invalid Smart Object ID: {args.SmartObjectArgs.ID} for device: {device.Name}");
            }
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

        /// <summary>
        /// Sees if a Smart Object button is pressed on a rising edge.
        /// </summary>
        /// <param name="args">The signal received</param>
        /// <returns>Bool, If the signal was a rising edge</returns>
        private static bool IsSmartRisingEdge(SmartObjectEventArgs args)
        {
            return args.SmartObjectArgs.BooleanOutput[args.Sig.Name].BoolValue;
        }

        /// <summary>
        /// Checks if the signal source is a specific input enum.
        /// </summary>
        /// <param name="args">The signal source</param>
        /// <param name="inputEnum">The input enum</param>
        /// <returns>If the source is the enum</returns>
        public static bool IsSignalSource(this SigEventArgs args, Enum inputEnum)
        {
            return args.Sig.Number == Convert.ToUInt32(inputEnum);
        }
    }
}
