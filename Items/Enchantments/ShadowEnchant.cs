using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fargowiltas.Items.Enchantments
{
	public class ShadowEnchant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Enchantment");
			Tooltip.SetDefault("'You feel your body slip into the deepest shadows' \n15% increased movement speed \n10% increased melee speed \nAttacks have a chance to inflict shadow flame \nSummons a Baby Eater of Souls, turn vanity off to despawn it");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
			item.rare = 1; 
			item.value = 20000; 
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
        {	
			FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>(mod);
			modPlayer.shadowEnchant = true;
			
			player.moveSpeed += 15f;
			player.meleeSpeed += 0.1f;
			
			if (player.whoAmI == Main.myPlayer)
            {
				if(!hideVisual)
				{
					modPlayer.shadowPet = true;
					
					if(player.FindBuffIndex(45) == -1)
					{
						if (player.ownedProjectileCounts[ProjectileID.BabyEater] < 1)
						{
							Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ProjectileID.BabyEater, 0, 2f, Main.myPlayer, 0f, 0f);
						}
					}
				}
				else
				{
						modPlayer.shadowPet = false;
				}
            }
        }
		
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShadowHelmet);
			recipe.AddIngredient(ItemID.ShadowScalemail);
			recipe.AddIngredient(ItemID.ShadowGreaves);
			recipe.AddIngredient(ItemID.LightlessChasms);
			recipe.AddIngredient(ItemID.EatersBone);
			
			recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
		
	




