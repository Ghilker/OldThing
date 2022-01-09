using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class SearchChildren
    {
        public static List<GameObject> SearchForTag(GameObject parent, string tag)
        {
            List<GameObject> childrenWithTag = new List<GameObject>();
            List<GameObject> allChildren = new List<GameObject>();

            int len = parent.transform.childCount;

            for (int i = 0; i < len; i++)
            {
                allChildren.Add(parent.transform.GetChild(i).gameObject);
            }

            foreach (GameObject child in allChildren)
            {
                if (child.tag == tag)
                    childrenWithTag.Add(child);
            }

            return childrenWithTag;
        }

        public static List<GameObject> AllChilds(GameObject root)
        {
            List<GameObject> result = new List<GameObject>();
            if (root.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in root.transform)
                {
                    Searcher(result, VARIABLE.gameObject);
                }
            }
            return result;
        }

        private static void Searcher(List<GameObject> list, GameObject root)
        {
            list.Add(root);
            if (root.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in root.transform)
                {
                    Searcher(list, VARIABLE.gameObject);
                }
            }
        }

        public static List<Vector3> FindAllPositions(Transform parent, string ignoreTag = null)
        {
            List<Vector3> positions = new List<Vector3>();
            if (!positions.Contains(parent.position))
            {
                positions.Add(parent.position);
            }

            foreach (Transform child in parent)
            {
                if (ignoreTag != null && child.gameObject.tag == ignoreTag)
                {
                    continue;
                }
                positions.Add(child.position);
                positions.AddRange(FindAllPositions(child));
            }
            return positions;
        }

    }

    public class RandomHelper
    {
        public static bool prob(int value = 50)
        {
            int chance = Random.Range(0, 101);
            if (chance <= value)
            {
                return true;
            }
            return false;
        }
        public static Vector3 RandomPosition(List<Vector3> positions)
        {
            int randomIndex = Random.Range(0, positions.Count);
            Vector3 randomPosition = positions[randomIndex];
            positions.RemoveAt(randomIndex);
            return randomPosition;
        }
    }

    public class DirectionalMovement
    {
        public static Vector3 MoveTo(direction dir, Vector3 startingPoint)
        {
            Vector3 endPoint = startingPoint;
            switch (dir)
            {
                case direction.NORTH:
                    endPoint += Vector3.up;
                    break;
                case direction.SOUTH:
                    endPoint += Vector3.down;
                    break;
                case direction.EAST:
                    endPoint += Vector3.right;
                    break;
                case direction.WEST:
                    endPoint += Vector3.left;
                    break;
            }
            return endPoint;
        }

        public static direction CheckVectorialDirection(Vector2 firstPoint, Vector2 secondPoint)
        {
            direction dir = direction.NORTH;
            float firstPointX = firstPoint.x;
            float firstPointY = firstPoint.y;
            float secondPointX = secondPoint.x;
            float secondPointY = secondPoint.y;
            if (firstPointY == secondPointY)
            {
                if (firstPointX > secondPointX)
                {
                    dir = direction.WEST;
                }
                else if (firstPointX < secondPointX)
                {
                    dir = direction.EAST;
                }
            }
            else if (firstPointX == secondPointX)
            {
                if (firstPointY > secondPointY)
                {
                    dir = direction.SOUTH;
                }
                else if (firstPointY < secondPointY)
                {
                    dir = direction.NORTH;
                }
            }
            return dir;
        }

        public static direction RotateDir(direction dir, bool clockwise = true)
        {
            if (clockwise == true)
            {
                if (dir == direction.NORTH)
                {
                    dir = direction.EAST;
                }
                else if (dir == direction.EAST)
                {
                    dir = direction.SOUTH;
                }
                else if (dir == direction.SOUTH)
                {
                    dir = direction.WEST;
                }
                else if (dir == direction.WEST)
                {
                    dir = direction.NORTH;
                }
            }
            else
            {
                if (dir == direction.NORTH)
                {
                    dir = direction.WEST;
                }
                else if (dir == direction.WEST)
                {
                    dir = direction.SOUTH;
                }
                else if (dir == direction.SOUTH)
                {
                    dir = direction.EAST;
                }
                else if (dir == direction.EAST)
                {
                    dir = direction.NORTH;
                }
            }
            return dir;
        }

        public static Vector3 MoveVectorToOffset(Vector3 oldPosition, direction dir, int offset = 1)
        {
            Vector3 connectorPosition = oldPosition;
            Vector3 centering = new Vector3(-offset, -offset, 0f);
            switch (dir)
            {
                case direction.NORTH:
                    connectorPosition += new Vector3(0f, 1f, 0f) * offset + centering;
                    break;
                case direction.SOUTH:
                    connectorPosition += new Vector3(0f, -1f, 0f) * offset + centering;
                    break;
                case direction.EAST:
                    connectorPosition += new Vector3(1f, 0f, 0f) * offset + centering;
                    break;
                case direction.WEST:
                    connectorPosition += new Vector3(-1f, 0f, 0f) * offset + centering;
                    break;
            }
            return connectorPosition;
        }

        public static direction ReverseDirection(direction dir)
        {
            direction newDir = dir;

            if (dir == direction.NORTH)
            {
                newDir = direction.SOUTH;
            }
            else if (dir == direction.SOUTH)
            {
                newDir = direction.NORTH;
            }
            else if (dir == direction.EAST)
            {
                newDir = direction.WEST;
            }
            else if (dir == direction.WEST)
            {
                newDir = direction.EAST;
            }

            return newDir;
        }

        public static bool VectorContainedInList(direction dir, Vector3 ourCoordinates, int offset, List<Vector3> allCoordinates)
        {
            bool isRoomPresent = false;
            Vector3 coordinateToCheck = GetVectorOffsetInDir(dir, ourCoordinates, offset);
            if (allCoordinates.Contains(coordinateToCheck))
            {
                isRoomPresent = true;
            }

            return isRoomPresent;
        }

        public static Vector3 GetVectorOffsetInDir(direction dir, Vector3 startingVector, int offset = 1)
        {
            Vector3 outputVector = startingVector;
            if (dir == direction.NORTH)
            {
                outputVector += new Vector3(0f, offset, 0f);
            }
            else if (dir == direction.SOUTH)
            {
                outputVector += new Vector3(0f, -offset, 0f);
            }
            else if (dir == direction.EAST)
            {
                outputVector += new Vector3(offset, 0f, 0f);
            }
            else if (dir == direction.WEST)
            {
                outputVector += new Vector3(-offset, 0f, 0f);
            }
            return outputVector;
        }
    }
}