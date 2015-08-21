using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace AppExtend.Plugin
{
    static class PluginManager
    {

        /// <summary>
        /// This routine loads files that implements a particular interface (TPlugin)
        /// </summary>
        /// <typeparam name="TPlugin">Interface that the file should implement in order to be loaded</typeparam>
        /// <param name="isRecursive">Indicates if the loader should look into subdirectories</param>
        /// <param name="pluginsPath">Path where the plugin should search</param>
        /// <param name="searchPattern">Filter in order to select the files</param>
        /// <returns>A list with all the instances of the plug in </returns>
        public static List<TPlugin> LoadPluginsOf<TPlugin>(Boolean isRecursive, String pluginsPath, String searchPattern)
        {
            List<TPlugin> pluginObjectList = new List<TPlugin>();
            String interfaceName = typeof(TPlugin).Name;
            String[] files = null;

            searchPattern = String.IsNullOrEmpty(searchPattern) ? @"*.dll" : searchPattern;
            //TODO: Add handling exception for reporting the right error message
            pluginsPath = Directory.Exists(pluginsPath) ? pluginsPath : Environment.CurrentDirectory;
            //TODO: Add handling exception for reporting the right error message
            files = Directory.GetFiles(pluginsPath, searchPattern, isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (String filename in files)
            {
                #if DEBUG
                System.Diagnostics.Debug.WriteLine(@"Loading file: {0}", filename);
                #endif
                try
                {
                    var types = from t in Assembly.LoadFrom(filename).GetTypes()
                                where (t.IsClass && !t.IsAbstract && t.GetInterface(interfaceName) != null)
                                select ((TPlugin)Activator.CreateInstance(t));
                    pluginObjectList.AddRange(types);
                }
                catch (Exception)
                {
                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(@"Loding fail: {0}", filename);
                #endif
                }
                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(@"Loading complete.");
                #endif
            }
            return pluginObjectList;
        }
    }
}
