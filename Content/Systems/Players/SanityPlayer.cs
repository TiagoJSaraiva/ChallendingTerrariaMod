using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;
using Terraria.Localization;
using ChallengingTerrariaMod.Content.Systems;

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class SanityPlayer : ModPlayer
    {
        public bool gloomActive = false;
        public float CurrentSanity;
        private int counter = 10;

        public int stressedThreshold = 900;
        public int scaredThreshold = 600;
        public int terrifiedThreshold = 300;

        public int _HitDirection;
        public override void Initialize()
        {
            CurrentSanity = SanitySystem.maxSanity;
        }
        public override void ResetEffects()
        {

        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (Player.HasBuff(ModContent.BuffType<Terrified>()))
            {
                if (!Player.HasBuff(ModContent.BuffType<Flee>()) && Main.rand.NextFloat() < 0.2f)
                {
                    _HitDirection = info.HitDirection;
                    Player.AddBuff(ModContent.BuffType<Flee>(), 120);
                }
            }
        }

        public override void PreUpdate()
        {

        }

        public override void PostUpdateBuffs()
        {
            if (Main.GameUpdateCount % RestSystem.restUpdateRate == 0) // Atualiza 1 vez por segundo
            {
                if (Player.active && !Player.dead && !Player.ghost)
                {
                    ApplySanityDebuffs();
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentSanity", CurrentSanity);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentSanity"))
            {
                CurrentSanity = tag.GetFloat("CurrentSanity");
            }
            else
            {
                CurrentSanity = 0; // Valor padrão se não houver dados salvos
            }
        }

        public override void OnRespawn()
        {
            CurrentSanity = SanitySystem.maxSanity;
        }

        public void gloomEffects()
        {
            if (counter-- >= 7) return;
            if (counter == 0)
            {
                var playerDeathReason = PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(Player.name + " got obliterated"));
                Player.KillMe(playerDeathReason, 999, 0);
                counter = 10;
                gloomActive = false;
                return;
            }
            Main.NewText($"{counter}...", Color.DarkRed);
        }

        private void ApplySanityDebuffs()
        {
            Player.ClearBuff(ModContent.BuffType<Terrified>());
            Player.ClearBuff(ModContent.BuffType<Scared>());
            Player.ClearBuff(ModContent.BuffType<Stressed>());

            if (CurrentSanity <= 0)
            {
                if (!gloomActive)
                {
                    Main.NewText("You feel that something's terrible is about to happen", Color.DarkRed);
                    gloomActive = true;
                }
                gloomEffects();
                Player.AddBuff(ModContent.BuffType<Terrified>(), 61);
            }
            else if (CurrentSanity <= terrifiedThreshold)
            {
                if (gloomActive == true)
                {
                    Main.NewText("You got lucky this time", Color.DarkRed);
                    gloomActive = false;
                }
                Player.AddBuff(ModContent.BuffType<Terrified>(), 61);
            }
            else if (CurrentSanity <= scaredThreshold)
            {
                Player.AddBuff(ModContent.BuffType<Scared>(), 61);
            }
            else if (CurrentSanity <= stressedThreshold)
            {
                Player.AddBuff(ModContent.BuffType<Stressed>(), 61);
            }
        }
    }
}
