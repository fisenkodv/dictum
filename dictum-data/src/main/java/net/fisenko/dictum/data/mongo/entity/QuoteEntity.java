package net.fisenko.dictum.data.mongo.entity;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import java.time.Instant;

@Getter
@Setter
@ToString
@NoArgsConstructor
public class QuoteEntity {

    public final static String COLLECTION_NAME = "quotes";

    @BsonId
    private ObjectId id;
    private String text;
    private String hash;
    private Instant createdAt;
    private String language;
}
