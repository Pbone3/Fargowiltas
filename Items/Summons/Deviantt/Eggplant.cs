using Terraria;
using Terraria.ID;

namespace Fargowiltas.Items.Summons.Deviantt
{
    public class Eggplant : BaseSummon
    {
        public override int NPCType => NPCID.DoctorBones;

        public override string NPCName => "Doctor Bones";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eggplant");
            Tooltip.SetDefault("Summons Doctor Bones" +
                               "\nOnly usable at night or underground");
        }

        public override bool CanUseItem(Player player) => !Main.dayTime || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;
    }
}