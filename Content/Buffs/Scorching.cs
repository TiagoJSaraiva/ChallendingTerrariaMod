using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Scorching : ModBuff
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
            player.pickSpeed += 0.50f;
            player.tileSpeed *= 0.5f;
            player.wallSpeed *= 0.5f;

            if (Main.GameUpdateCount % 60 == 0 && Main.rand.NextFloat() < 0.05f && !player.HasBuff(BuffID.OnFire))
            {
                player.AddBuff(BuffID.OnFire, 5 * 60);
            }

            PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral("Burned to death"));

            if (Main.GameUpdateCount % 60 == 0)
            {
                player.Hurt(deathReason, 15, 0, false, true, -1, false);
            }
        }
    }
}