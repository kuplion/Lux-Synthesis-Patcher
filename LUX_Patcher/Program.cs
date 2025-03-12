using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda;
using Noggog;
using Mutagen.Bethesda.Plugins;

namespace LuxPatcher
{
    public class Program
    {
        static ModKey Lux { get; } = ModKey.FromFileName("Lux.esp");
        static ModKey[] LuxAddons { get; } = [
            ModKey.FromNameAndExtension("Lux - Brighter interior nights.esp"),
            ModKey.FromNameAndExtension("Lux - Brighter Templates Dungeons only.esp"),
            ModKey.FromNameAndExtension("Lux - Brighter Templates Houses only.esp"),
            ModKey.FromNameAndExtension("Lux - Brighter Templates.esp"),
            ModKey.FromNameAndExtension("Lux - Even Brighter Templates Dungeons only.esp"),
            ModKey.FromNameAndExtension("Lux - Even Brighter Templates Houses only.esp"),
            ModKey.FromNameAndExtension("Lux - Even Brighter Templates.esp")
        ];

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "LuxPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (state.LoadOrder.TryGetValue(Lux) is not { Mod: not null } lux)
            {
                Console.Error.WriteLine("'Lux.esp' cannot be found. Make sure you have installed Lux.");
                return;
            };

            if (state.LoadOrder.TryGetValue("PL - Default.esp", out var _) && state.LoadOrder.TryGetValue("PL - Dark.esp", out var _))
            {
                Console.Error.WriteLine("You are using both 'PL - Default.esp' and 'PL - Dark.esp', please choose only one Lighting Template plugin.");
                return;
            }

            // Setup list of Placed Light plugins, adding the Lighting Template override plugins if present
            var luxPlugins = new List<ISkyrimModGetter> { lux.Mod };

            // Add addons to the list of placed light plugins if found
            foreach (var modKey in LuxAddons)
            {
                if (state.LoadOrder.TryGetValue(modKey) is not { Mod: not null } addon)
                    continue;
                luxPlugins.Add(addon.Mod);
            }

            var loadOrderLinkCache = state.LoadOrder.ToImmutableLinkCache();
            var luxLinkCache = luxPlugins.ToImmutableLinkCache();

            //Find all interior cells where Placed Light.esm is not already the winner
            var cellContexts = state.LoadOrder.PriorityOrder.Cell()
                .WinningContextOverrides(loadOrderLinkCache)
                .Where(static i => i.ModKey != Lux)
                .Where(static i => i.Record.Flags.HasFlag(Cell.Flag.IsInteriorCell))
                .Where(static i => !i.Record.MajorFlags.HasFlag(Cell.MajorFlag.Persistent));

            var cellMask = new Cell.TranslationMask(false)
            {
                Lighting = true
            };

            uint patchedCellCount = 0;
            foreach (var winningCellContext in cellContexts)
            {
                if (!luxLinkCache.TryResolve<ICellGetter>(winningCellContext.Record.FormKey, out var luxCellRecord))
                    continue;

                if (luxCellRecord.Lighting == null)
                    continue;

                // If the winning cell record already has the same lighting values as Placed Light, skip it.
                if (winningCellContext.Record.Equals(luxCellRecord, cellMask))
                    continue;

                winningCellContext.GetOrAddAsOverride(state.PatchMod).Lighting = luxCellRecord.Lighting.DeepCopy();
                patchedCellCount++;
            }

            uint patchedLightCount = 0;
            foreach (var winningLightRecord in state.LoadOrder.PriorityOrder.Light().WinningOverrides())
            {
                if (!luxLinkCache.TryResolve<ILightGetter>(winningLightRecord.FormKey, out var luxRecord))
                    continue;

                if (!loadOrderLinkCache.TryResolve<ILightGetter>(winningLightRecord.FormKey, out var originLightRecord, ResolveTarget.Origin))
                    continue;

                // Forward Light records if the winning record is using vanilla values
                if (winningLightRecord.Equals(originLightRecord) && !winningLightRecord.Equals(luxRecord))
                {
                    state.PatchMod.Lights.DuplicateInAsNewRecord(luxRecord);
                    patchedLightCount++;
                }
            }

            Console.WriteLine($"Patched {patchedCellCount} cells");
            Console.WriteLine($"Patched {patchedLightCount} lights");
        }
    }
}
