package com.xsdk.core.base;

import org.json.JSONObject;

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
