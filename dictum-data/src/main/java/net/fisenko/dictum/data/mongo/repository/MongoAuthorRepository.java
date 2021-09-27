package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Filters;
import com.mongodb.client.model.Sorts;
import com.mongodb.reactivestreams.client.FindPublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import de.cronn.reflection.util.PropertyUtils;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.data.mongo.entity.AuthorEntity;
import net.fisenko.dictum.data.mongo.util.Fields;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.model.domain.Author;
import net.fisenko.dictum.core.util.Strings;
import org.bson.conversions.Bson;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.ArrayList;
import java.util.List;

public class MongoAuthorRepository implements AuthorRepository {

    private final MappingService mappingService;
    private final MongoClient mongoClient;

    public MongoAuthorRepository(MappingService mappingService, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.mongoClient = mongoClient;
    }

    @Override
    public Flux<Author> getAuthors(String language, @Nullable String query, int limit, int offset) {
        final List<Bson> filters = new ArrayList<>();
        filters.add(Filters.eq(PropertyUtils.getPropertyName(AuthorEntity.class, AuthorEntity::getLanguage), language));

        if (!Strings.isNullOrEmpty(query)) {
            filters.add(Filters.regex(PropertyUtils.getPropertyName(AuthorEntity.class, AuthorEntity::getName), query, "i"));
        }

        final Bson filter = Filters.and(filters);
        final FindPublisher<AuthorEntity> result = getCollection().find(filter)
                                                                  .sort(Sorts.ascending(PropertyUtils.getPropertyName(AuthorEntity.class, AuthorEntity::getName)))
                                                                  .limit(limit)
                                                                  .skip(offset);

        return Flux.from(result).map(x -> mappingService.map(x, Author.class));
    }

    @Override
    public Mono<Author> getAuthor(String language, String id) {
        final Bson filter = Filters.and(Filters.eq(Fields.UNDERSCORE_ID, Fields.getId(id)),
                                        Filters.eq(PropertyUtils.getPropertyName(AuthorEntity.class, AuthorEntity::getLanguage), language));

        final FindPublisher<AuthorEntity> result = getCollection().find(filter);

        return Mono.from(result).map(x -> mappingService.map(x, Author.class));
    }

    private MongoCollection<AuthorEntity> getCollection() {
        return mongoClient
                .getDatabase("dictum")
                .getCollection(AuthorEntity.COLLECTION_NAME, AuthorEntity.class);
    }
}
