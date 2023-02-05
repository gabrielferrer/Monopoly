using System.Windows.Media;

namespace Monopoly.VM
{
    public class Player : ViewModelBase
    {
        private int money;

        public Player(string name, Color color)
        {
            Name = name;
            PlayerColor = new SolidColorBrush(color);
        }

        public void Clear()
        {
            money = 0;
        }

        public static bool operator == (Player a, Player b)
        {
            return a != null ? a.Name.Equals(b) : (object)b == null;
        }

        public static bool operator !=(Player a, Player b)
        {
            return a != null ? !a.Name.Equals(b) : (object)b != null;
        }

        public override bool Equals(object obj)
        {
            return obj is Player player && Name == player.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Player));
            stream.WriteLine($"Name: {Name}");
            stream.WriteLine($"Money: {money}");
        }
#endif
        public string Name { get; }

        public double PlayerWidth => UI.Constants.PlayerSize;

        public double PlayerHeight => UI.Constants.PlayerSize;

        public Brush PlayerColor { get; }
    }
}
