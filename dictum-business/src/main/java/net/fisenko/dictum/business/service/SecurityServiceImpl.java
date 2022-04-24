package net.fisenko.dictum.business.service;

import com.password4j.BCryptFunction;
import com.password4j.Password;
import com.password4j.types.BCrypt;
import net.fisenko.dictum.core.service.SecurityService;

public class SecurityServiceImpl implements SecurityService {

    private static final BCryptFunction B_CRYPT_FUNCTION = BCryptFunction.getInstance(BCrypt.B, 10);

    @Override
    public String hash(String password) {
        return Password.hash(password).with(B_CRYPT_FUNCTION).getResult();
    }

    @Override
    public boolean verify(String password, String hashedPassword) {
        return Password.check(password, hashedPassword).with(B_CRYPT_FUNCTION);
    }
}
