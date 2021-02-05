using UnityEngine;

namespace DigitalTwin.ROS {

    public abstract class ServiceArgs { }
    public abstract class ServiceValues { }

    public abstract class ServiceConsumer<A, V> : MonoBehaviour where A : ServiceArgs, new () where V : ServiceValues  {
        public string service;

        private Bridge bridge;
        
        public void CallService(A args) {
            if(bridge == null) {
                bridge = FindObjectOfType<Bridge>();
            }
            bridge.CallService<A, V>(service, args, new Bridge.ServiceCallback(ServiceResponse));
        }

        public abstract void ServiceResponse(V values);

        internal void ServiceResponse(ServiceValues values) {
            ServiceResponse((V)values);
        }
    }
}