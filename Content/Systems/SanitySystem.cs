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
using ChallengingTerrariaMod.Content.Buffs;

namespace ChallengingTerrariaMod.Content.Systems
{
    public class SanitySystem : ModSystem
    {
        public const float maxSanity = 1200;

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

        public override void ModifyLightingBrightness(ref float scale)
        {
            if (Main.LocalPlayer != null && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
            {
                var sanityPlayer = Main.LocalPlayer.GetModPlayer<SanityPlayer>();
                if (sanityPlayer.CurrentSanity <= 0)
                {
                    scale = 0.75f;
                }
                else if (sanityPlayer.CurrentSanity <= 300)
                {
                    scale = 0.85f;
                }
                else if (sanityPlayer.CurrentSanity <= 600)
                {
                    scale = 0.90f;
                }
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
                        if (!player.HasBuff(ModContent.BuffType<ArmoredMind>()))
                        {
                            if (player.ZoneDungeon || player.ZoneUnderworldHeight || player.ZoneCrimson || player.ZoneCorrupt) // If the player is in Dungeon, Underworld or corruption/crimson, he loses sanity
                            {
                                sanityPlayer.CurrentSanity -= 7.5f;
                            }
                            if (player.townNPCs > 2) // If the player is in a town, he gains sanity.
                            {
                                sanityPlayer.CurrentSanity += 9;
                            }
                            if (player.statLife <= (player.statLifeMax / 2)) // If player has less than half of his max health, he loses sanity.
                            {
                                sanityPlayer.CurrentSanity -= 9;
                            }
                        }
                        else
                        {
                            if (player.ZoneDungeon || player.ZoneUnderworldHeight || player.ZoneCrimson || player.ZoneCorrupt) // If the player is in Dungeon, Underworld or corruption/crimson, he loses sanity
                            {
                                sanityPlayer.CurrentSanity -= 5f;
                            }
                            if (player.townNPCs > 2) // If the player is in a town, he gains sanity.
                            {
                                sanityPlayer.CurrentSanity += 9;
                            }
                            if (player.statLife <= (player.statLifeMax / 2)) // If player has less than half of his max health, he loses sanity.
                            {
                                sanityPlayer.CurrentSanity -= 6f;
                            }
                        }
                        sanityPlayer.CurrentSanity += 3; // This is the normalization factor. It raises the sanity if the player is not under danger.

                        sanityPlayer.CurrentSanity = Utils.Clamp(sanityPlayer.CurrentSanity, 0, 1200);
                    }
                }
            }
        }
    }
}