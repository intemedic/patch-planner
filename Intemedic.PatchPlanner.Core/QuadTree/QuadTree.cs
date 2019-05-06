using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Intemedic.PatchPlanner
{
    /// <summary>
    ///     This class efficiently stores and retrieves arbitrarily sized and positioned
    ///     objects in a quad-tree data structure.  This can be used to do efficient hit
    ///     detection or visibility checks on objects in a virtual canvas.
    ///     The object does not need to implement any special interface because the Rectangle Bounds
    ///     of those objects is handled as a separate argument to Insert.
    /// </summary>
    internal partial class QuadTree<T> where T : class
    {
        private Rectangle _bounds; // overall bounds we are indexing.
        private Quadrant _root;
        private IDictionary<T, Quadrant> _table;


        /// <summary>
        ///     This determines the overall quad-tree indexing strategy, changing this bounds
        ///     is expensive since it has to re-divide the entire thing - like a re-hash operation.
        /// </summary>
        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                _bounds = value;
                this.Reindex();
            }
        }

        /// <summary>
        ///     Insert a node with given bounds into this QuadTree.
        /// </summary>
        /// <param name="node">The node to insert</param>
        /// <param name="bounds">The bounds of this node</param>
        public void Insert(T node, Rectangle bounds)
        {
            if (bounds.Width == 0 || bounds.Height == 0)
            {
                throw new ArgumentException("bounds must be non-zero", nameof(bounds));
            }

            if (_root == null)
            {
                _root = new Quadrant(null, bounds);
            }

            var parent = _root.Insert(node, bounds);

            if (_table == null)
            {
                _table = new Dictionary<T, Quadrant>();
            }

            _table[node] = parent;
        }

        /// <summary>
        ///     Get a list of elements that intersect the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to test</param>
        /// <returns>List of zero or mode elements found inside the given bounds</returns>
        public IEnumerable<T> GetIntersectingElements(Rectangle bounds)
        {
            return this.GetIntersectingNodes(bounds).Select(n => n.Node);
        }

        /// <summary>
        ///     Get a list of elements that are contained by the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to test</param>
        /// <returns>List of zero or mode elements found inside the given bounds</returns>
        public IEnumerable<T> GetContainingElements(Rectangle bounds)
        {
            return this.GetContainingNodes(bounds).Select(n => n.Node);
        }

        /// <summary>
        ///     Get a list of the nodes that intersect the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to test</param>
        /// <returns>List of zero or mode nodes found inside the given bounds</returns>
        public bool HasNodesInside(Rectangle bounds)
        {
            return _root != null && _root.HasIntersectingNodes(bounds);
        }

        /// <summary>
        ///     Get list of nodes that intersect the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to test</param>
        /// <returns>The list of nodes intersecting the given bounds</returns>
        private IEnumerable<QuadNode> GetIntersectingNodes(Rectangle bounds)
        {
            var result = new List<QuadNode>();
            _root?.GetIntersectingNodes(result, bounds);
            return result;
        }

        /// <summary>
        ///     Get list of nodes that are contained by the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to test</param>
        /// <returns>The list of nodes intersecting the given bounds</returns>
        private IEnumerable<QuadNode> GetContainingNodes(Rectangle bounds)
        {
            var result = new List<QuadNode>();
            _root?.GetContainingNodes(result, bounds);
            return result;
        }

        /// <summary>
        ///     Remove the given node from this QuadTree.
        /// </summary>
        /// <param name="node">The node to remove</param>
        /// <returns>True if the node was found and removed.</returns>
        public bool Remove(T node)
        {
            if (_table == null)
            {
                return false;
            }

            if (!_table.TryGetValue(node, out var parent))
            {
                return false;
            }

            parent.RemoveNode(node);
            _table.Remove(node);
            return true;

        }

        /// <summary>
        ///     Rebuild all the Quadrants according to the current QuadTree Bounds.
        /// </summary>
        private void Reindex()
        {
            _root = null;
            foreach (var n in this.GetIntersectingNodes(_bounds))
            {
                this.Insert(n.Node, n.Bounds);
            }
        }
    }
}