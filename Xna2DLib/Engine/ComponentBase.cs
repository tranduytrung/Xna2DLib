using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tranduytrung.Xna.Core;


namespace tranduytrung.Xna.Engine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ComponentBase : DrawableGameComponent
    {
        protected SpriteBatch SpriteBatch { get; private set; }

        public DrawableObject PresentableContent { get; set; }

        public ComponentBase(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            var bufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var bufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            var interactiveObj = PresentableContent as InteractiveObject;
            if (interactiveObj != null)
                interactiveObj.MouseInputCore(new Vector2(Input.MouseState.X, Input.MouseState.Y));

            PresentableContent.Measure(new Size(bufferWidth, bufferHeight));
            PresentableContent.Arrange(new Rectangle(0,0,bufferWidth, bufferHeight));
            PresentableContent.RenderTransform();
            PresentableContent.Update();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            PresentableContent.PrepareVisual();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            PresentableContent.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (PresentableContent != null)
                PresentableContent.Dispose();

            if (SpriteBatch != null)
                SpriteBatch.Dispose();

            base.Dispose(disposing);
        }
    }
}
