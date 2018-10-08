package com.xsdk.core;

import com.xsdk.core.base.DataModel;
import com.xsdk.core.base.Logger;
import com.xsdk.core.impl.AuthImpl;

public class Auth {
    public static final String TAG = Auth.class.getSimpleName();

    public static void init(AuthInitListener listener) {
        Logger.enterMethod(TAG);

        AuthImpl authImpl = AuthImpl.getInstance();
        authImpl.init(listener);

        Logger.exitMethod(TAG);
    }

    public static void login(AuthLoginListener listener) {
        Logger.enterMethod(TAG);

        AuthImpl authImpl = AuthImpl.getInstance();
        authImpl.login(listener);

        Logger.exitMethod(TAG);
    }

    public static void logout(AuthLogoutListener listener) {
        Logger.enterMethod(TAG);

        AuthImpl authImpl = AuthImpl.getInstance();
        authImpl.logout(listener);

        Logger.exitMethod(TAG);
    }

    public static abstract interface AuthInitListener {
        public abstract void onAuthInit(ResultAPI paramResultAPI, AuthInitResult paramAuthInitResult);
    }

    public static abstract interface AuthLoginListener {
        public abstract void onAuthLogin(ResultAPI paramResultAPI, Auth.Account paramAccount);
    }

    public static abstract interface AuthLogoutListener {
        public abstract void onAuthLogout(ResultAPI paramResultAPI);
    }

    public static class AuthInitResult extends DataModel {
        public Boolean isAuthorized;
        public String playerName;
        public String playerId;
    }

    public static class Account extends DataModel {
        public String uid;
        public String accessToken;
    }
}
