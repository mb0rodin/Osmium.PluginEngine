namespace Osmium.PluginEngine.Tests.Types
{
    internal class MultiplyingPlugin : ITestPlugin
    {
        public MultiplyingPlugin(int factor)
        {
            Factor = factor;
        }

        public int Factor { get; }

        public int DoFoo(int a, int b)
        {
            return (a + b) * Factor;
        }
    }
}
