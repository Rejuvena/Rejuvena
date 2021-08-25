﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using TomatoLib.Common.Systems.DrawEffects;

namespace Rejuvena.Content.DrawEffects
{
    public class JadeSparkle : BaseDrawEffect
    {
        public Asset<Texture2D> Asset => ModContent.Request<Texture2D>("Rejuvena/Content/DrawEffects/Sparkle");

        public float Timer;
        public float Rotation;
        public NPC NPC;
        public float TargetScale = Main.rand.NextFloat(0.2f, 0.5f);
        public float RotationIncrementation = Main.rand.NextFloat(-3f, 3f);

        public override float Scale { get; set; } = 0.15f;

        public JadeSparkle(Vector2 pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
        }
        int angletimer;
        public override void Update()
        {
            if (++angletimer > 360)
                angletimer = 0;

            Vector2 direction = Main.LocalPlayer.Center.DirectionFrom(position);
            velocity = direction;
            velocity += Vector2.One.RotatedBy(MathHelper.ToRadians(angletimer)) * new Vector2(3f, 1f);
            position += velocity;
            return;
            velocity.X *= 0.9f;
            velocity.Y *= 0.9f;


            Timer++;
            if (Timer < 10f)
            {
                Scale = MathHelper.Lerp(Scale, TargetScale, 0.3f);

                if (NPC != null)
                    position += NPC.velocity;
            }
            else
            {
                RotationIncrementation *= 0.93f;
                Scale = MathHelper.Lerp(Scale, TargetScale, -0.23f);

                if (NPC != null)
                    position += NPC.velocity * 0.6f;

                if (Scale <= 0)
                    Destroy();
            }


            Rotation += RotationIncrementation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null,
                Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(Asset.Value, position - Main.screenPosition, new Rectangle(0, 0, 54, 54),
                new Color(82, 128, 140),
                MathHelper.ToRadians(Rotation), new Vector2(54f / 2f, 54f / 2f), Scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                Main.GameViewMatrix.ZoomMatrix);
        }
    }
}