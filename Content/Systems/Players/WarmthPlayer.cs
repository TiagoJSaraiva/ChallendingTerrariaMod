// Em ChallengingTerrariaMod/Content/Systems/Players/WarmthPlayer.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using ChallengingTerrariaMod.Content.Systems;
using Terraria.DataStructures; 

// Adicione esta linha para acessar seus debuffs customizados
using ChallengingTerrariaMod.Content.Buffs; 

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class WarmthPlayer : ModPlayer
    {
        public int CurrentTemperature;
        private int _temperatureBeforeDeath;
        public int LastTemperatureChange = 0;

        public override void Initialize()
        {
            CurrentTemperature = WarmthSystem.ComfortableTemperature;
            _temperatureBeforeDeath = WarmthSystem.ComfortableTemperature;
            LastTemperatureChange = 0;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageReason)
        {
            _temperatureBeforeDeath = CurrentTemperature;
        }

        public override void OnRespawn()
        {
            CurrentTemperature = WarmthSystem.ComfortableTemperature;
            LastTemperatureChange = 0;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTemperature", CurrentTemperature);
            tag.Add("TemperatureBeforeDeath", _temperatureBeforeDeath);
            tag.Add("LastTemperatureChange", LastTemperatureChange);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentTemperature"))
            {
                CurrentTemperature = tag.GetInt("CurrentTemperature");
            }
            else
            {
                CurrentTemperature = WarmthSystem.ComfortableTemperature;
            }

            if (tag.ContainsKey("TemperatureBeforeDeath"))
            {
                _temperatureBeforeDeath = tag.GetInt("TemperatureBeforeDeath");
            }
            else
            {
                _temperatureBeforeDeath = WarmthSystem.ComfortableTemperature;
            }

            if (tag.ContainsKey("LastTemperatureChange"))
            {
                LastTemperatureChange = tag.GetInt("LastTemperatureChange");
            }
            else
            {
                LastTemperatureChange = 0;
            }
        }

        public override void PreUpdate()
        {
            WarmthSystem.PreviousTemperature[Player.whoAmI] = CurrentTemperature;
        }

        public override void PostUpdate()
        {
            if (Main.GameUpdateCount % WarmthSystem.TEMPERATURE_UPDATE_RATE == 0)
            { 
                if (Player.active && !Player.dead && !Player.ghost)
                {
                    ApplyWarmthDebuffs(); 
                }
            }
        }

        private void ApplyWarmthDebuffs()
        {

            Player.ClearBuff(ModContent.BuffType<Freezing>());
            Player.ClearBuff(ModContent.BuffType<VeryCold>());
            Player.ClearBuff(ModContent.BuffType<Cold>());

            if (CurrentTemperature <= 200) 
            {
                Player.AddBuff(ModContent.BuffType<Freezing>(), 60);
            }
            else if (CurrentTemperature <= 500)
            {
                Player.AddBuff(ModContent.BuffType<VeryCold>(), 60);
            }
            else if (CurrentTemperature <= 800) 
            {
                Player.AddBuff(ModContent.BuffType<Cold>(), 60);
            }

            Player.ClearBuff(ModContent.BuffType<Scorching>());
            Player.ClearBuff(ModContent.BuffType<Hot>());
            Player.ClearBuff(ModContent.BuffType<Warm>());

            if (CurrentTemperature >= 1800) 
            {
                Player.AddBuff(ModContent.BuffType<Scorching>(), 60);
            }
            else if (CurrentTemperature >= 1500)
            {
                Player.AddBuff(ModContent.BuffType<Hot>(), 60);
            }
            else if (CurrentTemperature >= 1200) 
            {
                Player.AddBuff(ModContent.BuffType<Warm>(), 60);
            }
        
        }
    }
}