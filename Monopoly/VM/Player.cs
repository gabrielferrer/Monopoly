using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Monopoly.VM
{
    public class Player : ViewModelBase
    {
        public Player(string name, Color color)
        {
            Name = name;
            PlayerColor = new SolidColorBrush(color);
            TitleDeeds = new ObservableCollection<Titles.TitleDeed>();
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
            stream.WriteLine($"Money: {Money}");
        }
#endif
        public string Name { get; }

        public int Money { get; set; }

        public ObservableCollection<Titles.TitleDeed> TitleDeeds { get; }

        public double PlayerWidth => UI.Constants.PlayerSize;

        public double PlayerHeight => UI.Constants.PlayerSize;

        public Brush PlayerColor { get; }
    }
}
