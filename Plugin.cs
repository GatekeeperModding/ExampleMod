using BepInEx;
using BepInEx.Logging;
using ExampleMod.Content.ItemControllers;
using Gatekeeper.Items;
using Gatekeeper.Utility;
using GKAPI;
using GKAPI.Achievements;
using GKAPI.Difficulties;
using GKAPI.Items;
using GKAPI.Lang;
using UnityEngine;

namespace ExampleMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(GKAPI.Plugin.PluginGuid)]
public class Plugin : GkPlugin
{
    internal new static ManualLogSource Log;

    public override void Load()
    {
        base.Load();
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    public override void AddContent()
    {
        if (EventHandler.State != EventHandler.LoadingState.PreInit)
        {
            Log.LogError("Content has to be added during Pre-Init!");
            return;
        }

        GlobalSettings.Instance.buildCheating = true;
        GlobalSettings.Instance.buildDebugConsole = true;

        var achievementAPI = AchievementsAPI.Instance;
        var baseAchievement = achievementAPI.AddAchievement(new GkAchievement.Builder());
        
        var itemAPI = ItemAPI.Instance;
        var testItem = itemAPI.AddItem(new GkItem.Builder("Test Item", "Test item description", $"{ColorHelper.WrapInColor("{[Mod1_Lvl1]}% (+{[Mod1_Lvl2]}% per stack)", Colors.Red)} to critical damage.")
            .WithId("TEST")
            .SetUnlocked(false)
            .SetHidden(false)
            .AddModification(ItemParamModificationType.CritDamagePerc, 0.5f, 0.25f)
        );
        var cloverItem = itemAPI.AddItem(new GkItem.Builder("Clover", "Nothing", "You're feeling lucky today")
            .SetUnlocked(true)
            .SetHidden(false)
            .WithItemType(ItemType.RunesOfCreation)
            .WithDropSource(ItemDropSource.Obelisks | ItemDropSource.Pedestal | ItemDropSource.EndOfRound)
            .WithMaxCount(15)
            .AddModification(ItemParamModificationType.RiftmakerProcChancePerc, 0.02f, 0.02f)
            .AddModification(ItemParamModificationType.JujuReleaseProb, 0.02f, 0.02f)
            .AddModification(ItemParamModificationType.SootheStoneSpawnProb, 0.02f, 0.02f)
            .AddModification(ItemParamModificationType.GreedsGambitItemDropChance, 0.02f, 0.01f)
            .AddModification(ItemParamModificationType.ElixDropChance, 0.05f, 0.02f)
            .AddModification(ItemParamModificationType.FlameDraftDropChance, 0.05f, 0.02f)
            .AddModification(ItemParamModificationType.BaneSealProcChancePerc, 0.05f, 0.02f)
            .AddModification(ItemParamModificationType.CursedShardChancePerc, 0.05f, 0.02f)
            .AddModification(ItemParamModificationType.ResectoidChancePerc, 0.1f, 0.04f)
            .AddModification(ItemParamModificationType.InertiaChancePerc, 0.05f, 0.04f)
            .AddModification(ItemParamModificationType.HvcChancePerc, 0.1f, 0.04f)
        );
        var bobItem = itemAPI.AddItem("Bob");
        var testTriad = itemAPI.AddTriad("TestTriad", [ItemID.RuneOfRebound, ItemID.Triumph, testItem.GetItemID], builder => builder.SetUnlocked(true).SetHidden(false));
        itemAPI.AddItemController<TestTriadItemController>(testTriad.GetItemID);
        
        baseAchievement.AddItems([testItem.Info, bobItem.Info, cloverItem.Info]);

        var diffAPI = DifficultiesAPI.Instance;
        /*diffAPI.AddDifficulty(new GkDifficulty.Builder()
            .WithName("Guardian")
            .WithPercentageName("300%")
            .WithDifficultyMultiplier(0.6f)
            .WithPrismMultiplier(2f)
            .WithEventsMinLevel(0)
            .WithColors(new Color(0.1f, 0.1f, 0.5f), new Color(0.1f, 0.3f, 0.8f))
            .WithExpLoopPow(1.4f)
            .WithExpPoints(31, 180, 50)
        );*/
        diffAPI.AddDifficulty(new GkDifficulty.Builder()
            .WithName("Wtf is this")
            .WithPercentageName("1000%")
            .WithDifficultyMultiplier(2f)
            .WithPrismMultiplier(4f)
            .WithEventsMinLevel(0)
            .WithColors(new Color(0.4f, 0.1f, 0.8f), new Color(0.4f, 0.3f, 0.8f))
            .WithExpLoopPow(2f)
            .WithExpPoints(35, 210, 60)
            .WithArenaValues(0, 1.6f, 10, 1.6f, 0.8f, -40, 150)
        );
    }
}
