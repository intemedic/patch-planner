using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Intemedic.PatchPlanner
{
    internal partial class QuadTree<T>
    {
        /// <summary>
        /// The canvas is split up into four Quadrants and objects are stored in the quadrant that contains them
        /// and each quadrant is split up into four child Quadrants recursively.  Objects that overlap more than
        /// one quadrant are stored in the this.nodes list for this Quadrant.
        /// </summary>
        internal class Quadrant
        {
            private QuadNode _nodes; // nodes that overlap the sub quadrant boundaries.

            // The quadrant is subdivided when nodes are inserted that are 
            // completely contained within those subdivisions.
            private Quadrant _topLeft;
            private Quadrant _topRight;
            private Quadrant _bottomLeft;
            private Quadrant _bottomRight;

            /// <summary>
            /// Construct new Quadrant with a given bounds all nodes stored inside this quadrant
            /// will fit inside this bounds.  
            /// </summary>
            /// <param name="parent">The parent quadrant (if any)</param>
            /// <param name="bounds">The bounds of this quadrant</param>
            public Quadrant(Quadrant parent, Rectangle bounds)
            {
                this.Parent = parent;
                Debug.Assert(bounds.Width != 0 && bounds.Height != 0, "Cannot have empty bound");
                if (bounds.Width == 0 || bounds.Height == 0)
                {
                    throw new ArgumentException("bounds must be non-zero", nameof(bounds));
                }
                this.Bounds = bounds;
            }

            /// <summary>
            /// The parent Quadrant or null if this is the root
            /// </summary>
            internal Quadrant Parent { get; }

            /// <summary>
            /// The bounds of this quadrant
            /// </summary>
            internal Rectangle Bounds { get; }

            /// <summary>
            /// Insert the given node
            /// </summary>
            /// <param name="node">The node </param>
            /// <param name="bounds">The bounds of that node</param>
            /// <returns></returns>
            internal Quadrant Insert(T node, Rectangle bounds)
            {

                Debug.Assert(bounds.Width != 0 && bounds.Height != 0, "Cannot have empty bound");
                if (bounds.Width == 0 || bounds.Height == 0)
                {
                    throw new ArgumentException("bounds must be non-zero", nameof(bounds));
                }

                var toInsert = this;
                while (true)
                {
                    var w = toInsert.Bounds.Width / 2;
                    if (w < 1)
                    {
                        w = 1;
                    }
                    var h = toInsert.Bounds.Height / 2;
                    if (h < 1)
                    {
                        h = 1;
                    }

                    // assumption that the Rectangle struct is almost as fast as doing the operations
                    // manually since Rectangle is a value type.

                    var topLeft = new Rectangle(toInsert.Bounds.Left, toInsert.Bounds.Top, w, h);
                    var topRight = new Rectangle(toInsert.Bounds.Left + w, toInsert.Bounds.Top, w, h);
                    var bottomLeft = new Rectangle(toInsert.Bounds.Left, toInsert.Bounds.Top + h, w, h);
                    var bottomRight = new Rectangle(toInsert.Bounds.Left + w, toInsert.Bounds.Top + h, w, h);

                    Quadrant child = null;

                    // See if any child quadrants completely contain this node.
                    if (topLeft.Contains(bounds))
                    {
                        if (toInsert._topLeft == null)
                        {
                            toInsert._topLeft = new Quadrant(toInsert, topLeft);
                        }
                        child = toInsert._topLeft;
                    }
                    else if (topRight.Contains(bounds))
                    {
                        if (toInsert._topRight == null)
                        {
                            toInsert._topRight = new Quadrant(toInsert, topRight);
                        }
                        child = toInsert._topRight;
                    }
                    else if (bottomLeft.Contains(bounds))
                    {
                        if (toInsert._bottomLeft == null)
                        {
                            toInsert._bottomLeft = new Quadrant(toInsert, bottomLeft);
                        }
                        child = toInsert._bottomLeft;
                    }
                    else if (bottomRight.Contains(bounds))
                    {
                        if (toInsert._bottomRight == null)
                        {
                            toInsert._bottomRight = new Quadrant(toInsert, bottomRight);
                        }
                        child = toInsert._bottomRight;
                    }

                    if (child != null)
                    {
                        toInsert = child;
                    }
                    else
                    {
                        var n = new QuadNode(node, bounds);
                        if (toInsert._nodes == null)
                        {
                            n.Next = n;
                        }
                        else
                        {
                            // link up in circular link list.
                            var x = toInsert._nodes;
                            n.Next = x.Next;
                            x.Next = n;
                        }
                        toInsert._nodes = n;
                        return toInsert;
                    }
                }
            }

            /// <summary>
            /// Returns all nodes in this quadrant that intersect the given bounds.
            /// The nodes are returned in pretty much random order as far as the caller is concerned.
            /// </summary>
            /// <param name="nodes">List of nodes found in the given bounds</param>
            /// <param name="bounds">The bounds that contains the nodes you want returned</param>
            internal void GetIntersectingNodes(List<QuadNode> nodes, Rectangle bounds)
            {
                if (bounds.IsEmpty)
                {
                    return;
                }

                var w = this.Bounds.Width / 2;
                var h = this.Bounds.Height / 2;

                // assumption that the Rectangle struct is almost as fast as doing the operations
                // manually since Rectangle is a value type.

                var topLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top, w, h);
                var topRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top, w, h);
                var bottomLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top + h, w, h);
                var bottomRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top + h, w, h);

                // See if any child quadrants completely contain this node.
                if (topLeft.IntersectsWith(bounds))
                {
                    _topLeft?.GetIntersectingNodes(nodes, bounds);
                }

                if (topRight.IntersectsWith(bounds))
                {
                    _topRight?.GetIntersectingNodes(nodes, bounds);
                }

                if (bottomLeft.IntersectsWith(bounds))
                {
                    _bottomLeft?.GetIntersectingNodes(nodes, bounds);
                }

                if (bottomRight.IntersectsWith(bounds))
                {
                    _bottomRight?.GetIntersectingNodes(nodes, bounds);
                }

                GetIntersectingNodes(_nodes, nodes, bounds);
            }

            /// <summary>
            /// Walk the given linked list of QuadNodes and check them against the given bounds.
            /// Add all nodes that intersect the bounds in to the list.
            /// </summary>
            /// <param name="last">The last QuadNode in a circularly linked list</param>
            /// <param name="nodes">The resulting nodes are added to this list</param>
            /// <param name="bounds">The bounds to test against each node</param>
            private static void GetIntersectingNodes(
                QuadNode last, 
                ICollection<QuadNode> nodes, 
                Rectangle bounds)
            {
                if (last != null)
                {
                    var n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (n.Bounds.IntersectsWith(bounds))
                        {
                            nodes.Add(n);
                        }
                    } while (n != last);
                }
            }


            /// <summary>
            /// Returns all nodes in this quadrant that are contained by the given bounds.
            /// The nodes are returned in pretty much random order as far as the caller is concerned.
            /// </summary>
            /// <param name="nodes">List of nodes found in the given bounds</param>
            /// <param name="bounds">The bounds that contains the nodes you want returned</param>
            internal void GetContainingNodes(List<QuadNode> nodes, Rectangle bounds)
            {
                if (bounds.IsEmpty)
                {
                    return;
                }

                var w = this.Bounds.Width / 2;
                var h = this.Bounds.Height / 2;

                // assumption that the Rectangle struct is almost as fast as doing the operations
                // manually since Rectangle is a value type.

                var topLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top, w, h);
                var topRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top, w, h);
                var bottomLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top + h, w, h);
                var bottomRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top + h, w, h);

                // See if any child quadrants completely contain this node.
                if (topLeft.IntersectsWith(bounds))
                {
                    _topLeft?.GetContainingNodes(nodes, bounds);
                }

                if (topRight.IntersectsWith(bounds))
                {
                    _topRight?.GetContainingNodes(nodes, bounds);
                }

                if (bottomLeft.IntersectsWith(bounds))
                {
                    _bottomLeft?.GetContainingNodes(nodes, bounds);
                }

                if (bottomRight.IntersectsWith(bounds))
                {
                    _bottomRight?.GetContainingNodes(nodes, bounds);
                }

                GetContainingNodes(_nodes, nodes, bounds);
            }

            /// <summary>
            /// Walk the given linked list of QuadNodes and check them against the given bounds.
            /// Add all nodes that are contained by the bounds in to the list.
            /// </summary>
            /// <param name="last">The last QuadNode in a circularly linked list</param>
            /// <param name="nodes">The resulting nodes are added to this list</param>
            /// <param name="bounds">The bounds to test against each node</param>
            private static void GetContainingNodes(
                QuadNode last,
                ICollection<QuadNode> nodes,
                Rectangle bounds)
            {
                if (last != null)
                {
                    var n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (bounds.Contains(n.Bounds))
                        {
                            nodes.Add(n);
                        }
                    } while (n != last);
                }
            }

            /// <summary>
            /// Return true if there are any nodes in this Quadrant that intersect the given bounds.
            /// </summary>
            /// <param name="bounds">The bounds to test</param>
            /// <returns>boolean</returns>
            internal bool HasIntersectingNodes(Rectangle bounds)
            {
                if (bounds.IsEmpty)
                {
                    return false;
                }
                var w = this.Bounds.Width / 2;
                var h = this.Bounds.Height / 2;

                // assumption that the Rectangle struct is almost as fast as doing the operations
                // manually since Rectangle is a value type.

                var topLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top, w, h);
                var topRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top, w, h);
                var bottomLeft = new Rectangle(this.Bounds.Left, this.Bounds.Top + h, w, h);
                var bottomRight = new Rectangle(this.Bounds.Left + w, this.Bounds.Top + h, w, h);

                var found = false;

                // See if any child quadrants completely contain this node.
                if (topLeft.IntersectsWith(bounds) && _topLeft != null)
                {
                    found = _topLeft.HasIntersectingNodes(bounds);
                }

                if (!found && topRight.IntersectsWith(bounds) && _topRight != null)
                {
                    found = _topRight.HasIntersectingNodes(bounds);
                }

                if (!found && bottomLeft.IntersectsWith(bounds) && _bottomLeft != null)
                {
                    found = _bottomLeft.HasIntersectingNodes(bounds);
                }

                if (!found && bottomRight.IntersectsWith(bounds) && _bottomRight != null)
                {
                    found = _bottomRight.HasIntersectingNodes(bounds);
                }
                if (!found)
                {
                    found = HasIntersectingNodes(_nodes, bounds);
                }
                return found;
            }

            /// <summary>
            /// Walk the given linked list and test each node against the given bounds/
            /// </summary>
            /// <param name="last">The last node in the circularly linked list.</param>
            /// <param name="bounds">Bounds to test</param>
            /// <returns>Return true if a node in the list intersects the bounds</returns>
            private static bool HasIntersectingNodes(QuadNode last, Rectangle bounds)
            {
                if (last != null)
                {
                    var n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (n.Bounds.IntersectsWith(bounds))
                        {
                            return true;
                        }
                    } while (n != last);
                }
                return false;
            }

            /// <summary>
            /// Remove the given node from this Quadrant.
            /// </summary>
            /// <param name="node">The node to remove</param>
            /// <returns>Returns true if the node was found and removed.</returns>
            internal bool RemoveNode(T node)
            {
                var result = false;
                if (_nodes != null)
                {
                    var p = _nodes;
                    while (p.Next.Node != node && p.Next != _nodes)
                    {
                        p = p.Next;
                    }
                    if (p.Next.Node == node)
                    {
                        result = true;
                        var n = p.Next;
                        if (p == n)
                        {
                            // list goes to empty
                            _nodes = null;
                        }
                        else
                        {
                            if (_nodes == n)
                            {
                                _nodes = p;
                            }

                            p.Next = n.Next;
                        }
                    }
                }
                return result;
            }

        }
    }
}