using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTProIntegrationsTestSimpleSharp.Src.Collections
{
    /// <summary>
    /// a collection of logic join numbers for the program.
    /// </summary>
    internal static class JoinSet
    {
        /// <summary>
        /// The collection of digital joins used in the program.
        /// </summary>
        public enum DigitalJoins : uint
        {
            // Button Joins
            TimeButtonPress = 20,
            TimeButtonEnable = 24,

            //Enable Joins
            WeatherButtonPress = 21,
            WeatherButtonEnable = 25,

            // Visibility Joins
            DateAndTimeWidgetVisibility = 22,
            WeatherWidgetVisibility = 23,
        }

        /// <summary>
        /// The collection of analog joins used in the program.
        /// </summary>
        public enum AnalogJoins : uint
        {
            // Feedback Joins
            RedSliderTouchFeedback = 30,
            GreenSliderTouchFeedback = 31,
            BlueSliderTouchFeedback = 32,

            // Color Chip Joins
            ColorChipRed = 33,
            ColorChipGreen = 34,
            ColorChipBlue = 35,
        }

        /// <summary>
        /// The collection of serial joins used in the program.
        /// </summary>
        public enum SerialJoins : uint
        {
            TextEntryOutput = 10,
            FormattedTextInput = 11,
        }

    }
}
