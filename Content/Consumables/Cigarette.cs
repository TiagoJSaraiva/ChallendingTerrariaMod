using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.Audio;

namespace ChallengingTerrariaMod.Content.Consumables
{
    public class Cigarette : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 240;
            Item.useTime = 240;
            Item.useTurn = true;
            Item.UseSound = new SoundStyle("ChallengingTerrariaMod/Assets/Audio/CigaretteSFX");;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(silver: 50);

            Item.buffType = ModContent.BuffType<ArmoredMind>();
            Item.buffTime = 60 * 120; // Duração do buff em ticks
        }

        public override void OnConsumeItem(Player player)
        {

        }
    }

    public class CigarettePlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.itemAnimation > 0)
            {
                if (Player.HeldItem.type == ModContent.ItemType<Cigarette>())
                {
                    // Gere partículas de fumaça a cada 2 ticks para um efeito mais sutil
                    if (Player.itemAnimation % 10 == 0) 
                    {
                        // A posição da partícula é ajustada para sair da boca do jogador
                        Vector2 dustPos = Player.Center + new Vector2(30 * Player.direction, -10);

                        // A velocidade é principalmente para cima (eixo Y negativo)
                        Vector2 dustVel = new Vector2(Main.rand.NextFloat(-0.05f, 0.05f), -Main.rand.NextFloat(4f, 2f));

                        // Crie a poeira
                        Dust dust = Dust.NewDustDirect(
                            dustPos,             // Posição
                            0,                   // Largura
                            0,                   // Altura
                            DustID.Smoke,        // Tipo de poeira. DustID.Cloud também é uma boa opção.
                            dustVel.X,           // Velocidade X
                            dustVel.Y,           // Velocidade Y
                            100,                 // Opacidade (0-255). Um valor maior que 0 faz a poeira ser mais transparente.
                            Color.White,         // Cor
                            1.2f                 // Tamanho (scale). Um valor maior faz a fumaça parecer mais densa.
                        );
                        
                        dust.noGravity = true;  // Faz a fumaça flutuar para cima
                        dust.fadeIn = 1.1f;     // Aumenta a duração da partícula antes de desaparecer
                    }
                }
            }
        }
    }
}