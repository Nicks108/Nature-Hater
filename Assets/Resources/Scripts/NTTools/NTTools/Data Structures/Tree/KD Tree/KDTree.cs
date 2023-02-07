using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools.NTTools.Data_Structures.Tree.KD_Tree
{
    internal class KDTreeGameObjects
    {
        private int _K = 0;
        public NodeGameObject RootNode;

        public struct NodeAndDistance
        {
            public NodeGameObject Node;
            public float Distance;
        };

        public int K
        {
            get { return _K; }
        }

        public KDTreeGameObjects(GameObject[] gameObjects, int Dimensions)
        {
            _K = Dimensions;
            RootNode = kdTree(gameObjects, this, 0);
        }

        public NodeGameObject kdTree(GameObject[] pointsList, KDTreeGameObjects Tree, int Depth = 0)
        {
            int kDimensions = Tree.K;
            int Axis = Depth%K;

            //Sort acending
            pointsList = pointsList.OrderByDescending(c => c.transform.position[Axis]).ToArray();

            int median = pointsList.Length/2;

            GameObject[] leftPoints;
            GameObject[] RightPoints;

            NTUtils.SplitArrayAtIndex(pointsList, median, out leftPoints, out RightPoints);
            RightPoints.ToList().Remove(RightPoints.ToList()[0]);
            return new NodeGameObject(_node: pointsList[median],
                LeftChild: kdTree(leftPoints, Tree, Depth + 1),
                RightChild: kdTree(RightPoints, Tree, Depth + 1)
                );
        }

        public GameObject FindClosestGameObject(GameObject point)
        {
            GameObject BestGuess = RootNode.Node;
            return FindClosestGameObject(point, BestGuess, RootNode);
        }

        public GameObject FindClosestGameObject(GameObject point, GameObject BestGuess, NodeGameObject CurrentNode)
        {
            if (NTUtils.SquaredEuclideanDistance(point.transform.position, CurrentNode.Node.transform.position)
                < NTUtils.SquaredEuclideanDistance(point.transform.position, BestGuess.transform.position))
            {
                BestGuess = CurrentNode.Node;

                if (CurrentNode.HasLeftNode && CurrentNode.HasRightNode)
                {
                    float distToLeft = NTUtils.SquaredEuclideanDistance(point.transform.position,
                        CurrentNode.LeftChildNode.Node.transform.position);
                    float distToRight = NTUtils.SquaredEuclideanDistance(point.transform.position,
                        CurrentNode.RightChildNode.Node.transform.position);

                    if (distToLeft < distToRight)
                        BestGuess = FindClosestGameObject(point, BestGuess, CurrentNode.LeftChildNode);
                    else
                        BestGuess = FindClosestGameObject(point, BestGuess, CurrentNode.RightChildNode);
                }
                else if (CurrentNode.HasLeftNode)
                {
                    BestGuess = FindClosestGameObject(point, BestGuess, CurrentNode.LeftChildNode);
                }
                else
                {
                    BestGuess = FindClosestGameObject(point, BestGuess, CurrentNode.RightChildNode);
                }

            }
            return BestGuess;
        }

        public GameObject[] FineKNearestGameObjects(GameObject point, GameObject[] NearestObjectsArray)
        {
            NodeAndDistance[] A = new NodeAndDistance[NearestObjectsArray.Length];

            A =FineKNearestGameObjects(A, RootNode, point);

            for (int i = 0; i < A.Length; i++)
            {
                NearestObjectsArray[i] = A[i].Node.Node;
            }

            return NearestObjectsArray;
        }

        public NodeAndDistance[] FineKNearestGameObjects(NodeAndDistance[] nearestGameObjects, NodeGameObject currentNode, GameObject Point)
        {
            nearestGameObjects = nearestGameObjects.OrderByDescending(c => c.Distance).ToArray();

            nearestGameObjects[0] = new NodeAndDistance()
                                    {
                                        Distance = NTUtils.SquaredEuclideanDistance(currentNode.Node.transform.position, Point.transform.position),
                                        Node = currentNode
                                    };

            if (
                NTUtils.SquaredEuclideanDistance(Point.transform.position,
                    currentNode.LeftChildNode.Node.transform.position) <
                NTUtils.SquaredEuclideanDistance(Point.transform.position,
                    currentNode.RightChildNode.Node.transform.position))
            {
                nearestGameObjects = FineKNearestGameObjects(nearestGameObjects, currentNode.LeftChildNode, Point);
            }
            else
            {
                nearestGameObjects = FineKNearestGameObjects(nearestGameObjects, currentNode.RightChildNode, Point);
            }

            return nearestGameObjects;
        }

        public void Add(GameObject gameObject)
        {
         //TODO finish add   
        }

        private void TraversTreeAdd(GameObject ToAdd, NodeGameObject CurrentNode)
        {
            
        }

        //ToDO finish subtract

        //TODO Test Tree
    }
}