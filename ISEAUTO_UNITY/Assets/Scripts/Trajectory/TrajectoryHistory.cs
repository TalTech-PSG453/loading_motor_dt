using System.Collections.Generic;
using UnityEngine;

public class TrajectoryHistory {
    public class Branch {
        private List<Vector3> nodes = new List<Vector3>();

        public void AddPoint(Vector3 point) {
            nodes.Add(point);
        }

        public void Clear() {
            nodes.Clear();
        }

        public int Count {
            get {
                return nodes.Count;
            }
        }

        public Vector3 RemoveLast() {
            var point = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);
            return point;
        }

        public List<Vector3> Nodes {
            get {
                return nodes;
            }
        }
    }

    private Branch currentBranch;

    public void NewPoint(Vector3 point) {
        Master.AddPoint(point);
        currentBranch = null;
    }

    public void NewRealPoint(Vector3 point) {
        RealPath.AddPoint(point);
    }

    public void Undo() {
        if(Master.Count == 0) {
            return;
        }

        var point = Master.RemoveLast();
        if(currentBranch == null) {
            currentBranch = new Branch();
            Branches.Add(currentBranch);
        }
        currentBranch.AddPoint(point);
    }

    public List<Branch> Branches { get; } = new List<Branch>();

    public Branch Master { get; } = new Branch();
    public Branch RealPath { get; } = new Branch();
}
