namespace VTProIntegrationsTestSimpleSharp.Src.Collections.SubPages
{
    /// <summary>
    /// a collection of logic join numbers for the sub page.
    /// </summary>
    internal static class TVManagementSubPage
    {

        /// <summary>
        /// The collection of digital joins used in the sub page.
        /// </summary>
        public enum DigitalJoins : uint
        {
            // Button Joins
            OnButtonPress = 13,
            OffButtonPress = 14,

            // Subpage Joins
            PageVisibility = 15,
        }

        /// <summary>
        /// The collection of Smart Graphic joins used in the sub page.
        /// </summary>
        public enum SmartJoins
        {
            // Smart Joins
            SmartDPad = 1,
            SmartKeypad = 2,
        }

    }
}
