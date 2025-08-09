// Em ChallengingTerrariaMod/Content/Buffs/Tired.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Tired : ModBuff
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
            player.GetDamage(DamageClass.Magic) -= 0.10f; // -10% de dano mágico
            player.GetDamage(DamageClass.Ranged) -= 0.10f; // -10% de dano ranged
            player.manaRegenCount -= (int)(player.manaRegenCount * 0.25f); // Reduz em 25% a contagem de regeneração
            // Isso fará com que a mana regenere mais lentamente. Ajuste o 0.25f conforme o desejado para "levemente".
        }
    }
}