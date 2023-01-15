using System.Collections.Generic;

namespace Monopoly.Spaces
{
    abstract class Space
    {
        protected List<Player> visitors;

        public Space()
        {
            visitors = new List<Player>();
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Space));
            foreach (var line in Text) stream.WriteLine($"{line}");
            foreach (var visitor in visitors) visitor.Log(stream);
            stream.WriteLine("--------------------");
        }
#endif

        public abstract IEnumerable<string> Text { get; }
    }
}
