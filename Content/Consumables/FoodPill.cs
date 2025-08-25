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
    public class FoodPill : ModItem
    {
        public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
            ItemID.Sets.FoodParticleColors[Item.type] = new Color[]
            {
                new Color(150, 93, 95),  // Marrom avermelhado
                new Color(192, 114, 76), // Marrom alaranjado
                new Color(140, 80, 80),  // Marrom escuro
                new Color(200, 120, 80), // Marrom claro
            };
			ItemID.Sets.IsFood[Type] = true; 
		}

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(silver: 50);
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.buffType = ModContent.BuffType<Nourished>();
            Item.consumable = true;
            Item.buffTime = 60 * 360;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(gold: 1);
        }
    }
}