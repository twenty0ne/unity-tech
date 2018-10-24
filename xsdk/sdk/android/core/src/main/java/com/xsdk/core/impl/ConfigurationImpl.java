package com.xsdk.core.impl;

public class ConfigurationImpl {
    private static final ConfigurationImpl configurationImpl = new ConfigurationImpl();

    private Boolean useLog = Boolean.valueOf(true);

    public static ConfigurationImpl getInstance()
    {
        return configurationImpl;
    }

    public Boolean getUseLog()
    {
        return useLog;
    }
}
