package com.xsdk.core.impl;

import com.xsdk.core.Auth;

/**
 * Created by twenty0ne on 2018/9/21.
 */

public class AuthImpl {

    public static String TAG = AuthImpl.class.getSimpleName();
    private static final AuthImpl authImpl = new AuthImpl();

    public static AuthImpl getInstance() {
        return authImpl;
    }

    public void init(final Auth.AuthInitListener initListener) {

    }

    public void login(final Auth.AuthLoginListener loginListener) {

    }

    public void logout(final Auth.AuthLogoutListener logoutListener) {

    }
}
