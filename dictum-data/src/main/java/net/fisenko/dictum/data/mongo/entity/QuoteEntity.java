package net.fisenko.dictum.data.mongo.entity;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.codecs.pojo.annotations.BsonProperty;
import org.bson.types.ObjectId;

import java.time.Instant;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class QuoteEntity {

    public final static String COLLECTION_NAME = "quotes";

    public final static String TEXT_FIELD_NAME = "text";
    public final static String HASH_FIELD_NAME = "hash";
    public final static String AUTHOR_FIELD_NAME = "author";
    public final static String AUTHOR_ID_FIELD_NAME = "author_id";
    public final static String ADDED_AT_FIELD_NAME = "added_at";
    public final static String LANGUAGE_FIELD_NAME = "language";
    public static final String LIKES_FIELD_NAME = "likes";

    @BsonId
    private ObjectId id;
    @BsonProperty(TEXT_FIELD_NAME)
    private String text;
    @BsonProperty(HASH_FIELD_NAME)
    private String hash;
    @BsonProperty(AUTHOR_FIELD_NAME)
    private AuthorEntity author;
    @BsonProperty(ADDED_AT_FIELD_NAME)
    private Instant addedAt;
    @BsonProperty(LANGUAGE_FIELD_NAME)
    private String language;
    @BsonProperty(LIKES_FIELD_NAME)
    private Integer likes;
}
