// ChallengingTerrariaMod/Content/Buffs/Stuffed.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Stuffed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stuffed");
            // Description.SetDefault("Very full. Movement speed, jump height, flight time, and mining speed are reduced.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.85f; // 15% de redução
            player.jumpSpeedBoost *= 0.85f; // 15% de redução
            player.wingTime /= 1.15f; // 15% de redução
            player.pickSpeed += 0.10f; // 10% mais lento
        }
    }
}