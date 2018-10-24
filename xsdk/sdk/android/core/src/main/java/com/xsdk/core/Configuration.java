package com.xsdk.core;

import com.xsdk.core.impl.ConfigurationImpl;

public class Configuration {
    public static Boolean getUseLog()
    {
        ConfigurationImpl configurationImpl = ConfigurationImpl.getInstance();
        return configurationImpl.getUseLog();
    }
}
