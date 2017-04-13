using System;
using CookieIoC;

namespace Osmium.PluginEngine.Tests.Types
{
    internal class CookiePlugin : ITestPlugin
    {
        public void BeforeActivation(CookieJar container)
        {
            throw new NotImplementedException();
        }

        public int DoFoo(int a, int b)
        {
            return a * 8 + b * 6;
        }
    }
}
