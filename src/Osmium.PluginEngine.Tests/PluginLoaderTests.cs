using Osmium.PluginEngine.Tests.Types;
using Osmium.PluginEngine.Types;
using Xunit;

namespace Osmium.PluginEngine.Tests
{
    public class PluginLoaderTests
    {
        private PluginLoader<IOsmiumPlugin> loader = new PluginLoader<IOsmiumPlugin>();

        [Fact]
        public void CanLoadInstancePlugin()
        {
            var cookiePluginInstance = new CookiePlugin();
            loader.Load(cookiePluginInstance);
        }

        [Fact]
        public void CanLoadTypePluginGeneric()
        {
            loader.Load<CookiePlugin>();
        }

        [Fact]
        public void CanLoadTypePluginType()
        {
            loader.Load(typeof(CookiePlugin));
        }

        [Fact]
        public void PluginsAreLoaded()
        {
            var multiplier1 = new MultiplyingPlugin(1);
            var multiplier2 = new MultiplyingPlugin(2);
            loader.LoadMany(multiplier1, multiplier2);
            Assert.Equal(2, loader.PluginCount);
            // run all plugins
            int sum = 0;
            foreach (var plugin in loader.Plugins)
            {
                var multiplier = (MultiplyingPlugin)plugin;
                sum += multiplier.DoFoo(1, 1);
            }
            Assert.Equal(6, sum);
        }
    }
}
