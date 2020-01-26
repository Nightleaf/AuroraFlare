using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraFlare.Model.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AuroraFlare.Model
{
    class NPCManager
    {
        static List<Vector2> positions = new List<Vector2>();

        static Random random = new Random();

        // The timer for the last time an npc spawned.
        static float lastSpawn;

        static Boolean spawned;

        // Content
        public static Texture2D[] NPCSprites;

        /// <summary>
        /// Loads all the content for NPCs.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadNPCContent(ContentManager content)
        {
            NPCSprites = new Texture2D[1];
            NPCSprites[0] = content.Load<Texture2D>("Game/Enemies/Enemy1");
        }

        /// <summary>
        /// Updates all NPCs.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            UpdateAllNPCS(gameTime);
            lastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (lastSpawn > 15f && !spawned)
            {
                AddNPC();
                spawned = true;
            }
            //RemoveAllNPCs();
        }

        /// <summary>
        /// Draws all NPC's.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            RenderAllNPCS(spriteBatch);   
        }

        /// <summary>
        /// Adds an NPC to the entity list.
        /// </summary>
        public static void AddNPC()
        {
            if (getCurrentNPCCount() < 20)
            {
                NPC npc = new NPC();
                npc.Initialize();
                npc.Position = new Vector2(random.Next(710) + 1, random.Next(1270) + 1);
            }
        }

        /// <summary>
        /// Updates all NPCs within the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateAllNPCS(GameTime gameTime)
        {
            foreach (Entity entity in EntityManager.EntityList)
            {
                if (entity != null)
                {
                    if (entity is NPC)
                    {
                        entity.Update(gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// Renders all NPCs.
        /// </summary>
        public static void RenderAllNPCS(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in EntityManager.EntityList)
            {
                if (entity != null)
                {
                    if (entity is NPC)
                    {
                        entity.Render(NPCSprites[0], spriteBatch);    
                    }
                }
            }
        }

        /// <summary>
        /// Removes all the NPCs that need to be removed.
        /// </summary>
        public static void RemoveAllNPCs()
        {
            foreach (Entity entity in EntityManager.EntityList)
            {
                if (entity != null)
                {
                    if (entity is NPC)
                    {
                        if (entity.IsDead)
                        {
                            if (entity.shouldBeRemoved)
                            {
                                EntityManager.EntityList.Remove(entity);
                            }
                        }
                    }
                }
            }
        }

        public static int getCurrentNPCCount()
        {
            int amount = 0;
            foreach (Entity entity in EntityManager.EntityList)
            { 
                if (entity != null)
                {
                    if (entity is NPC)
                    {
                        if (!entity.IsDead)
                        {
                            amount++;    
                        }
                    }
                }
            }
            return amount;
        }
    }
}
