using System;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    	// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using Crestron.SimplSharpPro.UI;

namespace VTProIntegrationsTestSimpleSharp
{
    public class ControlSystem : CrestronControlSystem
    {

        /// <summary>
        /// Declare the XPanel for Smart Graphics device, Can be any touchpanel but ive chosen XpanelForSmartGraphics for this example.
        /// </summary>
        XpanelForSmartGraphics xPanelOne;

        /// <summary>
        /// The entry point for the simple sharp application.
        /// </summary>
        public ControlSystem() : base()
        {
            try
            {

                // Set the thread pool size for user threads.
                Thread.MaxNumberOfUserThreads = 20;

                // Initialize the XPanel.
                xPanelOne = new XpanelForSmartGraphics(0x03, this);

                // Subscribe to the events for the XPanel.
                xPanelOne.SigChange += XPanelOne_SigChange;
                xPanelOne.OnlineStatusChange += XPanelOne_OnlineStatusChange;

                // Ensure the XPanel is registered with the system and ready to use.
                if (xPanelOne.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                {
                    throw new ArgumentException($"XPanel Failed to register correctly: {xPanelOne.ID}", nameof(xPanelOne));
                }

                // Subscribe to the program status event handler.
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(_ControllerProgramEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }


        /// <summary> 
        /// This method is called when the XPanel's Online Status changes.
        /// </summary>
        /// <param name="currentDevice">The Device that changed its online status.</param>
        /// <param name="args">The online status.</param>
        private void XPanelOne_OnlineStatusChange(GenericBase currentDevice, OnlineOfflineEventArgs args)
        {
            string onlineStatus;

            if (args.DeviceOnLine)
            {
                onlineStatus = "Online";
            }
            else
            {
                onlineStatus = "Offline";
            }

            CrestronConsole.PrintLine($"Device {currentDevice.Name}:{currentDevice.ID} | Status: {onlineStatus}", nameof(currentDevice), nameof(onlineStatus));
        }

        /// <summary>
        /// This method is called when a signal changes on the XPanel.
        /// </summary>
        /// <param name="currentDevice">The Device that had a signal change.</param>
        /// <param name="args">The signal that changed on the XPanel.</param>
        private void XPanelOne_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            SignalProcessor.ProcessSignalChange(currentDevice, args); 
        }

        public override void InitializeSystem()
        {
            try
            {
                SignalProcessor.Initialize(xPanelOne);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }

        /// <summary>
        /// Handles program status events from the controller.
        /// </summary>
        /// <param name="programStatusEventType">The event that changed eg. Paused, Resumed, and Stopping</param>
        void _ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                    //The program has been stopped.
                    //Close all threads. 
                    //Shutdown all Client/Servers in the system.
                    //General cleanup.
                    //Unsubscribe to all System Monitor events
                    break;
            }

        }

    }
}