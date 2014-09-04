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
            canvas.Children.Add(_mapView);

            MapControl = Maps.GetMap(MapName.Sagaland);
            _mapView.PresentableContent = MapControl;

            #endregion

            #region Dock Panel

            var panel = new DockPanel();
            canvas.Children.Add(panel);

            #endregion

            base.LoadContent();
        }
    }
}
