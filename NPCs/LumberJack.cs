using System.Collections.Generic;
using System.Linq;
using Fargowiltas.Items.Tiles;
using Fargowiltas.Items.Vanity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas.NPCs
{
    [AutoloadHead]
    public class LumberJack : ModNPC
    {
        private bool dayOver;
        private bool nightOver;

        public override bool Autoload(ref string name)
        {
            name = "LumberJack";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LumberJack");
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 40;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Guide;

            if (GetInstance<FargoConfig>().CatchNPCs)
            {
                Main.npcCatchable[npc.type] = true;
                npc.catchItem = (short)mod.ItemType("LumberJack");
            }
        }

        public override bool CanTownNPCSpawn(int numTownnpcs, int money)
        {
            return GetInstance<FargoConfig>().Lumber && FargoWorld.MovedLumberjack;
        }

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void AI()
        {
            if (!Main.dayTime)
            {
                nightOver = true;
            }

            if (Main.dayTime)
            {
                dayOver = true;
            }
        }

        public override string TownNPCName()
        {
            string[] names = { "Griff", "Jack", "Bruce", "Larry", "Will", "Jerry", "Liam", "Stan", "Lee", "Woody", "Leif", "Paul" };
            return Main.rand.Next(names);
        }

        public override string GetChat()
        {
            List<string> dialogue = new List<string>
            {
                "Dynasty wood? Between you and me, that stuff ain't real wood!",
                "Sure cactus isn't wood, but I can still chop it with me trusty axe.",
                "You wouldn't by chance have any fantasies about me... right?",
                "I eat a bowl of woodchips for breakfast... without any milk.",
                "TIIIIIIIIIMMMBEEEEEEEERRR!",
                "I'm a lumberjack and I'm okay, I sleep all night and I work all day!",
                "You won't ever need an axe again with me around.",
                "I have heard of people cutting trees with fish, who does that?",
                "You wanna see me work without my shirt on? Maybe in 2030.",
                "You ever seen the world tree?",
                "You want what? ...Sorry that's not the kind of wood I'm selling.",
                "Why don't I sell acorns? ...I replant all the trees I chop, don't you?",
                "What's the best kind of tree? ... Any if I can chop it.",
                "Can I axe you a question?",
                "Might take a nap under a tree later, care to join me?",
                "I'm an expert in all wood types.",
                "I wonder if there'll be more trees to chop in 1.4.",
                "Red is one of my favourite colors, right after wood.",
                "It's always flannel season.",
                "I'm glad my wood put such a big smile on your face."
            };

            int dryad = NPC.FindFirstNPC(NPCID.Dryad);
            if (dryad >= 0)
            {
                dialogue.Add($"{Main.npc[dryad].GivenName} told me to start hugging trees... I hug trees with my chainsaw.");
            }

            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0)
            {
                dialogue.Add($"I always see {Main.npc[nurse].GivenName} looking at my biceps when I'm working. Wonder if she wants some of my wood.");
            }

            if (Fargowiltas.ModLoaded["ThoriumMod"])
            {
                dialogue.Add("Astroturf? Sorry I only grow trees on real grass.");
                dialogue.Add("Yew tree? Sakura tree? Nope, haven't found any.");
            }

            return Main.rand.Next(dialogue);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Tree Treasures";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            Player player = Main.LocalPlayer;

            if (firstButton)
            {
                shop = true;
                return;
            }

            if (dayOver && nightOver)
            {
                string quote = "";

                if (player.ZoneDesert && !player.ZoneBeach)
                {
                    quote = "While I was chopping down a cactus these things leaped at me, why don't you take care of them?";
                    player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.Scorpion, ItemID.BlackScorpion }), 5);
                    player.QuickSpawnItem(ItemID.Cactus, 50);
                }
                else if (player.ZoneJungle)
                {
                    quote = "These mahogany trees are full of life, but a tree only has one purpose: to be chopped. Oh yea this fell out of the last one.";
                    player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.Buggy, ItemID.Sluggy, ItemID.Grubby, ItemID.Frog }), 5);
                    player.QuickSpawnItem(ItemID.RichMahogany, 50);
                    //add mango and pineapple
                }
                else if (player.ZoneHoly)
                {
                    quote = "This place is a bit fanciful for my tastes, but the wood's as choppable as any. Nighttime has these cool bugs though, take a few.";
                    player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.LightningBug }), 5);
                    player.QuickSpawnItem(ItemID.Pearlwood, 50);
                    //add fairies
                    //add star fruit and dragonfruit

                    //add prismatic lacewing if post plantera
                }
                else if (player.ZoneGlowshroom && Main.hardMode)
                {
                    quote = "Whatever causes these to glow is beyond me, you're probably gonna eat them anyway so have this while you're at it.";
                    player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.GlowingSnail, ItemID.TruffleWorm }), 5);
                    player.QuickSpawnItem(ItemID.GlowingMushroom, 50);
                    //add mushroom grass seeds

                }
                else if (player.ZoneCorrupt || player.ZoneCrimson)
                {
                    quote = "The trees here are probably the toughest in this branch of reality.. Sorry, just tree puns, I haven't found anything interesting here.";
                    //add elderberry and blackcurrant for corrupt
                    //add blood orange and rambutan to crimson
                }
                //plum or cherry, penguin
                else if (player.ZoneSnow)
                {
                    quote = "This neck of the woods is pretty eh? Here I've got some of my favorite wood for you.";

                    player.QuickSpawnItem(ItemID.BorealWood, 50);
                }
                else if (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)
                {
                    //add underground dialogue, he gives you gems and gem critters

                    //move current dialogue to underworld


                    quote = "I looked around here for a while and didn't find any trees. I did find this little thing though. Maybe you'll want it?";
                    player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.Snail }));
                    //add gem tree thing
                }
                //purity, most common option likely
                else// if (player.position.Y > Main.worldSurface)
                {
                    if (Main.dayTime)
                    {
                        //butterfly
                        if (Main.rand.Next(2) == 0)
                        {
                            quote = "Back in the day, people used to forge butterflies into powerful gear. We try to forget those days... but here have one.";
                            player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly, ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly }), 5);
                        }
                        else
                        {
                            quote = "These little critters are always falling out of the trees I cut down. Maybe you can find a use for them?";
                            player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.Grasshopper, ItemID.Squirrel, ItemID.SquirrelRed, ItemID.Bird, ItemID.BlueJay, ItemID.Cardinal }), 5);
                        }

                        //add fruit option
                        //apple, peach, apricot, grapefruit, lemon
                        //add eucalyptus sap as some rare meme
                        //add ladybug if its windy
                        //add rat if in graveyard
                    }
                    else
                    {
                        quote = "Chopping trees at night is always relaxing... well except for the flying eyeballs. Have one of these little guys to keep you company.";
                        player.QuickSpawnItem(Main.rand.Next(new int[] { ItemID.Firefly }));
                    }

                    player.QuickSpawnItem(ItemID.Wood, 50);
                }
                //add beach
                //drop seagull
                //drop coconut or banana
               

                Main.npcChatText = quote;
                dayOver = false;
                nightOver = false;
            }
            else
            {
                Main.npcChatText = "I'm resting after a good day of chopping, come back tomorrow and maybe I'll have something else for you.";
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.WoodPlatform);
            shop.item[nextSlot].shopCustomPrice = 5;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Wood);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.BorealWood);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.RichMahogany);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.PalmWood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Ebonwood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Shadewood);
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;

            if (Fargowiltas.ModLoaded["CrystiliumMod"])
            {
                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("CrystiliumMod").ItemType("CrystalWood"));
                shop.item[nextSlot].shopCustomPrice = 20;
                nextSlot++;
            }

            if (ModLoader.GetMod("CosmeticVariety") != null && NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("CosmeticVariety").ItemType("Starwood"));
                shop.item[nextSlot].shopCustomPrice = 20;
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(ItemID.Pearlwood);
            shop.item[nextSlot].shopCustomPrice = 20;
            nextSlot++;

            if (NPC.downedHalloweenKing)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SpookyWood);
                shop.item[nextSlot].shopCustomPrice = 50;
                nextSlot++;
            }

            if (Fargowiltas.ModLoaded["Redemption"])
            {
                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("Redemption").ItemType("AncientWood"));
                shop.item[nextSlot].shopCustomPrice = 20;
                nextSlot++;
            }

            if (Fargowiltas.ModLoaded["AAMod"])
            {
                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAmod").ItemType("Razewood"));
                shop.item[nextSlot].shopCustomPrice = 50;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAmod").ItemType("Bogwood"));
                shop.item[nextSlot].shopCustomPrice = 50;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModLoader.GetMod("AAmod").ItemType("OroborosWood"));
                shop.item[nextSlot].shopCustomPrice = 50;
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(ItemID.Cactus);
            shop.item[nextSlot].shopCustomPrice = 10;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.LivingWoodWand);
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackMask>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackBody>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<LumberjackPants>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(mod.ItemType("LumberJaxe"));
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.SharpeningStation);
            shop.item[nextSlot].shopCustomPrice = 100000;
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<WoodenToken>());
            shop.item[nextSlot].shopCustomPrice = 10000;
            nextSlot++;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("LumberJaxe");
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void NPCLoot()
        {
            FargoWorld.MovedLumberjack = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, Scale: 0.8f);
                }

                Vector2 pos = npc.position + new Vector2(Main.rand.Next(npc.width - 8), Main.rand.Next(npc.height / 2));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/LumberGore3"));

                pos = npc.position + new Vector2(Main.rand.Next(npc.width - 8), Main.rand.Next(npc.height / 2));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/LumberGore2"));

                pos = npc.position + new Vector2(Main.rand.Next(npc.width - 8), Main.rand.Next(npc.height / 2));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/LumberGore1"));
            }
            else
            {
                for (int k = 0; k < damage / npc.lifeMax * 50.0; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, Scale: 0.6f);
                }
            }
        }
    }
}
