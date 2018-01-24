using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XNetMatchInfoSnapshot
{
    // public NetworkID networkId { get; }
    //public NodeID hostNodeId { get; }
    public string networkId { get; set; }
    public string name { get; set; }
    // public int averageEloScore { get; set;  }
    //
    // 摘要:
    //     ///
    //     The maximum number of players this match can grow to.
    //     ///
    public int maxSize { get; set;  }
    //
    // 摘要:
    //     ///
    //     The current number of players in the match.
    //     ///
    public int currentSize { get; set; }
    public bool isPrivate { get; set; }
    public Dictionary<string, long> matchAttributes { get; set;  }
    // public List<MatchInfoDirectConnectSnapshot> directConnectInfos { get; }
}
