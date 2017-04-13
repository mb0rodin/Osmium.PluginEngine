using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

namespace Osmium.PluginEngine
{
    public class PluginLoader<TPlugin> where TPlugin : class
    {
        private List<TPlugin> loadedPlugins = new List<TPlugin>();

        public IEnumerable<TPlugin> Plugins => loadedPlugins;

        public int PluginCount => loadedPlugins.Count;

        /// <summary>
        /// Load a plugin class directly
        /// </summary>
        /// <param name="plugin"></param>
        public void Load(TPlugin plugin)
        {
            loadedPlugins.Add(plugin);
        }

        public void LoadMany(params TPlugin[] plugins)
        {
            foreach (var plugin in plugins)
            {
                Load(plugin);
            }
        }

        /// <summary>
        /// Load a plugin from a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Load<T>() where T : TPlugin
        {
            Load(typeof(T));
        }

        /// <summary>
        /// Load a plugin from a type. This type must inherit from TPlugin
        /// </summary>
        /// <param name="t"></param>
        public void Load(Type t)
        {
            var instance = (TPlugin)Activator.CreateInstance(t);
            loadedPlugins.Add(instance);
        }

        protected IEnumerable<Type> FindAssignableTypes(Assembly assembly)
        {
            //Next we'll loop through all the Types found in the assembly
            foreach (var aType in assembly.GetTypes())
            {
                if (aType.GetTypeInfo().IsPublic)
                { //Only look at public types
                    if (!aType.GetTypeInfo().IsAbstract)
                    {  //Only look at non-abstract types
                       //Gets a type object of the interface we need the plugins to match
                       //Type typeInterface = pluginType.GetInterface("IPlatinumPlugin", true);
                        var containsInterface = typeof(TPlugin).GetTypeInfo().IsAssignableFrom(aType);
                        //Make sure the interface we want to use actually exists
                        //if (typeInterface != null)
                        if (containsInterface)
                        {
                            yield return aType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load all plugin classes in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IEnumerable<TPlugin> LoadFrom(Assembly assembly)
        {
            var assemblyPlugins = new List<TPlugin>();
            //Get the type that can be assigned to the target interface
            foreach (var pluginType in FindAssignableTypes(assembly))
            {
                //Create a new available plugin since the type implements the IPlatinumPlugin interface

                var pluginInstance = (TPlugin)Activator.CreateInstance(pluginType);

                // Add the newly loaded plugin to the plugin collection
                assemblyPlugins.Add(pluginInstance);
            }
            loadedPlugins.AddRange(assemblyPlugins);
            return assemblyPlugins;
        }

        /// <summary>
        /// Load an Assembly from a file path
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public Assembly LoadAssembly(string assemblyPath) => Assembly.Load(AssemblyLoadContext.GetAssemblyName(assemblyPath));
    }
}
