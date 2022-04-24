package net.fisenko.dictum.core.service;

public interface SecurityService {

    String hash(String password);

    boolean verify(String password, String hashedPassword);
}
