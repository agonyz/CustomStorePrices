using HarmonyLib;

namespace CustomStorePrices.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalSetItemSalesPatch
    {
        [HarmonyPatch(nameof(Terminal.SetItemSales))]
        static void Postfix(ref Item[] ___buyableItemsList)
        {
            var configEntries = Plugin.Instance.GetConfigEntries();
            foreach (var item in ___buyableItemsList) {
                if (configEntries.TryGetValue(item.name, out var configEntry)) {
                    item.creditsWorth = configEntry.Value;
                } else {
                    Plugin.Log.LogWarning($"No config entry found for {item.name}");
                }
            }
        }
    }
}
