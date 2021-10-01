package net.fisenko.dictum.business.util;

import lombok.SneakyThrows;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;

public final class Hash {

    private Hash() {
    }

    @SneakyThrows
    public static String sha256(String text) {
        final MessageDigest digest = MessageDigest.getInstance("SHA-256");
        final byte[] bytes = digest.digest(text.getBytes(StandardCharsets.UTF_8));
        return bytesToHex(bytes);
    }

    private static String bytesToHex(byte[] hash) {
        final StringBuilder hexString = new StringBuilder(2 * hash.length);

        for (byte b : hash) {
            String hex = Integer.toHexString(0xff & b);
            if (hex.length() == 1) {
                hexString.append('0');
            }
            hexString.append(hex);
        }

        return hexString.toString();
    }
}
