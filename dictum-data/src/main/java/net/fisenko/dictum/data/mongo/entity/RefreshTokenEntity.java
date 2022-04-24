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
public class RefreshTokenEntity {

    public final static String COLLECTION_NAME = "refresh_tokens";

    public final static String USER_NAME_FIELD_NAME = "user_name";
    public final static String REFRESH_TOKEN_FIELD_NAME = "refresh_token";
    public final static String REVOKED_FIELD_NAME = "revoked";

    @BsonId
    private ObjectId id;
    @BsonProperty(USER_NAME_FIELD_NAME)
    private String userName;
    @BsonProperty(REFRESH_TOKEN_FIELD_NAME)
    private String refreshToken;
    @BsonProperty(REVOKED_FIELD_NAME)
    private boolean revoked;
}
