using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        foreach (var client in MatchplayNetworkServer.Instance.ClientData)
        {
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                Vector3 spawnPos;
                // Assign specific spawn positions based on character ID
                if (character.Id == 1)
                {
                    // Find the spawn point tagged as "RobertSpawnPoint"
                    GameObject spawnPoint = GameObject.FindGameObjectWithTag("RobertSpawnPoint");
                    if (spawnPoint != null)
                    {
                        spawnPos = spawnPoint.transform.position;
                    }
                    else
                    {
                        Debug.LogError("Spawn point 'RobertSpawnPoint' not found. Spawning at default location.");
                        spawnPos = new Vector3(0, 0, 0); // Fallback position if the spawn point is missing
                    }
                }
                else if (character.Id == 2)
                {
                    // Hard-coded spawn position for character with Id 2
                    spawnPos = new Vector3(-0.779999971f, -18.7600002f, -2.25999999f);
                }
                else
                {
                    // Default random position for any other characters
                    spawnPos = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                }

                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
            }
        }
    }
}
