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
        public class RestMeterUI : UIState
        {
                private UIElement _area;
                private UIImage _restMeterImage;

                private Asset<Texture2D>[] _restMeterSprites;
                private const int TotalSprites = 12;

                public override void OnInitialize()
                {
                        _area = new UIElement();
                        const int spriteWidth = 30;
                        const int spriteHeight = 50;

                        _area.Left.Set(800f, 0f);
                        _area.Top.Set(20f, 0f);
                        _area.Width.Set(spriteWidth, 0f);
                        _area.Height.Set(spriteHeight, 0f);
                        Append(_area);

                        _restMeterSprites = new Asset<Texture2D>[TotalSprites];
                        for (int i = 0; i < TotalSprites; i++)
                        {
                                string assetPath = $"ChallengingTerrariaMod/Content/Systems/UI/Images/RestMeter/RestMeter_{i}";
                                _restMeterSprites[i] = ModContent.Request<Texture2D>(assetPath, AssetRequestMode.ImmediateLoad);
                        }

                        if (_restMeterSprites[0].IsLoaded)
                        {
                                _restMeterImage = new UIImage(_restMeterSprites[0].Value);
                        }
                        else
                        {
                                _restMeterImage = new UIImage(TextureAssets.InventoryBack.Value);
                        }

                        _restMeterImage.Width.Set(spriteWidth, 0f);
                        _restMeterImage.Height.Set(spriteHeight, 0f);
                        _restMeterImage.Left.Set(0, 0f);
                        _restMeterImage.Top.Set(0, 0f);
                        _area.Append(_restMeterImage);

                }

                public override void Update(GameTime gameTime)
                {
                        base.Update(gameTime);

                        if (Main.LocalPlayer.dead || Main.LocalPlayer.ghost) return;
                        RestPlayer restPlayer = Main.LocalPlayer.GetModPlayer<RestPlayer>();
                        if (restPlayer == null) return;

                        int spriteIndex = (int)Math.Floor(restPlayer.CurrentRest / 100);
                        spriteIndex = Utils.Clamp(spriteIndex, 0, TotalSprites - 1);

                        Texture2D newSpriteTexture = _restMeterSprites[spriteIndex].Value;

                        _restMeterImage.SetImage(newSpriteTexture);
                        
                        if (_area.IsMouseHovering)
                        {

                                if (restPlayer.CurrentRest <= restPlayer.exhaustedThreshold)
                                {
                                        Main.instance.MouseText("Rest Meter\nYou're exhausted\nSleep a little!");
                                }
                                else if (restPlayer.CurrentRest <= restPlayer.sleepyThreshold)
                                {
                                        Main.instance.MouseText("Rest Meter\nYou're sleepy\nSleep a little!");
                                }
                                else if (restPlayer.CurrentRest <= restPlayer.tiredThreshold)
                                {
                                        Main.instance.MouseText("Rest Meter\nYou're tired\nSleep a little!");
                                }
                                else 
                                {
                                        Main.instance.MouseText("Rest Meter\nYou're rested");
                                }
                              
                        }
                }
        }
}