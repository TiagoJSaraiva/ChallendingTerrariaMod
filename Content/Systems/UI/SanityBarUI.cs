// Em ChallengingTerrariaMod/Content/Systems/UI/RestMeterUI.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements; 
using ChallengingTerrariaMod.Content.Systems.Players; 
using System; 
using ReLogic.Content;
using Terraria.GameContent;

namespace ChallengingTerrariaMod.Content.Systems.UI
{
        public class SanityBarUI : UIState
        {
                private UIElement _area;
                private UIImage _sanityMeterImage;

                private Asset<Texture2D>[] _sanityMeterSprites;
                private const int TotalSprites = 12;

                public override void OnInitialize()
                {
                        _area = new UIElement();
                        int spriteWidth = 30;
                        int spriteHeight = 51;

                        _area.Left.Set(750f, 0f);
                        _area.Top.Set(20f, 0f);
                        _area.Width.Set(spriteWidth, 0f);
                        _area.Height.Set(spriteHeight, 0f);
                        Append(_area);

                        _sanityMeterSprites = new Asset<Texture2D>[TotalSprites];
                        for (int i = 0; i < TotalSprites; i++)
                        {
                                string assetPath = $"ChallengingTerrariaMod/Content/Systems/UI/Images/SanityMeter/SanityMeter_{i}";
                                _sanityMeterSprites[i] = ModContent.Request<Texture2D>(assetPath, AssetRequestMode.ImmediateLoad);
                        }

                        if (_sanityMeterSprites[0].IsLoaded)
                        {
                                _sanityMeterImage = new UIImage(_sanityMeterSprites[0].Value);
                        }
                        else
                        {
                                _sanityMeterImage = new UIImage(TextureAssets.InventoryBack.Value);
                        }

                        _sanityMeterImage.Width.Set(spriteWidth, 0f);
                        _sanityMeterImage.Height.Set(spriteHeight, 0f);
                        _sanityMeterImage.Left.Set(0, 0f);
                        _sanityMeterImage.Top.Set(0, 0f);
                        _area.Append(_sanityMeterImage);

                }

                public override void Update(GameTime gameTime)
                {
                        base.Update(gameTime);

                        if (Main.LocalPlayer.dead || Main.LocalPlayer.ghost) return;
                        SanityPlayer sanityPlayer = Main.LocalPlayer.GetModPlayer<SanityPlayer>();
                        if (sanityPlayer == null) return;

                        int spriteIndex = (int)Math.Floor(sanityPlayer.CurrentSanity / 100f);
                        spriteIndex = Utils.Clamp(spriteIndex, 0, TotalSprites - 1);

                        Texture2D newSpriteTexture = _sanityMeterSprites[spriteIndex].Value;

                        _sanityMeterImage.SetImage(newSpriteTexture);

                        if (_area.IsMouseHovering)
                        {

                                if (sanityPlayer.CurrentSanity <= sanityPlayer.terrifiedThreshold)
                                {
                                        Main.instance.MouseText("Sanity meter\nYou're terrified\nto increase your sanity level, go to a safe place!");
                                }
                                else if (sanityPlayer.CurrentSanity <= sanityPlayer.scaredThreshold)
                                {
                                        Main.instance.MouseText("Sanity meter\nYou're scared\nto increase your sanity level, go to a safe place!");
                                }
                                else if (sanityPlayer.CurrentSanity <= sanityPlayer.stressedThreshold)
                                {
                                        Main.instance.MouseText("Sanity meter\nYou're stressed\nto increase your sanity level, go to a safe place!");
                                }
                                else 
                                {
                                        Main.instance.MouseText("Sanity meter\nYou're well");
                                }
                              
                        }
                        
                }
        }
}