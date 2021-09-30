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
public class AuthorEntity {

    public final static String COLLECTION_NAME = "authors";

    public final static String NAME_FIELD_NAME = "name";
    public final static String BIO_FIELD_NAME = "bio";
    public final static String LANGUAGE_FIELD_NAME = "language";

    @BsonId
    private ObjectId id;
    @BsonProperty(NAME_FIELD_NAME)
    private String name;
    @BsonProperty(BIO_FIELD_NAME)
    private String bio;
    @BsonProperty(LANGUAGE_FIELD_NAME)
    private String language;
}
