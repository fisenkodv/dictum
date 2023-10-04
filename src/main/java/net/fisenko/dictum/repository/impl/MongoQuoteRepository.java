package net.fisenko.dictum.repository.impl;

import com.github.jasync.sql.db.Connection;
import com.github.jasync.sql.db.ResultSet;
import com.github.jasync.sql.db.RowData;
import jakarta.inject.Singleton;
import lombok.SneakyThrows;
import net.fisenko.dictum.model.domain.Author;
import net.fisenko.dictum.model.domain.Language;
import net.fisenko.dictum.model.domain.Quote;
import net.fisenko.dictum.repository.QuoteRepository;

import java.util.List;

@Singleton
public class MongoQuoteRepository implements QuoteRepository {

    private final static int RANDOM_SAMPLE_SIZE = 100;
    private final Connection connection;

    public MongoQuoteRepository(Connection connection) {
        this.connection = connection;
    }

    @Override
    public Quote getRandomQuote(Long languageId) {
        return null;
    }

    @SneakyThrows
    @Override
    public Quote getQuote(Long quoteId) {
        return connection.sendPreparedStatement(
                """
                        SELECT 
                        q.id AS quote_id, 
                        q.text AS quote_text,
                        an.author_id AS author_id,
                        an.name AS author_name,
                        l.id AS language_id,
                        l.language AS language_language
                        FROM quotes AS q
                        INNER JOIN languages l ON l.id = q.language_id
                        INNER JOIN author_names an ON q.author_id = an.author_id AND an.language_id = q.language_id
                        WHERE q.id = ?""",
                List.of(quoteId)
        ).thenApply(queryResult -> {
            ResultSet resultSet = queryResult.getRows();
            if (resultSet.isEmpty())
                return null;

            RowData row = resultSet.get(0);

            Language language = new Language();
            language.setId(row.getLong("language_id"));
            language.setLanguage(row.getString("language_language"));

            Author author = new Author();
            author.setId(row.getLong("author_id"));
            author.setName(row.getString("author_name"));

            Quote result = new Quote();
            result.setId(row.getLong("quote_id"));
            result.setText(row.getString("quote_text"));
            result.setAuthor(author);
            result.setLanguage(language);


            return result;
        }).get();
    }
}
