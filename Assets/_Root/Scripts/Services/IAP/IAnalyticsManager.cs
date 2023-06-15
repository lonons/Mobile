namespace Services.IAP
{
    internal interface IAnalyticsManager
    {
        public void SendGameStarted();
        public void SendMainMenuOpened();
    }
}