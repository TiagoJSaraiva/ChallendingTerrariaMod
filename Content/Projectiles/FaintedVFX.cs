using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using ChallengingTerrariaMod.Content.Buffs;

namespace ChallengingTerrariaMod.Content.Projectiles
{
    public class FaintedVFX : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 2; // mantido sempre vivo
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<Fainted>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Top + new Vector2(0, -30);
            Projectile.timeLeft = 2;

            // Animação: troca de frame a cada 30 ticks
            if (++Projectile.frameCounter >= 30)
            {
                if (Projectile.frame == 3)
                {
                    Projectile.frame = 0;
                }
                else
                {
                    Projectile.frame = Projectile.frame + 1;
                }
                Projectile.frameCounter = 0;
            }
        }
    }
}
