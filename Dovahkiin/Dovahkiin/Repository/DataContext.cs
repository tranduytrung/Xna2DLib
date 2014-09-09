using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Constant;
using Dovahkiin.Maps;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Creature;

namespace Dovahkiin.Repository
{
    public class DataContext
    {
        private Map _map;
        private ICanvasObject _controllingObject;
        public static DataContext Current { get; private set; }

        public static void LoadDefault()
        {
            Current = new DataContext();
        }

        public event EventHandler<PropertyChangeEventArgs> MapChanged;
        public event EventHandler<PropertyChangeEventArgs> ControllingObjectChanged;

        public void Initialize()
        {
            Map = Maps.GetModel(Maps.SagaLand);

            var controllingObject = new Human {MovingSpeed = 50, ResouceId = Textures.Poo, X = 100, Y = 100};
            controllingObject.AddActionHandler(new MoveActionHandler());

            Map.AddObject(controllingObject);
            ControllingObject = controllingObject;
        }

        protected virtual void OnControllingObjectChanged(PropertyChangeEventArgs e)
        {
            EventHandler<PropertyChangeEventArgs> handler = ControllingObjectChanged;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnMapChanged(PropertyChangeEventArgs e)
        {
            EventHandler<PropertyChangeEventArgs> handler = MapChanged;
            if (handler != null) handler(this, e);
        }

        public Map Map
        {
            get { return _map; }
            private set
            {
                if (_map == value) return;
                var old = _map;
                _map = value;
                OnMapChanged(new PropertyChangeEventArgs(old, value));
            }
        }

        public ICanvasObject ControllingObject
        {
            get { return _controllingObject; }
            private set
            {
                if (_controllingObject == value) return;
                var old = _controllingObject;
                _controllingObject = value;
                OnControllingObjectChanged(new PropertyChangeEventArgs(old, value));
            }
        }
    }

    public class PropertyChangeEventArgs : EventArgs
    {
        public PropertyChangeEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
    }
}
