// Autogenerated by https://github.com/Mutagen-Modding/Mutagen.Bethesda.FormKeys

using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;

namespace Mutagen.Bethesda.FormKeys.SkyrimSE
{
    public static partial class LUX_PATCH
    {
        public static class LightingTemplate
        {
            private static FormLink<ILightingTemplateGetter> Construct(uint id) => new FormLink<ILightingTemplateGetter>(ModKey.MakeFormKey(id));
            public static FormLink<ILightingTemplateGetter> _LUX_Dark_Ice_LT => Construct(0x9F392D); // Lux_LTColdObscure_IceCaves [LGTM:089F392D]
            public static FormLink<ILightingTemplateGetter> _LUX_Dark_ThinFog_LT => Construct(0x1DDF91); // Lux_LTNordicRuinsLowNatural [LGTM:081DDF91]
            public static FormLink<ILightingTemplateGetter> _LUX_Dark_Falmer_LT => Construct(0x9F392C); // Lux_LTObscure_IceCaves [LGTM:089F392C]
            public static FormLink<ILightingTemplateGetter> _LUX_Dark_Dwemer_LT => Construct(0x9F392B); // Lux_LTObscure_Dwemer [LGTM:089F392B]
            public static FormLink<ILightingTemplateGetter> _LUX_Medium_LT => Construct(0x9F3928); // Lux_LTObscure_Forts [LGTM:089F3928]
            public static FormLink<ILightingTemplateGetter> _LUX_Bright_LT => Construct(0x3B1F31); // Lux_LTHouseBrightLowNorth [LGTM:083B1F31]
            public static FormLink<ILightingTemplateGetter> _LUX_Dark_LT => Construct(0x9F3929); // Lux_LTObscure_Mines [LGTM:089F3929]
            public static FormLink<ILightingTemplateGetter> _LUX_Brighter_LT => Construct(0x3CB6B2); // Lux_LTHouseBrightLowWest [LGTM:083CB6B2]

            // public static FormLink<ILightingTemplateGetter> _LUX_Bright_Ice_LT => Construct(0xa389);
            // public static FormLink<ILightingTemplateGetter> _LUX_Medium_Ice_LT => Construct(0xa38a);
            // public static FormLink<ILightingTemplateGetter> _LUX_Dark_Falmer_FogDark_LT => Construct(0xa68f);
            // public static FormLink<ILightingTemplateGetter> _LUX_Medium_ThinFog_LT => Construct(0x25cb2);
            // public static FormLink<ILightingTemplateGetter> _LUX_Dark_Falmer_FogFar_LT => Construct(0x1e4bf);
            // public static FormLink<ILightingTemplateGetter> _LUX_Medium_Falmer_LT => Construct(0xdfe4);
            // public static FormLink<ILightingTemplateGetter> _LUX_Brighter_Ice_LT => Construct(0xcfbb);
            // public static FormLink<ILightingTemplateGetter> _LUX_Dark_Falmer_FogNear_LT => Construct(0xc32f);
            // public static FormLink<ILightingTemplateGetter> _LUX_Test_LT => Construct(0x60ba8);
        }
    }
}
