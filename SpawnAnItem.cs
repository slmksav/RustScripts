using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
	[Info("SpawnAnItem", "Sauli  Savinainen", "0.1")]
	class SpawnAnItem:CovalencePlugin
	{

		[Command("spawnitem")]
		void cmd(IPlayer player, string command, string [] args)
		{
			if (args.Length < 1)
			{
				player.Message("Which item am I supposed to spawn?");
				return;
			}

			var name = args[0];
			var amount = 1;
			var skin = 0ul;
			if (args.Length > 1)
			{
				amount = int.Parse(args[1]);
			}

			if (args.Length > 2)
			{
				skin = ulong.Parse(args[2]);
			}

			var item = ItemManager.CreateByPartialName(name, amount, skin);

			if (item == null)
			{
				player.Message("Error: Item is null, check the name you typed.");
			}
			else
			{
				player.Message("Here is your item.");
				(player.Object as BasePlayer).GiveItem(item, BaseEntity.GiveItemReason.PickedUp);
			}
		}
	}
}
