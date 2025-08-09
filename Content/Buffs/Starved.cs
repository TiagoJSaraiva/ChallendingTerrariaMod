// ChallengingTerrariaMod/Content/Buffs/Starved.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Starved : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.itemAnimationMax = 999;
            player.wingTimeMax = 0; // Impede voo
            player.mount.Dismount(player); // Desmonta (se estiver montado)

            // Dano a cada 10 ticks (muito rapido)
            if (player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 10 == 0) // Melhor usar Main.GameUpdateCount
            {
                player.statLife -= 1; // Perde 1 de vida
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByOther(1), 1.0, 0);
                }
            }
            player.lifeRegen = 0;
            player.lifeRegenTime = 0;

        }
    }
}