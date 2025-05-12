using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public class Heap<K, D> where K : IComparable<K>
    {

        // This is a nested Node class whose purpose is to represent a node of a heap.
        private class Node : IHeapifyable<K, D>
        {
            // The Data field represents a payload.
            public D Data { get; set; }
            // The Key field is used to order elements with regard to the Binary Min (Max) Heap Policy, i.e. the key of the parent node is smaller (larger) than the key of its children.
            public K Key { get; set; }
            // The Position field reflects the location (index) of the node in the array-based internal data structure.
            public int Position { get; set; }

            public Node(K key, D value, int position)
            {
                Data = value;
                Key = key;
                Position = position;
            }

            // This is a ToString() method of the Node class.
            // It prints out a node as a tuple ('key value','payload','index')}.
            public override string ToString()
            {
                return "(" + Key.ToString() + "," + Data.ToString() + "," + Position + ")";
            }
        }

        // ---------------------------------------------------------------------------------
        // Here the description of the methods and attributes of the Heap<K, D> class starts

        public int Count { get; private set; }

        // The data nodes of the Heap<K, D> are stored internally in the List collection. 
        // Note that the element with index 0 is a dummy node.
        // The top-most element of the heap returned to the user via Min() is indexed as 1.
        private List<Node> data = new List<Node>();

        // We refer to a given comparer to order elements in the heap. 
        // Depending on the comparer, we may get either a binary Min-Heap or a binary  Max-Heap. 
        // In the former case, the comparer must order elements in the ascending order of the keys, and does this in the descending order in the latter case.
        private IComparer<K> comparer;

        // We expect the user to specify the comparer via the given argument.
        public Heap(IComparer<K> comparer)
        {
            this.comparer = comparer;

            // We use a default comparer when the user is unable to provide one. 
            // This implies the restriction on type K such as 'where K : IComparable<K>' in the class declaration.
            if (this.comparer == null) this.comparer = Comparer<K>.Default;

            // We simplify the implementation of the Heap<K, D> by creating a dummy node at position 0.
            // This allows to achieve the following property:
            // The children of a node with index i have indices 2*i and 2*i+1 (if they exist).
            data.Add(new Node(default(K), default(D), 0));
        }

        // This method returns the top-most (either a minimum or a maximum) of the heap.
        // It does not delete the element, just returns the node casted to the IHeapifyable<K, D> interface.
        public IHeapifyable<K, D> Min()
        {
            if (Count == 0) throw new InvalidOperationException("The heap is empty.");
            return data[1];
        }

        // Insertion to the Heap<K, D> is based on the private UpHeap() method
        public IHeapifyable<K, D> Insert(K key, D value)
        {
            Count++;
            Node node = new Node(key, value, Count);
            data.Add(node);
            UpHeap(Count);
            return node;
        }

        private void UpHeap(int start)
        {
            int position = start;
            while (position != 1)
            {
                if (comparer.Compare(data[position].Key, data[position / 2].Key) < 0) Swap(position, position / 2);
                position = position / 2;
            }
        }

        // This method swaps two elements in the list representing the heap. 
        // Use it when you need to swap nodes in your solution, e.g. in DownHeap() that you will need to develop.
        private void Swap(int from, int to)
        {
            Node temp = data[from];
            data[from] = data[to];
            data[to] = temp;
            data[to].Position = to;
            data[from].Position = from;
        }

        public void Clear()
        {
            for (int i = 0; i <= Count; i++) data[i].Position = -1;
            data.Clear();
            data.Add(new Node(default(K), default(D), 0));
            Count = 0;
        }

        public override string ToString()
        {
            if (Count == 0) return "[]";
            StringBuilder s = new StringBuilder();
            s.Append("[");
            for (int i = 0; i < Count; i++)
            {
                s.Append(data[i + 1]);
                if (i + 1 < Count) s.Append(",");
            }
            s.Append("]");
            return s.ToString();
        }

        // TODO: Your task is to implement all the remaining methods.
        // Read the instruction carefully, study the code examples from above as they should help you to write the rest of the code.
        // Deletes and returns the top element (min or max) from the heap.
        public IHeapifyable<K, D> Delete()
        {
            if (Count == 0)
                throw new InvalidOperationException("The heap is empty.");

            Node top = data[1];            //save top node to return later
            Node last = data[Count];       //last node in the heap

            data[1] = last;                //move last node top
            data[1].Position = 1;          //update its position to root

            data.RemoveAt(Count);         // Remove the duplicate at the end
            Count--;                      // Decrease the heap size

            if (Count > 0)
                DownHeap(1);              // Restore heap order

            top.Position = -1;            //mark the removed node
            return top;                   //return the original top element
        }


        //shifts down the node at a position to restore heap ordering 
        private void DownHeap(int position)
        {
            while (2 * position <= Count) //checks if the current node left child
            {
                int child = 2 * position; //start with the left child

                //check if right child exist and is smaller than the left child byusing comparer
                if (child + 1 <= Count && comparer.Compare(data[child + 1].Key, data[child].Key) < 0)//Choose the child depends on heap type
                    child++; //choose right child 

                //if parent is smaller/larger than child, stop
                if (comparer.Compare(data[child].Key, data[position].Key) >= 0)  //depends on comparer
                    break;

                Swap(position, child);  //swap with child
                position = child;       //continue down the tree
            }
        }

        //builds a minimum binary heap using the specified data according to the bottom-up approach.
        //builds the heap from key-value arrays using bottom-up heapify
        public IHeapifyable<K, D>[] BuildHeap(K[] keys, D[] values)
        {
            //ensure heap is empty before building
            if (Count != 0)
                throw new InvalidOperationException("Heap is not empty.");

            //ensure keys and values are of equal length
            if (keys.Length != values.Length)
                throw new ArgumentException("Keys and values must be of equal length.");

            Count = keys.Length;  //key is like a priority of the node inside heap

            //create an array to return node references in the same order as input
            IHeapifyable<K, D>[] nodes = new IHeapifyable<K, D>[Count];

            //create nodes then populate the heap list and return array
            for (int i = 0; i < Count; i++)
            {
                var node = new Node(keys[i], values[i], i + 1); //from parameter 
                data.Add(node);    //store in internal heap
                nodes[i] = node;   //save reference for returning
            }

            //bottom up heapify for heap property   
            for (int i = Count / 2; i >= 1; i--) //parent index at count/2
                DownHeap(i);  //estore heap ordering for the subtree rooted at index i

            //return the array of node references in original insertion order
            return nodes;
        }

        //decreases the key of a node and restores heap property
        public void DecreaseKey(IHeapifyable<K, D> element, K new_key)
        {
            Node node = data[element.Position]; //access the node stored at a position

            //check if the passed element is still the same object in the heap
            if (node != element)
                throw new InvalidOperationException("Node no longer matches heap structure.");

            //new key less than current key depending on comparer
            if (comparer.Compare(new_key, node.Key) > 0)
                throw new InvalidOperationException("New key must be less than or equal to current key.");

            node.Key = new_key;            //update the key
            UpHeap(node.Position);         //restore heap order by sifting up
        }

    }
}
