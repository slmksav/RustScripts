namespace Oxide.Plugins
{
    [Info("SimpleSwitchOn", "Sauli Savinainen", "0.1.1")]
    [Description("Simple switches always stay in  the on position.")]
    public class SimpleSwitchOn : RustPlugin
    {
        private void OnSwitchToggled(ElectricSwitch ss, BasePlayer player)
        {
            if (ss.prefabID == 2055550712)
            {
                if (ss.IsOn())
                {
                    ss.InvokeRepeating(() => ss.SetSwitch(true), 60f, 60f);
                }
                else ss.CancelInvoke();
            }
        }
    }
}