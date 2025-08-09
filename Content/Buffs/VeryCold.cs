using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class VeryCold : ModBuff
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
            player.pickSpeed += 0.15f; // -15% mining speed
            player.moveSpeed += 0.10f;

            // -1 hp/s (caso a regeneração do jogador seja positiva)
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0; // Zera a regeneração para que a penalidade seja sentida
            }
            player.lifeRegen -= 10; // 60 ticks = 1 hp/s. -1 hp/s
        }
    }
}