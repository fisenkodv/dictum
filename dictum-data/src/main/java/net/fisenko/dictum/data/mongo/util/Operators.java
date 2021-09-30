package net.fisenko.dictum.data.mongo.util;

public final class Operators {

    private Operators() {
    }

    public static String getPositionOperator(String field) {
        return field+".$";
    }
}
