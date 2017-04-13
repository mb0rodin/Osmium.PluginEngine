using CookieIoC;
using Osmium.PluginEngine.Tests.Types.MapGen;

namespace Osmium.PluginEngine.Tests.Types
{
    internal class StupidMapGenPlugin : IMapGenPlugin
    {
        public string Name => "Stupid Mapgen";

        public void BeforeActivation(CookieJar container)
        {
            container.Register<IMapModifier>(new AlternatingModifier());
            container.Register<IMapModifier>(new FirstFiveModifier());
        }
    }
}
