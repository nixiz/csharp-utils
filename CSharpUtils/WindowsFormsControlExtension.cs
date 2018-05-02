using System;
using System.Collections.Generic;
using System.Linq;

namespace CSUtils.WindowsFormsControlExtensions
{
    using Control = System.Windows.Forms.Control;

    public static class WindowsFormsControlExtensions
    {
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
        /// <param name="control">Control object instance</param>
        /// <param name="functor">Accessor of given Control object</param>
        /// <example>
        /// Sample Usage: 
        /// To Check all checkboxes in form control: 
        /// <code>
        /// (this as Form).MakeAll<CheckBox>(cb => cb.Checked = true);
        /// </code>
        /// </example>
        public static void MakeAll<T>(this Control control, Action<T> functor) where T : Control
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
