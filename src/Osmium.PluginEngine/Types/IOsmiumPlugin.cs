using CookieIoC;

namespace Osmium.PluginEngine.Types
{
    public interface IOsmiumPlugin
    {
        void BeforeActivation(CookieJar container);
    }
}
