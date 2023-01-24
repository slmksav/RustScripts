using Oxide
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("GuidedMissiles", "Sauli Savinainen", "0.9")]
    [Description("Makes launched rockets track minicopters in real time if the rocket has been fired near them on a 1000 unit (Unity for meter) radius. Does not guarantee they will get hit. Made for WarZone[RU] server")]
    class RocketTracker : RustPlugin
    {
        private GameObject minicopter;
        private BasePlayer player;
        private BaseProjectile projectile;

        private void Init()
        {
            RegisterHook("OnRocketLauncherFired", OnRocketLauncherFired);
            RegisterHook("OnMinicopterSpawned", OnMinicopterSpawned);
        }

        private void OnRocketLauncherFired(BasePlayer player, BaseProjectile proj)
        {
            this.player = player;
            projectile = proj;
            if (minicopter != null)
            {
                InvokeRepeating("UpdateRocketVelocity", 0f, 1f);
            }
        }

        private void OnMinicopterSpawned(BaseHelicopter helicopter)
        {
            float distance = Vector3.Distance(player.transform.position, helicopter.transform.position);
            if (distance <= 1000)
            {
                minicopter = helicopter.gameObject;
            }
        }

		private void UpdateRocketVelocity()
		{
			Vector3 currentPosition = projectile.transform.position;
			Vector3 minicopterPosition = minicopter.transform.position;
			NormalizeRocketTrajectory(currentPosition, minicopterPosition);
		}
		private void NormalizeRocketTrajectory(Vector3 currentPosition, Vector3 minicopterPosition)
		{
		float distance = Vector3.Distance(currentPosition, minicopterPosition);
			if (distance > 1000)
			{
				Vector3 direction = (minicopterPosition - currentPosition).normalized;
				projectile.GetComponent<Rigidbody>().velocity = direction * projectile.GetComponent<RocketProjectile>().speed;
			}
		}
    }
}