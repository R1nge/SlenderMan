using Meryuhi.Rendering;
using Unity.Netcode;
using UnityEngine.Rendering;

namespace Characters.Slender
{
    public class ChangeFog : NetworkBehaviour
    {
        private Volume _volume;

        private void Start()
        {
            print($"Is local {IsOwner && IsLocalPlayer && NetworkObject.IsOwner && NetworkObject.IsLocalPlayer}");
            if (!IsOwner && !IsLocalPlayer && !NetworkObject.IsOwner && !NetworkObject.IsLocalPlayer) return;
            _volume = FindObjectOfType<Volume>();
            //_volume.enabled = false;
            var vol = _volume.profile;
            
            if (vol.TryGet(out FullScreenFog fog))
            {
                fog.intensity.Override(0);
                fog.density.Override(0);
            }
        }
    }
}