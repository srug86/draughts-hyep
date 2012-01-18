using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Draughts.Presentation
{
    /// <summary>
    /// Clase que implementa una cola circular.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class BoundedQueue<T>
    {
        /// <summary>
        /// Cola de tipo T.
        /// </summary>
        T[] que;
        /// <summary>
        /// Principio de la cola.
        /// </summary>
        int head;       
        /// <summary>
        /// Final de la cola.
        /// </summary>
        int tail;     

        /// <summary>
        /// Constructor de la cola circular <see cref="BoundedQueue&lt;T&gt;"/>.
        /// </summary>
        /// <param name="quesize">Tamaño.</param>
        public BoundedQueue(int quesize)
        {
            head = tail = -1;
            que = new T[quesize];
        }

        /// <summary>
        /// Método para mete elementos en la cola.
        /// </summary>
        /// <param name="elem">Elemento de tipo T.</param>
        public void enqueue(T elem)
        {
            int newIndex = nextIndex(tail);
            if (newIndex == head)
                Console.Write("Error al introducir valores.");
            tail = newIndex;
            que[newIndex] = elem;
            if (head == -1)
                head = 0;
        }

        /// <summary>
        /// Eliminar de la cola.
        /// </summary>
        /// <returns>Tipo T</returns>
        public T dequeue()
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

        /// <summary>
        /// Siguiente indice de la cola.
        /// </summary>
        /// <param name="index">Índice.</param>
        /// <returns>Índice</returns>
        public int nextIndex(int index)
        {
            return (index + 1) % que.Length;
        }
        /// <summary>
        /// Siguiente elemento de la cola.
        /// </summary>
        /// <param name="n">Índice.</param>
        /// <returns>Tipo T.</returns>
        public T getNext(int n)
        {
            int i = (n + 1) % que.Length;
            return que[i];
        }

        /// <summary>
        /// Elemento anterior de la cola.
        /// </summary>
        /// <param name="m">Índice.</param>
        /// <returns>Tipo T.</returns>
        public T getPrev(int m)
        {
            if (m <= 0) m = m + 10;
            int i = (m - 1) % que.Length;
            return que[i];
        }
    }
}