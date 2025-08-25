using Terraria;
using Terraria.GameContent.UI.Elements; 
using Terraria.UI; 
using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Graphics; 
using Terraria.ModLoader;
using System;
using ReLogic.Content;
using ChallengingTerrariaMod.Content.Systems.Players;

namespace ChallengingTerrariaMod.Content.Systems.UI
{
    public class HungerBarUI : UIState
    {
        private UIElement area;
        private UIImage hungerMeterImage; // A imagem que mostra o preenchimento da fome

        private Asset<Texture2D>[] hungerFillTextures;
        private const int TotalSprites = 15; 

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(850f, 0f);
            area.Top.Set(20f, 0f);
            area.Width.Set(30, 0f);
            area.Height.Set(50, 0f);
            Append(area);

            hungerFillTextures = new Asset<Texture2D>[TotalSprites];
            for (int i = 0; i < TotalSprites; i++)
            {
                string texturePath = $"ChallengingTerrariaMod/Content/Systems/UI/Images/HungerMeter/HungerMeter_{i}";
                hungerFillTextures[i] = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad);
            }

            // Inicializa a UIImage com a imagem apropriada para a fome inicial do jogador (cheio)
            hungerMeterImage = new UIImage(hungerFillTextures[GetSpriteIndex(HungerSystem.MaxHungerNormal)]);
            hungerMeterImage.Left.Set(0, 0f);
            hungerMeterImage.Top.Set(0, 0f);
            hungerMeterImage.Width.Set(30, 0f);
            hungerMeterImage.Height.Set(50, 0f);
            area.Append(hungerMeterImage);
        }

        // Este método é chamado a cada frame do jogo para atualizar a UI
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HungerPlayer hungerPlayer = Main.LocalPlayer.GetModPlayer<HungerPlayer>();

            if (Main.LocalPlayer != null && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
            {
                int newSpriteIndex = GetSpriteIndex(hungerPlayer.CurrentHunger);

                hungerMeterImage.SetImage(hungerFillTextures[newSpriteIndex]);
            }
            
            if (area.IsMouseHovering)
            {
                if (hungerPlayer.CurrentHunger >= HungerSystem.MaxHungerDebuffThreshold_Bloated)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're bloated\nIf you eat more, you might throw up!");
                }
                else if (hungerPlayer.CurrentHunger >= HungerSystem.MaxHungerDebuffThreshold_Full)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're full\nIf you eat more, you might throw up!");
                }
                else if (hungerPlayer.CurrentHunger <= HungerSystem.HungerDebuffThreshold_Starved)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're starving\nEat something!\nThe better the quality of the food, the more it satisfies you.");
                }
                else if (hungerPlayer.CurrentHunger <= HungerSystem.HungerDebuffThreshold_Famished)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're famished\nEat something!\nThe better the quality of the food, the more it satisfies you.");
                }
                else if (hungerPlayer.CurrentHunger <= HungerSystem.HungerDebuffThreshold_Hungry)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're hungry\nEat something!\nThe better the quality of the food, the more it satisfies you.");
                }
                else if (hungerPlayer.CurrentHunger <= HungerSystem.HungerDebuffThreshold_Peckish)
                {
                    Main.instance.MouseText("Hunger Meter\nYou're peckish\nEat something!\nThe better the quality of the food, the more it satisfies you.");
                }
                else
                {
                    Main.instance.MouseText("Hunger Meter\nYou're well fed");
                }
            }
        }
        private int GetSpriteIndex(float hungerValue)
        {
            // Define o sprite base para o estado "normal" (1100 de fome)
            const int NormalHungerSpriteIndex = 11; 
            const float BaseHungerValue = HungerSystem.MaxHungerNormal; 

            int calculatedSpriteIndex;

            if (hungerValue > BaseHungerValue)
            {
                // Calcula quantos pontos de fome estão acima do normal
                float hungerAboveNormal = hungerValue - BaseHungerValue;
                // Cada 100 pontos acima de BaseHungerValue incrementa 1 sprite
                int spritesIncrement = (int)Math.Floor(hungerAboveNormal / 100f);

                calculatedSpriteIndex = NormalHungerSpriteIndex + spritesIncrement;
            }
            // Lógica para valores ABAIXO de MaxHungerNormal (1100)
            else 
            {
                // Calcula quantos pontos de fome estão abaixo do normal
                float hungerBelowNormal = BaseHungerValue - hungerValue;
                int spritesDecrement;
                // Cada 20 pontos abaixo de BaseHungerValue decrementa 1 sprite
                
                spritesDecrement = (int)Math.Floor(hungerBelowNormal / 100f);
            
                calculatedSpriteIndex = NormalHungerSpriteIndex - spritesDecrement;
            }

            return Utils.Clamp(calculatedSpriteIndex, 0, TotalSprites - 1);
        }
    }
}