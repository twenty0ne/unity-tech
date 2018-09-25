package com.xsdk.core.base;

import org.json.JSONObject;

/**
 * Created by twenty0ne on 2018/9/25.
 */

public class DataModel {
    public JSONObject toJson()
    {
        return Android.fieldToJSON(this);
    }

    public String toString()
    {
        return "";
    }
}
