package com.xsdk.core.base;

import org.json.JSONArray;
import org.json.JSONObject;
import org.json.JSONException;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by twenty0ne on 2018/9/25.
 */

public class Android
{
    public static JSONObject fieldToJSON(Object object)
    {
        JSONObject resJson = new JSONObject();

        Field[] memberFields = object.getClass().getFields();
        for (Field field : memberFields) {
            try
            {
                Class<?> fieldType = field.getType();
                String name = field.getName();
                Object fieldValue = field.get(object);
                if (fieldValue != null) {
                    if (fieldType.isArray())
                    {
                        JSONArray jsonArray = new JSONArray();

                        Object[] values = (Object[])field.get(object);
                        for (Object value : values) {
                            if (value != null) {
                                jsonArray.put(value);
                            }
                        }
                        try
                        {
                            resJson.put(name, jsonArray);
                        }
                        catch (JSONException localJSONException4) {}
                    }
                    else if (((fieldValue instanceof List)) || ((fieldValue instanceof ArrayList)))
                    {
                        JSONArray jsonArray = new JSONArray();

                        List values = (List)field.get(object);
                        for (Object value : values) {
                            if (value != null) {
                                if ((value instanceof DataModel)) {
                                    jsonArray.put(((DataModel)value).toJson());
                                } else {
                                    jsonArray.put(value);
                                }
                            }
                        }
                        try
                        {
                            resJson.put(name, jsonArray);
                        }
                        catch (JSONException localJSONException5) {}
                    }
                    else if (fieldType.isEnum())
                    {
                        Enum<?> value = (Enum)field.get(object);
                        if (value != null) {
                            try
                            {
                                resJson.put(name, value.name());
                            }
                            catch (JSONException localJSONException2) {}
                        }
                    }
                    else
                    {
                        Object value = field.get(object);
                        try
                        {
                            resJson.put(name, value);
                        }
                        catch (JSONException localJSONException3) {}
                    }
                }
            }
            catch (IllegalAccessException|IllegalArgumentException e)
            {
                e.printStackTrace();
            }
        }
        return resJson;
    }
}
