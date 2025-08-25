using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Projectiles;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameContent.ItemDropRules;
using ChallengingTerrariaMod.Content.Consumables;
using System.Linq;

namespace ChallengingTerrariaMod.Content.GlobalChanges
{
    public class GlobalItemChanges : GlobalItem
    {

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.CoffeeCup)
            {
                item.buffType = ModContent.BuffType<Cafeinated>();
                item.buffTime = 120 * 60;
                item.rare = ItemRarityID.Green;
                item.consumable = true;
                item.value = Item.buyPrice(silver: 50);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.CoffeeCup)
            {
                tooltips.RemoveAll(t => t.Name == "Tooltip0");

                TooltipLine buffTooltip = tooltips.Find(t => t.Name == "Tooltip1");
                buffTooltip.Text = "Retards rest loss.";

                TooltipLine descriptionLine = new TooltipLine(Mod, "CustomDescription", "'Hello darkness my old friend'");
                tooltips.Add(descriptionLine);
            }

            if (item.type == ItemID.WarmthPotion)
            {
                TooltipLine descriptionLine = new TooltipLine(Mod, "CustomDescription", "Warms your body");
                tooltips.Add(descriptionLine);
            }

            if (item.type == ItemID.ObsidianSkinPotion)
            {
                TooltipLine descriptionLine = new TooltipLine(Mod, "CustomDescription", "Cools your body");
                tooltips.Add(descriptionLine);
            }
        }
    }

    public class GlobalNPC_Changes : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.SkeletonCommando)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cigarette>(), 30, 1, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AdrenalineInjection>(), 30, 1, 1));
            }
            if (npc.type == NPCID.UndeadViking)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cigarette>(), 50, 1, 1));
            }
            if (npc.type == NPCID.SnowFlinx)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cigarette>(), 50, 1, 1));
            }
            if (npc.type == NPCID.ArmoredViking)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cigarette>(), 50, 1, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AdrenalineInjection>(), 50, 1, 1));
            }



            if (npc.type == NPCID.Necromancer)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vitamin>(), 30, 1, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FoodPill>(), 30, 1, 1));
            }
            if (npc.type == NPCID.DesertGhoul)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vitamin>(), 50, 1, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FoodPill>(), 50, 1, 1));
            }
        
            if (npc.type == NPCID.WalkingAntlion)
            {

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vitamin>(), 50, 1, 1));
            }
            if (npc.type == NPCID.FlyingAntlion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Vitamin>(), 50, 1, 1));
            }
            

            if (npc.type == NPCID.AngryTrapper)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CaffeinePill>(), 50, 1, 1));
            }
            if (npc.type == NPCID.BoneLee)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CaffeinePill>(), 30, 1, 1));
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            int Item1;
            int Item2;

            if (!Condition.Hardmode.IsMet())
            {
                Item1 = Main.rand.Next(1, 4);
                do
                {
                    Item2 = Main.rand.Next(1, 4);
                } while (Item1 == Item2);

                switch (Item1)
                {
                    case 1:
                        shop[nextSlot] = ModContent.ItemType<Cigarette>();
                        nextSlot++; break;
                    case 2:
                        shop[nextSlot] = ModContent.ItemType<Vitamin>();
                        nextSlot++; break;
                    case 3:
                        shop[nextSlot] = ItemID.CoffeeCup;
                        nextSlot++; break;
                }

                switch (Item2)
                {
                    case 1:
                        shop[nextSlot] = ModContent.ItemType<Cigarette>();
                        nextSlot++; break;
                    case 2:
                        shop[nextSlot] = ModContent.ItemType<Vitamin>();
                        nextSlot++; break;
                    case 3:
                        shop[nextSlot] = ItemID.CoffeeCup;
                        nextSlot++; break;
                }
            }
            else
            {
                Item1 = Main.rand.Next(1, 7);
                do
                {
                    Item2 = Main.rand.Next(1, 7);
                } while (Item1 == Item2);

                switch (Item1)
                {
                    case 1:
                        shop[nextSlot] = ModContent.ItemType<Cigarette>();
                        nextSlot++; break;
                    case 2:
                        shop[nextSlot] = ModContent.ItemType<Vitamin>();
                        nextSlot++; break;
                    case 3:
                        shop[nextSlot] = ItemID.CoffeeCup;
                        nextSlot++; break;
                    case 4:
                        shop[nextSlot] = ModContent.ItemType<CaffeinePill>();
                        nextSlot++; break;
                    case 5:
                        shop[nextSlot] = ModContent.ItemType<FoodPill>();
                        nextSlot++; break;
                    case 6: 
                        shop[nextSlot] = ModContent.ItemType<AdrenalineInjection>();
                        nextSlot++; break;
                }

                switch (Item2)
                {
                    case 1:
                        shop[nextSlot] = ModContent.ItemType<Cigarette>();
                        nextSlot++; break;
                    case 2:
                        shop[nextSlot] = ModContent.ItemType<Vitamin>();
                        nextSlot++; break;
                    case 3:
                        shop[nextSlot] = ItemID.CoffeeCup;
                        nextSlot++; break;
                    case 4:
                        shop[nextSlot] = ModContent.ItemType<CaffeinePill>();
                        nextSlot++; break;
                    case 5:
                        shop[nextSlot] = ModContent.ItemType<FoodPill>();
                        nextSlot++; break;
                    case 6: 
                        shop[nextSlot] = ModContent.ItemType<AdrenalineInjection>();
                        nextSlot++; break;
                }
            }
        }
    }
}