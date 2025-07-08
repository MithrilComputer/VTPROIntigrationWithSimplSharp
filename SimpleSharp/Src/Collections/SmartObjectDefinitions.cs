namespace VTProIntigrationTestSimpleSharp.Src.Collections
{
    /// <summary>
    /// Smart Object Definitions for programing smart object signals
    /// </summary>
    internal static class SmartObjectDefinitions
    {
        /// <summary>
        /// The collection of buttons associated with the DPad smart object.
        /// </summary>
        public enum DPad : uint
        {
            // Button Joins
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4,
            Center = 5
        }

        /// <summary>
        /// The collection of buttons associated with the KeyPad smart object.
        /// </summary>
        public enum KeyPad : uint
        {
            // Button Joins
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Zero = 0,

            Misc_1 = 99999,
            Misc_2 = 99998
        }
    }
}
