using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSUtils.ConvertExtension
{
    public static class ConvertExtension
    {
        /// <summary>
        /// Parse given object with desired default value
        /// </summary>
        /// <typeparam name="T">Type of object to parse</typeparam>
        /// <param name="obj">Input Object</param>
        /// <param name="defaultValue">Default value for unsuccessfull parsing</param>
        /// <returns>Parsed value of <typeparamref name="T"/> or default <typeparamref name="T"/> </returns>
        public static T ConvertWithDefault<T>(this object obj, T defaultValue = default(T)) /*where T : INumeric*/
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


    }
}
