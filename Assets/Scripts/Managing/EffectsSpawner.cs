using UnityEngine;
using System.Collections;

public class EffectsSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private ObjectsMerger _merger;
    [SerializeField] private float _delayBeforeDeletion;

    private void OnEnable()
    {
        _merger.MergeableObjectsMerged += SpawnParticles;
    }

    private void OnDisable()
    {
        _merger.MergeableObjectsMerged -= SpawnParticles;
    }

    private void SpawnParticles(Vector2 posToSpawnAt, MergeableObjectLevel objectType)
    {
        int particleToSpawnIndex = (int)objectType;

        if(particleToSpawnIndex >= _particles.Length)
            return;

        ParticleSystem particleToSpawn = _particles[particleToSpawnIndex];

        GameObject particleObject = Instantiate(particleToSpawn, posToSpawnAt, Quaternion.identity).gameObject;
        StartCoroutine(DeleteInSeconds(particleObject));
    }

    private IEnumerator DeleteInSeconds(GameObject objectToDelete)
    {
        yield return new WaitForSeconds(_delayBeforeDeletion);

        Destroy(objectToDelete);
    }
}
