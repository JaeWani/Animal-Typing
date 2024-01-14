using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public enum PlayerState
{
    Master,
    User
}
public class GameManager : MonoBehaviour
{

   
    #region Variable;

    public static GameManager instance;


    public int randomValue
    {
        get
        {
            return Random.Range(0, attackSentences.Count - 1);
        }
    }
    public PhotonView photonView { get; private set; }

    [SerializeField] private Text text_1, text_2;

    [Header("In Game UI")]
    [SerializeField] private Text AttackSentenceText;
    [SerializeField] private Text interferenceWordText;
    [SerializeField] private Text healWordText;

    public InputField inputField;

    [Header("Photon Player")]
    public string Player_1_Name, Player_2_Name;

    [Header("Sentence , Word")]
    public List<string> attackSentences = new List<string>();
    public List<string> interferenceWords = new List<string>();
    public List<string> healWords = new List<string>();

    public string currentAttackSentence = null;
    public string interferenceWord = null;
    public string healWord = null;
    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        photonView.RPC("SetNickName", RpcTarget.All);

        SetRandomAttackSentence(Random.Range(0, attackSentences.Count - 1));

        text_1.text = Player_1_Name;
        text_2.text = Player_2_Name;
    }

    private void Update()
    {
        AttackSentenceText.text = currentAttackSentence;
    }

    [PunRPC]
    public void SetNickName()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.IsMasterClient) Player_1_Name = item.NickName + " /  Master";
            else Player_2_Name = item.NickName + " /  User";
        }
    }

    public static void SetRandomAttackSentence(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomAttackSentenceRPC", RpcTarget.All, index);
    }

    public static void SetRandomAttackSentence(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomAttackSentenceRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void SetRandomAttackSentenceRPC(int index)
    {
        currentAttackSentence = attackSentences[index];
    }
    public void NextRound(int index, int instanceID, PlayerState playerState)
    {
        SetRandomAttackSentence(index, instanceID);
    }
}
