// Em ChallengingTerrariaMod/Content/Buffs/Sleepy.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Sleepy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sleepy");
            // Description.SetDefault("You are starting to feel sleepy and your mind isn't working properly. Magic and ranged damage reduced greatly. Mana regen reduced.");

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Redução de dano
            player.GetDamage(DamageClass.Magic) -= 0.15f; // -15% de dano mágico
            player.GetDamage(DamageClass.Ranged) -= 0.20f; // -20% de dano ranged

            // Redução da regeneração de mana
            player.manaRegenCount -= (int)(player.manaRegenCount * 0.50f); // Reduz em 50% a contagem de regeneração (mais significativo)
        }
    }
}