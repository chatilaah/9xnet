using System.Collections.Generic;
using System.Linq;

namespace W9xNET.RcReader.Base
{
    /// <summary>
    /// A Win32-specific user-interface element style.
    /// </summary>
    public class RcControlStyle
    {
        private readonly Dictionary<string, bool> StyleAttributes;

        /// <summary>
        /// Returns the start index of newly added attributes from subclasses.
        /// </summary>
        protected readonly int SaStartIndex;

        public RcControlStyle(string[] styles, string[] stylesEx = null)
        {
            SaStartIndex = styles.Length;

            int count = styles.Length;

            if (stylesEx != null)
            {
                count += stylesEx.Length;
            }

            StyleAttributes = new Dictionary<string, bool>();

            int iNext = 0;
            for (int i = 0; i < count; i++)
            {
                bool first = i < SaStartIndex;
                StyleAttributes[first ? styles[i] : stylesEx[iNext]] = false;

                if (!first)
                {
                    iNext += 1;
                }
            }
        }

        /// <summary>
        /// Gets the style attribute's value.
        /// </summary>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>Returns the style attribute's value.</returns>
        public bool this[string attribute]
        {
            get => this.StyleAttributes[attribute];
            set => this.StyleAttributes[attribute] = value;
        }

        /// <summary>
        /// Decodes the Win32 style to a string.
        /// </summary>
        /// <returns></returns>
        public virtual string Decode()
        {
            var result = string.Empty;

            foreach (var sa in StyleAttributes)
            {
                if (sa.Value)
                {
                    result += $" | {sa.Key}";
                }
            }

            return result.Substring(3);
        }

        /// <summary>
        /// Simple redirect to Decode().
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Decode();

        /// <summary>
        /// Resets all properties to FALSE.
        /// </summary>
        public virtual void Reset()
        {
            var keys = StyleAttributes.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                StyleAttributes[keys[i]] = false;
            }
        }
    }
}