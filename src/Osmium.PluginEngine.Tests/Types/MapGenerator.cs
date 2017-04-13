using CookieIoC;
using Osmium.PluginEngine.Tests.Types.MapGen;

namespace Osmium.PluginEngine.Tests.Types
{
    internal class MapGenerator
    {
        public CookieJar Container { get; }

        private PluginLoader<IMapGenPlugin> pluginLoader;

        public MapGenerator(CookieJar container)
        {
            Container = container;
            pluginLoader = new PluginLoader<IMapGenPlugin>();
        }

        public void LoadPlugins()
        {
            pluginLoader.Load(new StupidMapGenPlugin());
            foreach (var plugin in pluginLoader.Plugins)
            {
                plugin.BeforeActivation(Container);
            }
        }

        public int[] Run()
        {
            // Create default map
            var map = new int[12];
            // Load registered modifiers
            var modifiers = Container.ResolveAll<IMapModifier>();
            foreach (var mod in modifiers)
            {
                mod.Apply(map);
            }
            return map;
        }
    }
}
