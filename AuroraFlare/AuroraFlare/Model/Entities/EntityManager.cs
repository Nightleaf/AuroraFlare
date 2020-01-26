using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraFlare.Model.Entities
{
    class EntityManager
    {

        public static List<Entity> EntityList;

        /// <summary>
        /// Adds an entity to the list.
        /// </summary>
        /// <param name="entity">The entity we are adding.</param>
        public static void AddEntity(Entity entity) {
            if (!EntityList.Contains(entity)) {
                EntityList.Add(entity);
            }
        }

        /// <summary>
        /// Removes an entity from the list.
        /// </summary>
        /// <param name="entity">The entity we are removing.</param>
        public static void RemoveEntity(Entity entity) {
            if (EntityList.Count > 0) {
                if (entity != null) {
                    EntityList.Remove(entity);
                }
            }        
        }
    }
}
