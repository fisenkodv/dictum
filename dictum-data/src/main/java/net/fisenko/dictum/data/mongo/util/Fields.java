package net.fisenko.dictum.data.mongo.util;

import org.bson.types.ObjectId;

import java.util.Arrays;

public final class Fields {

    public static final String ID = "id";
    public static final String UNDERSCORE_ID = "_id";
    public static final String DOLLAR_SIGN = "$";

    private Fields() {
    }

    public static ObjectId getId(String id) {
        return new ObjectId(id);
    }

    public static String getFieldPath(String field) {
        return DOLLAR_SIGN + field;
    }

    public static String combine(String... paths) {
        return String.join(".", Arrays.asList(paths));
    }
}

