namespace VTProIntegrationsTestSimpleSharp.Src.Collections.SubPages
{
    /// <summary>
    /// a collection of logic join numbers for the sub page.
    /// </summary>
    internal static class CurtainManagementSubPage
    {
        /// <summary>
        /// The collection of digital joins used in the sub page.
        /// </summary>
        public enum DigitalJoins : uint
        {
            // Button Joins
            OpenButtonPress = 9,
            CloseButtonPress = 11,

            // Enable Joins
            OpenButtonEnable = 10,
            CloseButtonEnable = 12,

            // Subpage Joins
            PageVisibility = 16
        }

    }
}
