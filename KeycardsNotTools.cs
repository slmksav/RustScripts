namespace Oxide.Plugins
{
    [Info("Keycards =! Tools", "Sauli Savinainen", "0.1.0")]
    [Description("Keycards  will  not decay like tools.")]
    public class Keycards : RustPlugin
    {
        private object OnLoseCondition(Item item, float amount)
        {
            if (item.info.shortname.Contains("keycard"))
            {
                item.condition = item.maxCondition;
                return true;
            }

            return null;
        }
    }
}