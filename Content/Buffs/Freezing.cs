// Em ChallengingTerrariaMod/Content/Buffs/Freezing.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Freezing : ModBuff
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

            if (Main.GameUpdateCount % 60 == 0 && Main.rand.NextFloat() < 0.05f && !player.HasBuff(BuffID.Frostburn))
            {
                player.AddBuff(BuffID.Frostburn, 7 * 60);
            }
            
            // 15% de chance de contrair o debuff frozen repentinamente a cada 60 ticks
            if (Main.GameUpdateCount % 60 == 0 && Main.rand.NextFloat() < 0.15f)
            {
                player.AddBuff(BuffID.Frozen, 120); // 2 segundos de Frozen (buff vanilla)
            }
        }
    }
}