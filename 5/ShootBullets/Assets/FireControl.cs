using UnityEngine;
using UnityEngine.Networking;


public class FireControl : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        
        if (Input.GetKeyDown("space"))
        {
            CmdShoot();
        }
    }

    [ClientRpc]
    private void RpcShoot()
    {
        if (!isServer)
            SpawnBullet();
    }

    [Command]
    private void CmdShoot()
    {
        SpawnBullet();
        RpcShoot();
    }


    private void SpawnBullet()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 50;
        Destroy(bullet, 5.0f);
    }
}