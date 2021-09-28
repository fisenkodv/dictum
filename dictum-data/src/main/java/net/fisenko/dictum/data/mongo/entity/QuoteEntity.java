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
    public final static String CREATED_AT_FIELD_NAME = "created_at";
    public final static String LANGUAGE_FIELD_NAME = "language";

    @BsonId
    private ObjectId id;
    @BsonProperty(TEXT_FIELD_NAME)
    private String text;
    @BsonProperty(HASH_FIELD_NAME)
    private String hash;
    @BsonProperty(AUTHOR_FIELD_NAME)
    private AuthorEntity author;
    @BsonProperty(CREATED_AT_FIELD_NAME)
    private Instant createdAt;
    @BsonProperty(LANGUAGE_FIELD_NAME)
    private String language;
}
