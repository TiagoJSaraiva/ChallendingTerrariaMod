using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems.Players;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Flee : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var sanityPlayer = player.GetModPlayer<SanityPlayer>();

            if (sanityPlayer._HitDirection != 0)
            {
                player.velocity.X = 5f * sanityPlayer._HitDirection;
                player.direction = sanityPlayer._HitDirection; ;
            }
            player.controlLeft = false;
            player.controlRight = false;
        }
    }
}