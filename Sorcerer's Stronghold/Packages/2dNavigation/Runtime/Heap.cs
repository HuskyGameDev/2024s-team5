using System;
using System.Collections;
using System.Collections.Generic;

public class Heap<T> where T : IHeapItem<T>{
    T[] items;
    int size;

    public Heap(int maxSize){
        items = new T[maxSize];
    }

    public void add(T item){
        item.HeapIndex = size;
        items[size] = item;
        upheap(item);
        size++;
    }

    public T removeFirst(){
        T firstItem = items[0];
        size--;
        items[0] = items[size];
        items[0].HeapIndex = 0;
        downheap(items[0]);
        return firstItem;
    }

    public int getSize(){
        return size;
    }

    public void updateItem(T item){
        upheap(item);
    }

    public bool contains(T item){
        return Equals(items[item.HeapIndex], item);
    }

    void upheap(T item){
        while (true){
            int parentIndex = (item.HeapIndex - 1) / 2;
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0){
                swap(item, items[parentIndex]);
            }else{
                break;
            }
        }
    }

    void downheap(T item){
        while (true){
            int leftIndex = (item.HeapIndex * 2) + 1;
            int rightIndex = leftIndex + 1;
            int swapIndex = 0;

            if (leftIndex < size){
                swapIndex = leftIndex;
                if (rightIndex < size){
                    if (items[leftIndex].CompareTo(items[rightIndex]) < 0){
                        swapIndex = rightIndex;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0){
                    swap(item, items[swapIndex]);
                }else{
                    return;
                }
            }else{
                return;
            }
        }
    }

    void swap(T item1, T item2){
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;
        int item1Index = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = item1Index;
    }
}

public interface IHeapItem<T> : IComparable<T>{
    int HeapIndex{
        get;
        set;
    }
}