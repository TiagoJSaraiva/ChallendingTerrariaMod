using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.Audio;
using ChallengingTerrariaMod.Content.Projectiles;
using Terraria.ModLoader.IO;

namespace ChallengingTerrariaMod.Content.Consumables
{
    public class AdrenalineInjection : ModItem
    {
        public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;
		}

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(silver: 50);
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = null;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.buffType = ModContent.BuffType<ArmoredMind>();
            Item.consumable = true;
            Item.buffTime = 60 * 360;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(gold: 1);
        }
    }
}