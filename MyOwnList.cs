using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_3
{
    class Node<T>
    {
        #region properties
        public T Value { get; set; } 
        public Node<T> next { get; set; } 
        public Node<T> prev { get; set; }
        #endregion
        #region constructor
        public Node(T value) => Value = value;
        #endregion
    }
    internal class MyOwnList<T> : IEnumerable<T>
    {                                         
        #region fields
        private Node<T> head;
        private Node<T> tail;
        private int count;
        #endregion
        #region constructor
        public MyOwnList()
        { head = null; tail = null; count = 0; }
        #endregion
        #region methods
        public int Count => count;
        public bool IsEmpty => head == null;
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new ArgumentOutOfRangeException("index");
                Node<T> current = head;
                for (int i = 0; i < index; i++)
                    current = current.next;
                return current.Value;
            }
        }
        public void AddToHead(T value)
        {
            Node<T> node = new Node<T>(value);
            if (count == 0)
            {
                node.next = null;
                node.prev = null;
                head = node;
                tail = node;
            }
            else if (count != 0)
            {
                node.next = head;
                node.prev = null;
                head.prev = node;
                head = node;
            }
            count++;
        }
        public void AddToTail(T value)
        {
            Node<T> node = new Node<T>(value);
            if (count == 0)
            {
                node.next = null;
                node.prev = null;
                head = node;
                tail = node;
            }
            else if (count != 0)
            {
                node.next = null;
                node.prev = tail;
                tail.next = node;
                tail = node;
            }
            count++;
        }
        public void Insert(int index, T value)
        {
            if (index < 0 || index > count)
                throw new ArgumentOutOfRangeException("index");
            if (index == count) AddToTail(value);
            else if (index == 0 && count != 0) AddToHead(value);
            else
            {
                Node<T> current = head;
                for (int i = 0; i < index; i++) current = current.next;
                Node<T> node = new Node<T>(value);
                node.prev = current.prev;
                (node.prev).next = node;
                node.next = current;
                (node.next).prev = node;
            }
            count++;
        }
        public void Remove(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException("index");
            Node<T> current = head;
            if (index == 0 && count == 1)
            {
                head = null;
                tail = null;
            }
            else if (index == 0 && count > 1)
            {
                head = current.next;
                (current.next).prev = null;
            }
            else if (index > 0 && index == count - 1)
            {
                for (int i = 0; i < index; i++) current = current.next;
                tail = current.prev;
                (current.prev).next = null;
            }
            else
            {
                for (int i = 0; i < index; i++) current = current.next;
                (current.prev).next = current.next;
                (current.next).prev = current.prev;
            }
            count--;
        }
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (count != 0)
            {
                Node<T> current = head;
                while (current != null)
                {
                    yield return current.Value;
                    current = current.next;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {                                      
            return GetEnumerator();
        }
        #endregion
    }
}