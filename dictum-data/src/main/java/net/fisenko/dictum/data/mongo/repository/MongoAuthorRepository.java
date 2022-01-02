package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Filters;
import com.mongodb.client.model.Sorts;
import com.mongodb.client.model.TextSearchOptions;
import com.mongodb.reactivestreams.client.FindPublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.model.Author;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.util.Strings;
import net.fisenko.dictum.data.mongo.entity.AuthorEntity;
import net.fisenko.dictum.data.mongo.util.Fields;
import org.bson.conversions.Bson;
import org.jetbrains.annotations.Nullable;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.ArrayList;
import java.util.List;

public class MongoAuthorRepository implements AuthorRepository {

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public MongoAuthorRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Flux<Author> getAuthors(String language, @Nullable String query, int limit, int offset) {
        final List<Bson> filters = new ArrayList<>();

        if (!Strings.isNullOrEmpty(query)) {
            filters.add(Filters.text(query, new TextSearchOptions().caseSensitive(false)));
        }

        filters.add(Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language));

        final Bson filter = Filters.and(filters);
        final FindPublisher<AuthorEntity> result = getCollection().find(filter)
                                                                  .sort(Sorts.ascending(AuthorEntity.NAME_FIELD_NAME))
                                                                  .limit(limit)
                                                                  .skip(offset);

        return Flux.from(result).map(x -> mappingService.map(x, Author.class));
    }

    @Override
    public Mono<Author> getAuthor(String language, String authorId) {
        final Bson filter = Filters.and(Filters.eq(Fields.UNDERSCORE_ID, Fields.getId(authorId)),
                                        Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language));

        final FindPublisher<AuthorEntity> result = getCollection().find(filter);

        return Mono.from(result).map(x -> mappingService.map(x, Author.class));
    }

    @Override
    public Mono<Long> getAuthorsCount(String language) {
        final Publisher<Long> result = getCollection().countDocuments(Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language));

        return Mono.from(result);
    }

    private MongoCollection<AuthorEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(AuthorEntity.COLLECTION_NAME, AuthorEntity.class);
    }
}
