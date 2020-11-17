using System;
using System.Collections.Generic;

namespace ChainableDemo
{

    /// <summary>
    /// Defines an object which can associate with other objects of the same type to form a collection.
    /// Chains of objects implementing Chainable can be managed by a simple "context" object called "Chain." Segmented
    /// chains are used to control access from scope/context
    /// 
    /// NOTE:
    /// Classes deriving Chainable<> must implement IDisposable. In Dispose(), the Chainable<> must call
    /// UnlinkSelfFromChain() so that the Chain is not broken.
    /// </summary>
    public abstract class Chainable : IDisposable
    {
        internal Chainable prevNode = null;
        internal Chainable nextNode = null;

        #region without Chain<T>

        /// <summary>
        /// Enumerates the items in the current Chain context from the Node's perspective. Enumerates self first.
        /// </summary>
        /// <returns>Enumerated nodes</returns>
        protected internal IEnumerable<T> GetNodesInChain<T>() where T : Chainable
        {
            // enumerate self
            yield return (T)this;
            // enumerate forward
            foreach (var thing in DereferenceForward(this))
            {
                yield return (T)thing;
            }
            // enumerate backward
            foreach (var thing in DereferenceBackward(this))
            {
                yield return (T)thing;
            }
        }

        /// <summary>
        /// Performs a linear search through the associated chain to find a node with the given UId
        /// </summary>
        /// <param name="uid">The unique identifier to use for search</param>
        /// <returns></returns>
        protected internal T SearchChain<T>(string uid) where T : Chainable
        {
            foreach (var item in GetNodesInChain<T>())
            {
                if (item.NodeId == uid)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the given node to the current chain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected internal void AddNodeToChain(Chainable node)
        {
            Chainable right = this.nextNode;
            Chainable left = this;
            HookUpNodes(left, node);
            HookUpNodes(node, right);
        }

        /// <summary>
        /// Un-links the node from its current chain
        /// </summary>
        protected internal void UnlinkFromChain()
        {
            Chainable left = prevNode;
            Chainable right = nextNode;
            HookUpNodes(left, right);

            // set nulls
            prevNode = null;
            nextNode = null;
        }

        #endregion

        #region helpers
        /// <summary>
        /// Helper to follow previous nodes backward to beginning, skipping 'node'
        /// </summary>
        /// <returns>Enumerated nodes</returns>
        private static IEnumerable<Chainable> DereferenceBackward(Chainable node)
        {
            Chainable current = node?.prevNode;
            while (current != null)
            {
                yield return current;
                current = current.prevNode;
            }
        }

        /// <summary>
        /// Helper to follow next nodes forward to the end, skipping 'node'
        /// </summary>
        /// <returns>Enumerated nodes</returns>
        private static IEnumerable<Chainable> DereferenceForward(Chainable node)
        {
            Chainable current = node?.nextNode;
            while (current != null)
            {
                yield return current;
                current = current.nextNode;
            }
        }

        /// <summary>
        /// Connects left and right nodes together
        /// </summary>
        /// <param name="left">Node to connect on left</param>
        /// <param name="right">Node to connect on right</param>
        private static void HookUpNodes(Chainable left, Chainable right)
        {
            if (left != null)
            {
                left.nextNode = right;
            }

            if (right != null)
            {
                right.prevNode = left;
            }
        }

        #endregion

        /// <summary>
        /// Object implementing Chainable<T> must unlink itself upon being destroyed
        /// </summary>
        public abstract void Dispose();

        public abstract string NodeId { get; }
    }

}
