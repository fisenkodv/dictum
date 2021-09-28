package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Aggregates;
import com.mongodb.client.model.Field;
import com.mongodb.client.model.Filters;
import com.mongodb.reactivestreams.client.AggregatePublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.configuration.DatabaseConfiguration;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.domain.Quote;
import net.fisenko.dictum.data.mongo.entity.AuthorEntity;
import net.fisenko.dictum.data.mongo.entity.QuoteEntity;
import net.fisenko.dictum.data.mongo.util.Fields;
import org.bson.Document;
import reactor.core.publisher.Mono;

import java.util.List;

public class MongoQuoteRepository implements QuoteRepository {

    private final static int RANDOM_SAMPLE_SIZE = 1000;

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public MongoQuoteRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Mono<Quote> getRandomQuote(String language) {
        final String AUTHOR_ID_FIELD_NAME = "author_id";
        final String AUTHOR_FIELD_NAME = "author";

        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(
                List.of(
                        Aggregates.sample(RANDOM_SAMPLE_SIZE),
                        Aggregates.match(Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language)),
                        Aggregates.addFields(new Field<>(AUTHOR_ID_FIELD_NAME, new Document("$toObjectId", Fields.getFieldPath(AUTHOR_ID_FIELD_NAME)))),
                        Aggregates.lookup(AuthorEntity.COLLECTION_NAME, AUTHOR_ID_FIELD_NAME, Fields.UNDERSCORE_ID, QuoteEntity.AUTHOR_FIELD_NAME),
                        Aggregates.unwind(Fields.getFieldPath(AUTHOR_FIELD_NAME)),
                        Aggregates.limit(1)
                ));

        return Mono.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    private MongoCollection<QuoteEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class);
    }
}
