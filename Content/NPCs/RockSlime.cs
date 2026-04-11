using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace yeetz.Content.NPCs;

// practice mob ai

public class RockSlime : ModNPC
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.aiStyle = -1;
        NPC.Size = new Vector2(44, 30);
        NPC.lifeMax = 220;
        NPC.damage = 10;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.6f;
    }

    public override void AI()
    {
        base.AI();
        Player player = Main.player[NPC.target];
        NPC.TargetClosest(false);
        Vector2 targetPosition = player.Center - new Vector2(0, 80);
        NPC.ai[1]++;
        NPC.ai[2]++;
        if (!Collision.CanHitLine(NPC.Bottom, 1, 1, NPC.Center + NPC.velocity, 1, 1))
        {
            NPC.velocity.X *= 0.85f;
        }
        if (NPC.Distance(player.Center) > 120 && NPC.HasValidTarget && NPC.ai[1] <= 140)
        {
            NPC.ai[0] = 0;
            NPC.ai[1] = 0;
        }
        if (NPC.Distance(player.Center) < 120 && NPC.HasValidTarget)
        {
            NPC.ai[0] = 1;
            NPC.ai[2] = 0;
        }

        if (NPC.ai[0] == 0)
        {
            NPC.noGravity = false;
            if (NPC.ai[2] % 150 == 0)
            {
                NPC.velocity.Y += -6.5f;
            }

            if (NPC.ai[2] >= 150)
            {
                NPC.velocity.X = NPC.DirectionTo(player.Center).X * 3;
            }

            if (NPC.ai[2] >= 151)
            {
                NPC.ai[2] = 0;
            }
        }
        
        if (NPC.ai[0] == 1 && NPC.HasValidTarget)
        {
            NPC.noGravity = false;
            if (NPC.ai[1] >= 100 && NPC.ai[1] <= 140)
            {
                NPC.velocity = (targetPosition - NPC.Center) * 0.08f;
            }

            if (NPC.ai[1] >= 140 && NPC.ai[1] <= 180)
            {
                NPC.noGravity = true;
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
            }

            if (NPC.ai[1] >= 180 && NPC.ai[1] <= 200)
            {
                NPC.noGravity = false;
                NPC.velocity.X = 0;
                NPC.velocity.Y += 1f;
            }

            if (NPC.ai[1] >= 180 && NPC.ai[1] <= 200 && !Collision.CanHitLine(NPC.Bottom, NPC.width, 1, NPC.Center + NPC.velocity, 1, 1))
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(NPC.BottomLeft, NPC.width, NPC.height, DustID.Smoke, NPC.velocity.X, NPC.velocity.Y, Scale: 1.2f);
                }
            }

            if (NPC.ai[1] == 220)
            {
                NPC.ai[1] = 0;
                NPC.ai[0] = 0;
            }
        }
    }
}