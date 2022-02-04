using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Node<T>//this class is Item of ny List
    {
        #region properties
        public T Value { get; set; } //our value
        public Node<T> next { get; set; } //container for next list item
        public Node<T> prev { get; set; }//container for previous list item
        #endregion
        #region constructor
        public Node(T value)=>Value = value;
        #endregion
    }
    internal class MyOwnList<T>:IEnumerable<T>// List class wich can
    {                                         // include any types
        #region fields
        private Node<T> head;//first item
        private Node<T> tail;//last item
        private int count;//list size
        #endregion
        #region constructor
        public MyOwnList() 
        { head = null; tail = null; count = 0; }
        #endregion
        #region methods
        public int Count => count;//returns list size
        public bool IsEmpty => head == null;//returns value:if list is empty
        public T this [int index]//indexer
        {
            get 
            {
                if (index < 0 || index >= count) 
                    throw new ArgumentOutOfRangeException("index");
                Node<T> current = head;
                for (int i = 0; i < index; i++)
                   current= current.next;
                return current.Value;
            }
        }
        public void AddToHead(T value)//adds item to head of list
        {
            Node<T> node = new Node<T>(value);
            if (count==0)
            {
                node.next = null;
                node.prev = null;
                head = node;
                tail = node;
            }
            else if(count!=0)
            {
                node.next = head;
                node.prev = null;
                head.prev = node;
                head = node;
            }
            count++;
        }
        public void AddToTail(T value)//adds item to end of list
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
        public void Insert(int index,T value)//inserts item in list by index
        {
            if (index < 0 || index > count)
                throw new ArgumentOutOfRangeException("index");
            if(index==count) AddToTail(value);
            else if (index == 0 && count != 0) AddToHead(value);
            else
            {
                Node<T> current = head;
                for (int i = 0; i < index; i++)current = current.next;
                Node<T> node = new Node<T> (value);
                node.prev = current.prev;
                (node.prev).next = node;
                node.next = current;
                (node.next).prev = node;
            }
            count++;
        }
        public void Remove(int index)//removes item by index
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
                for (int i = 0; i < index; i++)current = current.next;
                (current.prev).next = current.next;
                (current.next).prev = current.prev;
            }
            count--;
        }
        public void Clear()//clears all items
        {
            head = null;
            tail = null;
            count = 0;
        }
        public IEnumerator<T> GetEnumerator()// gets items
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
        IEnumerator IEnumerable.GetEnumerator()//I don't know what it is
        {                                      //but it's necessary
            return GetEnumerator();
        }
        #endregion
    }
}
