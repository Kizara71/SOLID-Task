using UnityEngine;

public interface IReloadable
{
    void Reload();
    bool IsReloading { get;} // is this a public get or a private get? I would assume public get and private set, but I want to confirm
}
