using Osmium.PluginEngine.Types;

namespace Osmium.PluginEngine.Tests.Types
{
    internal interface IMapGenPlugin : IOsmiumPlugin
    {
        string Name { get; }
    }
}
