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
            player.statDefense /= 1.8f;
            player.endurance /= 1.8f;
            player.pickSpeed += 0.60f;
        }
    }

    public class StarvedPlayer : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(ModContent.BuffType<Starved>()))
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 20;
            }
        }
    }
}