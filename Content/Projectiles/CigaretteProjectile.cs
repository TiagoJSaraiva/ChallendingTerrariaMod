using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using ChallengingTerrariaMod.Content.Buffs;

namespace ChallengingTerrariaMod.Content.Projectiles
{
    public class CigaretteProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 40;
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

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = player.itemAnimation;
            Projectile.Center = player.Top + new Vector2(player.direction * 14, 20);
            Projectile.spriteDirection = Projectile.direction = player.direction;
            Projectile.rotation = 0;


            Projectile.frameCounter++;
            
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame > 2)
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
