// Em ChallengingTerrariaMod/Content/Systems/UI/WarmthMeterUI.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using ReLogic.Content;
using ChallengingTerrariaMod.Content.Systems.Players;
using ChallengingTerrariaMod.Content.Systems;
using System;

namespace ChallengingTerrariaMod.Content.Systems.UI
{
    public class WarmthMeterUI : UIState
    {
        private UIImage warmthMeterImage;

        public Asset<Texture2D> warmthBaseTexture;
        public Asset<Texture2D>[] warmthUpTextures;
        public Asset<Texture2D>[] warmthDownTextures;

        private const int MaxPairedSprites = 37;
        private const int TemperatureInterval = 52; 

        private int _updateSpriteTickCounter;
        private const int SPRITE_UPDATE_RATE = 60; // Atualiza a cada 60 ticks (1 segundo)

        public override void OnInitialize()
        {
            warmthBaseTexture = ModContent.Request<Texture2D>("ChallengingTerrariaMod/Content/Systems/UI/Images/WarmthMeter/Warmthmeter_base", AssetRequestMode.ImmediateLoad);

            warmthUpTextures = new Asset<Texture2D>[MaxPairedSprites + 1];
            warmthDownTextures = new Asset<Texture2D>[MaxPairedSprites + 1];

            for (int i = 1; i <= MaxPairedSprites; i++)
            {
                warmthUpTextures[i] = ModContent.Request<Texture2D>($"ChallengingTerrariaMod/Content/Systems/UI/Images/WarmthMeter/Warmthmeter_{i}_up", AssetRequestMode.ImmediateLoad);
                warmthDownTextures[i] = ModContent.Request<Texture2D>($"ChallengingTerrariaMod/Content/Systems/UI/Images/WarmthMeter/Warmthmeter_{i}_down", AssetRequestMode.ImmediateLoad);
            }

            warmthMeterImage = new UIImage(warmthBaseTexture.Value);
            warmthMeterImage.Left.Set(700f, 0f);
            warmthMeterImage.Top.Set(20f, 0f);
            warmthMeterImage.Width.Set(60f, 0f);
            warmthMeterImage.Height.Set(60f, 0f);
            warmthMeterImage.SetPadding(0);
            Append(warmthMeterImage);

            // ALTERAÇÃO AQUI: Inicializa _updateSpriteTickCounter para garantir a atualização no primeiro tick
            _updateSpriteTickCounter = SPRITE_UPDATE_RATE; // Ou SPRITE_UPDATE_RATE - 1, dependendo da ordem exata de Update e incremento. Com SPRITE_UPDATE_RATE, ele roda no primeiro tick.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            WarmthPlayer warmthPlayer = Main.LocalPlayer.GetModPlayer<WarmthPlayer>();

            if (warmthMeterImage == null) return;

            _updateSpriteTickCounter++;

            if (_updateSpriteTickCounter >= SPRITE_UPDATE_RATE)
            {
                _updateSpriteTickCounter = 0; // Reseta o contador para começar a contagem de 60 ticks

                int currentTemperature = warmthPlayer.CurrentTemperature;
                int lastTemperatureChange = warmthPlayer.LastTemperatureChange;

                Asset<Texture2D> newTextureAsset = warmthBaseTexture;

                if (currentTemperature == WarmthSystem.ComfortableTemperature && lastTemperatureChange == 0)
                {
                    newTextureAsset = warmthBaseTexture;
                }
                else
                {
                    int steps = (int)Math.Floor(Math.Abs(currentTemperature - WarmthSystem.ComfortableTemperature) / (double)TemperatureInterval);

                    steps = Math.Max(1, steps);

                    int calculatedSpriteIndex;

                    if (currentTemperature > WarmthSystem.ComfortableTemperature) // Temperatura está alta (quente)
                    {
                        calculatedSpriteIndex = 19 - (steps - 1);
                    }
                    else // Temperatura está baixa (frio)
                    {
                        calculatedSpriteIndex = 19 + (steps - 1);
                    }

                    calculatedSpriteIndex = Math.Clamp(calculatedSpriteIndex, 1, MaxPairedSprites);

                    if (lastTemperatureChange > 0)
                    {
                        newTextureAsset = warmthUpTextures[calculatedSpriteIndex];
                    }
                    else if (lastTemperatureChange < 0)
                    {
                        newTextureAsset = warmthDownTextures[calculatedSpriteIndex];
                    }
                    else
                    {
                        if (currentTemperature > WarmthSystem.ComfortableTemperature)
                        {
                            newTextureAsset = warmthDownTextures[calculatedSpriteIndex];
                        }
                        else
                        {
                            newTextureAsset = warmthUpTextures[calculatedSpriteIndex];
                        }
                    }
                }
                warmthMeterImage.SetImage(newTextureAsset.Value);
            }
            
            if (warmthMeterImage.IsMouseHovering)
            {
                if (warmthPlayer.CurrentTemperature <= 200)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're freezing\nFind a heat source to warm you up! For example, a campfire.");
                }
                else if (warmthPlayer.CurrentTemperature <= 500)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're very cold\nFind a heat source to warm you up! For example, a campfire.");
                }
                else if (warmthPlayer.CurrentTemperature <= 800)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're feeling cold\nFind a heat source to warm you up! For example, a campfire.");
                }
                else if (warmthPlayer.CurrentTemperature >= 1800)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're scorching\nFind something to cool you down! Like water, for example.");
                }
                else if (warmthPlayer.CurrentTemperature >= 1500)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're hot\nFind something to cool you down! Like water, for example.");
                }
                else if (warmthPlayer.CurrentTemperature >= 1200)
                {
                    Main.instance.MouseText("Warmth Meter\nYou're warm\nFind something to cool you down! Like water, for example.");
                }
                else
                {
                    Main.instance.MouseText("Warmth Meter\nYou're confortable");
                }
            }
        }
    }
}