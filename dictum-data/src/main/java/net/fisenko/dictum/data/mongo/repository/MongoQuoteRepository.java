package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Aggregates;
import com.mongodb.client.model.Filters;
import com.mongodb.reactivestreams.client.AggregatePublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import de.cronn.reflection.util.PropertyUtils;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.configuration.DatabaseConfiguration;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.domain.Quote;
import net.fisenko.dictum.data.mongo.entity.QuoteEntity;
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
        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(List.of(
                Aggregates.sample(RANDOM_SAMPLE_SIZE),
                Aggregates.match(Filters.eq(PropertyUtils.getPropertyName(QuoteEntity.class, QuoteEntity::getLanguage), language))
        ));

        return Mono.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    private MongoCollection<QuoteEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class);
    }
}
