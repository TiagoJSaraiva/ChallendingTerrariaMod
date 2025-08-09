using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Hot : ModBuff
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
            player.statDefense -= 5; // -5 defense
            player.GetDamage(DamageClass.Generic) -= 0.10f; // -10% damage

            // Não pode regenerar vida. Zera a regeneração de vida.
            player.lifeRegen = 0;
        }
    }
}