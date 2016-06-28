//-----------------------------------------------------------------------
// <copyright file="WindowsRegistry.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace SerialSniffer
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// This class contains all the registry access methods.
    /// It is possible to define the subfolder where to look for the registry entries.
    /// The class exposes a number <c>polymorfic</c> static methods named Get and Set, each one has as a first
    /// parameter the key in the SerialSniffer registry sub key and a parameter of the selected type.
    /// There are getters and setters for strings, booleans, integers and lists of strings.
    /// <para />
    /// For instance, to save an integer under the name <c>"IntName"</c>, the following call should be made
    ///     <c>Registry.Set("IntName", 9);</c>
    /// while retrieving the same number should be
    ///     <c>int n;
    ///     Registry.Get("IntName", out n, defaultValue);</c>
    ///     where defaultValue is an integer number returned in n if the key is not present in the registry.
    /// </summary>
    public static class WindowsRegistry
    {
        /// <summary>
        /// All the registry values are stored in the SOFTWARE\SerialSniffer
        /// </summary>
        private static string basePath = "SOFTWARE\\SerialSnifferr";

        /// <summary>
        /// Determines if the passed (absolute) key exists.
        /// </summary>
        /// <param name="absoluteKey">Absolute Key, i.e. path of a key from the root of register.</param>
        /// <returns>Returns true if the passed (absolute) key exists.</returns>
        public static bool ExistsAbsoluteKeyClassesRoot(string absoluteKey)
        {
            var subTree = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(absoluteKey);
            return subTree != null;
        }

        #region String getter and setter
        /// <summary>
        /// Gets a value from the registry sub tree of SerialSniffer
        /// If the SerialSniffer sub tree is not present, it creates it
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="defaultValue">default value to associate to the key if not present</param>
        /// <returns>value for the key</returns>
        public static string Get(string key, string defaultValue = null) 
        {
            var serialSnifferRegistry = OpenSsSubKey();

            if (defaultValue == null)
            {
                return (string)serialSnifferRegistry.GetValue(key);
            }
            else
            {
                return (string)serialSnifferRegistry.GetValue(key, defaultValue);
            }
        }

        /// <summary>
        /// Associates a key to a value in the SerialSniffer sub tree of the registry
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="value">value to associate</param>
        public static void Set(string key, string value)
        {
            var serialsnifferRegistry = OpenSsSubKey(true);
            serialsnifferRegistry.SetValue(key, value);
        }
        #endregion
        #region bool getter and setter
        /// <summary>
        /// Decodes a boolean value from the registry
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="value">boolean associated to the key</param>
        /// <param name="defValue">default value to be used if no key is present</param>
        /// <returns>String representation of the value</returns>
        public static string Get(string key, out bool value, bool defValue)
        {
            string v = Get(key, defValue ? "1" : "0");
            if (v == "1")
            {
                value = true;
            }
            else
            {
                value = false;
            }

            return v;
        }

        /// <summary>
        /// Associates a boolean value to a registry key
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="value">boolean value to associate</param>
        public static void Set(string key, bool value)
        {
            Set(key, value ? "1" : "0");
        }
        #endregion
        #region int getter and setter
        /// <summary>
        /// Associates an integer value to a registry key
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="value">integer value to associate</param>
        public static void Set(string key, int value)
        {
            Set(key, value.ToString());
        }

        /// <summary>
        /// Decodes an integer value from the registry
        /// </summary>
        /// <param name="key">key in the registry under the SerialSniffer sub tree to search</param>
        /// <param name="value">integer associated to the key</param>
        /// <param name="defValue">default value to be used if no key is present</param>
        /// <returns>String representation of the value</returns>
        public static string Get(string key, out int value, int defValue)
        {
            string v = Get(key, defValue.ToString());
            if (!int.TryParse(v, out value))
            {
                value = defValue;
            }

            return v;
        }
        #endregion
        #region list of strings getter and setter
        /// <summary>
        /// Accesses a composite registry value
        /// </summary>
        /// <param name="key">Registry name.</param>
        /// <returns>List of values attached to the passed key. If no key was present, it returns a void list, i.e. a list containing 0 elements.</returns>
        public static List<string> Gets(string key)
        {
            var serialsnifferRegistry = OpenSsSubKey();
            object o = serialsnifferRegistry.GetValue(key);
            if (o == null)
            {
                return new List<string>();
            }

            string[] ls = (string[])o;
            return new List<string>(ls);
        }

        /// <summary>
        /// Associates a key to an array of strings in the SerialSniffer sub tree of the registry
        /// </summary>
        /// <param name="key">key in the registry</param>
        /// <param name="values">list of string to associate</param>
        public static void Set(string key, List<string> values)
        {
            var serialSnifferRegistry = OpenSsSubKey(true);
            string[] vals = values.ToArray();
            serialSnifferRegistry.SetValue(key, vals);
        }
        #endregion

        /// <summary>
        /// Retrieves the current selected item in a <c>combobox</c> from the SerialSniffer sub tree of the Registry.
        /// It can be used in cases where the strings stored in the <c>combobox</c> do not map directly to strings, e.g. <c>enums</c>.
        /// </summary>
        /// <param name="key">Key in the registry.</param>
        /// <param name="comboBox">Combo box receiving the information extracted.</param>
        /// <param name="defValue">Default value if nothing is stored in the registry.</param>
        /// <param name="registry2item">If not null, maps the string retrieved from the registry to the string value that is actually contained in the combo box</param>
        public static void Get(string key, ComboBox comboBox, string defValue, Dictionary<string, string> registry2item = null)
        {
            string value = Get(key, defValue);
            if (registry2item != null)
            {
                if (registry2item.ContainsKey(value))
                {
                    value = registry2item[value];
                }
            }

            int indexToSelect = 0;
            bool found = false;
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == value)
                {
                    found = true;
                    break;
                }

                indexToSelect++;
            }

            if (found)
            {
                comboBox.SelectedIndex = indexToSelect;
            }
        }

        /// <summary>
        /// Retrieves the selected value for a <c>combobox </c>stored in the Registry
        /// </summary>
        /// <param name="key">Key in the SerialSniffer sub tree of the Registry where the current value for the combo box is stored</param>
        /// <param name="combobox"><c>combobox</c> receiving the data</param>
        public static void Set(string key, ComboBox combobox)
        {
            int selectedIndex = combobox.SelectedIndex;

            if (selectedIndex != -1)
            {
                Set(key, combobox.Items[selectedIndex].ToString());
            }
        }

        /// <summary>
        /// Opens the <c>SerialSniffer</c> registry <c>subtree</c> creating it if not existent (default)
        /// </summary>
        /// <param name="writable">True if the registry key must be writable</param>
        /// <param name="createIfDoesNotExist">True if the <c>subtree</c> must be created if it does not exist</param>
        /// <returns>A reference to the opened registry <c>subtree</c>,</returns>
        private static Microsoft.Win32.RegistryKey OpenSsSubKey(bool writable = false, bool createIfDoesNotExist = true)
        {
            Microsoft.Win32.RegistryKey result = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(basePath, writable);
            if (createIfDoesNotExist && result == null)
            {
                result = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(basePath);
            }

            return result;
        }
    }
}
