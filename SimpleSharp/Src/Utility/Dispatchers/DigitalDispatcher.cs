using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;
using System.Collections.Generic;
using VTProIntegrationsTestSimpleSharp.Src.Collections;

namespace VTProIntigrationTestSimpleSharp.Src.Utility.Dispatchers
{
    /// <summary>
    /// Dispatcher for digital signals in the VTProIntegrationTestSimpleSharp application.
    /// </summary>
    internal static class DigitalDispatcher
    {
        /// <summary>
        /// A dictionary that maps digital join IDs to their corresponding actions.
        /// </summary>
        public static Dictionary<uint , Action<BasicTriList, SigEventArgs>> _buttonDispatch = 
        new Dictionary<uint , Action<BasicTriList, SigEventArgs>>
        {
            {(uint)MainPage.DigitalJoins.TimeButtonPress, ButtonLogic.OnTimeButtonPress}

        };
    }
}
