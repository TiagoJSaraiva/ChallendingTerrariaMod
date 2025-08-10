using Terraria;
using Terraria.ModLoader;
using Terraria.UI; 
using Microsoft.Xna.Framework;
using System.Collections.Generic; 


using ChallengingTerrariaMod.Content.Systems.UI;
using ChallengingTerrariaMod.Content.Systems.Players; 

namespace ChallengingTerrariaMod.Content.Systems
{
    public class HungerSystem : ModSystem
    {
        // Variáveis para a UI 
        public UserInterface HungerUserInterface;
        public HungerBarUI HungerBar;

        // Constantes do sistema de fome 
        public const float MaxHungerNormal = 1200; // Valor da fome considerado "cheio" (sem debuffs/buffs de fome/saciedade)
        public const float AbsoluteMaxHunger = 1500; // Valor máximo que a fome pode atingir (acima disso, vomita)

        // Thresholds para os debuffs de saciedade
        public const float MaxHungerDebuffThreshold_Full = 1300; // Limite para "Full"
        public const float MaxHungerDebuffThreshold_Bloated = 1400; // Limite para "Bloated"

        // Thresholds para os debuffs de fome
        public const float HungerDebuffThreshold_Peckish = 900; // Limite para "Peckish"
        public const float HungerDebuffThreshold_Hungry = 600; // Limite para "Hungry"
        public const float HungerDebuffThreshold_Famished = 300; // Limite para "Famished"
        public const float HungerDebuffThreshold_Starved = 0; // Limite para "Starved"

        // Taxas de decremento de fome 
        public const int HungerTickRate = 60; // Frequência de decremento (60 ticks = 1 segundo)
        public const float HungerDecrementIdle = 1f; // Fome perdida por tick (parado)
        public const float HungerDecrementMoving = 2f; // Fome adicional perdida por tick (em movimento)

        public override void Load()
        {
            HungerBar = new HungerBarUI();

            HungerUserInterface = new UserInterface();

            HungerUserInterface.SetState(HungerBar);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (HungerUserInterface?.CurrentState != null)
            {
                HungerUserInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryLayerIndex != -1)
            {
                layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
                    "ChallengingTerrariaMod: Hunger Bar",
                    delegate
                    {
                        if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                        {
                            HungerUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}