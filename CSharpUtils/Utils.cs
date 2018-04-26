using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CSUtils
{
    public static class CSUtils
    {

        /// <summary>
        /// Parse given object with desired default value
        /// </summary>
        /// <typeparam name="T">Type of object to parse</typeparam>
        /// <param name="obj">Input Object</param>
        /// <param name="defaultValue">Default value for unsuccessfull parsing</param>
        /// <returns>Parsed value of <typeparamref name="T"/> or default <typeparamref name="T"/> </returns>
        public static T Parse<T>(this object obj, T defaultValue = default(T)) /*where T : INumeric*/
        {
            T value = defaultValue;
            try
            {
                value = (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (Exception)
            {
                // do nothing
            }
            return value;
        }

        /// <summary>
        /// Get all controlled objects (by type) that given control have.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <returns>Array of controlled control objects</returns>
        /// <example>
        /// Sample Usage: 
        /// Get all checkboxes in form control: 
        /// <code>
        /// foreach (CheckBox cb in this.GetAll(typeof(CheckBox)))
        /// {
        ///     cb.Checked = true;
        /// }
        /// </code>
        /// Advance Usage: 
        /// <code>
        /// var listOfEnabledTextBoxes = this.GetAll(typeof(TextBox)).Where(tb => tb.Enabled);
        /// </code>
        /// </example>
        public static IEnumerable<T> GetAll<T>(this Control control) where T: Control
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll<T>(ctrl))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Access all control of type <typeparamref name="T"/> and do whatever you need for given control
        /// </summary>
        /// <typeparam name="T">Windows Form Control Object</typeparam>
        /// <param name="control">Form instance</param>
        /// <param name="functor">Accessor of given Control object</param>
        /// <example>
        /// Sample Usage: 
        /// To Check all checkboxes in form control: 
        /// <code>
        /// (this as Form).MakeAll<CheckBox>(cb => cb.Checked = true);
        /// </code>
        /// </example>
        public static void MakeAll<T>(this Form control, Action<T> functor) where T : Control
        {
            var controls = control.Controls.Cast<Control>();
            var enm = controls.SelectMany(ctrl => GetAll<T>(ctrl))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == typeof(T));
            foreach (var item in enm)
            {
                functor?.Invoke((T)item);
            }
        }

    }

}
