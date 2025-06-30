using System;
using Crestron.SimplSharp;                          	// For Basic SIMPL# Classes
using Crestron.SimplSharpPro;                       	// For Basic SIMPL#Pro classes
using Crestron.SimplSharpPro.CrestronThread;        	// For Threading
using Crestron.SimplSharpPro.Diagnostics;		    	// For System Monitor Access
using Crestron.SimplSharpPro.DeviceSupport;         	// For Generic Device Support
using Crestron.SimplSharpPro.UI;

namespace VTProIntigrationTestSimpleSharp
{
    public class ControlSystem : CrestronControlSystem
    {
        XpanelForHtml5 xPanelOne;

        public ControlSystem() : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;

                xPanelOne = new XpanelForHtml5(0x03, this);
                xPanelOne.SigChange += XPanelOne_SigChange;

                xPanelOne.OnlineStatusChange += XPanelOne_OnlineStatusChange;

                if (xPanelOne.Register() != eDeviceRegistrationUnRegistrationResponse.Success)
                {
                    throw new ArgumentException($"Xpanel Failed to register correctly: {xPanelOne.ID}", nameof(xPanelOne));
                }

                //Subscribe to the controller events (Program, and Ethernet)
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(_ControllerProgramEventHandler);
                CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(_ControllerEthernetEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }


        /// <summary> 
        /// This method is called when the XPanel's Online Status changes.
        /// </summary>
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
        private void XPanelOne_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            if (currentDevice == xPanelOne)
            {
                SignalProcessor.ProcessSignalChange(currentDevice, args);
            }
               
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

        void _ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            switch (ethernetEventArgs.EthernetEventType)
            {//Determine the event type Link Up or Link Down
                case (eEthernetEventType.LinkDown):
                    //Next need to determine which adapter the event is for. 
                    //LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                        //
                    }
                    break;
                case (eEthernetEventType.LinkUp):
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {

                    }
                    break;
            }
        }

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