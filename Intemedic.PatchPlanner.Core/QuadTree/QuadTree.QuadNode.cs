using System.Drawing;

namespace Intemedic.PatchPlanner
{
    internal partial class QuadTree<T>
    {
        /// <summary>
        /// Each node stored in the tree has a position, width & height.
        /// </summary>
        internal class QuadNode
        {
            /// <summary>
            /// Construct new QuadNode to wrap the given node with given bounds
            /// </summary>
            /// <param name="node">The node</param>
            /// <param name="bounds">The bounds of that node</param>
            public QuadNode(T node, Rectangle bounds)
            {
                this.Node = node;
                this.Bounds = bounds;
            }

            /// <summary>
            /// The node
            /// </summary>
            public T Node { get; set; }

            /// <summary>
            /// The Rectangle bounds of the node
            /// </summary>
            public Rectangle Bounds { get; }

            /// <summary>
            /// QuadNodes form a linked list in the Quadrant.
            /// </summary>
            public QuadNode Next { get; set; }
        }
    }
}