using Terraria;
using Terraria.ModLoader;
using Terraria.ID; 
using ChallengingTerrariaMod.Content.Systems.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.Localization;
using ChallengingTerrariaMod.Content.Systems.UI;
using System;

namespace ChallengingTerrariaMod.Content.Systems
{
    public class SanitySystem : ModSystem
    {
        public const int maxSanity = 1200;
        public int sanityGainPerSecond = 4;
        public int sanityLossPerSecond = 12;

        // UI do sono
        public static UserInterface SanityUserInterface;
        public static SanityBarUI SanityUIState; 

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SanityUIState = new SanityBarUI();
                SanityUIState.Activate();
                SanityUserInterface = new UserInterface();
                SanityUserInterface.SetState(SanityUIState);
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                SanityUIState = null;
                SanityUserInterface = null;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (SanityUserInterface?.CurrentState != null)
            {
                SanityUserInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ChallengingTerrariaMod: Sanity UI",
                    delegate
                    {
                        // Desenha a UI apenas se o jogador local estiver ativo e não morto/fantasma
                        if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                        {
                            SanityUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdatePlayers()
        {
            // A lógica de atualização do sono só acontece a cada segundo (60 ticks)
            if (Main.GameUpdateCount % 60 == 0)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && !player.ghost)
                    {
                        SanityPlayer sanityPlayer = player.GetModPlayer<SanityPlayer>();
                        if (sanityPlayer == null) continue;

                        // Sanity logic

                        if (player.ZoneDungeon || player.ZoneUnderworldHeight || player.ZoneCrimson || player.ZoneCorrupt)
                        {
                            sanityPlayer.CurrentSanity -= 6;
                        }
                        else if (player.townNPCs > 2)
                        {
                            sanityPlayer.CurrentSanity += 12;
                        }
                        else
                        {
                            if (sanityPlayer.CurrentSanity < maxSanity)
                            {
                                sanityPlayer.CurrentSanity += 3;
                            }
                        }
                        
                        sanityPlayer.CurrentSanity = Utils.Clamp(sanityPlayer.CurrentSanity, 0, 1200);
                    }
                }
            }
        }
    }
}