namespace Monopoly.Events
{
    public class CurrentPlayerChangedArgs : System.EventArgs
    {
        public VM.Player CurrentPlayer { get; set; }
    }
}
