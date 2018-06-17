using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDataSender : MonoBehaviour
{
    public ServerConnection con;
    public SessionData data;
	void Start()
    {
        con.UpdateSessionData(data);
        //con.GetSessionData(data, 1);
    }
}
