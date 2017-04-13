namespace Osmium.PluginEngine.Tests.Types.MapGen
{
    internal class FirstFiveModifier : IMapModifier
    {
        public void Apply(int[] map)
        {
            for (int i = 0; i < 5 && i < map.Length; i++)
            {
                map[i] = 1;
            }
        }
    }
}
