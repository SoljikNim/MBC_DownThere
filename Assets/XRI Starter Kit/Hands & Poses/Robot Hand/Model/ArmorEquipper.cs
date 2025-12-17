using System.Collections.Generic;
using UnityEngine;

public class ArmorEquipper : MonoBehaviour
{
    [SerializeField] Transform armorParent;
    [SerializeField] Transform playerRootBone;              // Player의 Hips 또는 Armature 루트
    [SerializeField] SkinnedMeshRenderer playerSMR;         // 플레이어 기본 바디 SMR (rootBone 참조용)


    private Dictionary<string, Transform> boneMap; // boneName → playerBone 캐싱

    void Awake()
    {
        // 플레이어 본 전체 캐싱
        boneMap = new Dictionary<string, Transform>();
        CacheBones(playerRootBone);
    }

    // 재귀적으로 플레이어 본 전체 수집
    void CacheBones(Transform t)
    {
        boneMap[t.name] = t;
        foreach (Transform child in t)
            CacheBones(child);
    }
    // ------------------------------
    //        장비 장착 함수
    // ------------------------------
    public void Equip(GameObject armorPrefab)
    {
        GameObject armor = Instantiate(armorPrefab);
        SkinnedMeshRenderer[] armorSMRs = armor.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var smr in armorSMRs)
            RemapSMR(smr);

        // 장비 오브젝트를 플레이어 하위로
        armor.transform.SetParent(armorParent, false);
        var remove = armor.GetComponentInChildren<ShouldRemove>();
        if (remove != null)
        {
            Destroy(remove.gameObject);
        }
        playerSMR.gameObject.SetActive(false);
    }

    // ------------------------------
    //      본 리타겟 함수
    // ------------------------------
    private void RemapSMR(SkinnedMeshRenderer smr)
    {
        // rootBone 교체
        smr.rootBone = playerSMR.rootBone;

        // bones 배열 교체
        Transform[] newBones = new Transform[smr.bones.Length];

        for (int i = 0; i < smr.bones.Length; i++)
        {
            string boneName = smr.bones[i].name;

            if (boneMap.TryGetValue(boneName, out Transform targetBone))
                newBones[i] = targetBone;
            else
                Debug.LogWarning($"[ArmorEquip] 플레이어에서 본을 찾을 수 없음: {boneName}");
        }

        smr.bones = newBones;
    }
}
