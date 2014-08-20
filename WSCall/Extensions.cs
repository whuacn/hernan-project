using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{

//https://dnpextensions.codeplex.com/wikipage?title=System.String

    /// <summary>
    /// Set of very useful extension methods for hour by hour use in .NET code.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method to evaluate if object is null.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T @this)
        {
            return @this == null;
        }
        /// <summary>
        /// Extension method to evaluate if object is not null
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T @this)
        {
            return @this != null;
        }
        /// <summary>
        /// Extension method to evaluate if the specified object exists within the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T @this, params T[] list)
        {
            //if (null == @this)
            //throw new ArgumentNullException("instance is null, can't check against null.");
            return list.Contains(@this);
        }
        /// <summary>
        /// Extension method to evaluate if the specified object doest not exists within the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool NotIn<T>(this T @this, params T[] list)
        {
            return !list.Contains(@this);
        }
        /// <summary>
        /// Extension method that performs the operation string.Format 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string @this, params object[] args)
        {
            return string.Format(@this, args);
        }
        /// <summary>
        /// Extension method that returns whether the specified Enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
        {
            return (@this == null || @this.Count() == 0);
        }
        /// <summary>
        /// Extension method that performs a boolean evaluation if @this is of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool Is<T>(this T @this)
        {
            return @this is T;
        }
        /// <summary>
        /// Extension method that performs a safe cast for @this as T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T As<T>(this T @this) where T : class
        {
            return @this as T;
        }
  

    }
}
