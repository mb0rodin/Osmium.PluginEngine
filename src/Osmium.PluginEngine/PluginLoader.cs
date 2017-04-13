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
            foreach (var aType in assembly.GetTypes())
            {
                if (aType.GetTypeInfo().IsPublic) // only look at public types
                {
                    if (!aType.GetTypeInfo().IsAbstract) // only look at non-abstract types
                    {
                        var containsInterface = typeof(TPlugin).GetTypeInfo().IsAssignableFrom(aType);
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
            foreach (var pluginType in FindAssignableTypes(assembly))
            {
                var pluginInstance = (TPlugin)Activator.CreateInstance(pluginType);

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
