using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;

using Noggog;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Synthesis;

using ELE_Patcher.Utility;

namespace ELE_Patcher
{
    public class Program
    {
		private static readonly ModKey KeyELE = ModKey.FromNameAndExtension("ELE_SSE.esp");

		public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "Synthesis ELE patch.esp")
				.AddRunnabilityCheck(state =>
				{
					state.LoadOrder.AssertHasMod(KeyELE, true, "\n\nELE plugin missing, not active, or inaccessible to patcher!\n\n");
				})
				.Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
			PatchVanilla(state);
            PatchModded(state);
            Console.WriteLine();
        }

		private static void PatchVanilla(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
		{
			using var mod = KeyELE.GetModAndMasters(state, out var masters);
			Console.WriteLine("\nPatching records from ELE_SSE.esp:");

			#region Image spaces
			Console.WriteLine("- Image spaces");
			foreach (var modded in mod.ImageSpaces)
			{
				if (!modded.InitializeRecordVars(state, KeyELE, masters, out var vanillas, out ImageSpace? patched, out var changed))
					continue;

				patched.PatchHdr(vanillas, modded, ref changed);
				patched.PatchCinematic(vanillas, modded, ref changed);
				patched.PatchTint(vanillas, modded, ref changed);

				if (changed)
					state.PatchMod.ImageSpaces.Set(patched);
			}
			#endregion
			#region Lights
			Console.WriteLine("- Lights");
			foreach (var modded in mod.Lights)
			{
				if (!modded.InitializeRecordVars(state, KeyELE, masters, out var vanillas, out Light? patched, out var changed))
					continue;

				patched.PatchRecordFlags(vanillas, modded, ref changed);
				patched.PatchFlags(vanillas, modded, ref changed);

				patched.GetMasks(vanillas, modded, out var moddedEquals, out var vanillasEqual, out var doCopy);
				doCopy.MaskObjectBounds(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskRadius(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskColor(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskNearClip(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskFadeValue(vanillasEqual, moddedEquals, ref changed);
				patched.DeepCopyIn(modded, doCopy);

				if (changed)
					state.PatchMod.Lights.Set(patched);
			}
			#endregion
			#region Worldspaces
			Console.WriteLine("- Worldspaces");
			var worldspaces = mod.AsEnumerable().Worldspace().WinningContextOverrides();
			foreach (var moddedContext in worldspaces)
			{
				var modded = moddedContext.Record;

				if (!modded.InitializeRecordVars(state, KeyELE, masters, out var vanillas, out Worldspace? patched, out var changed))
					continue;

				patched.GetMasks(vanillas, modded, out var moddedEquals, out var vanillasEqual, out var doCopy);
				doCopy.MaskInteriorLighting(vanillasEqual, moddedEquals, ref changed);

				if (changed)
					moddedContext
						.GetOrAddAsOverride(state.PatchMod)
						.DeepCopyIn(modded, doCopy);
			}
			#endregion
			#region Cells
			Console.WriteLine("- Cells");
			var cellMask = new Cell.TranslationMask(false)
			{
				Flags = true
			};
			var cells = mod.AsEnumerable().Cell().WinningContextOverrides(state.LinkCache);
			foreach (var moddedContext in cells)
			{
				var modded = moddedContext.Record;

				if (!modded.InitializeRecordVars(state, KeyELE, masters, out var vanillas, out Cell? patched, out var changed))
					continue;

				patched.PatchFlags(vanillas, modded, ref changed);

				patched.GetMasks(vanillas, modded, out var moddedEquals, out var vanillasEqual, out var doCopy);
				doCopy.MaskLighting(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskLightingTemplate(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskWaterHeight(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskWaterNoiseTexture(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskSkyAndWeather(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskImageSpace(vanillasEqual, moddedEquals, ref changed);

				if (changed)
				{
					var patchedIntoMod = moddedContext.GetOrAddAsOverride(state.PatchMod);
					patchedIntoMod.DeepCopyIn(patched, cellMask);
					patchedIntoMod.DeepCopyIn(modded, doCopy);
				}
			}
			#endregion
			#region Places objects
			Console.WriteLine("- Placed objects");
			var placedObjects = mod.AsEnumerable().PlacedObject().WinningContextOverrides(state.LinkCache);
			var placedObjectMask = new PlacedObject.TranslationMask(false)
			{
				MajorRecordFlagsRaw = true,
				Primitive = new(true),
				LightData = new(true)
			};
			foreach (var moddedContext in placedObjects)
			{
				var modded = moddedContext.Record;

				if (!modded.InitializeRecordVars(state, KeyELE, masters, out var vanillas, out PlacedObject? patched, out var changed))
					continue;

				patched.PatchRecordFlags(vanillas, modded, ref changed);
				patched.PatchPrimitive(vanillas, modded, ref changed);
				patched.PatchLightData(vanillas, modded, ref changed);

				patched.GetMasks(vanillas, modded, out var moddedEquals, out var vanillasEqual, out var doCopy);
				doCopy.MaskBoundHalfExtents(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskUnknown(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskLightingTemplate(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskImageSpace(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskLocationRef(vanillasEqual, moddedEquals, ref changed);
				doCopy.MaskPlacement(vanillasEqual, moddedEquals, ref changed);

				if (changed)
				{
					var patchedIntoMod = moddedContext.GetOrAddAsOverride(state.PatchMod);
					patchedIntoMod.DeepCopyIn(patched, placedObjectMask);
					patchedIntoMod.DeepCopyIn(modded, doCopy);
				}
			}
			#endregion
		}

		private static void PatchModded(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
		{
			#region Check supported mods
			// Moved reading of supported mods here in case user doesn't want to patch mods
			HashSet<ModKey> SupportedMods = new()
			{
				Modded.KeyBruma,
				Modded.KeyCRF,
				Modded.KeyDarkend,
				Modded.KeyFalskaar,
				Modded.KeyHelgenReborn,
				Modded.KeyLanterns,
				Modded.KeyLotDb,
				Modded.KeyRavengate,
				Modded.KeyMedievalLanterns
			};

			HashSet<ModKey> presentSupportedMods = new();
			foreach (var mod in SupportedMods)
				if (state.LoadOrder.HasMod(mod, true))
					presentSupportedMods.Add(mod);

			if (presentSupportedMods.Count == 0)
				return;
			#endregion

			Console.WriteLine("\nPatching records from supported mods:");

			#region Cells
			Console.WriteLine("- Cells");
			var cellMask = new Cell.TranslationMask(false)
			{
				Lighting = true,
				LightingTemplate = true,
				ImageSpace = true
			};
			var CellInfoList = presentSupportedMods.GetCellInfo(state);
			foreach (var cellInfo in CellInfoList)
			{
				if (!cellInfo.Key.TryResolveContext<ISkyrimMod, ISkyrimModGetter, ICell, ICellGetter>(state.LinkCache, out var winner))
					continue;

				var patched = winner.Record.DeepCopy();
				bool changed = false;
				var fetchedInfo = cellInfo.Value;

				patched.PatchLightingTemplate(fetchedInfo.lightingTemplate, ref changed);
				patched.PatchImageSpace(fetchedInfo.imageSpace, ref changed);

				if (changed)
					winner
						.GetOrAddAsOverride(state.PatchMod)
						.DeepCopyIn(patched, cellMask);
			}
			presentSupportedMods.PatchCellsExtra(state);
			#endregion
			#region Image spaces
			Console.WriteLine("- Image spaces");
			var moddedImagespaceInfo = presentSupportedMods.GetImageSpaceInfo();
			foreach (var info in moddedImagespaceInfo)
			{
				if (!info.Key.TryResolve(state.LinkCache, out var winner))
					continue;
				var patched = winner.DeepCopy();
				var changed = true;

				patched.PatchCinematic(info.Value, ref changed);

				if (changed)
					state.PatchMod.ImageSpaces.Set(patched);
			}
			#endregion
			#region Lights
			Console.WriteLine("- Lights");
			var moddedLightsInfo = presentSupportedMods.GetLightsInfo();
			foreach (var info in moddedLightsInfo)
			{
				if (!info.Key.TryResolve(state.LinkCache, out var winner))
					continue;
				var patched = winner.DeepCopy();
				var changed = true;

				patched.PatchColor(info.Value, ref changed);

				if (changed)
					state.PatchMod.Lights.Set(patched);
			}
			presentSupportedMods.PatchLightsExtra(state);
			#endregion
		}
	}
}