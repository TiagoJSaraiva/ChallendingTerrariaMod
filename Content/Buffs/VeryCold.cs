using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class VeryCold : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chilled");
            // Description.SetDefault("-15% mining speed, -1 hp/s, 30% chance to gain Chilled debuff (30s) every 60 ticks."); //
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            // Para o debuff 'Chilled' que você pode contrair, talvez não use buffNoTimeDisplay,
            // mas para o buff principal que mantém o efeito, pode usar.
            // Vou manter buffNoTimeDisplay para os buffs principais do sistema de temperatura.
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