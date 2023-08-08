using System.Collections;
using Characters.Human;
using Unity.Netcode;
using UnityEngine;

public class SetLayer : NetworkBehaviour
{
    //TODO: redo, activate it on game start
    private IEnumerator Start()
    {
        if (!NetworkObject.IsOwner) yield break;
        yield return new WaitForSeconds(2);
        var players = FindObjectsByType<HumanMovementView>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Human");
        }
    }
}