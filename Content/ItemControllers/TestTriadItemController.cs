using Gatekeeper.CameraScripts.HUD.SkillPanelStuff;
using Gatekeeper.General.Events.Characters;
using GKAPI.Items;

namespace ExampleMod.Content.ItemControllers;

public class TestTriadItemController : CustomItemController
{
    public override void ClientHandleSkillUsed(EventClientCharacterSkillUsed eventData)
    {
        if (IsItemInInventory && eventData.SkillType == SkillType.Third)
        {
            Plugin.Log.LogInfo("TestTriadItemController::HandleThirdSkillUsed");
        }
    }
}