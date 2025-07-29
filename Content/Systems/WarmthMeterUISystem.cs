using Terraria;
using Terraria.ModLoader;
using Terraria.UI; 
using Microsoft.Xna.Framework;
using System.Collections.Generic;


using ChallengingTerrariaMod.Content.Systems.UI;

namespace ChallengingTerrariaMod.Content.Systems
{
    // Esta classe ModSystem é responsável por instanciar, atualizar e desenhar sua WarmthMeterUI.
    public class WarmthMeterUISystem : ModSystem
    {
      
        internal UserInterface WarmthUserInterface;

       
        internal WarmthMeterUI WarmthBarUI; 

        // Este método é chamado quando o seu mod é carregado.
        public override void Load()
        {
            // A UI só deve ser criada no lado do cliente (jogadores), não em servidores dedicados.
            if (!Main.dedServ)
            {
                WarmthBarUI = new WarmthMeterUI();
                WarmthBarUI.Activate();
                WarmthUserInterface = new UserInterface();
                WarmthUserInterface.SetState(WarmthBarUI);
            }
        }

        // Este método é chamado a cada tick do jogo, depois de tudo mais ter sido atualizado.
        // É onde a UserInterface (e, por sua vez, a WarmthBarUI) é atualizada.
        public override void UpdateUI(GameTime gameTime)
        {
            WarmthUserInterface?.Update(gameTime);
        }

        // Este método é crucial: ele diz ao tModLoader para inserir sua UI nas camadas de desenho do jogo.
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ChallengingTerrariaMod: Warmth Meter Display", 
                    delegate 
                    {
                        if (WarmthUserInterface?.CurrentState != null &&
                            Main.LocalPlayer != null && Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                        {
                            WarmthUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true; 
                    },
                    InterfaceScaleType.UI)); 
            }
        }
    }
}