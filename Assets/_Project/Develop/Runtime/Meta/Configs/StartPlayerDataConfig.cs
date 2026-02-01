using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Configs
{
    [CreateAssetMenu(fileName = "StartPlayerDataConfig", menuName = "Configs/Gameplay/StartPlayerDataConfig")]
    public class StartPlayerDataConfig : ScriptableObject
    {
        [field: SerializeField] public int DefaultGoldAmount { get; private set; }
    }
}
