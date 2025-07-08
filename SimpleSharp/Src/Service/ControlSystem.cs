using System;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using Crestron.SimplSharpPro.UI;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using VTProIntigrationTestSimpleSharp.Src.Service;

namespace VTProIntegrationsTestSimpleSharp
{
    public class ControlSystem : CrestronControlSystem
    {
        /// <summary>
        /// The main XPanel, XpanelForSmartGraphics for this example.
        /// </summary>
        XpanelForSmartGraphics xPanelOne;

        /// <summary>
        /// The LogicController, which handles the logic for the touchpanel.
        /// </summary>
        LogicController logicController;

        ///<summary> Ignore for now, 
        ///Location of the SDG file for the XPanel.
        ///</summary>
        string SDGFile = Path.Combine(Directory.GetCurrentDirectory(), "XpanelIntigration.sgd");

        /// <summary>
        /// The entry point for the simple sharp application.
        /// </summary>
        public ControlSystem() : base()
        {
            try
            {
                // Set the thread pool size for user threads.
                Thread.MaxNumberOfUserThreads = 20;

                // Initialize the XPanel for Smart Graphics with an IPID of 0x03.
                xPanelOne = new XpanelForSmartGraphics(0x03, this);

                // Register the XPanel and smart Graphics.
                SetupSmartPanel(xPanelOne, SDGFile);

                logicController = new LogicController();

                // Subscribe to the program status event handler.
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(_ControllerProgramEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }

        public override void InitializeSystem()
        {
            try
            {
                logicController.InitializePanelLogic(xPanelOne);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }

        /// <summary>
        /// Configures a Smart Graphics panel by registering it, subscribing to events, and loading its SGD file.
        /// </summary>
        /// <param name="SGDFile">The Smart Graphics definition file path.</param>
        /// <param name="IPID">The IPID to assign to the XPanel.</param>
        /// <returns>The initialized and registered XpanelForSmartGraphics object.</returns>
        private void SetupSmartPanel(BasicTriListWithSmartObject panel, string SGDFile)
        {
            // Subscribe to the events for the XPanel.
            panel.SigChange += XPanel_SigChange;
            panel.OnlineStatusChange += XPanel_OnlineStatusChange;

            // Ensure the XPanel is registered with the system and ready to use.
            if (panel.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
            {
                throw new ArgumentException($"XPanel Failed to register correctly: {panel.ID}");
            }

            // Set the XPanel to use the SDG file.
            // Check if the SDG file exists before loading it.
            if (!File.Exists(SGDFile))
            {
                throw new FileNotFoundException($"SDG file not found: {SGDFile};");
            }
            else if (panel.LoadSmartObjects(SGDFile) <= 0)
            {
                throw new WarningException($"Failed to load smart object -> File: {SGDFile}; Device: {panel.Name};");
            }

            // subscribe to the SmartObject signal changes.
            foreach (KeyValuePair<uint, SmartObject> pair in panel.SmartObjects)
            {
                pair.Value.SigChange += Xpanel_SmartGraphicsSigChange;
            }
        }

        /// <summary>
        /// This method is called when the XPanel's online status changes.
        /// </summary>
        /// <param name="currentDevice"></param>
        /// <param name="args"></param>
        private void XPanel_OnlineStatusChange(GenericBase currentDevice, OnlineOfflineEventArgs args)
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

            CrestronConsole.PrintLine($"Device {currentDevice.Name}:{currentDevice.ID} | Status: {onlineStatus}");
        }

        /// <summary>
        /// This method is called when a signal changes on the XPanel.
        /// </summary>
        /// <param name="currentDevice">The Device that had a signal change.</param>
        /// <param name="args">The signal that changed on the XPanel.</param>
        private void XPanel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            SignalProcessor.ProcessSignalChange(currentDevice, args); 
        }

        /// <summary>
        /// This method is called when a signal changes specifically with Smart Graphics on the XPanel.
        /// </summary>
        /// <param name="currentDevice">The Device that had a signal change.</param>
        /// <param name="args">The signal that changed on the XPanel's smart objects.</param>
        private void Xpanel_SmartGraphicsSigChange(GenericBase currentDevice, SmartObjectEventArgs args)
        {
            SignalProcessor.ProcessSmartGraphicsChange(currentDevice, args);
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
                    // Not used in this example, but you can use it to pause threads or timers.
                    break;
                case (eProgramStatusEventType.Resumed):
                    // Not used in this example, but you can use it to resume any paused threads or timers.
                    break;
                case (eProgramStatusEventType.Stopping):
                    
                    //The program has been stopped.
                    CrestronConsole.PrintLine("Program stopping, cleaning up...");

                    //Close all threads, Not used in this example.

                    //Shutdown all Client/Servers in the system. Not used in this example.

                    //General cleanup.
          
                    // If the XPanel is not null and registered, unregister and dispose of it.
                    CleanUp();

                    //Unsubscribe to all System Monitor events
                    CrestronEnvironment.ProgramStatusEventHandler -= _ControllerProgramEventHandler;

                    CrestronConsole.PrintLine("Cleanup finished, exiting.");
                    break;
            }
        }

        /// <summary>
        /// Master cleanup method to unregister, dispose and clear used memory.
        /// </summary>
        private void CleanUp()
        {
            // Clean up the XPanel if it exists and is registered.
            if (xPanelOne != null && xPanelOne.Registered)
            {
                // Clear the logic controller.
                logicController = null;

                // Unsubscribe to all XPanel events.
                xPanelOne.SigChange -= XPanel_SigChange;
                xPanelOne.OnlineStatusChange -= XPanel_OnlineStatusChange;
                CrestronConsole.PrintLine("Unsubscribed from XPanelOne events");

                //Unregister the XPanel from the system.
                xPanelOne.UnRegister();
                CrestronConsole.PrintLine("Unregistered XPanelOne");

                // Dispose of the XPanel object.
                xPanelOne.Dispose();
                xPanelOne = null;
                CrestronConsole.PrintLine("Disposed XPanelOne");
            }
        }
    }
}