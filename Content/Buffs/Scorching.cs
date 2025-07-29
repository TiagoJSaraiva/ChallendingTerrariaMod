using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Scorching : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Scorching");
            // Description.SetDefault("-15 defense, -20% damage, cannot regenerate life, and -10 hp/s."); //
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 15; // -15 defense
            player.GetDamage(DamageClass.Generic) -= 0.20f; // -20% damage

            // Não pode regenerar vida. Zera a regeneração de vida.
            player.lifeRegen = 0;
            player.lifeRegen -= 100; // -10 hp/s (10 * 60 ticks/s)
        }
    }
}