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
public class LanguageEntity {

    public final static String COLLECTION_NAME = "languages";

    public final static String CODE_FIELD_NAME = "code";
    public final static String LANGUAGE_FIELD_NAME = "language";

    @BsonId
    private ObjectId id;
    @BsonProperty(CODE_FIELD_NAME)
    private String code;
    @BsonProperty(LANGUAGE_FIELD_NAME)
    private String language;
}
