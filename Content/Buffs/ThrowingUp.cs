// ChallengingTerrariaMod/Content/Buffs/ThrowingUp.cs
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures; // Para PlayerDeathReason

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class ThrowingUp : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Throwing Up");
            // Description.SetDefault("You are violently throwing up.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            // Main.persistentBuff[Type] = false; // Default para buffs que não são salvos
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.20f;
            player.velocity.X = 0.30f;

            // Partículas verdes (BloodFX, mas verde)
            if (Main.rand.NextBool(5)) // A cada 5 frames
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.Smoke, 0f, 0f, 100, new Color(0, 255, 0), 1.5f);
            }

            // Tocar áudio (exemplo: som de vômito, ou som de debuff)
            // if (player.whoAmI == Main.myPlayer && Main.rand.NextBool(60)) // A cada segundo, aprox.
            // {
            //     SoundEngine.PlaySound(SoundID.NPCDeath1, player.position); // Substitua por seu som
            // }
        }
    }
}