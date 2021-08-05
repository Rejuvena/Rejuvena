﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rejuvena.Content.DrawEffects;
using Rejuvena.Core.CoreSystems.DrawEffects;
using Terraria;
using Terraria.GameContent;

namespace Rejuvena.Content.Items.Materials
{
    /// <summary>
    ///     Basic material dropped outside of the tower on the Island.
    /// </summary>
    public class JadeGemstone : RejuvenaItem
    {
        public bool InitializedEffects;
        public bool Floating;
        public int SavedSpawnTime;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.maxStack = 999;
            Item.Size = new Vector2(16f, 16f);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

            if (!InitializedEffects)
                return;

            byte difference = (byte) Math.Clamp(Item.timeSinceItemSpawned - SavedSpawnTime, byte.MinValue, byte.MaxValue);

            lightColor.A = (byte) (byte.MaxValue - difference);

            spriteBatch.Draw(TextureAssets.Item[Type].Value,
                Item.Center - Main.screenPosition, null,
                new Color(lightColor.A, lightColor.A, lightColor.A, lightColor.A), rotation, TextureAssets.Item[Type].Size() / 2f, scale,
                SpriteEffects.None, 0f);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (!InitializedEffects)
                return;

            if (Floating)
            {
                gravity = 0f;

                if (Item.timeSinceItemSpawned - SavedSpawnTime > 30)
                    Floating = false;
            }
            else if (gravity == 0f)
                gravity = 0.1f;

            byte difference = (byte)Math.Clamp(Item.timeSinceItemSpawned - SavedSpawnTime, byte.MinValue, byte.MaxValue);

            if (difference >= byte.MaxValue)
                return;

            if (difference > 45 && Main.rand.NextBool(Math.Abs(-difference) / 5))
                DrawEffectManager.Instance.DrawEffects.Add(
                    new JadeSparkle(Item.Center, Main.rand.NextVector2Circular(5f, 5f))
                    {
                        TargetScale = Main.rand.NextFloat(0.2f, 0.4f)
                    });
        }

        public void SetInitialSpawn()
        {
            Floating = true;
            SavedSpawnTime = Item.timeSinceItemSpawned;
            Item.velocity = Vector2.Zero;
            InitializedEffects = true;
        }
    }
}