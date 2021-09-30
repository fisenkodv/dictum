package net.fisenko.dictum.core.util;

import org.jetbrains.annotations.Nullable;

public class Strings {

    private Strings() {
    }

    public static boolean compareIgnoreCase(@Nullable String a, @Nullable String b) {
        return a != null && a.equalsIgnoreCase(b);
    }

    public static boolean isNullOrEmpty(@Nullable String str) {
        return str == null || str.isEmpty();
    }
}
