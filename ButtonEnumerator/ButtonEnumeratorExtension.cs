using System;
using System.Collections.Generic;
using System.Linq;

namespace CSUtils
{
    using System.Windows.Forms;

    public static class ButtonEnumeratorExtension
    {
        //
        // Summary:
        //     Summary here.
        //
        // Parameters:
        //   ctrl_object:
        //     Control object to be processed.
        //
        //   keyValueMap:
        //     An enumeration of given control object states.
        //
        //   afterStateChange:
        //     A transition action when state changed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     s is null.
        //
        //   T:System.FormatException:
        //     s is not in the correct format.
        //
        //   T:System.OverflowException:
        //     s represents a number less than System.Int32.MinValue or greater than System.Int32.MaxValue.
        public static void Enumerate(this Control ctrl_object, Dictionary<string, object> keyValueMap, Action<Control, object> afterStateChange = null)
        {
            // find current state of control
            // state -1 bile dönse, 0 olarak devam eder
            int state = Array.IndexOf(keyValueMap.Keys.ToArray(), ctrl_object.Text);

            // shift to new state
            int new_state = (state + 1) % keyValueMap.Count;

            // apply new state variables
            ctrl_object.Text = keyValueMap.ElementAt(new_state).Key;
            ctrl_object.Tag = keyValueMap.ElementAt(new_state).Value;

            // send new tag to functor to make an action with new state.
            afterStateChange?.Invoke(ctrl_object, ctrl_object.Tag);
        }

        //
        // Summary:
        //     Summary here.
        //
        // Parameters:
        //   ctrl_object:
        //     Control object to be processed.
        //
        //   keyValueMap:
        //     An enumeration of given control object states.
        //
        //   afterStateChange:
        //     A transition action when state changed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     s is null.
        //
        //   T:System.FormatException:
        //     s is not in the correct format.
        //
        //   T:System.OverflowException:
        //     s represents a number less than System.Int32.MinValue or greater than System.Int32.MaxValue.
        public static void Enumerate(this Control ctrl_object, string[] strings, Action<Control, object> afterStateChange = null)
        {
            int tb_tag = ctrl_object.Tag.Parse<int>();
            tb_tag = (tb_tag + 1) % strings.Length;

            ctrl_object.Text = strings[tb_tag];
            ctrl_object.Tag = tb_tag;
            // send new tag to functor to make an action with new state.
            afterStateChange?.Invoke(ctrl_object, ctrl_object.Tag);
        }

        //
        // Summary:
        //     Summary here.
        //
        // Parameters:
        //   ctrl_object:
        //     Control object to be processed.
        //
        //   keyValueMap:
        //     An enumeration of given control object states.
        //
        //   afterStateChange:
        //     A transition action when state changed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     s is null.
        //
        //   T:System.FormatException:
        //     s is not in the correct format.
        //
        //   T:System.OverflowException:
        //     s represents a number less than System.Int32.MinValue or greater than System.Int32.MaxValue.
        public static void Enumerate(this Control ctrl_object, string[] strings, object newTag, Action<Control, object> afterStateChange = null)
        {
            int tb_tag = newTag.Parse<int>();

            ctrl_object.Text = strings[tb_tag];
            ctrl_object.Tag = tb_tag;
            // send new tag to functor to make an action with new state.
            afterStateChange?.Invoke(ctrl_object, ctrl_object.Tag);
        }
    }
}
