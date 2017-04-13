using System.Collections.Generic;
using System.Reflection;

namespace Osmium.PluginEngine
{
    public class PluginLoader<TPlugin> where TPlugin : class
    {
        private List<TPlugin> loadedPlugins = new List<TPlugin>();

        public IEnumerable<TPlugin> Plugins => loadedPlugins;

        /// <summary>
        /// Load a plugin class directly
        /// </summary>
        /// <param name="plugin"></param>
        public void Load(TPlugin plugin)
        {
            loadedPlugins.Add(plugin);
        }

        protected IEnumerable<Type> FindAssignableTypes(Assembly pluginAssembly)
        {
            //Next we'll loop through all the Types found in the assembly
            foreach (var pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.GetTypeInfo().IsPublic)
                { //Only look at public types
                    if (!pluginType.GetTypeInfo().IsAbstract)
                    {  //Only look at non-abstract types
                       //Gets a type object of the interface we need the plugins to match
                       //Type typeInterface = pluginType.GetInterface("IPlatinumPlugin", true);
                        var containsInterface = typeof(T).GetTypeInfo().IsAssignableFrom(pluginType);
                        //Make sure the interface we want to use actually exists
                        //if (typeInterface != null)
                        if (containsInterface)
                        {
                            yield return pluginType;
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
            var pluginAssignableTypes = FindAssignableTypes(pluginAssembly);
            foreach (var pluginAssignableType in pluginAssignableTypes)
            {
                //Create a new available plugin since the type implements the IPlatinumPlugin interface
                var pluginInstance = (T)Activator.CreateInstance(pluginAssembly.GetType(pluginAssignableType.ToString()));
                var loadedPlugin = new LoadedPlugin<T>
                {
                    Assembly = pluginAssembly,
                    Instance = pluginInstance
                };

                //Call the plugin's initialize method
                loadedPlugin.Instance.Initialize();

                //Add the newly loaded plugin to the plugin collection
                AvailablePlugins.Add(loadedPlugin);
                assemblyPlugins.Add(loadedPlugin);
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
