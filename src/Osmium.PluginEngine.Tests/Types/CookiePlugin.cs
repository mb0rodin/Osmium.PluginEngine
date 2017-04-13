namespace Osmium.PluginEngine.Tests.Types
{
    internal class CookiePlugin : ITestPlugin
    {
        public int DoFoo(int a, int b)
        {
            return a * 8 + b * 6;
        }
    }
}
