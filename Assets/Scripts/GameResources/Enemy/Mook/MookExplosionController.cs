using GameResources.Character;
using UnityEngine;

namespace GameResources.Enemy.Mook
{
    public class MookExplosionController: MonoBehaviour, ICharacterComponent
    {
        public void OnInit()
        {
            
        }

        public void OnUpdate()
        {
            
        }

        public void OnDeInit()
        {
            
        }

        public void Explode()
        {
            AppHandler.BulletManager.SpawnEnemyExplosion(transform.position, transform.forward);
        }
    }
}