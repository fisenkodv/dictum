package net.fisenko.dictum.business.service;

import net.fisenko.dictum.core.service.SecurityService;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertTrue;

public class SecurityServiceTest {

    @Test
    public void shouldCreatePasswordHashAndValidate() {
        final SecurityService securityService = new SecurityServiceImpl();
        final String password = String.valueOf(System.nanoTime());
        final String passwordHash = securityService.hash(password);

        assertTrue(securityService.verify(password, passwordHash));
    }
}
