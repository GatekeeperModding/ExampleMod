using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using ExampleMod.Content.ItemControllers;
using Gatekeeper.Items;
using Gatekeeper.PoolScripts;
using Gatekeeper.Utility;
using Gatekeeper.Utility.SceneHelper;
using GKAPI;
using GKAPI.Achievements;
using GKAPI.Difficulties;
using GKAPI.Enemies;
using GKAPI.Items;
using GKAPI.Lang;
using RNGNeeds;
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
        var (gd, diff) = diffAPI.AddDifficulty(new GkDifficulty.Builder()
            .WithName("Sentinel")
            .WithDifficultyValues("500%", 2f, 4f)
            .WithGameEventsData(0, 2, 1, diffAPI.CreateGameEventsProbabilities(0.5f))
            .WithSirenSpawnData(5, 10)
            .WithEliteSpawnData(0.2f, 0.08f, 0.5f, 0.09f, 7, 3, 2)
            .WithInstabilityCapsuleSpeed(3)
            .WithColors(new Color(0.4f, 0.1f, 0.8f), new Color(0.4f, 0.3f, 0.8f))
            .WithExpLoopPow(2f)
            .WithExpPoints(35, 210, 60)
            .WithArenaValues(1.6f, 10, 1.6f, 0.8f, -40, 150)
        );

        var enemiesAPI = EnemiesAPI.Instance;
        var probAurora1Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyGrouch, 0.03f)
            .AddItem(PoolItemID.EnemyVice, 0.05f)
            .AddItem(PoolItemID.EnemyLumer, 0.19f)
            .AddItem(PoolItemID.EnemySentry, 0.3f)
            .AddItem(PoolItemID.EnemyMount, 0.3f)
            .AddItem(PoolItemID.EnemyLucidEthel, 0.04f)
            .AddItem(PoolItemID.EnemyKeeperS1, 0.01f)
            .Build();
        var probAurora2Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyGrouch, 0.01f)
            .AddItem(PoolItemID.EnemyVice, 0.02f)
            .AddItem(PoolItemID.EnemyLumer, 0.18f)
            .AddItem(PoolItemID.EnemySentry, 0.33f)
            .AddItem(PoolItemID.EnemyMount, 0.35f)
            .AddItem(PoolItemID.EnemyLucidEthel, 0.10f)
            .AddItem(PoolItemID.EnemyKeeperS1, 0.01f)
            .Build();
        var probAurora3Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyGrouch, 0.01f)
            .AddItem(PoolItemID.EnemyVice, 0.02f)
            .AddItem(PoolItemID.EnemyLumer, 0.17f)
            .AddItem(PoolItemID.EnemySentry, 0.31f)
            .AddItem(PoolItemID.EnemyMount, 0.33f)
            .AddItem(PoolItemID.EnemyLucidEthel, 0.15f)
            .AddItem(PoolItemID.EnemyKeeperS1, 0.01f)
            .Build();

        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AuroraScenes, new Vector2Int(1, 5), probAurora1Loop, "EST_1XX 1Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AuroraScenes, new Vector2Int(6, 11), probAurora2Loop, "EST_1XX 2Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AuroraScenes, new Vector2Int(12, -1), probAurora3Loop, "EST_1XX 3Loop_500");

        var probCelestium1Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyKinetic, 0.15f)
            .AddItem(PoolItemID.EnemyWindsHerald, 0.2f)
            .AddItem(PoolItemID.EnemyHeartless, 0.24f)
            .AddItem(PoolItemID.EnemyCrystalGuard, 0.18f)
            .AddItem(PoolItemID.EnemyMaluard, 0.19f)
            .AddItem(PoolItemID.EnemyInitiator, 0.04f)
            .Build();
        var probCelestium2Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyKinetic, 0.06f)
            .AddItem(PoolItemID.EnemyWindsHerald, 0.08f)
            .AddItem(PoolItemID.EnemyHeartless, 0.1f)
            .AddItem(PoolItemID.EnemyCrystalGuard, 0.35f)
            .AddItem(PoolItemID.EnemyMaluard, 0.30f)
            .AddItem(PoolItemID.EnemyInitiator, 0.10f)
            .AddItem(PoolItemID.EnemyIdol, 0.01f)
            .Build();
        var probCelestium3Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyKinetic, 0.04f)
            .AddItem(PoolItemID.EnemyWindsHerald, 0.06f)
            .AddItem(PoolItemID.EnemyHeartless, 0.06f)
            .AddItem(PoolItemID.EnemyCrystalGuard, 0.35f)
            .AddItem(PoolItemID.EnemyMaluard, 0.33f)
            .AddItem(PoolItemID.EnemyInitiator, 0.15f)
            .AddItem(PoolItemID.EnemyIdol, 0.01f)
            .Build();
        
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.CelestiumScenes, new Vector2Int(1, 5), probCelestium1Loop, "EST_2XX 1Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.CelestiumScenes, new Vector2Int(6, 11), probCelestium2Loop, "EST_2XX 2Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.CelestiumScenes, new Vector2Int(12, -1), probCelestium3Loop, "EST_2XX 3Loop_500");
        
        var probPurgatory1Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyDoomed, 0.17f)
            .AddItem(PoolItemID.EnemyDeadrunner, 0.12f)
            .AddItem(PoolItemID.EnemyEidolon, 0.12f)
            .AddItem(PoolItemID.EnemyHermit, 0.16f)
            .AddItem(PoolItemID.EnemyFiendSkull, 0.19f)
            .AddItem(PoolItemID.EnemyGrenadier, 0.20f)
            .AddItem(PoolItemID.EnemyDamnedSpellcaster, 0.04f)
            .Build();
        var probPurgatory2Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyDoomed, 0.03f)
            .AddItem(PoolItemID.EnemyDeadrunner, 0.08f)
            .AddItem(PoolItemID.EnemyEidolon, 0.09f)
            .AddItem(PoolItemID.EnemyHermit, 0.19f)
            .AddItem(PoolItemID.EnemyFiendSkull, 0.28f)
            .AddItem(PoolItemID.EnemyGrenadier, 0.16f)
            .AddItem(PoolItemID.EnemyDamnedSpellcaster, 0.17f)
            .Build();
        var probPurgatory3Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyDeadrunner, 0.08f)
            .AddItem(PoolItemID.EnemyEidolon, 0.09f)
            .AddItem(PoolItemID.EnemyHermit, 0.15f)
            .AddItem(PoolItemID.EnemyFiendSkull, 0.28f)
            .AddItem(PoolItemID.EnemyGrenadier, 0.20f)
            .AddItem(PoolItemID.EnemyDamnedSpellcaster, 0.20f)
            .Build();
        
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PurgatoryScenes, new Vector2Int(1, 5), probPurgatory1Loop, "EST_3XX 1Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PurgatoryScenes, new Vector2Int(6, 11), probPurgatory2Loop, "EST_3XX 2Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PurgatoryScenes, new Vector2Int(12, -1), probPurgatory3Loop, "EST_3XX 3Loop_500");
        
        var probAridune1Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyRattle, 0.16f)
            .AddItem(PoolItemID.EnemyToxic, 0.17f)
            .AddItem(PoolItemID.EnemyThorn, 0.12f)
            .AddItem(PoolItemID.EnemyDrought, 0.18f)
            .AddItem(PoolItemID.EnemyHowler, 0.08f)
            .AddItem(PoolItemID.EnemyWanderer, 0.25f)
            .AddItem(PoolItemID.EnemyQuake, 0.04f)
            .Build();
        var probAridune2Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyRattle, 0.04f)
            .AddItem(PoolItemID.EnemyToxic, 0.09f)
            .AddItem(PoolItemID.EnemyThorn, 0.09f)
            .AddItem(PoolItemID.EnemyDrought, 0.28f)
            .AddItem(PoolItemID.EnemyHowler, 0.10f)
            .AddItem(PoolItemID.EnemyWanderer, 0.30f)
            .AddItem(PoolItemID.EnemyQuake, 0.10f)
            .Build();
        var probAridune3Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyRattle, 0.03f)
            .AddItem(PoolItemID.EnemyToxic, 0.08f)
            .AddItem(PoolItemID.EnemyThorn, 0.09f)
            .AddItem(PoolItemID.EnemyDrought, 0.28f)
            .AddItem(PoolItemID.EnemyHowler, 0.09f)
            .AddItem(PoolItemID.EnemyWanderer, 0.28f)
            .AddItem(PoolItemID.EnemyQuake, 0.15f)
            .Build();
        
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AriduneScenes, new Vector2Int(1, 5), probAridune1Loop, "EST_4XX 1Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AriduneScenes, new Vector2Int(6, 11), probAridune2Loop, "EST_4XX 2Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.AriduneScenes, new Vector2Int(12, -1), probAridune3Loop, "EST_4XX 3Loop_500");
        
        var probPalium1Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyMourn, 0.18f)
            .AddItem(PoolItemID.EnemyCaveCreeper, 0.12f)
            .AddItem(PoolItemID.EnemyGnash, 0.09f)
            .AddItem(PoolItemID.EnemyAshling, 0.15f)
            .AddItem(PoolItemID.EnemyCaldeon, 0.12f)
            .AddItem(PoolItemID.EnemyHulk, 0.15f)
            .AddItem(PoolItemID.EnemyEmberling, 0.15f)
            .AddItem(PoolItemID.EnemyInferna, 0.04f)
            .Build();
        var probPalium2Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyMourn, 0.11f)
            .AddItem(PoolItemID.EnemyCaveCreeper, 0.12f)
            .AddItem(PoolItemID.EnemyGnash, 0.09f)
            .AddItem(PoolItemID.EnemyAshling, 0.20f)
            .AddItem(PoolItemID.EnemyCaldeon, 0.12f)
            .AddItem(PoolItemID.EnemyHulk, 0.13f)
            .AddItem(PoolItemID.EnemyEmberling, 0.13f)
            .AddItem(PoolItemID.EnemyInferna, 0.10f)
            .Build();
        var probPalium3Loop = new TemplateHelper.ProbListBuilder()
            .AddItem(PoolItemID.EnemyMourn, 0.03f)
            .AddItem(PoolItemID.EnemyCaveCreeper, 0.04f)
            .AddItem(PoolItemID.EnemyGnash, 0.08f)
            .AddItem(PoolItemID.EnemyAshling, 0.30f)
            .AddItem(PoolItemID.EnemyCaldeon, 0.12f)
            .AddItem(PoolItemID.EnemyHulk, 0.12f)
            .AddItem(PoolItemID.EnemyEmberling, 0.13f)
            .AddItem(PoolItemID.EnemyInferna, 0.18f)
            .Build();
        
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PaliumScenes, new Vector2Int(1, 5), probPalium1Loop, "EST_5XX 1Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PaliumScenes, new Vector2Int(6, 11), probPalium2Loop, "EST_5XX 2Loop_500");
        enemiesAPI.AddKnowingTemplate(gd, true, TemplateHelper.PaliumScenes, new Vector2Int(12, -1), probPalium3Loop, "EST_5XX 3Loop_500");
    }
}
