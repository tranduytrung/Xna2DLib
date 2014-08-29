using tranduytrung.Xna.Core;
using tranduytrung.Xna.Map;

namespace tranduytrung.DragonCity.Template
{
    public interface ITemplate
    {
        DrawableObject PresentableContent { get; }
        DrawableObject ContextMenu { get; }
        /// <summary>
        /// Call this method when context menu is on
        /// </summary>
        void Start();
        /// <summary>
        /// Call this method when context menu is off
        /// </summary>
        void End();

        /// <summary>
        /// Bind data to template to prepare visual
        /// </summary>
        /// <param name="data">data for applying</param>
        void ApplyData( object data);
    }
}
