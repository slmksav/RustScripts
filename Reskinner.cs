using UnityEngine;
using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("Reskinner", "Sauli Savinainen", "1.0.0")]
    [Description("A very powerful reskin tool which  allows you to reskin ANY entity using an unique workshop address for its texture.")]
    class Reskinner : RustPlugin
    {
        class PluginConfig
        {
            public List<string> Items { get; set; }
        }
        private PluginConfig config;

        private void Init()
        {
            permission.RegisterPermission(Perm, this);
            config = Config.ReadObject<PluginConfig>();
        }

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        private PluginConfig GetDefaultConfig()
        {
            return new PluginConfig
            {
                Items = new List<string>()
                {
                    "door.hinged.wood",
                    "door.hinged.metal",
                    "door.hinged.toptier"
                }
            };
        }
        const string Perm = "reskinner.use";
        [ChatCommand("skin")]
        void SkinCommand(BasePlayer player, string cmd, string[] args)
        {
            if (!permission.UserHasPermission(player.UserIDString, Perm))
            {
                player.ChatMessage("No permission.");
                return;
            }
            ulong id;
            if (!ulong.TryParse(args[0], out id))
            {
                player.ChatMessage("Please enter a valid number.");
                return;
            }


            RaycastHit ray;
            if (Physics.Raycast(player.eyes.HeadRay(), out ray))
            {
                BaseEntity ent = ray.GetEntity();
                if (ent != null)
                {
                    Puts(ent.ShortPrefabName);
                    ent.skinID = id;
                    ent.SendNetworkUpdate();
                }
            }
            else
            {
                player.ChatMessage("Invalid entity.");
            }
        }
    }
}