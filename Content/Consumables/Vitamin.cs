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
    public class Vitamin : ModItem
    {
        public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.IsFood[Type] = true; 
		}

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(silver: 50);
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item3;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.buffType = ModContent.BuffType<Nourished>();
            Item.consumable = true;
            Item.buffTime = 60 * 120;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(silver: 50);
        }
    }
}