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
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.CoffeeCup)
            {
                tooltips.RemoveAll(t => t.Name == "Tooltip0");

                TooltipLine buffTooltip = tooltips.Find(t => t.Name == "Tooltip1");
                buffTooltip.Text = "Prevents rest loss.";

                TooltipLine descriptionLine = new TooltipLine(Mod, "CustomDescription", "'Hello darkness my old friend'");
                tooltips.Add(descriptionLine);
            }
        }
    }
}