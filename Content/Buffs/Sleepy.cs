// Em ChallengingTerrariaMod/Content/Buffs/Sleepy.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Sleepy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
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