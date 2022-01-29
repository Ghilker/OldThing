using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        public static List<GameObject> SearchAllChildTag(GameObject root, string tag)
        {
            List<GameObject> childrenWithTag = new List<GameObject>();
            List<GameObject> allChildren = new List<GameObject>();
            allChildren = AllChilds(root);

            foreach (GameObject child in allChildren)
            {
                if (child.tag != tag)
                {
                    continue;
                }
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

    public class ListHelper
    {
        public static void ListToDebug<T>(List<T> listToDebug)
        {
            string output = "List contents: ";
            foreach (var listObj in listToDebug)
            {
                output += listObj.ToString() + ", ";
            }
            Debug.Log(output);
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
        public static Vector3 RandomPosition(List<Vector3> positions, bool removeFromList = false)
        {
            if (positions.Count < 1)
            {
                Debug.LogError("List count less than 1");
                return Vector3.zero;
            }
            int randomIndex = Random.Range(0, positions.Count);
            Vector3 randomPosition = positions[randomIndex];
            if (removeFromList)
            {
                positions.Remove(randomPosition);
            }
            return randomPosition;
        }
        public static void ShuffleList<T>(List<T> listToRandomize)
        {
            for (int i = 0; i < listToRandomize.Count; i++)
            {
                var temp = listToRandomize[i];
                int randomIndex = Random.Range(i, listToRandomize.Count);
                listToRandomize[i] = listToRandomize[randomIndex];
                listToRandomize[randomIndex] = temp;
            }
        }

        public static int GetRandomWeightedIndex(float[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;

            float weight;
            float total = 0;
            int i;
            for (i = 0; i < weights.Length; i++)
            {
                weight = weights[i];
                if (float.IsInfinity(weight)) return i;
                else if (weight >= 0f && !float.IsNaN(weight)) { total += weights[i]; }
            }

            float r = Random.value;
            float s = 0f;

            for (i = 0; i < weights.Length; i++)
            {
                weight = weights[i];
                if (float.IsNaN(weight) || weight <= 0f) continue;

                s += weight / total;
                if (s >= r) return i;
            }

            return -1;
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
                    endPoint += Vector3.forward;
                    break;
                case direction.SOUTH:
                    endPoint += Vector3.back;
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

        public static direction CheckVectorialDirection(Vector3 firstPoint, Vector3 secondPoint)
        {
            direction dir = direction.NORTH;
            float firstPointX = firstPoint.x;
            float firstPointZ = firstPoint.z;
            float secondPointX = secondPoint.x;
            float secondPointZ = secondPoint.z;
            if (firstPointZ == secondPointZ)
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
                if (firstPointZ > secondPointZ)
                {
                    dir = direction.SOUTH;
                }
                else if (firstPointZ < secondPointZ)
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

        public static Vector3 GetVectorOffsetInDir(direction dir, Vector3 startingVector, int offsetX = 0, int offsetY = 0, int offsetZ = 0)
        {
            Vector3 outputVector = startingVector;
            if (dir == direction.NORTH)
            {
                outputVector += new Vector3(0f, 0f, offsetZ);
            }
            else if (dir == direction.SOUTH)
            {
                outputVector += new Vector3(0f, 0f, -offsetZ);
            }
            else if (dir == direction.EAST)
            {
                outputVector += new Vector3(offsetX, 0f, 0f);
            }
            else if (dir == direction.WEST)
            {
                outputVector += new Vector3(-offsetX, 0f, 0f);
            }
            return outputVector;
        }

    }

    public class WorldTexGen
    {
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontsize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontsize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontsize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontsize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }

    public class MouseCamera
    {
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }

    public class NavMeshHelper
    {
        public static Vector3 RandomNavmeshLocation(float closestRadius, float furthestRadius, GameObject navMeshAgent)
        {
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            bool foundPosition = false;
            while (!foundPosition)
            {
                Vector3 randomDirection = RandomPositionAroundCircle(closestRadius, furthestRadius, navMeshAgent);
                if (NavMesh.SamplePosition(randomDirection, out hit, furthestRadius, NavMesh.AllAreas))
                {
                    finalPosition = hit.position;
                    foundPosition = true;
                }
            }
            return finalPosition;
        }

        public static Vector3 RandomPositionAroundCircle(float closestRadius, float furthestRadius, GameObject gameObject)
        {
            Vector3 randomDirection = Random.insideUnitSphere * furthestRadius;
            randomDirection += gameObject.transform.position;
            randomDirection.y = gameObject.transform.position.y;
            float distance = Vector3.Distance(randomDirection, gameObject.transform.position);
            while (distance > furthestRadius && distance < closestRadius)
            {
                randomDirection = Random.insideUnitCircle * furthestRadius;
                randomDirection += gameObject.transform.position;
                distance = Vector3.Distance(randomDirection, gameObject.transform.position);
            }
            return randomDirection;
        }
    }

}