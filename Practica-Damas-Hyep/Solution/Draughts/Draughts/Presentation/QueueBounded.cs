using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Draughts.Presentation
{
    class BoundedQueue<T>
    {
        T[] que;
        int head;       // remove from head
        int tail;       // insert at tail

        public BoundedQueue(int quesize)
        {
            head = tail = -1;
            que = new T[quesize];
        }

        public void enqueue(T elem)  // if next index to tail == head => Q is FULL
        {
            int newIndex = nextIndex(tail);
            if (newIndex == head)
                Console.Write("Error al introducir valores.");
            tail = newIndex;
            que[newIndex] = elem;
            if (head == -1)
                head = 0;
        }

        public T dequeue()  // After removing from head, if that was the only element in Q
        // Mark Q to be empty by setting head and tail to -1
        {
            if (head == -1)
                Console.Write("Cola vacía.");
            T elem = que[head];
            que[head] = default(T);
            if (head == tail)
            {
                head = tail = -1;
            }
            else
            {
                head = nextIndex(head);
            }
            return elem;
        }

        public int nextIndex(int index)
        {
            return (index + 1) % que.Length;
        }
        public T getNext(int n)
        {
            int i = (n + 1) % que.Length;
            return que[i];
        }

        public T getPrev(int m)
        {
            int i = (m - 1) % que.Length;
            return que[i];
        }
    }
}