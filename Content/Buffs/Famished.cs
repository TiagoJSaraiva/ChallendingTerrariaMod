// ChallengingTerrariaMod/Content/Buffs/Famished.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Famished : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Famished");
            // Description.SetDefault("You're starving! Significant reduction to mining speed, damage, and critical strike chance. You take damage over time and cannot naturally regenerate life.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.pickSpeed += 0.25f;
            if (player.statDefense - 10f == 0)
            {
                player.statDefense -= player.statDefense;
            }
            else
            {
                player.statDefense -= 10;
            }

            player.GetDamage(DamageClass.Generic) -= 0.15f; // 15% de redução de dano
            player.GetCritChance(DamageClass.Generic) -= 10; // 10% de redução de chance de crítico

            player.lifeRegen = 0;
            player.lifeRegenTime = 0;
        }
    }
}