package net.fisenko.dictum.data.mongo.util;

import com.mongodb.DBRef;
import org.bson.types.ObjectId;

import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public final class Fields {

    public static final String ID = "id";
    public static final String UNDERSCORE_ID = "_id";
    public static final String POSITIONAL_OPERATOR = "$";

    private Fields() {
    }

    public static ObjectId getId(String id) {
        return new ObjectId(id);
    }

    public static DBRef getRef(String id, String collectionName) {
        return new DBRef(collectionName, new ObjectId(id));
    }

    public static List<DBRef> getRefs(List<String> ids, String collectionName) {
        return ids
                .stream()
                .map(x -> new DBRef(collectionName, new ObjectId(x)))
                .collect(Collectors.toList());
    }

    public static String combine(String... paths) {
        return String.join(".", Arrays.asList(paths));
    }
}

