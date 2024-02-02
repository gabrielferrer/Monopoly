namespace Monopoly.Events
{
    public class DiceThrownArgs : System.EventArgs
    {
        public int FirstDie { get; set; }

        public int SecondDie { get; set; }
    }
}
