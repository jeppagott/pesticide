using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using yeetz.Content.Projectiles;

namespace yeetz.Content.NPCs;

public class Stinkshroom : ModNPC
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.aiStyle = -1;
        NPC.Size = new Vector2(30, 40);
        NPC.lifeMax = 110;
        NPC.damage = 10;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 4;
    }

    public override void AI()
    {
        base.AI();
        Player player = Main.player[NPC.target];
        NPC.TargetClosest(false);
        NPC.noGravity = true;
        NPC.ai[1]++; 
        NPC.velocity *= 0.96f;
        if (NPC.ai[0] == 0 && NPC.HasValidTarget)
        {
            NPC.ai[3] = NPC.rotation;
            if (NPC.ai[1] < 80)
            {
                NPC.velocity *= 0.95f;
            }
            if (NPC.ai[1] == 80)
            {
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2;
                NPC.velocity = NPC.DirectionTo(player.Center) * 7;
                SoundEngine.PlaySound(SoundID.Item16, NPC.Center);
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.BubbleBurst_White, NPC.velocity.X * -0.6f, NPC.velocity.Y * -0.6f, Scale: 1f, newColor: Color.PaleGreen, Alpha: 210).noGravity = false;
                }
            }

            if (NPC.ai[1] >= 100)
            {
                NPC.ai[1] = 0;
                if (++NPC.ai[2] > 3)
                {
                    NPC.ai[0] = 1;
                    NPC.ai[2] = 0;
                }
            }
        }

        if (NPC.ai[0] == 1 && NPC.HasValidTarget)
        {
            if (NPC.ai[1] < 80)
            {
                NPC.velocity *= 0.95f;
                NPC.rotation = NPC.ai[3] + MathF.Sin(NPC.ai[1] * 0.45f * NPC.ai[1] / 70) * 0.3f;
            }

            if (NPC.ai[1] >= 80 && NPC.ai[1] <= 100)
            {
                NPC.ai[3] = NPC.rotation;
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2;
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 16, 0.06f);
                if (NPC.ai[1] % 5 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item16, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center), ModContent.ProjectileType<stinkcloud>(), 20, 1);
                }
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.BubbleBurst_White, NPC.velocity.X * -0.6f, NPC.velocity.Y * -0.6f, Scale: 1f, newColor: Color.PaleGreen, Alpha: 215).noGravity = false;
                }
            }

            if (NPC.ai[1] >= 110)
            {
                NPC.ai[1] = 0;
                NPC.ai[0] = 0;
            }
        }
    }
    
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex0 = TextureAssets.Npc[Type].Value;
        Main.EntitySpriteDraw(tex0, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None);
        return false;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter >= 5)
        {
            NPC.frame.Y += frameHeight;
            NPC.frameCounter = 0;
            if (NPC.frame.Y > frameHeight * 3)
            {
               NPC.frame.Y = 0;
            }
        }
        base.FindFrame(frameHeight);
    }

    public override void OnKill()
    {
        SoundEngine.PlaySound(SoundID.Item16, NPC.Center);
        base.OnKill();
    }
}