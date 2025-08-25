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
    public class Cigarette : ModItem
    {
        private int CigarettesSmoked;

        public override void SetStaticDefaults()
        {
            CigarettesSmoked = 0;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.DrinkLong;
            Item.useAnimation = 240;
            Item.useTime = 240;
            Item.shoot = ModContent.ProjectileType<CigaretteProjectile>();
            Item.shootSpeed = 1f;
            Item.useTurn = true;
            Item.UseSound = new SoundStyle("ChallengingTerrariaMod/Assets/Audio/CigaretteSFX"); ;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 50);
            Item.noUseGraphic = true;

            Item.buffTime = 120 * 60;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<ArmoredMind>(), 60 * 120);
            CigarettesSmoked++;
            if (CigarettesSmoked >= 50)
            {
                player.AddBuff(ModContent.BuffType<Cancer>(), 60 * 3000);
                Main.NewText("You haven't been smoking in moderation, have you?", Color.White);
                CigarettesSmoked = 0;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CigarettesSmoked", CigarettesSmoked);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CigarettesSmoked"))
            {
                CigarettesSmoked = tag.GetInt("CigarettesSmoked");
            }
            else
            {
                CigarettesSmoked = 0; 
            }
        }
    }

    public class CigarettePlayer : ModPlayer
    {
        public int timeLeft_cancer = 0;

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            for (int i = 0; i < Player.buffType.Length; i++)
            {
                if (Player.buffType[i] == ModContent.BuffType<Cancer>())
                {
                    timeLeft_cancer = Player.buffTime[i];
                    break;
                }
            }
        }
        public override void OnRespawn()
        {
            Player.AddBuff(ModContent.BuffType<Cancer>(), timeLeft_cancer);
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            if (Player.HasBuff(ModContent.BuffType<Cancer>()))
            {
                price += Item.buyPrice(platinum: 10);
            }
        }
    }
}