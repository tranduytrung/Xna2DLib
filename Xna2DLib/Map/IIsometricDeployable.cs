using System.Collections.Generic;

namespace tranduytrung.Xna.Map
{
    public interface IIsometricDeployable
    {
        /// <summary>
        /// Left most position
        /// This property use for display sprite
        /// </summary>
        IsometricCoords Left { get; }


        /// <summary>
        /// Right most position
        /// This property use for display sprite
        /// </summary>
        IsometricCoords Right { get; }

        /// <summary>
        /// Top most position
        /// This property use for display sprite
        /// </summary>
        IsometricCoords Top { get; }

        /// <summary>
        /// Bottom most position, this is also where the map call draw method to sprite
        /// This property use for display sprite
        /// </summary>
        IsometricCoords Bottom { get; }

        /// <summary>
        /// Level
        /// </summary>
        int Level { get; }


        /// <summary>
        /// Collection of positions that will be deployed
        /// </summary>
        IEnumerable<IsometricCoords> Formation { get; }

        /// <summary>
        /// Determines where to deploy including Left, Top, Right, Bottom and Formation properties
        /// Assume that base position is (0,0)
        /// </summary>
        /// <param name="position">the position of isometric coordinates</param>
        /// <param name="x">the ratio of position in base position (0,0)</param>
        /// <param name="y">the ratio of position in base position (0,0)</param>
        void Deploy(IsometricCoords position, double x = 0.5, double y = 0.5);
    }
}
