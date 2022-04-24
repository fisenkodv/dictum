package net.fisenko.dictum.data.mongo.entity;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.codecs.pojo.annotations.BsonProperty;
import org.bson.types.ObjectId;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class UserEntity {

    public final static String COLLECTION_NAME = "users";

    public final static String USER_NAME_FIELD_NAME = "user_name";
    public final static String PASSWORD_FIELD_NAME = "password_hash";

    @BsonId
    private ObjectId id;
    @BsonProperty(USER_NAME_FIELD_NAME)
    private String userName;
    @BsonProperty(PASSWORD_FIELD_NAME)
    private String passwordHash;
}
