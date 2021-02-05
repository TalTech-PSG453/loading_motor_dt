using System.Collections.Generic;

namespace DigitalTwin.Utils {

    public class Pool<T> where T : new() {
        private Queue<T> pool = new Queue<T>();

        public T Get() {
            if(pool.Count == 0) {
                return new T();
            } else {
                return pool.Dequeue();
            }
        }

        public void Free(T obj) {
            pool.Enqueue(obj);
        }
    }
}