using Oxide.Core;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("MultiRoleCopters", "Sauli Savinainen", "1.0.0")]
    [Description("Minicopters have unlimited fuel and allow riders to drop timed explosives (C4) by pressing the B key. Made for the WarZone[RU] server")]
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
        }
    }
}