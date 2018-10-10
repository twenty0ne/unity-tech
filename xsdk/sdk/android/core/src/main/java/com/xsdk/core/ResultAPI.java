package com.xsdk.core;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Objects;

public class ResultAPI {
    public static final int SUCCESS = 0;

    public ResultAPI(int errorCode, Code code) {
    }

    public static enum Code {
        Success(0, "Success");

        private int value;
        private String keyForMessage;

        private Code(int i, String key) {
            this.value = i;
            this.keyForMessage = key;
        }

        public int getValue() {
            return this.value;
        }

        public String getKey() {
            return this.keyForMessage;
        }
    }

    public int errorCode = 0;
    public Code code = Code.Success;
    private String errorMessageFormat = "";
    private String messageFormat = "";
    private String additionalMessage = null;

    public JSONObject toJson()
    {
        JSONObject resJsonParam = new JSONObject();
        try
        {
            resJsonParam.put("errorCode", this.errorCode);
            resJsonParam.put("code", this.code.getValue());
            resJsonParam.put("errorMessage", getErrorMessage());
            resJsonParam.put("message", getMessage());
        }
        catch (JSONException e)
        {
            e.printStackTrace();
        }
        return resJsonParam;
    }

    public String getMessage()
    {
        return String.format(this.messageFormat, new Object[]{ this.additionalMessage });
    }

    public String getErrorMessage()
    {
        return String.format(this.errorMessageFormat, new Object[]{ this.additionalMessage });
    }
}
