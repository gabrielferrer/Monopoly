using Monopoly.VM;
using System.Windows;

namespace Monopoly.UI
{
    public abstract class View<T> : Window where T : WindowViewModel
    {
        public View()
        {
            DataContext = CreateDataContext();
            DataContext.Window = this;
        }

        protected abstract T CreateDataContext();

        public new T DataContext
        {
            get
            {
                return (T)base.DataContext;
            }
            set
            {
                base.DataContext = value;
            }
        }
    }
}
