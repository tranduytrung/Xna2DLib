using System;
using Dovahkiin.Repository;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Map;

namespace Dovahkiin.Screen
{
    public class GamePlayScreen : ComponentBase
    {
        private ScrollableView _mapView;

        public IsometricMap MapControl { get; private set; }

        public GamePlayScreen(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            #region Canvas

            var canvas = new Canvas();
            PresentableContent = canvas;

            #endregion

            #region Map control

            _mapView = new ScrollableView();
            _mapView.Decelerator = 1000;
            canvas.Children.Add(_mapView);

            #endregion

            #region Dock Panel

            var panel = new DockPanel();
            canvas.Children.Add(panel);

            #endregion

            DataContext.Initialize();
            OnMapChanged(null, null);
            DataContext.Current.MapChanged += OnMapChanged;
            base.LoadContent();
        }

        private void OnMapChanged(object sender, EventArgs e)
        {
            MapControl = Repository.Maps.GetControl(DataContext.Current.Map);
            _mapView.PresentableContent = MapControl;
        }
    }
}
