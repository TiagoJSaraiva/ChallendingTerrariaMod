using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems.Projectiles;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class SanityPlayer : ModPlayer
    {
        public float CurrentSanity;
        public override void Initialize()
        {
            CurrentSanity = 1200;
        }
        public override void ResetEffects()
        {
        }

        public override void PreUpdate()
        {

        }

        public override void PostUpdateBuffs()
        {
           
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

        private void ApplySanityDebuffs()
        {
           
        }
    }
}
