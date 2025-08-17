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
            player.pickSpeed += 0.30f;
            player.tileSpeed *= 0.7f;
            player.wallSpeed *= 0.7f;

            if (Main.GameUpdateCount % 60 == 0 && Main.rand.NextFloat() < 0.05f && !player.HasBuff(BuffID.OnFire))
            {
                player.AddBuff(BuffID.OnFire, 5 * 60);
            }
        }
    }
}