package net.fisenko.dictum.micronaut.configuration;

import io.micronaut.context.annotation.ConfigurationBuilder;
import io.micronaut.context.annotation.ConfigurationProperties;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;
import net.fisenko.dictum.core.configuration.ApplicationConfiguration;
import net.fisenko.dictum.core.configuration.DatabaseConfiguration;

@Getter
@Setter
@ToString
@ConfigurationProperties("dictum")
public class MicronautApplicationConfiguration implements ApplicationConfiguration {

    @ConfigurationBuilder(configurationPrefix = "db")
    private DatabaseConfiguration DB = new DatabaseConfiguration();
}
