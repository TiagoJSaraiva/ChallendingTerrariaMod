using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;


namespace ChallengingTerrariaMod.Content.NPCs
{
    public class Pharmacist : ModNPC
    {
        private bool hasQuest = false;
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        // private static int ShimmerHeadIndex;
        // private static Profiles.StackedNPCProfile NPCProfile;

        // public static LocalizedText UpgradedText { get; private set; } USAR ISSO NO FUTURO PARA TRADUZIR O JOGO

        // Sets a unique message when the NPC dies.
        // See also NPCID.Sets.IsTownChild if you just want the message used by Angler and Princess.
        // See ModifyDeathMessage() way below for more details
        public override LocalizedText DeathMessage => this.GetLocalization("DeathMessage");

        public override void Load()
        {
            // Adds our Shimmer Head to the NPCHeadLoader.
            // ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            // Connects this NPC with a custom emote.
            // This makes it when the NPC is in the world, other NPCs will "talk about him".
            // By setting this you don't have to override the PickEmote method for the emote to appear.
            //NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike) // Example Person prefers the forest.
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Like) // Example Person dislikes the snow.
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Love) // Loves living near the Nurse.
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike) // Dislikes living near the merchant.
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
            ; // < Mind the semicolon!

            // This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
            // NPCProfile = new Profiles.StackedNPCProfile(
            //     new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party")
            // );

            ContentSamples.NpcBestiaryRarityStars[Type] = 3; // We can override the default bestiary star count calculation by setting this.

            // UpgradedText = this.GetLocalization("Upgraded");
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,

				// Sets your NPC's flavor text in the bestiary. (use localization keys)
				new FlavorTextBestiaryInfoElement("The pharmacist is searching for something unknown, and to do so he'll need all the help he can get. But of course, whoever can provide this assistance will be rewarded."),
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Retrieve the gore types. This NPC has shimmer and party variants for head, arm, and leg gore. (12 total gores)
                string variant = "";
                // if (NPC.IsShimmerVariant)
                // 	variant += "_Shimmer";  COLOCAR NO FUTURO VARIANTE SHIMMER E PARTY
                // if (NPC.altTexture == 1)
                // 	variant += "_Party";
                int hatGore = NPC.GetPartyHatGore();
                int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
                int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
                int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

                // Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
                if (hatGore > 0)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        { // Requirements for the town NPC to spawn.
            return NPC.downedBoss1;
        }

        // public override ITownNPCProfile TownNPCProfile() {
        // 	return NPCProfile;
        // }

        public override List<string> SetNPCNameList()
        {
            if (Main.rand.NextFloat() < 0.05f)
            {
                return new List<string>()
                {
                    "Walter W."
                };
            }

            return new List<string>() {
                "Henry",
                "Michael",
                "William",
                "Oliver",
                "John",
                "Charles"
            };
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            NumberOfTimesTalkedTo++;

            if (hasQuest) {
                if (Main.LocalPlayer.HasItem(ItemID.IronOre))
                {
                    Main.LocalPlayer.ConsumeItem(ItemID.IronOre);

                    Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_GiftOrReward(), ItemID.GoldBar);

                    hasQuest = false;

                    chat.Add(Language.GetTextValue("Thank you! This is going to help me so much. Here's your reward"));
                    chat.Add(Language.GetTextValue("You have no idea how much this helps me. Here, take this."));
                    chat.Add(Language.GetTextValue("Just when I needed it! Take this, you deserve it"));
                    return chat;
                }
                else
                {
                    // Se o jogador NÃO tem o item, retorna uma fala lembrando-o da quest.
                    Main.npcChatCornerItem = ItemID.IronOre; // Exibe o ícone do item novamente
                    return Language.GetTextValue("Did you find what I wanted?");
                }
            }

            if (NumberOfTimesTalkedTo <= 1)
            {
                chat.Add(Language.GetTextValue("Hello, it's a pleasure to meet you! forgive my haste, but this research is time-sensitive and means everything to me."));
                return chat;
            }

            if (NumberOfTimesTalkedTo <= 10)
            {
                chat.Add(Language.GetTextValue("Oh, if it isn't my dear helper! Could you give me a hand?"));
            }

            if (NPC.GivenName == "Walter W." && NumberOfTimesTalkedTo % 20 == 0)
            {
                chat.Add(Language.GetTextValue("I am the one who knocks!"));
                chat.Add(Language.GetTextValue("Now, say my name."));
                chat.Add(Language.GetTextValue("Jesse, Let's cook."));
                chat.Add(Language.GetTextValue("Jesse, unshit my pants."));
                return chat;
            }

            // These are things that the NPC has a chance of telling you when you talk to it.
            chat.Add(Language.GetTextValue("I’ve been running these tests for weeks, but the data still doesn’t add up..."));
            chat.Add(Language.GetTextValue("We’re running out of reagents, and the cultures are unstable; if I don’t find a solution soon, my entire research could go down the drain."));
            chat.Add(Language.GetTextValue("The chemical composition of these wild plants might reveal a breakthrough in my research, yet the data remains frustratingly inconclusive."));
            chat.Add(Language.GetTextValue("I apologize if I seem anxious at times, but my life depends on this research."));

            string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

            // Here is some additional logic based on the chosen chat line. In this case, we want to display an item in the corner for StandardDialogue4.
            if (chosenChat == Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.StandardDialogue4"))
            {
                // Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
                Main.npcChatCornerItem = ItemID.HiveBackpack;
            }

            return chosenChat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = "Quest";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                if (!hasQuest)
                {
                    hasQuest = true; // Define o estado da quest como ativo

                    Main.npcChatText = "Eu preciso de 10 minérios de ferro! Traga-os para mim para uma recompensa!";
                    Main.npcChatCornerItem = ItemID.IronOre;

                }
            }
        }
        
    }
}