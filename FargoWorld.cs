using Fargowiltas.NPCs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Fargowiltas
{
    public class FargoWorld : ModWorld
    {
        internal static int AbomClearCD;
        internal static bool MovedLumberjack;
        internal static int WoodChopped;

        internal static bool OverloadGoblins;
        internal static bool OverloadPirates;
        internal static bool OverloadPumpkinMoon;
        internal static bool OverloadFrostMoon;
        internal static bool OverloadMartians;
        internal static bool OverloadedSlimeRain;

        internal static bool[] CurrentSpawnRateTile;
        internal static Dictionary<string, bool> DownedBools = new Dictionary<string, bool>();

        // Do not change the order or name of any of these value names, it will fuck up loading. Any new additions should be added at the end.
        private readonly string[] tags = new string[]
        {
            "lumberjack",
            "betsy",
            "boss",
            "rareEnemy",
            "pinky",
            "undeadMiner",
            "tim",
            "doctorBones",
            "mimic",
            "wyvern",
            "runeWizard",
            "nymph",
            "moth",
            "rainbowSlime",
            "paladin",
            "medusa",
            "clown",
            "iceGolem",
            "sandElemental",
            "mothron",
            "mimicHallow",
            "mimicCorrupt",
            "mimicCrimson",
            "mimicJungle",
            "goblinSummoner",
            "flyingDutchman",
            "dungeonSlime",
            "pirateCaptain",
            "skeletonGun",
            "skeletonMage",
            "boneLee",
            "darkMage",
            "ogre",
            "headlessHorseman",
            "babyGuardian", 
            "squirrel",
            "worm"
        };

        public override void Initialize()
        {
            foreach (string tag in tags)
            {
                DownedBools[tag] = false;
            }

            AbomClearCD = 0;
            WoodChopped = 0;

            OverloadGoblins = false;
            OverloadPirates = false;
            OverloadPumpkinMoon = false;
            OverloadFrostMoon = false;
            OverloadMartians = false;
            OverloadedSlimeRain = false;

            CurrentSpawnRateTile = new bool[Main.netMode == NetmodeID.Server ? 255 : 1];
        }

        public override TagCompound Save()
        {
            List<string> downed = new List<string>();
            foreach (string tag in tags)
            {
                downed.AddWithCondition(tag, DownedBools[tag]);
            }

            return new TagCompound
            {
                { "downed", downed },
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("downed");
            foreach (string downedTag in tags)
            {
                DownedBools[downedTag] = downed.Contains(downedTag);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            foreach (string tag in tags)
            {
                DownedBools[tag] = reader.ReadBoolean();
            }

            AbomClearCD = reader.ReadInt32();
            WoodChopped = reader.ReadInt32();
            Fargowiltas.SwarmActive = reader.ReadBoolean();
        }

        public override void NetSend(BinaryWriter writer)
        {
            foreach (string tag in tags)
            {
                writer.Write(DownedBools[tag]);
            }

            writer.Write(AbomClearCD);
            writer.Write(WoodChopped);
            writer.Write(Fargowiltas.SwarmActive);
        }

        public override void PostUpdate()
        {
            // seasonals
            Main.halloween = GetInstance<FargoConfig>().Halloween;
            Main.xMas = GetInstance<FargoConfig>().Christmas;

            // swarm reset in case something goes wrong
            if (Main.netMode != NetmodeID.MultiplayerClient && Fargowiltas.SwarmActive 
                && NoBosses() && !NPC.AnyNPCs(NPCID.EaterofWorldsHead) && !NPC.AnyNPCs(NPCID.DungeonGuardian) && !NPC.AnyNPCs(NPCID.DD2DarkMageT1))
            {
                Fargowiltas.SwarmActive = false;
                FargoGlobalNPC.LastWoFIndex = -1;
                FargoGlobalNPC.WoFDirection = 0;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData);
            }

            if (AbomClearCD > 0)
            {
                AbomClearCD--;
            }

            if (OverloadGoblins && Main.invasionType != InvasionID.GoblinArmy)
            {
                OverloadGoblins = false;
            }

            if (OverloadPirates && Main.invasionType != InvasionID.PirateInvasion)
            {
                OverloadPirates = false;
            }

            if (OverloadPumpkinMoon && !Main.pumpkinMoon)
            {
                OverloadPumpkinMoon = false;
            }

            if (OverloadFrostMoon && !Main.snowMoon)
            {
                OverloadFrostMoon = false;
            }

            if (OverloadMartians && Main.invasionType != InvasionID.MartianMadness)
            {
                OverloadMartians = false;
            }

            if (OverloadedSlimeRain && !Main.slimeRain)
            {
                OverloadedSlimeRain = false;
            }
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            ref bool current = ref CurrentSpawnRateTile[0];
            bool oldSpawnRateTile = current;
            current = tileCounts[mod.TileType("RegalStatueSheet")] > 0;

            if (Main.netMode == NetmodeID.MultiplayerClient && current != oldSpawnRateTile)
            {
                ModPacket packet = ModContent.GetInstance<Fargowiltas>().GetPacket();
                packet.Write((byte)1);
                packet.Write(current);
                packet.Send();
            }
        }

        public override void PreUpdate()
        {
            bool rate = false;
            for (int i = 0; i < CurrentSpawnRateTile.Length; i++)
            {
                if (CurrentSpawnRateTile[i])
                {
                    Player player = Main.player[i];
                    if (player.active)
                    {
                        if (!player.dead)
                        {
                            rate = true;
                        }
                    }
                    else
                    {
                        CurrentSpawnRateTile[i] = false;
                    }
                }
            }

            if (rate)
            {
                Main.checkForSpawns += 81;
            }
        }

        private bool NoBosses() => Main.npc.All(i => !i.active || !i.boss);
    }
}