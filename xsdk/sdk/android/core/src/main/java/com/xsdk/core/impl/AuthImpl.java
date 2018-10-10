package com.xsdk.core.impl;

import android.util.Log;

import com.xsdk.core.Auth;
import com.xsdk.core.ResultAPI;

public class AuthImpl {

    public static String TAG = AuthImpl.class.getSimpleName();
    private static final AuthImpl authImpl = new AuthImpl();

    public static AuthImpl getInstance() {
        return authImpl;
    }

    public void init(final Auth.AuthInitListener initListener) {
        Log.d("AuthImpl", "xx-- init 0 > ");
        if (initListener != null){
            Log.d("AuthImpl", "xx-- init 1 > ");
            initListener.onAuthInit(new ResultAPI(0, ResultAPI.Code.Success), null);
        }
    }

    public void login(final Auth.AuthLoginListener loginListener) {

    }

    public void logout(final Auth.AuthLogoutListener logoutListener) {

    }
}
