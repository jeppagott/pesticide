using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using yeetz.Content.Projectiles;

namespace yeetz.Content.NPCs;

public class Exol : ModNPC
{
    private static Asset<Texture2D> Glow => ModContent.Request<Texture2D>("yeetz/Content/NPCs/Exol_Glow");
    
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex = TextureAssets.Npc[Type].Value;
        Main.EntitySpriteDraw(tex, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2 + new Vector2(6, 0), NPC.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(Glow.Value, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2 + new Vector2(6, 0), NPC.scale, SpriteEffects.None);
        return false;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.aiStyle = -1;
        NPC.lifeMax = 10000;
        NPC.damage = 100;
        NPC.Size = new Vector2(140, 140);
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.lavaImmune = true;
        NPC.buffImmune[BuffID.OnFire] = NPC.buffImmune[BuffID.OnFire3] = true;
        NPC.boss = true;
        NPC.knockBackResist = 0;
        NPC.HitSound = SoundID.NPCHit41;
        NPC.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
        Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/ExolTheme");
    }

    public override void AI()
    {
        base.AI();
        Dust.NewDustDirect(NPC.position + new Vector2(50, 55), 40, 30, DustID.Torch, NPC.velocity.X * 0.8f, -20, Scale: 1.6f).noGravity = true;
        Player player = Main.player[NPC.target];
        NPC.TargetClosest(false);
        
        Vector2 targetPosition = player.Center - new Vector2(0, 200);
        if (NPC.ai[0] != 3)
            NPC.velocity = (targetPosition - NPC.Center) * 0.2f;
        NPC.ai[1]++;
        Vector2 vel0 = NPC.DirectionTo(player.Center).RotatedByRandom(0.1f) * 10;
        Vector2 vel1 = NPC.DirectionTo(player.Center + new Vector2(0, -500)).RotatedBy(NPC.ai[1] * 0.124) * 7;
        if (NPC.ai[0] == 0)
        {
            if (NPC.ai[1] % 30 == 0)
            {
                NPC.damage = 0;
                Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center + vel0 * 5,vel0, ModContent.ProjectileType<phfire>(), 20, 1);
            }
            if (NPC.ai[1] >= 200)
            {
                NPC.ai[1] = 0;
                NPC.ai[0] = 1;
            }
                
        }

        if (NPC.ai[0] == 1)
        {
            if (NPC.ai[1] % 4 == 2)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center + vel1 * 5, vel1, ModContent.ProjectileType<phfire>(), 20, 1);
            }

            if (NPC.ai[1] >= 40)
            {
                NPC.ai[1] = 0;
                NPC.ai[0] = 2;
            }
        }
        
        if (NPC.ai[0] == 2)
        {
            if (NPC.ai[1] >= 40)
            {
                if (NPC.ai[1] % 51 == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center, NPC.DirectionTo(player.Center).RotatedBy(i * 0.9f) * 5, ModContent.ProjectileType<homingfire>(), 20, 1, ai2: NPC.target);
                    }
                }
                if (NPC.ai[1] >= 100)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[0] = 3;
                }
            }
        }

        if (NPC.ai[0] == 3)
        {
            if (NPC.ai[1] < 60)
            {
                NPC.velocity *= 0.94f;
            }

            if (NPC.ai[1] == 60)
            {
                NPC.velocity = NPC.DirectionTo(player.Center) * -20;
            }
            
            if (NPC.ai[1] == 69)
            {
                NPC.damage = 100;
                NPC.velocity = NPC.DirectionTo(player.Center) * 14;
                SoundEngine.PlaySound(new SoundStyle("yeetz/Assets/Sounds/scuffeddash"), NPC.Center);
                for (int i = 0; i < 50; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, NPC.velocity.X * -0.8f, NPC.velocity.Y * -0.8f, Scale: 1.5f);
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Torch, NPC.velocity.X * 0.8f, NPC.velocity.Y * 0.8f, Scale: 2.5f).noGravity = true;
                    if (Main.rand.NextBool(4))
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Scale: 1.8f);
                    }
                }
            }

            if (NPC.ai[1] >= 69)
            {
                if (!Collision.CanHitLine(NPC.Center, 1, 1, NPC.Center + NPC.velocity, 1, 1))
                {
                    NPC.velocity *= 0.91f;
                }
                for (int i = 0; i < 7; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, NPC.velocity.X * 0.8f, NPC.velocity.Y * 0.8f,  Scale: 1.3f);
                }
            }
            
            if (NPC.ai[1] >=71 && NPC.velocity.Length() <= 35)
            {
                NPC.velocity *= 1.13f;
            }

            if (NPC.ai[1] >= 90)
            {
                NPC.ai[1] = 0;
                if (++NPC.ai[2] > 3)
                {
                    NPC.ai[0] = 0;
                    NPC.ai[2] = 0;
                    NPC.damage = 0;
                }
            }
        }
    }
}