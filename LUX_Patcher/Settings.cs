using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace LUX_Patcher
{
	public record Settings
	{
		[Tooltip("Most Lux conflicts can be solved by putting Lux late in the load order and using the wealth of available patches. Select this to skip patching records from Lux.esp, can save some time.")]
		public bool SkipVanilla = false;

		[Tooltip("Skip applying built in patches for Beyond Bruma, CRF, Darkend, Falskaar, Helgen Reborn, LoS, LoTD, MLoS, and Ravengate.")]
		public bool SkipModded = false;
	}
}
