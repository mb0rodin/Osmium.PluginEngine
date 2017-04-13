using Osmium.PluginEngine.Types;

namespace Osmium.PluginEngine.Tests.Types
{
    interface ITestPlugin : IOsmiumPlugin
    {
        int DoFoo(int a, int b);
    }
}
