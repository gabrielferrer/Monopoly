namespace Monopoly.Events
{
    public class DiceThrownArgs : System.EventArgs
    {
        public VM.Player CurrentPlayer { get; set; }

        public int FirstDie { get; set; }

        public int SecondDie { get; set; }
    }
}
