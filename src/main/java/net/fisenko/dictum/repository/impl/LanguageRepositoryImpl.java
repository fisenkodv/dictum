package net.fisenko.dictum.repository.impl;

import com.github.jasync.sql.db.Connection;
import com.github.jasync.sql.db.ResultSet;
import com.github.jasync.sql.db.RowData;
import jakarta.inject.Singleton;
import lombok.SneakyThrows;
import net.fisenko.dictum.model.domain.Language;
import net.fisenko.dictum.repository.LanguageRepository;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

@Singleton
public class LanguageRepositoryImpl implements LanguageRepository {

    private final Connection connection;

    public LanguageRepositoryImpl(Connection connection) {
        this.connection = connection;
    }

    @Override
    @SneakyThrows
    public Collection<Language> getLanguages() {
        return connection.sendQuery("SELECT * FROM languages").thenApply(queryResult -> {
            List<Language> result = new ArrayList<>();
            ResultSet resultSet = queryResult.getRows();

            for (RowData data : resultSet) {
                Language language = new Language();
                language.setId(data.getLong("id"));
                language.setCode(data.getString("code"));
                language.setLanguage(data.getString("language"));
                result.add(language);
            }

            return result;
        }).get();
    }
}
