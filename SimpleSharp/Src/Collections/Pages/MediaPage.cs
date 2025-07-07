using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTProIntegrationsTestSimpleSharp.Src.Collections.Pages
{
    /// <summary>
    /// a collection of logic join numbers for the page.
    /// </summary>
    internal static class MediaPage
    {
        /// <summary>
        /// The collection of digital joins used in the page.
        /// </summary>
        public enum DigitalJoins : uint
        {

            // Button Joins
            TVButtonPress = 5,
            ScreenButtonPress = 7,

            // Enable Joins
            TVButtonEnable = 6,
            ScreenButtonEnable = 8,
        }

    }
}
