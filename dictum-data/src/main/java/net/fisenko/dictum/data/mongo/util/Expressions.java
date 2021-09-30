package net.fisenko.dictum.data.mongo.util;

import org.bson.Document;
import org.bson.conversions.Bson;

public final class Expressions {

    private Expressions() {
    }

    public static Bson toObjectId(String id) {
        return new Document("$toObjectId", id);
    }
}
