using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Explosives
{
    public class Instavator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Instavator");
            Tooltip.SetDefault("Creates a hellevator instantly\nDo not use if any important building is below");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.width = 10;
            item.height = 32;
            item.maxStack = 99;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.value = Item.buyPrice(0, 0, 3);
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("InstaProj");
            item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Dynamite, 50);
            recipe.AddIngredient(ItemID.RopeCoil, 10);
            recipe.AddIngredient(ItemID.Torch, 99);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}