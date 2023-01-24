using Oxide.Core;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("MultiRoleCopters", "Sauli Savinainen", "1.0.0")]
    [Description("Minicopters have unlimited fuel and allow riders to drop timed explosives (C4) by pressing the B key. In addition, the driver can fire military-grade ammunition into any direction by pressing/holding the firing key (LMB by default). Made for the WarZone[RU] server")]
	class MultiRoleCopters : RustPlugin
    {
        private Dictionary<ulong, Timer> cooldownTimers = new Dictionary<ulong, Timer>();
        private void OnEntitySpawned(BaseNetworkable entity)
        {
            if (entity is BaseHelicopter)
            {
                ((BaseHelicopter)entity).fuelStorage.capacity = 100000f;
                ((BaseHelicopter)entity).fuelStorage.SetFuel(100000f);
            }
        }
		
		private Dictionary<BasePlayer, Timer> m39timers = new Dictionary<BasePlayer, Timer>();
		private void OnPlayerInput(BasePlayer player, InputState input)
		{
			if (input.WasJustPressed(BUTTON.B) && player.GetMounted() is BaseHelicopter)
			{
				if (cooldownTimers.ContainsKey(player.userID))
				{
					SendReply(player, "cooldown is in effect!");
					return;
				}
				var c4 = GameManager.server.CreateEntity("assets/prefabs/weapons/c4/c4.prefab", new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
				c4.transform.position = player.transform.position + new Vector3(0, 0, -10);
				c4.Spawn();
				cooldownTimers[player.userID] = timer.Once(6f, () => cooldownTimers.Remove(player.userID));
			}
			if (input.WasJustPressed(BUTTON.ATTACK) && player.GetMounted() is BaseHelicopter)
			{
				var m39 = GameManager.server.CreateEntity("assets/prefabs/weapons/m39/m39.entity.prefab", new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
				m39.transform.position = player.eyes.position;
				m39.transform.rotation = player.eyes.rotation;
				m39.IsVisible = false;
				m39.Spawn();
				m39.ServerUse();
				if (m39timers.ContainsKey(player))
				{
					m39timers[player].Destroy();
				}
				m39timers[player] = timer.Once(1f, () =>
				{
					m39.Kill();
					m39timers.Remove(player);
				});
			}
			if (input.IsDown(BUTTON.ATTACK) == false && m39timers.ContainsKey(player))
			{
				m39timers[player].Destroy();
				m39timers.Remove(player);
			}
		}
		private void OnPlayerDisconnected(BasePlayer player)
		// C4 stays in effect when lpayer is disconnected (since it is a singular bomb)
		{
			if (m39timers.ContainsKey(player))
			{
				m39timers[player].Destroy();
				m39timers.Remove(player);
			}
		}
    }
} 