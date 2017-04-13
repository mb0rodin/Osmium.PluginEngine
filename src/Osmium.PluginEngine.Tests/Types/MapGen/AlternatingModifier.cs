namespace Osmium.PluginEngine.Tests.Types.MapGen
{
    internal class AlternatingModifier : IMapModifier
    {
        public void Apply(int[] map)
        {
            for (int i = 0; i < map.Length - 1; i += 2)
            {
                map[i] = 1;
            }
        }
    }
}
