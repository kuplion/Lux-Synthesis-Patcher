using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace ELE_Patcher
{
	public record Settings
	{
		[Tooltip("Most Lux conflicts can be solved by putting Lux late in the load order. Select this to skip patching records from Lux.esp, can save some time.")]
		public bool SkipVanilla = false;
	}
}
