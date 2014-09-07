using System;
using Dovahkiin.Maps;

namespace Dovahkiin.Repository
{
    public class DataContext
    {
        private Map _map;
        public static DataContext Current { get; private set; }

        public static void Initialize()
        {
            Current = new DataContext();
            Current.Map = Maps.GetModel(Maps.SagaLand);
        }

        public Map Map
        {
            get { return _map; }
            private set
            {
                if (_map == value) return;
                _map = value;
                OnMapChanged();
            }
        }

        public event EventHandler MapChanged;

        protected virtual void OnMapChanged()
        {
            EventHandler handler = MapChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
