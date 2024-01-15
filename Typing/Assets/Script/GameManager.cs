using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Master,
    User
}
public class GameManager : MonoBehaviourPun
{
    #region Variable;

    public static GameManager instance;
    public int randomAttackValue
    {
        get { return Random.Range(0, attackSentences.Count - 1); }
    }
    public int randomHealValue
    {
        get { return Random.Range(0, healWords.Count - 1); }
    }
    public int randomInterference
    {
        get { return Random.Range(0, interferenceWords.Count - 1); }
    }
    public PhotonView photonView { get; private set; }

    [SerializeField] private Text text_1, text_2;

    [Header("In Game System")]
    public int player_1_Score = 3, player_2_Score = 3;

    public float interferenceTime = 1.5f;
    public bool isInterference = false;

    public bool isGameOver = false;
    [Header("In Game UI")]
    [SerializeField] private Text AttackSentenceText;
    [SerializeField] private Text interferenceWordText;
    [SerializeField] private Text healWordText;

    [SerializeField] private Text UserScoreText;
    [SerializeField] private Text MasterScoreText;

    [SerializeField] private Image interferenceImage;

    public InputField inputField;

    [Header("Photon Player")]
    public string Player_1_Name, Player_2_Name;

    [Header("Sentence , Word")]
    public List<string> attackSentences = new List<string>();
    public List<string> interferenceWords = new List<string>();
    public List<string> healWords = new List<string>();

    public string currentAttackSentence = null;
    public string currentInterferenceWord = null;
    public string currentHealWord = null;
    #endregion



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        player_1_Score = 3;
        player_2_Score = 3;

        photonView = GetComponent<PhotonView>();

        photonView.RPC("SetNickName", RpcTarget.All);

        SetRandomAttackSentence(Random.Range(0, attackSentences.Count - 1));
        SetRandomHealWord(Random.Range(0, healWords.Count - 1));
        SetRandomInterference(Random.Range(0, interferenceWords.Count - 1));

        text_1.text = Player_1_Name;
        text_2.text = Player_2_Name;

    }

    private void Update()
    {
        AttackSentenceText.text = currentAttackSentence;
        interferenceWordText.text = currentInterferenceWord;
        healWordText.text = currentHealWord;

        MasterScoreText.text = player_1_Score.ToString();
        UserScoreText.text = player_2_Score.ToString();
        if (player_1_Score <= 0 || player_2_Score <= 0) photonView.RPC("GameOver", RpcTarget.All);
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

    public static void SetRandomHealWord(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomHealWordRPC", RpcTarget.All, index);
    }
    public static void SetRandomHealWord(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomHealWordRPC", RpcTarget.All, index);
    }

    public static void SetRandomInterference(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomInterferenceWordRPC", RpcTarget.All, index);
    }
    public static void SetRandomInterference(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomInterferenceWordRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void SetRandomAttackSentenceRPC(int index)
    {
        currentAttackSentence = attackSentences[index];
    }

    [PunRPC]
    public void SetRandomHealWordRPC(int index)
    {
        currentHealWord = healWords[index];
    }

    [PunRPC]
    public void SetRandomInterferenceWordRPC(int index)
    {
        currentInterferenceWord = interferenceWords[index];
    }

    public static void NextAttackSentence(int index, int instanceID, PlayerState playerState)
    {
        SetRandomAttackSentence(index, instanceID);
        instance.photonView.RPC("Attack", RpcTarget.All, playerState);
    }
    public static void NextHealWord(int index, int instanceID, PlayerState playerState)
    {
        SetRandomHealWord(index, instanceID);
        instance.photonView.RPC("Heal", RpcTarget.All, playerState);
    }
    public static void NextInterference(int index, int instanceID)
    {
        SetRandomInterference(index, instanceID);
        instance.Interference();
    }

    [PunRPC]
    public void Attack(PlayerState playerState)
    {
        if (playerState == PlayerState.Master) player_2_Score--;
        else player_1_Score--;
    }

    [PunRPC]
    public void Heal(PlayerState playerState)
    {
        if (playerState == PlayerState.Master && player_1_Score < 3) player_1_Score++;
        else if (playerState == PlayerState.User && player_2_Score < 3) player_2_Score++;
    }
    public void Interference()
    {
        instance.photonView.RPC("InterferenceRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void InterferenceRPC()
    {
        StopAllCoroutines();
        StartCoroutine(interference());

        IEnumerator interference()
        {
            interferenceImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(interferenceTime);
            interferenceImage.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    public void GameOver()
    {
        if (isGameOver == false)
        {
            PhotonNetwork.LeaveRoom();
            isGameOver = true;
        }
    }
}
