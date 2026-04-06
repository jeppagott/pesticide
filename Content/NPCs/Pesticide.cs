using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace yeetz.Content.NPCs;

public class Pesticide : ModNPC
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.aiStyle = -1;
        NPC.Size = new Vector2(26, 44);
        NPC.lifeMax = 3500;
        NPC.damage = 50;
        NPC.boss = true;
        NPC.knockBackResist = 0;
    }

    public override void AI()
    {
        base.AI();
        Player player = Main.player[NPC.target];
        NPC.ai[1]++;
        if (NPC.ai[0] == 0)
        {
            
        }
    }
}