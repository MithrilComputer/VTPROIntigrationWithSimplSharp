using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTProIntegrationsTestSimpleSharp.Src.Collections;

namespace VTProIntigrationTestSimpleSharp.Src.Utility.Helpers
{
    /// <summary>
    /// Generic Helpers for the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    internal static class GHelpers
    {
        /// <summary>
        /// Sets a digital join on the device.
        /// </summary>
        /// <param name="device"> The device to set the digital join on.</param>
        /// <param name="digitalJoin"> The digital join to set.</param>
        /// <param name="value"> The value to set the digital join to.</param>
        public static void SetDigitalJoin(BasicTriList device, MainPage.DigitalJoins digitalJoin, bool value)
        {
            device.BooleanInput[(uint)digitalJoin].BoolValue = value;
        }

        /// <summary>
        /// Sets a analog join on the device.
        /// </summary>
        /// <param name="device"> The device to set the analogJoin join on.</param>
        /// <param name="analogJoin"> The analogJoin join to set.</param>
        /// <param name="value"> The value to set the analogJoin join to.</param>
        public static void SetAnalogJoin(BasicTriList device, MainPage.AnalogJoins analogJoin, ushort value)
        {
            device.UShortInput[(uint)analogJoin].UShortValue = value;
        }

        /// <summary>
        /// Sets a serial join on the device.
        /// </summary>
        /// <param name="device"> The device to set the serial join on.</param>
        /// <param name="serialJoin"> The serial join to set.</param>
        /// <param name="value"> The value to set the serial join to.</param>
        public static void SetSerialJoin(BasicTriList device, MainPage.SerialJoins serialJoin, string value)
        {
            device.StringInput[(uint)serialJoin].StringValue = value;
        }
    }
}
