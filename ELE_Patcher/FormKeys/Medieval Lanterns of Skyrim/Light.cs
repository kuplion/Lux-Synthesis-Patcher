// Autogenerated by https://github.com/Mutagen-Modding/Mutagen.Bethesda.FormKeys

using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;

namespace Mutagen.Bethesda.FormKeys.SkyrimSE
{
    public static partial class MedievalLanternsOfSkyrim
    {
        public static class Light
        {
            private static FormLink<ILightGetter> Construct(uint id) => new FormLink<ILightGetter>(ModKey.MakeFormKey(id));
            public static FormLink<ILightGetter> MLOS_candlelight_01 => Construct(0x898a8);
        }
    }
}