// ChallengingTerrariaMod/Content/Buffs/Starved.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;

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
            player.wingTimeMax = 0; // Impede voo
            player.mount.Dismount(player); // Desmonta (se estiver montado)

            if (player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 10 == 0) // Melhor usar Main.GameUpdateCount
            {
                PlayerDeathReason reason = PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(player.name + " turned to ashes"));
                player.Hurt(reason, 1, 0, false, true, -1, false, 200);
            }
            player.lifeRegen = 0;
            player.lifeRegenTime = 0;

        }
    }
}