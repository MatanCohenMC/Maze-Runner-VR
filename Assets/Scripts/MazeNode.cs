using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    Start,
    End
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] private GameObject[] m_Walls;
    [SerializeField] private MeshRenderer m_Floor; 

    public void SetState(NodeState i_State)
    {
        switch(i_State)
        {
            case NodeState.Available:
                m_Floor.material.color = Color.white; break;
            case NodeState.Current:
                m_Floor.material.color = Color.yellow; break;
            case NodeState.Completed:
                m_Floor.material.color = Color.blue; break;
            case NodeState.Start:
                m_Floor.material.color = Color.green; break;
            case NodeState.End:
                m_Floor.material.color = Color.red; break;
        }
    }

    public void RemoveWall(int i_WallToRemove)
    {
        m_Walls[i_WallToRemove].gameObject.SetActive(false);
    }
}
