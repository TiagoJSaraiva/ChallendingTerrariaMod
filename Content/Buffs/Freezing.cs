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
            player.pickSpeed += 0.30f; // -30% mining speed
            player.lifeRegen -= 40; // -10 hp/s (10 * 60 ticks/s)
            player.moveSpeed += 0.20f;
           

            // 5% de chance de contrair o debuff frozen repentinamente a cada 60 ticks
            if (Main.GameUpdateCount % 60 == 0 && Main.rand.NextFloat() < 0.05f)
            {
                player.AddBuff(BuffID.Frozen, 120); // 2 segundos de Frozen (buff vanilla)
            }
        }
    }
}