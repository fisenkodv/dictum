package net.fisenko.dictum.micronaut.bean;

import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.business.service.SecurityServiceImpl;
import net.fisenko.dictum.core.service.SecurityService;

@Factory
public class SecurityFactory {

//    @Singleton
//    public AuthenticationProvider getAuthenticationProvider() {
//        return new DictumAuthenticationProvider();
//    }

//    @Singleton
//    public RefreshTokenPersistence getRefreshTokenPersistence() {
//        return new DictumRefreshTokenPersistence();
//    }

    @Singleton
    public SecurityService securityService() {
        return new SecurityServiceImpl();
    }
}
