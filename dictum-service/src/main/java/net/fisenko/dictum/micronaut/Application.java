package net.fisenko.dictum.micronaut;

import io.micronaut.runtime.Micronaut;
import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeIn;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeType;
import io.swagger.v3.oas.annotations.info.Contact;
import io.swagger.v3.oas.annotations.info.Info;
import io.swagger.v3.oas.annotations.info.License;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.security.SecurityScheme;

@OpenAPIDefinition(
        info = @Info(
                title = "${dictum.api.title}",
                version = "${dictum.api.version}",
                description = "${dictum.api.description}",
                license = @License(name = "${dictum.api.licence}", url = "${dictum.api.licence.url}"),
                contact = @Contact(url = "${dictum.api.contact.url}", name = "${dictum.api.contact.name}", email = "${dictum.api.contact.email}")
        )
)
@SecurityScheme(
        name = "BearerAuth",
        type = SecuritySchemeType.HTTP,
        in = SecuritySchemeIn.HEADER,
        scheme = "bearer",
        bearerFormat = "jwt"
)
@SecurityRequirement(name = "BearerAuth")
public class Application {

    public static void main(String[] args) {
        Micronaut.run(Application.class, args);
    }
}
