package net.fisenko.dictum.data.mongo.entity;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class AuthorEntity {

    public final static String COLLECTION_NAME = "authors";

    @BsonId
    private ObjectId id;
    private String language;
    private String name;
    private String bio;
}
