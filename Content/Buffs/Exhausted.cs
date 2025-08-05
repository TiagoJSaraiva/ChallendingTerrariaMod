// Em ChallengingTerrariaMod/Content/Buffs/Exhausted.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID; // Para BuffID.Confused, BuffID.Drunk

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Exhausted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Exhausted");
            // Description.SetDefault("You are extremely exhausted. Magic and ranged damage severely reduced and mana regen is stopped. Your body don't obey you anymore.");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Redução de dano
            player.GetDamage(DamageClass.Magic) -= 0.35f; // -35% de dano mágico
            player.GetDamage(DamageClass.Ranged) -= 0.40f; // -40% de dano ranged

            // Mana regen parada
            player.manaRegen = 0; // Zera a regeneração de mana
            player.manaRegenBonus = 0; // Garante que não há bônus
            player.manaRegenDelay = 99999; // Aumenta o delay para um valor muito alto
        }
    }
}